/*
 *  HydraX - Copyright 2018 Philip/Scobalula
 *  
 *  This file is subject to the license terms set out in the
 *  "LICENSE.txt" file. 
 * 
 */
using System;
using System.Collections.Generic;
using System.IO;
using PhilUtil;
using HydraLib.T7.Assets.JsonFiles;
using Newtonsoft.Json;

namespace HydraLib.T7.Assets.JsonFiles
{
    /// <summary>
    /// Structured Table Class
    /// </summary>
    public class StructuredTable
    {
        /// <summary>
        /// Meta Data (Not used by Linker)
        /// </summary>
        [JsonProperty(PropertyName="_meta")]
        public Dictionary<string, object> Meta = new Dictionary<string, object>();

        /// <summary>
        /// Structured Data
        /// </summary>
        [JsonProperty(PropertyName = "data")]
        public Dictionary<string, object>[] Data = new Dictionary<string, object>[0];

        /// <summary>
        /// Total Data Count from all Entries in this table
        /// </summary>
        [JsonIgnore]
        public int DataCount { get; set; }

        /// <summary>
        /// Number of properties in this table
        /// </summary>
        [JsonIgnore]
        public int PropertyCount { get; set; }

        /// <summary>
        /// Number of entries in this table
        /// </summary>
        [JsonIgnore]
        public int EntryCount { get; set; }

        /// <summary>
        /// Memory Address of Properties
        /// </summary>
        [JsonIgnore]
        public long PropertiesLocation { get; set; }

        /// <summary>
        /// Saves Structured Table to a JSON file
        /// </summary>
        /// <param name="path">File Path</param>
        public void Save(string path)
        {
            PathUtil.CreateFilePath(path);

            using (JsonTextWriter output = new JsonTextWriter(new StreamWriter(path)))
            {
                output.Formatting = Formatting.Indented;
                output.Indentation = 4;
                output.IndentChar = ' ';

                JsonSerializer serializer = new JsonSerializer
                {
                    NullValueHandling = NullValueHandling.Ignore,
                };

                serializer.Serialize(output, this);
            }
        }
    }
}

namespace HydraLib.T7.Assets
{
    /// <summary>
    /// Structured Table Logic
    /// </summary>
    class StructuredTableUtil
    {
        /// <summary>
        /// Loads Structured Tables from Memory
        /// </summary>
        /// <param name="poolInfo">Asset Pool Information</param>
        /// <returns>List of Assets Found</returns>
        public static List<Asset> LoadFromMemory(AssetPoolInformation poolInfo)
        {
            List<Asset> assetList = new List<Asset>();

            for (int i = 0; assetList.Count < poolInfo.AssetCount; i++)
            {
                byte[] assetData = MemoryUtil.ReadBytes(T7Util.ActiveProcess, poolInfo.StartLocation + (poolInfo.EntrySize * i), poolInfo.EntrySize);

                Asset asset = new Asset
                {
                    NameLocation = BitConverter.ToInt64(assetData, 0),
                    AssetType = poolInfo.PoolName,
                    StartLocation = BitConverter.ToInt64(assetData, 24)
                };

                if (Util.IsNullAsset(asset, poolInfo))
                    continue;

                StructuredTable structuredData = new StructuredTable()
                {
                    DataCount = BitConverter.ToInt32(assetData, 8),
                    PropertyCount = BitConverter.ToInt32(assetData, 12),
                    EntryCount = BitConverter.ToInt32(assetData, 16),
                    PropertiesLocation = BitConverter.ToInt64(assetData, 40)
                };

                structuredData.Data = new Dictionary<string, object>[structuredData.EntryCount];

                asset.Path = MemoryUtil.ReadNullTerminatedString(T7Util.ActiveProcess, asset.NameLocation);
                asset.DisplayName = Path.GetFileName(asset.Path);
                asset.Data = structuredData;
                asset.ExportFunction = ExportFromMemory;

                asset.Info = String.Format("Entries - {0} Properties - {1}", structuredData.EntryCount, structuredData.PropertyCount);

                assetList.Add(asset);
            }

            return assetList;
        }

        /// <summary>
        /// Exports Structured Tables from Memory
        /// </summary>
        public static bool ExportFromMemory(Asset asset)
        {
            StructuredTable structuredData = (StructuredTable)asset.Data;

            // Each entry is 24 bytes, so the table size in bytes will be equal to total data count * 24
            byte[] buffer = MemoryUtil.ReadBytes(T7Util.ActiveProcess, asset.StartLocation, structuredData.DataCount * 24);

            // Properties, each entry will contain the same number of properties, just 0 if wasn't used
            string[] properties = LoadProperties(structuredData.PropertiesLocation, structuredData.PropertyCount);

            structuredData.Meta.Add("Exported via", "HydraX by Scobalula");

            int k = 0;

            for(int i = 0; i < structuredData.EntryCount; i++)
            {
                structuredData.Data[i] = new Dictionary<string, object>();


                for(int j = 0; j < structuredData.PropertyCount; j++, k++)
                {
                    int dataType = BitConverter.ToInt32(buffer, k * 24);

                    switch(dataType)
                    {
                        // Strings
                        case 1:
                            structuredData.Data[i][properties[j]] = MemoryUtil.ReadNullTerminatedString(T7Util.ActiveProcess, BitConverter.ToInt64(buffer, 8 + k * 24));
                            break;
                        // Integers
                        case 2:
                            structuredData.Data[i][properties[j]] = BitConverter.ToInt32(buffer, 16 + k * 24);
                            break;
                        default:
                            break;
                    }
                }
            }

            structuredData.Save("exported_files\\" + asset.Path);

            return true;
        }

        /// <summary>
        /// Loads Properties used by a Structured Table
        /// </summary>
        public static string[] LoadProperties(long address, int numProperies)
        {
            byte[] buffer = MemoryUtil.ReadBytes(T7Util.ActiveProcess, address, 16 * numProperies);

            string[] properties = new string[numProperies];

            for(int i = 0; i < numProperies; i++)
                properties[i] = MemoryUtil.ReadNullTerminatedString(T7Util.ActiveProcess, BitConverter.ToInt64(buffer, i * 16));

            return properties;
        }
    }
}
