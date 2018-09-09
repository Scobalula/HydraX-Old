/*
 *  HydraX - Copyright 2018 Philip/Scobalula
 *  
 *  This file is subject to the license terms set out in the
 *  "LICENSE.txt" file. 
 * 
 */
using System;

namespace HydraLib
{
    /// <summary>
    /// Hydra Exception
    /// </summary>
    public class HydraException : Exception
    {
        /// <summary>
        /// Initializes a new instance of Hydra Exception.
        /// </summary>
        public HydraException()
        {
        }

        /// <summary>
        /// Initializes a new instance of Hydra Exception with an error message.
        /// </summary>
        public HydraException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Initializes a new instance of Hydra Exception with an error message and reference to the inner exception.
        /// </summary>
        public HydraException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    /// <summary>
    /// Holds Asset Data
    /// </summary>
    public class Asset
    {
        /// <summary>
        /// UI Display Name
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Asset Path/Name
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Asset Type
        /// </summary>
        public string AssetType { get; set; }

        /// <summary>
        /// UI Display Info
        /// </summary>
        public string Info { get; set; }

        /// <summary>
        /// Name Location
        /// </summary>
        public long NameLocation { get; set; }

        /// <summary>
        /// Data start location
        /// </summary>
        public long StartLocation { get; set; }

        /// <summary>
        /// Data end location
        /// </summary>
        public long EndLocation { get; set; }

        /// <summary>
        /// Asset Size in bytes
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// Asset's export function
        /// </summary>
        public Func<Asset, bool> ExportFunction { get; set; }

        /// <summary>
        /// Asset Data (The Export Function should handle casting this)
        /// </summary>
        public object Data { get; set; }
    }

    /// <summary>
    /// Holds Asset Pool Information including counts, start locations, etc.
    /// </summary>
    public struct AssetPoolInformation
    {
        /// <summary>
        /// Asset Pool Name
        /// </summary>
        public string PoolName { get; set; }

        /// <summary>
        /// First Asset Location
        /// </summary>
        public long StartLocation { get; set; }

        /// <summary>
        /// Last Asset Location
        /// </summary>
        public long EndLocation { get; set; }

        /// <summary>
        /// Last Asset Location if this Pool was full
        /// </summary>
        public long MaxEndLocation { get; set; }

        /// <summary>
        /// Asset Entry Size
        /// </summary>
        public int EntrySize { get; set; }

        /// <summary>
        /// Number of Assets in this Pool
        /// </summary>
        public int AssetCount { get; set; }

        /// <summary>
        /// Max Number of Assets 
        /// </summary>
        public int MaxAssetCount { get; set; }
    }

    /// <summary>
    /// Common Utility Functions
    /// </summary>
    public class Util
    {
        /// <summary>
        /// Loads 32 Byte Asset Pool Information buffer into a struct
        /// </summary>
        /// <param name="buffer">32 Byte Buffer with Asset Pool Data</param>
        /// <param name="poolName">Name of this Pool</param>
        /// <returns>AssetPoolInformation struct</returns>
        public static AssetPoolInformation LoadAssetPoolInfo(byte[] buffer, string poolName)
        {
            int assetEntrySize    = BitConverter.ToInt32(buffer, 8);
            int assetPoolMaxCount = BitConverter.ToInt32(buffer, 12);
            long firstEntryPtr    = BitConverter.ToInt64(buffer, 0);
            long lastEntryPtr     = BitConverter.ToInt64(buffer, 24);
            long lastEntryMax     = firstEntryPtr + (assetEntrySize * assetPoolMaxCount);
            int assetPoolCount    = BitConverter.ToInt32(buffer, 20);

            return new AssetPoolInformation()
            {
                PoolName       = poolName,
                StartLocation  = firstEntryPtr,
                EndLocation    = lastEntryPtr,
                MaxEndLocation = lastEntryMax,
                EntrySize      = assetEntrySize,
                AssetCount     = assetPoolCount,
                MaxAssetCount  = assetPoolMaxCount
            };
        }

        /// <summary>
        /// Checks if an asset is null by checking if the Name pointer points to another asset within the pool (If it's null it may point to the next value asset)
        /// </summary>
        /// <param name="nameAddress">Memory Address of the asset's Name</param>
        /// <param name="poolInfo">Asset Pool Information</param>
        /// <returns>True if the asset is considered "null"</returns>
        public static bool IsNullAsset(long nameAddress, AssetPoolInformation poolInfo)
        {
            return nameAddress >= poolInfo.StartLocation && nameAddress <= poolInfo.MaxEndLocation;
        }

        /// <summary>
        /// Checks if an asset is null by checking if the Name pointer points to another asset within the pool (If it's null it may point to the next value asset)
        /// </summary>
        /// <param name="nameAddress">Asset</param>
        /// <param name="poolInfo">Asset Pool Information</param>
        /// <returns>True if the asset is considered "null"</returns>
        public static bool IsNullAsset(Asset asset, AssetPoolInformation poolInfo)
        {
            return asset.NameLocation >= poolInfo.StartLocation && asset.NameLocation <= poolInfo.MaxEndLocation;
        }
    }
}
