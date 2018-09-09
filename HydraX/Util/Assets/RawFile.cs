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
    /// RawFile Logic
    /// </summary>
    class RawFile
    {
        /// <summary>
        /// Loads RawFiles from Memory
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
                    NameLocation = BitConverter.ToInt64(assetData, 0)
                };

                if (Util.IsNullAsset(asset, poolInfo))
                    continue;

                asset.AssetType = poolInfo.PoolName;
                asset.StartLocation = BitConverter.ToInt64(assetData, 16);
                asset.Size = BitConverter.ToInt32(assetData, 8);
                asset.Path = MemoryUtil.ReadNullTerminatedString(T7Util.ActiveProcess, asset.NameLocation);
                asset.DisplayName = Path.GetFileName(asset.Path);
                asset.ExportFunction = ExportFromMemory;

                if (Path.GetExtension(asset.Path) == ".atr")
                {
                    asset.Size -= 4;
                    asset.StartLocation += 4;
                    asset.ExportFunction = ExportAnimTreeFromMemory;
                }

                asset.Info = String.Format("Size - {0:0.00}KB", asset.Size / 1024.0);


                assetList.Add(asset);

            }

            return assetList;
        }

        /// <summary>
        /// Exports an Animation Tree from Memory
        /// </summary>
        public static bool ExportAnimTreeFromMemory(Asset asset)
        {
            PathUtil.CreateFilePath("exported_files\\" + asset.Path);
            File.WriteAllBytes("exported_files\\" + asset.Path, DeflateUtil.Decode(MemoryUtil.ReadBytes
                (
                    T7Util.ActiveProcess, 
                    asset.StartLocation, 
                    asset.Size
                )).ToArray());
            return true;
        }

        /// <summary>
        /// Exports a rawfile from memory
        /// </summary>
        public static bool ExportFromMemory(Asset asset)
        {
            string assetPath = asset.Path.
                Replace(".gsc", ".gscc").
                Replace(".csc", ".cscc").
                Replace(".gsh", ".gshc").
                Replace(".lua", ".luac");
            PathUtil.CreateFilePath("exported_files\\" + assetPath);
            File.WriteAllBytes("exported_files\\" + assetPath, MemoryUtil.ReadBytes
                (
                    T7Util.ActiveProcess, 
                    asset.StartLocation, 
                    asset.Size
                ));
            return true;
        }

        /// <summary>
        /// Loads a Raw File from a Fast File
        /// </summary>
        public static void LoadFromFastFile(T7Util.FastFile fastFile, Asset asset)
        {
            // Asset Size
            asset.Size = fastFile.DecodedStream.ReadInt32();
            fastFile.DecodedStream.Seek(12, SeekOrigin.Current);
            asset.Path = fastFile.DecodedStream.ReadNullTerminatedString();
            asset.DisplayName = Path.GetFileName(asset.Path);
            asset.ExportFunction = ExportFromFastFile;
            asset.StartLocation = fastFile.DecodedStream.BaseStream.Position;
            asset.Info = String.Format("Size - {0:0.00}KB", asset.Size / 1024.0);

            switch (Path.GetExtension(asset.Path))
            {
                case ".gsc":
                case ".csc":
                case ".gsh":
                    asset.AssetType = "scriptparsetree";
                    break;
                case ".atr":
                    asset.ExportFunction = ExportAnimTreeFromFastFile;
                    asset.StartLocation += 4;
                    asset.Size -= 4;
                    break;
                default:
                    asset.AssetType = "rawfile";
                    break;
            }
        }

        /// <summary>
        /// Exports an Animation Tree from a Fast File
        /// </summary>
        public static bool ExportAnimTreeFromFastFile(Asset asset)
        {
            T7Util.ActiveFastFile.DecodedStream.Seek(asset.StartLocation, SeekOrigin.Begin);
            PathUtil.CreateFilePath("exported_files\\" + asset.Path);
            File.WriteAllBytes("exported_files\\" + asset.Path, DeflateUtil.Decode(T7Util.ActiveFastFile.DecodedStream.ReadBytes(asset.Size)).ToArray());
            return true;
        }

        /// <summary>
        /// Exports a rawfile from a Fast File
        /// </summary>
        public static bool ExportFromFastFile(Asset asset)
        {
            string assetPath = asset.Path.
                Replace(".gsc", ".gscc").
                Replace(".csc", ".cscc").
                Replace(".gsh", ".gshc").
                Replace(".lua", ".luac");
            T7Util.ActiveFastFile.DecodedStream.Seek(asset.StartLocation, SeekOrigin.Begin);
            PathUtil.CreateFilePath("exported_files\\" + assetPath);
            File.WriteAllBytes("exported_files\\" + assetPath, T7Util.ActiveFastFile.DecodedStream.ReadBytes(asset.Size));
            return true;
        }
    }
}

