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
using HydraLib.GDT;
using PhilUtil;

namespace HydraLib.T7.Assets
{
    /// <summary>
    /// Rumble Logic
    /// </summary>
    class Rumble
    {
        /// <summary>
        /// High Rumble File Address
        /// </summary>
        public long HighRumblePointer { get; set; }

        /// <summary>
        /// Low Rumble File Address
        /// </summary>
        public long LowRumblePointer { get; set; }

        /// <summary>
        /// High Rumble File Name
        /// </summary>
        public string HighRumble { get; set; }

        /// <summary>
        /// Low Rumble File Name
        /// </summary>
        public string LowRumble { get; set; }

        /// <summary>
        /// Rumble Duration
        /// </summary>
        public float Duration { get; set; }

        /// <summary>
        /// Rumble Range
        /// </summary>
        public float Range { get; set; }

        /// <summary>
        /// Rumble Camera Shake Range
        /// </summary>
        public float CamShakeRange { get; set; }

        /// <summary>
        /// Rumble Camera Shake Scale
        /// </summary>
        public float CamShakeScale { get; set; }

        /// <summary>
        /// Rumble Camera Shake Duration
        /// </summary>
        public float CamShakeDuration { get; set; }

        /// <summary>
        /// Rumble Pulse Scale
        /// </summary>
        public float PulseScale { get; set; }

        /// <summary>
        /// Rumble Outer Pulse Radius
        /// </summary>
        public float PulseRadiusOuter { get; set; }

        /// <summary>
        /// Should this Rumble Fade With Distance
        /// </summary>
        public int FadeWithDistance { get; set; }

        /// <summary>
        /// Should this Rumble Broadcast
        /// </summary>
        public int Broadcast { get; set; }

        /// <summary>
        /// Loads Rumbles from Memory
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

                Rumble rumble = new Rumble()
                {
                    HighRumblePointer = BitConverter.ToInt64(assetData, 16),
                    LowRumblePointer  = BitConverter.ToInt64(assetData, 24),
                    Duration          = (float)(BitConverter.ToInt32(assetData, 8) / 1000.0),
                    Range             = BitConverter.ToSingle(assetData, 12),
                    FadeWithDistance  = BitConverter.ToInt32(assetData, 32),
                    Broadcast         = BitConverter.ToInt32(assetData, 36),
                    CamShakeRange     = BitConverter.ToSingle(assetData, 40),
                    CamShakeDuration  = (float)(BitConverter.ToInt32(assetData, 44) / 1000.0),
                    CamShakeScale     = BitConverter.ToSingle(assetData, 48),
                    PulseRadiusOuter  = BitConverter.ToSingle(assetData, 52),
                    PulseScale        = BitConverter.ToSingle(assetData, 56),
                };

                asset.AssetType      = poolInfo.PoolName;
                asset.Path           = MemoryUtil.ReadNullTerminatedString(T7Util.ActiveProcess, asset.NameLocation);
                asset.DisplayName    = Path.GetFileName(asset.Path);
                asset.ExportFunction = ExportFromMemory;
                asset.Info           = String.Format("Size - {0:0.00}KB", asset.Size / 1024.0);
                asset.Data           = rumble;

                assetList.Add(asset);


            }

            return assetList;
        }

        /// <summary>
        /// Exports a Rumble GDT from Memory
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        public static bool ExportFromMemory(Asset asset)
        {
            Rumble rumble = (Rumble)asset.Data;

            if (rumble.HighRumblePointer > 0)
                rumble.HighRumble = ExportRumbleFile(rumble.HighRumblePointer);

            if (rumble.LowRumblePointer > 0)
                rumble.LowRumble = ExportRumbleFile(rumble.LowRumblePointer);

            GDTUtil.WriteRumble(asset.Path, rumble);

            return true;
        }

        /// <summary>
        /// Exports a Rumble File from Memory
        /// </summary>
        public static string ExportRumbleFile(long location)
        {
            string rumbleName = MemoryUtil.ReadNullTerminatedString(T7Util.ActiveProcess, MemoryUtil.ReadInt64(T7Util.ActiveProcess, location));

            string assetPath = "exported_files\\rumble\\" + rumbleName;

            int numRumbleEntries = MemoryUtil.ReadInt32(T7Util.ActiveProcess, location + 8);

            PathUtil.CreateFilePath(assetPath);


            using (StreamWriter streamWriter = new StreamWriter(assetPath))
            {
                streamWriter.WriteLine("RUMBLEGRAPHFILE");
                streamWriter.WriteLine();
                streamWriter.WriteLine(numRumbleEntries.ToString());

                using (BinaryReader binaryReader = new BinaryReader(new MemoryStream(MemoryUtil.ReadBytes(T7Util.ActiveProcess, location + 12, 8 * numRumbleEntries))))
                {
                    for (int i = 0; i < numRumbleEntries; i++)
                    {
                        float float1 = binaryReader.ReadSingle();
                        float float2 = binaryReader.ReadSingle();
                        streamWriter.WriteLine("{0:0.0000} {1:0.0000}", float1, float2);
                    }
                }
            }

            return rumbleName;
        }
    }
}
