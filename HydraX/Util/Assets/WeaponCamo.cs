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
using System.Linq;
using PhilUtil;
using Newtonsoft.Json;

namespace HydraLib.T7.Assets
{
    /// <summary>
    /// Weapon Camo Table Logic
    /// </summary>
    public class WeaponCamo
    {
        /// <summary>
        /// Holds a Camo Materials
        /// </summary>
        public class CamoMaterial
        {
            /// <summary>
            /// Camo's Material Name
            /// </summary>
            public string Material { get; set; }

            /// <summary>
            /// Detail Normal Map for this Camo
            /// </summary>
            public string DetailNormalMap { get; set; }

            /// <summary>
            /// X Axis Translation
            /// </summary>
            [JsonIgnore]
            public double TranslationX { get; set; }

            /// <summary>
            /// Y Axis Translation
            /// </summary>
            [JsonIgnore]
            public double TranslationY { get; set; }

            /// <summary>
            /// X Axis Scale
            /// </summary>
            [JsonIgnore]
            public double ScaleX { get; set; }

            /// <summary>
            /// Y Axis Scale
            /// </summary>
            [JsonIgnore]
            public double ScaleY { get; set; }

            /// <summary>
            /// Rotation
            /// </summary>
            [JsonIgnore]
            public double Rotation { get; set; }

            /// <summary>
            /// Normal Map Blend Amount
            /// </summary>
            public double NormalMapBlend { get; set; }

            /// <summary>
            /// Gloss Map Blend Amount
            /// </summary>
            public double GlossMapBlend { get; set; }

            /// <summary>
            /// Detail Normal Map X Axis Scale
            /// </summary>
            public double NormalScaleX { get; set; }

            /// <summary>
            /// Detail Normal Map Y Axis Scale
            /// </summary>
            public double NormalScaleY { get; set; }

            /// <summary>
            /// Detail Normal Map Height
            /// </summary>
            public double NormalHeight { get; set; }

            /// <summary>
            /// Use Normal Map on this Camo
            /// </summary>
            public int UseNormalMap = 1;

            /// <summary>
            /// Use Gloss Map on this Camo
            /// </summary>
            public int UseGlossMap = 1;

            /// <summary>
            /// Base Material Count
            /// </summary>
            [JsonIgnore]
            public int BaseMaterialCount = 0;

            /// <summary>
            /// Base Materials
            /// </summary>
            [JsonIgnore]
            public string[] BaseMaterials = new string[10];

            /// <summary>
            /// Camo Masks
            /// </summary>
            [JsonIgnore]
            public string[] CamoMasks = new string[10];
        }

        /// <summary>
        /// Holds a Camo Entry
        /// </summary>
        public class CamoEntry
        {
            /// <summary>
            /// Memory Location
            /// </summary>
            [JsonIgnore]
            public long Location { get; set; }

            /// <summary>
            /// Number of Materials
            /// </summary>
            [JsonIgnore]
            public int MaterialCount { get; set; }

            /// <summary>
            /// Materials
            /// </summary>
            public List<CamoMaterial> CamoMaterials = new List<CamoMaterial>();
        }

        /// <summary>
        /// Weapon Camos
        /// </summary>
        public CamoEntry[] Camos { get; set; }

        /// <summary>
        /// Loads Weapon Camo Assets from Memory
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
                    NameLocation = BitConverter.ToInt64(assetData, 0)
                };

                if (Util.IsNullAsset(asset.NameLocation, poolInfo))
                    continue;

                asset.AssetType = poolInfo.PoolName;
                asset.StartLocation = BitConverter.ToInt64(assetData, 8);
                asset.Path = MemoryUtil.ReadNullTerminatedString(T7Util.ActiveProcess, asset.NameLocation);
                asset.DisplayName = Path.GetFileName(asset.Path);
                asset.ExportFunction = ExportFromMemory;

                WeaponCamo weaponCamo = new WeaponCamo()
                {
                    Camos = new CamoEntry[BitConverter.ToInt32(assetData, 16)]
                };

                asset.Data = weaponCamo;

                asset.Info = String.Format("Camos - {0}", weaponCamo.Camos.Length);

