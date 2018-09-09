/*
 *  HydraX - Copyright 2018 Philip/Scobalula
 *  
 *  This file is subject to the license terms set out in the
 *  "LICENSE.txt" file. 
 * 
 */
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using PhilUtil;
using HydraLib.T7.Assets;
using HydraX;

namespace HydraLib
{
    /// <summary>
    /// T7 Logic
    /// </summary>
    public partial class T7Util
    {
        /// <summary>
        /// Active T7 Process Pointer
        /// </summary>
        public static IntPtr ActiveProcess = IntPtr.Zero;

        /// <summary>
        /// Active T7 Fast File
        /// </summary>
        public static FastFile ActiveFastFile { get; set; }

        /// <summary>
        /// Loads Assets from T7's Memory
        /// </summary>
        /// <param name="process"></param>
        public static List<Asset> Load(Process process)
        {
            Stopwatch watch = Stopwatch.StartNew();
            ActiveProcess = MemoryUtil.OpenProcess(MemoryUtil.PROCESS_VM_READ, false, process.Id);

            LoggingUtil.ActiveLogger.Log("Searching for required data", MessageType.INFO);
            FindDataPools((long)process.MainModule.BaseAddress);
            LoggingUtil.ActiveLogger.Log(String.Format("Found required data in {0:0.00} seconds", watch.ElapsedMilliseconds / 1000.0), MessageType.INFO);
            watch.Stop();
            List<Asset> assetList = new List<Asset>();
            Console.WriteLine(StringPoolEndAddress);
            for (int i = 0; i < XASSET_POOLS_COUNT; i++)
            {
                byte[] poolInfoBuffer = MemoryUtil.ReadBytes(ActiveProcess, AssetPoolAddress + (i * 32), 32);
                var poolInfo = Util.LoadAssetPoolInfo(poolInfoBuffer, AssetPools[i].AssetPoolName);
                if (AssetPools[i].LoadFunction != null)
                {
                    /*
                    if (Settings.ActiveSettings?.ExportOptions.ContainsKey(AssetPools[i].AssetPoolName) == false)
                        continue;

                    if (Settings.ActiveSettings?.ExportOptions[AssetPools[i].AssetPoolName] == false)
                        continue;
                    */

                    assetList.AddRange(AssetPools[i].LoadFunction(poolInfo));
                }
            }
            return assetList.OrderBy(x => x.AssetType).ToList();
        }

        /// <summary>
        /// Attempts to local required data pools from Black Ops 3's Memory
        /// </summary>
        /// <param name="address">Address to search from.</param>
        public static void FindDataPools(long address)
        {
            // Attempt to find Asset/String Pools
            AssetPoolAddress = MemoryUtil.FindBytes
                (
                    ActiveProcess,
                    new byte[] { 0x78, 0x00, 0x00, 0x00, 0x13, 0x01, 0x00, 0x00 },
                    address,
                    CheckAssetPoolLocation
                ) - 8;

            StringPoolStartAddress = MemoryUtil.FindBytes
                (
                    ActiveProcess,
                    new byte[] { 0x01, 0x00, 0x00, 0x04, 0x65, 0x6E, 0x5F },
                    address,
                    CheckStringPoolLocation
                ) - 24;

            StringPoolEndAddress = StringPoolStartAddress + 1811796;
        }

        /// <summary>
        /// Verifies Asset Pool location by checking the entry sizes of the first 3 pools
        /// </summary>
        public static bool CheckAssetPoolLocation(long address)
        {
            byte[] buffer = MemoryUtil.ReadBytes(ActiveProcess, address - 8, 128);

            if (BitConverter.ToInt32(buffer, 8) == 120 &&
                BitConverter.ToInt32(buffer, 40) == 1680 &&
                BitConverter.ToInt32(buffer, 104) == 248)
                return true;

            return false;
        }

