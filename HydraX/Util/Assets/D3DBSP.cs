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
    /// Entity Map Logic
    /// </summary>
    class D3DBSP
    {
        /// <summary>
        /// Loads Entity Maps from Memory
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
                    Size = BitConverter.ToInt32(assetData, 16)
                };

                if (asset.NameLocation >= poolInfo.StartLocation && asset.NameLocation <= poolInfo.MaxEndLocation)
                    continue;

                asset.Path              = MemoryUtil.ReadNullTerminatedString(T7Util.ActiveProcess, asset.NameLocation);
                asset.DisplayName = Path.GetFileName(asset.Path);
                asset.AssetType         = poolInfo.PoolName;
                asset.Info              = String.Format("Size - {0:0.00}KB", asset.Size / 1024.0);
                asset.ExportFunction    = ExportFromMemory;
                assetList.Add(asset);
            }

            return assetList;
        }

        /// <summary>
        /// Exports an Entity Map from Black Ops III's Memory
        /// </summary>
        /// <param name="asset">Asset to export</param>
        /// <returns>True if we succeeded, False if we failed</returns>
        public static bool ExportFromMemory(Asset asset)
        {
            string assetPath = asset.Path;
            PathUtil.CreateFilePath("exported_files\\" + assetPath);
            File.WriteAllBytes("exported_files\\" + assetPath, MemoryUtil.ReadBytes
                (
                    T7Util.ActiveProcess,
                    asset.StartLocation,
                    asset.Size
                ));
            return true;
        }


    }
}
