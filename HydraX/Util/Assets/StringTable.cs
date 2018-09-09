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
    /// StringTable Logic
    /// </summary>
    class StringTable
    {
        /// <summary>
        /// String Table Row
        /// </summary>
        public class Row
        {
            /// <summary>
            /// Row Columns
            /// </summary>
            public string[] Columns { get; set; }
        }

        /// <summary>
        /// Number of Rows
        /// </summary>
        public int RowCount { get; set; }

        /// <summary>
        /// Number of Columns
        /// </summary>
        public int ColumnCount { get; set; }

        /// <summary>
        /// Rows in this String Table
        /// </summary>
        public Row[] Rows { get; set; }

        /// <summary>
        /// Loads StringTables from Memory
        /// </summary>
        /// <param name="poolInfo">Asset Pool Data</param>
        /// <returns>Asset List</returns>
        public static List<Asset> LoadFromMemory(AssetPoolInformation poolInfo)
        {
            List<Asset> assetList = new List<Asset>();

            for (int i = 0; assetList.Count < poolInfo.AssetCount; i++)
            {
                byte[] strTableData = MemoryUtil.ReadBytes(T7Util.ActiveProcess, poolInfo.StartLocation + (poolInfo.EntrySize * i), poolInfo.EntrySize);

                Asset asset = new Asset();
                StringTable table = new StringTable
                {
                    ColumnCount = BitConverter.ToInt32(strTableData, 8),
                    RowCount = BitConverter.ToInt32(strTableData, 12)
                };

                table.Rows = new Row[table.RowCount];

                asset.NameLocation = BitConverter.ToInt64(strTableData, 0);
                asset.AssetType = poolInfo.PoolName;
                asset.StartLocation = BitConverter.ToInt64(strTableData, 16);
                asset.EndLocation = BitConverter.ToInt64(strTableData, 24);

                if (Util.IsNullAsset(asset, poolInfo))
                    continue;

                asset.Size = (int)(asset.EndLocation - asset.StartLocation);
                asset.Path = MemoryUtil.ReadNullTerminatedString(T7Util.ActiveProcess, asset.NameLocation);
                asset.DisplayName = Path.GetFileName(asset.Path);
                asset.Data = table;

                asset.ExportFunction = Export;

                asset.Info = String.Format("Columns - {0} Rows - {1}", table.ColumnCount, table.RowCount);

                assetList.Add(asset);
            }

            return assetList;
        }

        /// <summary>
        /// Exports StringTables from Memory
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        public static bool Export(Asset asset)
        {
            StringTable table = (StringTable)asset.Data;

            byte[] strTableBuffer = MemoryUtil.ReadBytes(T7Util.ActiveProcess, asset.StartLocation, asset.Size);

            if (strTableBuffer == null)
                return false;


            using (BinaryReader input = new BinaryReader(new MemoryStream(strTableBuffer)))
            {
                for (int i = 0; i < table.RowCount; i++)
                {
                    table.Rows[i] = new Row
                    {
                        Columns = new string[table.ColumnCount]
                    };

                    for (int j = 0; j < table.ColumnCount; j++)
                    {
                        table.Rows[i].Columns[j] = MemoryUtil.ReadNullTerminatedString(T7Util.ActiveProcess, input.ReadInt64());

                        input.Seek(8, SeekOrigin.Current);
                    }
                }
            }

            StringBuilder strOut = new StringBuilder();

            foreach (Row col in table.Rows)
            {
                foreach (string n in col.Columns)
                {
                    strOut.Append(string.Format("{0},", n));
                }

                strOut.AppendLine();
            }

            PathUtil.CreateFilePath("exported_files\\" + asset.Path);

            File.WriteAllText("exported_files\\" + asset.Path, strOut.ToString());

            return true;
        }
    }
}