        /// <summary>
        /// Verifies String Pool by checking the pointer to the pool
        /// </summary>
        public static bool CheckStringPoolLocation(long address)
        {
            if (MemoryUtil.ReadInt64(ActiveProcess, address - 1052) == address - 28)
                return true;

            return false;
        }

        /// <summary>
        /// Returns a string from Black Ops III's String Pool
        /// </summary>
        /// <param name="index">Index of the string</param>
        /// <returns>String if in Pool Range, otherwise null.</returns>
        public static string GetString(int index)
        {
            long stringAddress = StringPoolStartAddress + (0x1C * index);

            Console.WriteLine(stringAddress);

            if (stringAddress >= StringPoolStartAddress && stringAddress <= StringPoolEndAddress)
                return MemoryUtil.ReadNullTerminatedString(ActiveProcess, stringAddress);


            Console.WriteLine("NULL");

            return null;

        }

        /// <summary>
        /// Returns the name of an asset from a pointer
        /// </summary>
        /// <param name="address">Asset's Location in Memory</param>
        /// <returns>Asset Name</returns>
        public static string GetAssetName(long address)
        {
            return MemoryUtil.ReadNullTerminatedString(ActiveProcess, MemoryUtil.ReadInt64(ActiveProcess, address));
        }

        /// <summary>
        /// Gets name of an image (Used by CamoTable exporter) from a pointer
        /// </summary>
        /// <param name="address">Asset's Location in Memory</param>
        /// <returns>Image Name</returns>
        public static string GetImageName(long address)
        {
            return MemoryUtil.ReadNullTerminatedString(ActiveProcess, MemoryUtil.ReadInt64(ActiveProcess, address + 0xF8));
        }

        /// <summary>
        /// Clears all loaded data
        /// </summary>
        public static void Clear()
        {
            ActiveFastFile?.DecodedStream?.Dispose();
            if (ActiveFastFile?.DecodedFile != null)
                if (File.Exists(ActiveFastFile.DecodedFile))
                    File.Delete(ActiveFastFile.DecodedFile);
            ActiveProcess = IntPtr.Zero;
            ActiveFastFile = null;

        }

        /// <summary>
        /// Loads assets from any given asset pool and drumps data to a text file
        /// </summary>
        /// <param name="poolInfo"></param>
        /// <returns></returns>
        public static List<Asset> DefaultLoadFromMemory(AssetPoolInformation poolInfo)
        {
            List<Asset> assetList = new List<Asset>();

            string dataFile = String.Format("{0}_Asset_Data.txt", poolInfo.PoolName);

            using (StreamWriter streamWriter = new StreamWriter(dataFile, true))
            {
                streamWriter.WriteLine("Pool Name       : {0}", poolInfo.PoolName); 
                streamWriter.WriteLine("Pool Entry Size : {0}", poolInfo.EntrySize); 
                streamWriter.WriteLine("Pool Count      : {0}", poolInfo.AssetCount); 
                streamWriter.WriteLine("Pool Max Count  : {0}", poolInfo.MaxAssetCount);
                streamWriter.WriteLine("Pool Size       : {0} Bytes", poolInfo.AssetCount * poolInfo.EntrySize);

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

                    asset.Info = "NULL";

                    streamWriter.WriteLine(asset.Path);


                    assetList.Add(asset);

                }
            }

            return assetList;
        }

        public class FastFile
        {
            /// <summary>
            /// Fast File Compression Types
            /// </summary>
            public enum CompressionType
            {
                NONE,
                ZLIB,
                UNKNOWN,
                LZ4,

            }

            /// <summary>
            /// Fast File Header
            /// </summary>
            public class FastFileHeader
            {
                /// <summary>
                /// Fast File 8 Byte Magic
                /// </summary>
                public string Magic { get; set; }

                /// <summary>
                /// Fast File Version
                /// </summary>
                public int Version { get; set; }

                /// <summary>
                /// Fast File Compression Type
                /// </summary>
                public CompressionType Compression { get; set; }

