/*
 *  HydraX - Copyright 2018 Philip/Scobalula
 *  
 *  This file is subject to the license terms set out in the
 *  "LICENSE.txt" file. 
 * 
 */
using System;
using System.IO;
using System.Collections.Generic;
using PhilUtil;

namespace HydraLib.T7.Assets
{
    /// <summary>
    /// Animation Mapping Table Logic
    /// </summary>
    class AnimMappingTable
    {

        /// <summary>
        /// Animation Mapping Table Row
        /// </summary>
        public class AnimationMap
        {
            /// <summary>
            /// Name of Map
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Entries within this Map
            /// </summary>
            public string[] Entries { get; set; }

            /// <summary>
            /// Creates a new Animation Mapping Entry
            /// </summary>
            /// <param name="name">Entry Name</param>
            /// <param name="entryCount">Entry Count</param>
            public AnimationMap(string name, int entryCount)
            {
                Name = name;
                Entries = new string[entryCount];
            }
        }

        /// <summary>
        /// Loads Animation Mapping Tables from Memory
        /// </summary>
        /// <param name="poolInfo">Asset Pool Data</param>
        /// <returns>Asset List</returns>
        public static List<Asset> LoadFromMemory(AssetPoolInformation poolInfo)
        {
            List<Asset> assetList = new List<Asset>();

            for (int i = 0; assetList.Count < poolInfo.AssetCount; i++)
            {
                byte[] assetData = MemoryUtil.ReadBytes(T7Util.ActiveProcess, poolInfo.StartLocation + (poolInfo.EntrySize * i), poolInfo.EntrySize);

                Asset asset = new Asset
                {
                    NameLocation = BitConverter.ToInt64(assetData, 0),
                    StartLocation = BitConverter.ToInt64(assetData, 8),
                };

                if (Util.IsNullAsset(asset, poolInfo))
                    continue;

                AnimationMap[] rows = new AnimationMap[BitConverter.ToInt64(assetData, 16)];

                asset.Path = MemoryUtil.ReadNullTerminatedString(T7Util.ActiveProcess, asset.NameLocation);
                asset.DisplayName = Path.GetFileName(asset.Path);
                asset.AssetType = poolInfo.PoolName;
                asset.Data = rows;
                asset.Info = String.Format("Entries - {0}", rows.Length);
                asset.ExportFunction = ExportFromMemory;
                assetList.Add(asset);
            }

            return assetList;
        }


        /// <summary>
        /// Exports an Animation Mapping Table from Memory
        /// </summary>
        public static bool ExportFromMemory(Asset asset)
        {
            string assetName = "exported_files\\animtables\\" + asset.Path;

            AnimationMap[] rows = (AnimationMap[])asset.Data;

            int offset = 0;

            // Process Rows
            for (int i = 0; i < rows.Length; i++)
            {
                int stringIndex = MemoryUtil.ReadInt32(T7Util.ActiveProcess, asset.StartLocation + offset);
                offset += 16;
                int numEntries = MemoryUtil.ReadInt32(T7Util.ActiveProcess, asset.StartLocation + offset);
                offset += 8;
                rows[i] = new AnimationMap(T7Util.GetString(stringIndex), numEntries);
            }

            // Process Entries
            for (int i = 0; i < rows.Length; i++)
            {
                for (int j = 0; j < rows[i].Entries.Length; j++)
                {
                    int stringIndex = MemoryUtil.ReadInt32(T7Util.ActiveProcess, asset.StartLocation + offset);
                    offset += 4;

                    rows[i].Entries[j] = T7Util.GetString(stringIndex);
                }
            }

            PathUtil.CreateFilePath(assetName);

            using (StreamWriter output = new StreamWriter(assetName))
            {
                output.WriteLine("#");
                for (int i = 0; i < rows.Length; i++)
                {
                    output.Write(rows[i].Name);

                    for (int j = 0; j < rows[i].Entries.Length; j++)
                    {
                        output.Write(",{0}", rows[i].Entries[j]);
                    }

                    output.WriteLine();
                }
            }

            return true;
        }

        /// <summary>
        /// Loads an Animation Mapping Table from a Fast File
        /// </summary>
        public static void LoadFromFastFile(T7Util.FastFile fastFile, Asset asset)
        {
            int numRows = fastFile.DecodedStream.ReadInt32();

            fastFile.DecodedStream.Seek(4, SeekOrigin.Current);

            asset.Path = fastFile.DecodedStream.ReadNullTerminatedString();
            asset.DisplayName = Path.GetFileName(asset.Path);
            asset.StartLocation = fastFile.DecodedStream.BaseStream.Position;
            asset.AssetType = "animmappingtable";
            asset.Data = numRows;
            asset.ExportFunction = ExportFromFastFile;
            asset.Info = String.Format("Entries - {0}", numRows);
        }

        /// <summary>
        /// Exports an Animation Mapping Table from a Fast File
        /// </summary>
        public static bool ExportFromFastFile(Asset asset)
        {
            T7Util.ActiveFastFile.DecodedStream.Seek(asset.StartLocation, SeekOrigin.Begin);

            int numRows = (int)asset.Data;

            string assetName = "exported_files\\animtables\\" + asset.Path;

            AnimationMap[] rows = new AnimationMap[numRows];

            // Process Rows
            for (int i = 0; i < numRows; i++)
            {
                int stringIndex = T7Util.ActiveFastFile.DecodedStream.ReadInt32();
                T7Util.ActiveFastFile.DecodedStream.ReadInt32();

                long separator = T7Util.ActiveFastFile.DecodedStream.ReadInt64();
                int numEntries = T7Util.ActiveFastFile.DecodedStream.ReadInt32();

                T7Util.ActiveFastFile.DecodedStream.Seek(4, SeekOrigin.Current);

                rows[i] = new AnimationMap(T7Util.ActiveFastFile.GetString(stringIndex - 1), numEntries);
            }

            // Process Entries
            for (int i = 0; i < numRows; i++)
                for (int j = 0; j < rows[i].Entries.Length; j++)
                    rows[i].Entries[j] = T7Util.ActiveFastFile.GetString(T7Util.ActiveFastFile.DecodedStream.ReadInt32() - 1);


            PathUtil.CreateFilePath(assetName);

            using (StreamWriter output = new StreamWriter(assetName))
            {
                output.WriteLine("#");
                for (int i = 0; i < numRows; i++)
                {
                    output.Write(rows[i].Name);

                    for (int j = 0; j < rows[i].Entries.Length; j++)
                    {
                        output.Write(",{0}", rows[i].Entries[j]);
                    }

                    output.WriteLine();
                }
            }

            return true;
        }
    }
}
