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
using PhilUtil.MathUtil;

namespace HydraLib.T7.Assets
{
    /// <summary>
    /// PhysPreset Logic
    /// </summary>
    class PhysPreset
    {
        /// <summary>
        /// If this entity can float
        /// </summary>
        public int CanFloat { get; set; }

        /// <summary>
        /// Object Mass in Pounds
        /// </summary>
        public double Mass { get; set; }

        /// <summary>
        /// How much this object will bounce
        /// </summary>
        public double Bounce { get; set; }

        /// <summary>
        /// Object Friction
        /// </summary>
        public double Friction { get; set; }

        /// <summary>
        /// Linear Damping Scale
        /// </summary>
        public double LinearDamping { get; set; }

        /// <summary>
        /// Angular Damping Scale
        /// </summary>
        public double AngularDamping { get; set; }

        /// <summary>
        /// Bullet Force Scale
        /// </summary>
        public double BulletForceScale { get; set; }

        /// <summary>
        /// Explisve Force Scale
        /// </summary>
        public double ExplosiveForceScale { get; set; }

        /// <summary>
        /// Graity Scale
        /// </summary>
        public double GravityScale { get; set; }

        /// <summary>
        /// Object Impact Effects
        /// </summary>
        public string ImpactFXTable { get; set; }

        /// <summary>
        /// Object Impact Sounds
        /// </summary>
        public string ImpactSoundTable { get; set; }

        /// <summary>
        /// Trail FX Played on Object
        /// </summary>
        public string TrailFX { get; set; }

        /// <summary>
        /// Mass Offset
        /// </summary>
        public Vector3 MassOffset { get; set; }

        /// <summary>
        /// Buoyancy Force Min
        /// </summary>
        public Vector3 BuoyancyMin { get; set; }

        /// <summary>
        /// Buoyancy Force Max
        /// </summary>
        public Vector3 BuoyancyMax { get; set; }

        /// <summary>
        /// Saves Physics Preset to a GDT file.
        /// </summary>
        public void Save(string name)
        {
            string path = "exported_files\\source_data\\physpreset_gdts\\" + name + "_gdt.gdt";

            PathUtil.CreateFilePath(path);

            using (StreamWriter streamWriter = new StreamWriter(path))
            {
                streamWriter.WriteLine("{");
                streamWriter.WriteLine("	\"{0}\" ( \"physpreset.gdf\" )", name);
                streamWriter.WriteLine("	{");
                streamWriter.WriteLine("		\"configstringFileType\" \"PHYSIC\"");
                streamWriter.WriteLine("		\"bounce\" \"{0:0.000}\"", Bounce);
                streamWriter.WriteLine("		\"bulletForceScale\" \"{0:0.000}\"", BulletForceScale);
                streamWriter.WriteLine("		\"explosiveForceScale\" \"{0:0.000}\"", ExplosiveForceScale);
                streamWriter.WriteLine("		\"buoyancyMinX\" \"{0:0.000}\"", BuoyancyMin.X);
                streamWriter.WriteLine("		\"buoyancyMinY\" \"{0:0.000}\"", BuoyancyMin.Y);
                streamWriter.WriteLine("		\"buoyancyMinZ\" \"{0:0.000}\"", BuoyancyMin.Z);
                streamWriter.WriteLine("		\"buoyancyMaxX\" \"{0:0.000}\"", BuoyancyMax.X);
                streamWriter.WriteLine("		\"buoyancyMaxY\" \"{0:0.000}\"", BuoyancyMax.Y);
                streamWriter.WriteLine("		\"buoyancyMaxZ\" \"{0:0.000}\"", BuoyancyMax.Z);
                streamWriter.WriteLine("		\"canFloat\" \"{0}\"", CanFloat);
                streamWriter.WriteLine("		\"damping_angular\" \"{0:0.000}\"", AngularDamping);
                streamWriter.WriteLine("		\"damping_linear\" \"{0:0.000}\"", LinearDamping);
                streamWriter.WriteLine("		\"friction\" \"{0:0.000}\"", Friction);
                streamWriter.WriteLine("		\"gravityScale\" \"{0:0.000}\"", GravityScale);
                streamWriter.WriteLine("		\"mass\" \"{0:0.000}\"", Mass);
                streamWriter.WriteLine("		\"massOffsetX\" \"{0:0.000}\"", MassOffset.X);
                streamWriter.WriteLine("		\"massOffsetY\" \"{0:0.000}\"", MassOffset.Y);
                streamWriter.WriteLine("		\"massOffsetZ\" \"{0:0.000}\"", MassOffset.Z);
                streamWriter.WriteLine("		\"impactsFxTable\" \"{0}\"", ImpactFXTable);
                streamWriter.WriteLine("		\"impactsSoundsTable\" \"{0}\"", ImpactSoundTable);
                streamWriter.WriteLine("		\"trailFX\" \"{0}\"", TrailFX);
                streamWriter.WriteLine("	}");
                streamWriter.WriteLine("}");
                streamWriter.WriteLine();
            }
        }

