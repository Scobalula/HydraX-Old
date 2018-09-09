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
    class ScriptParseTree
    {
        /// <summary>
        /// Loads Scripts from Memory
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

                asset.Info = String.Format("Size - {0:0.00}KB", asset.Size / 1024.0);


                assetList.Add(asset);

            }

            return assetList;
        }

        /// <summary>
        /// Exports Scripts from Memory
        /// </summary>
        public static bool ExportFromMemory(Asset asset)
        {
            byte[] data = MemoryUtil.ReadBytes(T7Util.ActiveProcess, asset.StartLocation, asset.Size);

            string assetPath = asset.Path.
                Replace(".gsc", ".gscc").
                Replace(".csc", ".cscc").
                Replace(".gsh", ".gshc").
                Replace(".lua", ".luac");

            PathUtil.CreateFilePath("exported_files\\" + assetPath);

            File.WriteAllBytes("exported_files\\" + assetPath, data);

            return true;
        }
    }
}