                /// <summary>
                /// Fast File Name
                /// </summary>
                public string Name { get; set; }

                /// <summary>
                /// Fast File Build Machine
                /// </summary>
                public string BuildMachine { get; set; }

                /// <summary>
                /// Fast File Decoded Size
                /// </summary>
                public int DecodedSize { get; set; }

                /// <summary>
                /// Loads 16 Byte Fast File Header
                /// </summary>
                /// <param name="inputStream"></param>
                /// <returns></returns>
                public static FastFileHeader Load(BinaryReader inputStream)
                {
                    FastFileHeader header = new FastFileHeader
                    {
                        Magic = inputStream.ReadFixedString(8),
                        Version = inputStream.ReadInt32(),
                        Compression = (CompressionType)(inputStream.ReadInt32() >> 8)
                    };
                    inputStream.Seek(2096, SeekOrigin.Current);

                    return header;
                }
            }

            /// <summary>
            /// Default Asset Byte Sequence to Search for
            /// </summary>
            public static byte[] DefaultNeedle = { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };

            /// <summary>
            /// Fast File Header
            /// </summary>
            public FastFileHeader Header { get; set; }

            /// <summary>
            /// Fast File Name
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Fast File Build Machine
            /// </summary>
            public string BuildMachine { get; set; }

            /// <summary>
            /// Fast File Path
            /// </summary>
            public string File { get; set; }

            /// <summary>
            /// Fast File Decoded Path
            /// </summary>
            public string DecodedFile { get; set; }

            /// <summary>
            /// Fast File Size
            /// </summary>
            public int Size { get; set; }

            /// <summary>
            /// Fast File Decoded Size
            /// </summary>
            public int DecodedSize { get; set; }

            /// <summary>
            /// Fast File Asset Count
            /// </summary>
            public int AssetCount { get; set; }

            /// <summary>
            /// Fast File Strings
            /// </summary>
            private string[] Strings { get; set; }

            /// <summary>
            /// Fast File Decoded Binary Reader
            /// </summary>
            public BinaryReader DecodedStream { get; set; }

            /// <summary>
            /// Fast File Assets
            /// </summary>
            public Dictionary<string, Asset> Assets = new Dictionary<string, Asset>();

            public void VerifyValue(string inputValue, string expectedValue, string exceptionMessage)
            {
                if(inputValue != expectedValue)
                    throw new HydraException(exceptionMessage);
            }

            public void VerifyValue(int inputValue, int expectedValue, string exceptionMessage)
            {
                if (inputValue != expectedValue)
                    throw new HydraException(exceptionMessage);
            }

            /// <summary>
            /// Decodes Fast File to a new file
            /// </summary>
            /// <param name="fileName">Fast File Path</param>
            public void Decode(string fileName, Func<float, bool> UpdateCallback = null)
            {
                File = fileName;

                string output = File + ".decoded.dat";

                using (BinaryReader reader = new BinaryReader(new FileStream(fileName, FileMode.Open)))
                {
                    // Load 584 Bytes Header containing Magic, Version, etc.
                    Header = FastFileHeader.Load(reader);

                    // Verify Header's data, ensuring this FF came from T7 and is ZLIB FF
                    VerifyValue
                        (
                            Header.Magic,           
                            "TAff0000", 
                            String.Format("File Magic \"{0}\" does not match T7's \"TAff0000\"", Header.Magic)
                        );

                    VerifyValue
                        (
                            Header.Version,
                            0x27E,      
                            String.Format("Fast File Version \"0x{0:x}\" does not match T7's \"0x251\"", Header.Version)
                        );

                    VerifyValue
                        (
                            (int)Header.Compression,
                            (int)CompressionType.ZLIB,
                            "Invalid Compression Type, Expecting ZLIB Fast File."
                        );


                    using (var outputStream = new FileStream(output, FileMode.Create))
                    {
                        float progress = 0;
                        int blockSize = -1;

                        while(blockSize != 0 && UpdateCallback(progress))
                        {

                            blockSize = reader.ReadInt32();
                            int decodedBlockSize = reader.ReadInt32();
                            // Sometimes not exactly same as blockSize
                            // but is always the size of the block?
                            int blockSize2 = reader.ReadInt32();
                            // "blockSize" pos should = this
                            int blockPosition = reader.ReadInt32();
                            int blockHeader = reader.ReadInt16();

                            // There's blocks of data I don't quite understand
                            // in the FF, this is a hacky way through skip it.
                            if (blockHeader != 0xD48)
                            {
                                if (!ResolveNextZlibBlock(reader))
                                    break;

                                continue;
                            }

                            DeflateUtil.Decode(reader.ReadBytes(blockSize2 - 2)).CopyTo(outputStream);

                            progress = ((float)reader.BaseStream.Position / reader.BaseStream.Length) * (float)100.0;
                        }
                    }

                    DecodedFile = output;
                }
            }