        /// <summary>
        /// Loads Physics Presets from Memory
        /// </summary>
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

                long fxTrailPtr             = BitConverter.ToInt64(assetData, 96);
                long impactfxTablePtr       = BitConverter.ToInt64(assetData, 104);
                long impactSoundsTablePtr   = BitConverter.ToInt64(assetData, 112);


                PhysPreset physPreset = new PhysPreset
                {
                    Mass                = BitConverter.ToSingle(assetData, 12) * 1000.0,
                    Bounce              = BitConverter.ToSingle(assetData, 16),
                    Friction            = BitConverter.ToSingle(assetData, 20),
                    LinearDamping       = BitConverter.ToSingle(assetData, 24),
                    AngularDamping      = BitConverter.ToSingle(assetData, 28),
                    BulletForceScale    = BitConverter.ToSingle(assetData, 32),
                    ExplosiveForceScale = BitConverter.ToSingle(assetData, 36),
                    CanFloat            = BitConverter.ToInt32(assetData, 48),
                    GravityScale        = BitConverter.ToSingle(assetData, 52),
                    ImpactFXTable       = impactfxTablePtr > 0 ? T7Util.GetAssetName(impactfxTablePtr) : "",
                    ImpactSoundTable    = impactSoundsTablePtr > 0 ? T7Util.GetAssetName(impactSoundsTablePtr) : "",
                    TrailFX             = fxTrailPtr > 0 ? T7Util.GetAssetName(fxTrailPtr) : "",
                    MassOffset          = new Vector3
                    (   
                        BitConverter.ToSingle(assetData, 56), 
                        BitConverter.ToSingle(assetData, 60), 
                        BitConverter.ToSingle(assetData, 64)
                    ),
                    BuoyancyMin         = new Vector3
                    (
                        BitConverter.ToSingle(assetData, 68), 
                        BitConverter.ToSingle(assetData, 72), 
                        BitConverter.ToSingle(assetData, 76)
                    ),
                    BuoyancyMax         = new Vector3
                    (
                        BitConverter.ToSingle(assetData, 80), 
                        BitConverter.ToSingle(assetData, 84),
                        BitConverter.ToSingle(assetData, 88)
                    )
                };


                asset.AssetType      = poolInfo.PoolName;
                asset.StartLocation  = BitConverter.ToInt64(assetData, 16);
                asset.Size           = BitConverter.ToInt32(assetData, 8);
                asset.Path           = MemoryUtil.ReadNullTerminatedString(T7Util.ActiveProcess, asset.NameLocation);
                asset.DisplayName    = Path.GetFileName(asset.Path);
                asset.Data           = physPreset;
                asset.ExportFunction = ExportFromMemory;

                asset.Info = String.Format("Mass - {0:0.00}", physPreset.Mass);

                assetList.Add(asset);

            }

            return assetList;
        }

        /// <summary>
        /// Exports Physic Presets from Memory
        /// </summary>
        public static bool ExportFromMemory(Asset asset)
        {
            PhysPreset physPreset = (PhysPreset)asset.Data;

            physPreset.Save(asset.Path);


            return true;
        }
    }
}
