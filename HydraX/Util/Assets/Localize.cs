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
using System.Text;
using PhilUtil;

namespace HydraLib.T7.Assets
{
    /// <summary>
    /// Localized String Logic
    /// </summary>
    class Localize
    {
        /// <summary>
        /// Reference Strings (WEAPON_NAME, etc.)
        /// </summary>
        public List<long> References = new List<long>();

        /// <summary>
        /// Localized Strings
        /// </summary>
        public List<long> LocalizedStrings = new List<long>();

        /// <summary>
        /// Loads Localized Strings from Memory
        /// </summary>
        /// <param name="poolInfo">Asset Pool Data</param>
        /// <returns>Asset List</returns>
        public static List<Asset> LoadFromMemory(AssetPoolInformation poolInfo)
        {
            List<Asset> assetList = new List<Asset>();

            Asset asset = new Asset
            {
                Path = "localizedstrings.str",
            };

            Localize localize = new Localize();

            for (int i = 0; localize.References.Count < poolInfo.AssetCount; i++)
            {
                byte[] assetData = MemoryUtil.ReadBytes(T7Util.ActiveProcess, poolInfo.StartLocation + (poolInfo.EntrySize * i), poolInfo.EntrySize);

                long localizedPtr = BitConverter.ToInt64(assetData, 0);
                long referencePtr = BitConverter.ToInt64(assetData, 8);

                if(Util.IsNullAsset(localizedPtr, poolInfo))
                    continue;

                localize.References.Add(referencePtr);
                localize.LocalizedStrings.Add(localizedPtr);
            }
            

            asset.AssetType = poolInfo.PoolName;
            asset.Data = localize;
            asset.DisplayName = Path.GetFileName(asset.Path);
            asset.ExportFunction = ExportFromMemory;
            asset.Info = String.Format("Strings - {0}", poolInfo.AssetCount);

            assetList.Add(asset);

            return assetList;
        }

        /// <summary>
        /// Exports Localized Strings from Memory
        /// </summary>
        public static bool ExportFromMemory(Asset asset)
        {
            PathUtil.CreateFilePath("exported_files\\" + asset.Path);

            Localize localize = (Localize)asset.Data;

            StringBuilder output = new StringBuilder();

            output.AppendLine("VERSION				\"1\"");
            output.AppendLine("CONFIG				\"C:\\projects\\cod\\t7\\bin\\StringEd.cfg\"");
            output.AppendLine("FILENOTES		    \"Dumped via HydraX by Scobalula\"");

            output.AppendLine();

            for (int i = 0; i < localize.LocalizedStrings.Count; i++)
            {
                output.AppendLine(String.Format("REFERENCE            {0}", MemoryUtil.ReadNullTerminatedString(T7Util.ActiveProcess, localize.References[i])));
                output.AppendLine(String.Format("LANG_ENGLISH         \"{0}\"", MemoryUtil.ReadNullTerminatedString(T7Util.ActiveProcess, localize.LocalizedStrings[i])));
                output.AppendLine();
            }

            output.AppendLine("ENDMARKER");

            File.WriteAllText("exported_files\\" + asset.Path, output.ToString());

            return true;
        }
    }
}