            /// <summary>
            /// 
            /// </summary>
            public List<Asset> Load(Func<bool> CancelCheck)
            {
                List<Asset> assetList = new List<Asset>();

                if (!FileUtil.CanAccessFile(DecodedFile))
                {
                    return null;
                }

                DecodedStream = new BinaryReader(new FileStream(DecodedFile, FileMode.Open));

                LoadStrings();

                long[] offsets = DecodedStream.FindBytes(DefaultNeedle);

                for (int i = 0; i < offsets.Length && CancelCheck(); i++)
                    ProcessPotentialAsset(offsets[i], assetList);

                Assets.Clear();

                return assetList.OrderBy(x => x.AssetType).ToList();
            }

            /// <summary>
            /// Processes a potential asset 
            /// </summary>
            /// <param name="offset"></param>
            public void ProcessPotentialAsset(long offset, List<Asset> assetList)
            {
                DecodedStream.Seek(offset, SeekOrigin.Begin);
                // Read as a buffer to reduce IO calls
                byte[] buffer = DecodedStream.ReadBytes(384);

                int stringOffset = 0;

                // If these are -1 or 0 etc. then extend our
                // "stringOffset" as these bytes won't be a str
                if (BitConverter.ToInt64(buffer, 0) == -1)
                {
                    if (BitConverter.ToInt32(buffer, 11) == 0)
                        stringOffset = 16;
                    else
                        stringOffset = 8;
                }
                else if (BitConverter.ToInt32(buffer, 3) == 0)
                {
                    stringOffset = 8;
                }

                // Keep at max of 255 (we shouldn't be dealing with longer)
                string name = ByteUtil.ReadCString(buffer, stringOffset);

                // Take only A-Z, Slashes, _ and .
                if (!Regex.Match(name, @"^[\\/a-zA-Z0-9_.]+$").Success)
                    return;

                // Avoid issues with Path class
                string extension = name.Split('.').Last();


                if (Assets.ContainsKey(name))
                    return;

                // TODO: set this up for other use cases i.e. GUI app
                Asset asset = new Asset();

                // Switch known extensions
                switch (extension)
                {
                    // "RawFiles" (including LUA and GSC/CSC)
                    case "script":
                    case "lua":
                    case "gsc":
                    case "csc":
                    case "gsh":
                    case "vision":
                    case "cfg":
                    case "graph":
                    case "txt":
                    case "atr":
                        DecodedStream.Seek(offset - 16, SeekOrigin.Begin);
                        RawFile.LoadFromFastFile(this, asset);
                        if (!Assets.ContainsKey(asset.Path))
                        {
                            Console.WriteLine(extension);
                            if (Settings.ActiveSettings?.ExportOptions[asset.AssetType] == false)
                                return;
                            Assets[asset.Path] = asset;
                            assetList.Add(asset);
                        }
                        break;
                    // AI AnimStateMachines
                    case "ai_asm":
                        DecodedStream.Seek(offset - 32, SeekOrigin.Begin);
                        AnimStateMachine.LoadFromFastFile(this, asset);
                        if (!Assets.ContainsKey(asset.Path))
                        {
                            if (Settings.ActiveSettings?.ExportOptions[asset.AssetType] == false)
                                return;
                            Assets[asset.Path] = asset;
                            assetList.Add(asset);
                        }
                        break;
                    // AI AnimSelectorTables
                    case "ai_ast":
                        DecodedStream.Seek(offset + 8, SeekOrigin.Begin);
                        AnimSelectorTable.LoadFromFastFile(this, asset);
                        if (!Assets.ContainsKey(asset.Path))
                        {
                            if (Settings.ActiveSettings?.ExportOptions[asset.AssetType] == false)
                                return;
                            Assets[asset.Path] = asset;
                            assetList.Add(asset);
                        }
                        break;
                    // AI AnimMappingTables
                    case "ai_am":
                        DecodedStream.Seek(offset + 8, SeekOrigin.Begin);
                        AnimMappingTable.LoadFromFastFile(this, asset);
                        if (!Assets.ContainsKey(asset.Path))
                        {
                            if (Settings.ActiveSettings?.ExportOptions[asset.AssetType] == false)
                                return;
                            Assets[asset.Path] = asset;
                            assetList.Add(asset);
                        }
                        break;
                    // AI Behavior Trees
                    case "ai_bt":
                        DecodedStream.Seek(offset - 16, SeekOrigin.Begin);
                        BehaviorTree.LoadFromFastFile(this, asset);
                        if (!Assets.ContainsKey(asset.Path))
                        {
                            if (Settings.ActiveSettings?.ExportOptions[asset.AssetType] == false)
                                return;
                            Assets[asset.Path] = asset;
                            assetList.Add(asset);
                        }
                        break;
                    default:
                        break;
                }
            }