                assetList.Add(asset);

            }

            return assetList;
        }

        /// <summary>
        /// Exports a Weapon Camo Asset from Memory
        /// </summary>
        public static bool ExportFromMemory(Asset asset)
        {
            WeaponCamo weaponCamo = (WeaponCamo)asset.Data;

            string path = "exported_files\\source_data\\camo_gdts\\" + asset.Path + "_gdt.gdt";

            // Current Address Tracker
            long addressTracker = asset.StartLocation;
            // Buffer
            byte[] buffer;
            // Material/CamoMask Variables
            string baseMaterial;
            string camoMask;
            // Memory Address Varialbes
            long baseMaterialAddress;
            long camoMaskAddress;
            long materialAddress;
            long namesAddress;
            long detailNormalAddress;

            buffer = MemoryUtil.ReadBytes(T7Util.ActiveProcess, addressTracker, weaponCamo.Camos.Length * 16);

            // These blocks contain the location of each entry, and how many materials it has (should only be 0 - 2)
            for (int i = 0; i < weaponCamo.Camos.Length; i++)
            {
                weaponCamo.Camos[i] = new CamoEntry
                {
                    Location      = BitConverter.ToInt64(buffer, 16 * i + 8),
                    MaterialCount = BitConverter.ToInt32(buffer, 16 * i)
                };
            }

            for (int i = 0, j = 0; i < weaponCamo.Camos.Length; i++, j = 0)
            {
                do
                {
                    weaponCamo.Camos[i].CamoMaterials.Add(new CamoMaterial());

                    // Only process if we have mtls, but we must add the entry to maintain indexes even if 0 mtls
                    if (weaponCamo.Camos[i].MaterialCount > 0)
                    {
                        buffer = MemoryUtil.ReadBytes(T7Util.ActiveProcess, weaponCamo.Camos[i].Location + (312 * j), 312);

                        namesAddress        = BitConverter.ToInt64(buffer, 8);
                        materialAddress     = BitConverter.ToInt64(buffer, 32);
                        detailNormalAddress = BitConverter.ToInt64(buffer, 72);

                        weaponCamo.Camos[i].CamoMaterials[j].BaseMaterialCount = BitConverter.ToInt32(buffer, 0);
                        weaponCamo.Camos[i].CamoMaterials[j].UseNormalMap      = ByteUtil.GetBitAsInt(buffer[24], 1);
                        weaponCamo.Camos[i].CamoMaterials[j].UseGlossMap       = ByteUtil.GetBitAsInt(buffer[24], 1);
                        weaponCamo.Camos[i].CamoMaterials[j].Material          = materialAddress > 0 ? T7Util.GetAssetName(materialAddress)?.Split('/').Last() : "";
                        weaponCamo.Camos[i].CamoMaterials[j].TranslationX      = Math.Round(BitConverter.ToSingle(buffer, 40), 2);
                        weaponCamo.Camos[i].CamoMaterials[j].TranslationY      = Math.Round(BitConverter.ToSingle(buffer, 44), 2);
                        weaponCamo.Camos[i].CamoMaterials[j].ScaleX            = Math.Round(BitConverter.ToSingle(buffer, 48), 2);
                        weaponCamo.Camos[i].CamoMaterials[j].ScaleY            = Math.Round(BitConverter.ToSingle(buffer, 52), 2);
                        weaponCamo.Camos[i].CamoMaterials[j].Rotation          = Math.Round(BitConverter.ToSingle(buffer, 56), 2);
                        weaponCamo.Camos[i].CamoMaterials[j].GlossMapBlend     = Math.Round(BitConverter.ToSingle(buffer, 60), 2);
                        weaponCamo.Camos[i].CamoMaterials[j].NormalMapBlend    = Math.Round(BitConverter.ToSingle(buffer, 60), 2);
                        weaponCamo.Camos[i].CamoMaterials[j].DetailNormalMap   = detailNormalAddress > 0 ? T7Util.GetImageName(detailNormalAddress) : "";
                        weaponCamo.Camos[i].CamoMaterials[j].NormalHeight      = Math.Round(BitConverter.ToSingle(buffer, 80), 2);
                        weaponCamo.Camos[i].CamoMaterials[j].NormalScaleX      = Math.Round(BitConverter.ToSingle(buffer, 84), 2);
                        weaponCamo.Camos[i].CamoMaterials[j].NormalScaleY      = Math.Round(BitConverter.ToSingle(buffer, 88), 2);

                        for (int q = 0; q < weaponCamo.Camos[i].CamoMaterials[j].BaseMaterialCount && weaponCamo.Camos[i].CamoMaterials[j].BaseMaterialCount < 10 && namesAddress > 0; q++)
                        {
                            baseMaterialAddress                                   = MemoryUtil.ReadInt64(T7Util.ActiveProcess, namesAddress + (q * 16));
                            camoMaskAddress                                       = MemoryUtil.ReadInt64(T7Util.ActiveProcess, namesAddress + (q * 16 + 8));
                            baseMaterial                                          = baseMaterialAddress > 0 ? T7Util.GetAssetName(baseMaterialAddress) : "";
                            camoMask                                              = camoMaskAddress > 0 ? T7Util.GetImageName(camoMaskAddress) : "";
                            weaponCamo.Camos[i].CamoMaterials[j].BaseMaterials[q] = baseMaterial?.Split('/').Last();
                            weaponCamo.Camos[i].CamoMaterials[j].CamoMasks[q]     = camoMask?.Split('/').Last();
                        }
                    }

                    j++;

                } while (j < weaponCamo.Camos[i].MaterialCount);

            }

            SaveWeaponCamo(path, asset.Path, weaponCamo);

            return true;
        }

        /// <summary>
        /// Dumps Camos to a Json file for use in CamoChairs. For Best results use smg_standard's.
        /// </summary>
        public void Save(string path)
        {
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
        /// <summary>
        /// Writes Weapon Camo Asset to a GDT
        /// </summary>
        public static void SaveWeaponCamo(string path, string assetName, WeaponCamo weaponCamo)
        {
            PathUtil.CreateFilePath(path);

            string[] weaponCamos = new string[10];

            using (StreamWriter streamWriter = new StreamWriter(path))
            {
                streamWriter.WriteLine("{");

                int weaponCamoIndex = 1;
                int weaponCamoAssetIndex = 0;

                // Dump all entries to a GDT
                for(int i = 0; i < weaponCamo.Camos.Length; i++, weaponCamoIndex++)
                {
                    // APE Weapon Camo Assets can only hold 76 entries by default (defined in the AWI), compiled they're all together
                    if(i % 75 == 0)
                    {
                        weaponCamos[weaponCamoAssetIndex] = String.Format("{0}_ship", assetName);

                        if (i != 0)
                        {
                            streamWriter.WriteLine("		\"numCamos\" \"{0}\"", weaponCamoIndex - 1);
                            // This name actually seems to matter to linker, if it isn't i.e. base76 it no likey
                            weaponCamos[weaponCamoAssetIndex] = String.Format("{0}_base{1}", assetName, i + 1);
                            streamWriter.WriteLine("	}");
                        }

                        streamWriter.WriteLine("	\"{0}\" ( \"weaponcamo.gdf\" )", weaponCamos[weaponCamoAssetIndex]);
                        streamWriter.WriteLine("	{");
                        streamWriter.WriteLine("		\"baseIndex\" \"{0}\"", i + 1);
                        streamWriter.WriteLine("		\"configstringFileType\" \"WEAPONCAMO\"");

                        weaponCamoAssetIndex++;
                        weaponCamoIndex = 1;
                    }

                    for (int j = 0; j < weaponCamo.Camos[i].CamoMaterials.Count; j++)
                    {

                        for(int q = 0; q < weaponCamo.Camos[i].CamoMaterials[j].BaseMaterials.Length; q++)
                        {
                            streamWriter.WriteLine("		\"material{0}_{1}_base_material_{2}\" \"{3}\"", j + 1, weaponCamoIndex, q + 1, weaponCamo.Camos[i].CamoMaterials[j].BaseMaterials[q]);
                        }

                        for (int q = 0; q < weaponCamo.Camos[i].CamoMaterials[j].BaseMaterials.Length; q++)
                        {
                            streamWriter.WriteLine("		\"material{0}_{1}_camo_mask_{2}\" \"{3}\"", j + 1, weaponCamoIndex, q + 1, weaponCamo.Camos[i].CamoMaterials[j].CamoMasks[q]);
                        }

                        streamWriter.WriteLine("		\"material{0}_{1}_detail_normal_height\" \"{2}\"", j + 1, weaponCamoIndex, weaponCamo.Camos[i].CamoMaterials[j].NormalHeight);
                        streamWriter.WriteLine("		\"material{0}_{1}_detail_normal_map\" \"{2}\"", j + 1, weaponCamoIndex, weaponCamo.Camos[i].CamoMaterials[j].DetailNormalMap);
                        streamWriter.WriteLine("		\"material{0}_{1}_detail_normal_scale_x\" \"{2}\"", j + 1, weaponCamoIndex, weaponCamo.Camos[i].CamoMaterials[j].NormalScaleX);
                        streamWriter.WriteLine("		\"material{0}_{1}_detail_normal_scale_y\" \"{2}\"", j + 1, weaponCamoIndex, weaponCamo.Camos[i].CamoMaterials[j].NormalScaleY);
                        streamWriter.WriteLine("		\"material{0}_{1}_gloss_blend\" \"{2}\"", j + 1, weaponCamoIndex, weaponCamo.Camos[i].CamoMaterials[j].GlossMapBlend);
                        streamWriter.WriteLine("		\"material{0}_{1}_material\" \"{2}\"", j + 1, weaponCamoIndex, weaponCamo.Camos[i].CamoMaterials[j].Material);
                        streamWriter.WriteLine("		\"material{0}_{1}_normal_amount\" \"{2}\"", j + 1, weaponCamoIndex, weaponCamo.Camos[i].CamoMaterials[j].NormalMapBlend);
                        streamWriter.WriteLine("		\"material{0}_{1}_numBaseMaterials\" \"{2}\"", j + 1, weaponCamoIndex, weaponCamo.Camos[i].CamoMaterials[j].BaseMaterialCount);
                        streamWriter.WriteLine("		\"material{0}_{1}_rotation\" \"{2}\"", j + 1, weaponCamoIndex, weaponCamo.Camos[i].CamoMaterials[j].Rotation);
                        streamWriter.WriteLine("		\"material{0}_{1}_scale_x\" \"{2}\"", j + 1, weaponCamoIndex, weaponCamo.Camos[i].CamoMaterials[j].ScaleX);
                        streamWriter.WriteLine("		\"material{0}_{1}_scale_y\" \"{2}\"", j + 1, weaponCamoIndex, weaponCamo.Camos[i].CamoMaterials[j].ScaleY);
                        streamWriter.WriteLine("		\"material{0}_{1}_trans_x\" \"{2}\"", j + 1, weaponCamoIndex, weaponCamo.Camos[i].CamoMaterials[j].TranslationX);
                        streamWriter.WriteLine("		\"material{0}_{1}_trans_y\" \"{2}\"", j + 1, weaponCamoIndex, weaponCamo.Camos[i].CamoMaterials[j].TranslationY);
                        streamWriter.WriteLine("		\"material{0}_{1}_useGlossMap\" \"{2}\"", j + 1, weaponCamoIndex, weaponCamo.Camos[i].CamoMaterials[j].UseGlossMap);
                        streamWriter.WriteLine("		\"material{0}_{1}_useNormalMap\" \"{2}\"", j + 1, weaponCamoIndex, weaponCamo.Camos[i].CamoMaterials[j].UseNormalMap);

                    }
                }
                streamWriter.WriteLine("		\"numCamos\" \"{0}\"", weaponCamoIndex - 1);
                streamWriter.WriteLine("	}");

                // Write actually table
                streamWriter.WriteLine("	\"{0}\" ( \"weaponcamotable.gdf\" )", assetName);
                streamWriter.WriteLine("	{");
                streamWriter.WriteLine("		\"configstringFileType\" \"WEAPONCAMO\"");
                streamWriter.WriteLine("		\"numCamoTables\" \"{0}\"", weaponCamos.Count(x => x != null));
                for(int i = 0; i < weaponCamos.Length; i++)
                {
                    streamWriter.WriteLine("		\"table_{0:D2}_name\" \"{1}\"", (i + 1), weaponCamos[i]);
                }
                streamWriter.WriteLine("	}");
                streamWriter.WriteLine("}");
                streamWriter.WriteLine();
            }
        }
    }
}