            /// <summary>
            /// Attempts to find next ZLIB Block if we hit a non-ZLIB Block
            /// </summary>
            /// <param name="input">Input Reader</param>
            /// <returns>True/False if found.</returns>
            public bool ResolveNextZlibBlock(BinaryReader input)
            {
                // T7 FF ZLIB Header
                byte[] needle = { 0x48, 0x0D };
                // Loop until we find valid block
                while (true)
                {
                    // Search for next ZLIB Header
                    long[] result = input.FindBytes(needle, true, input.BaseStream.Position);
                    // EOF or no headers
                    if (result.Length == 0)
                        return false;
                    // Seek back to block Position
                    input.Seek(result[0] - 6, SeekOrigin.Begin);
                    // Check Block Position
                    if (input.ReadInt32() == input.BaseStream.Position - 16)
                    {
                        input.Seek(result[0] - 18, SeekOrigin.Begin);
                        return true;
                    }
                    else
                    {
                        input.Seek(result[0] + 2, SeekOrigin.Begin);
                        continue;
                    }
                }
            }

            /// <summary>
            /// Loads Fast File String Table
            /// </summary>
            private void LoadStrings()
            {
                // These strings are used by anims, models, AI files, etc.
                // and need to be loaded.
                if (DecodedStream != null)
                {
                    DecodedStream.Seek(0, SeekOrigin.Begin);

                    Strings = new string[DecodedStream.ReadInt32()];

                    DecodedStream.Seek(56 + (Strings.Length - 1) * 8, SeekOrigin.Begin);

                    for (int i = 0; i < Strings.Length; i++)
                        Strings[i] = DecodedStream.ReadNullTerminatedString();
                }
            }

            /// <summary>
            /// Gets a string from the Fast Files String Table. 
            /// </summary>
            /// <param name="index">Index of the string.</param>
            /// <returns>String if exists, otherwise null.</returns>
            public string GetString(int index)
            {
                return Strings?.ElementAtOrDefault(index);
            }

        }
    }
}
