/*
 *  HydraX Behavior Tree Logic - Copyright 2018 Philip/Scobalula
 *  
 *  This file is subject to the license terms set out in the
 *  "LICENSE.txt" file. 
 * 
 */
using HydraLib.T7.Assets.JsonFiles;
using Newtonsoft.Json;
using PhilUtil;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

// TODO: Make use of Pointers in the Memory Exporter

namespace HydraLib.T7.Assets.JsonFiles
{
    public class Behavior
    {
        /// <summary>
        /// Behavior Types TODO: Switch to Array now that we have all types
        /// </summary>
        [JsonIgnore]
        private static Dictionary<int, string> Types = new Dictionary<int, string>()
        {
            { 0 ,       "action" },
            { 1 ,       "condition_blackboard" },
            { 2 ,       "condition_script" },
            { 3 ,       "condition_script_negate" },
            { 4 ,       "condition_service_script" },
            { 5 ,       "decorator_random" },
            { 6 ,       "decorator_script" },
            { 7 ,       "decorator_timer" },
            { 8 ,       "parallel" },
            { 9 ,       "sequence" },
            { 10 ,      "selector" },
            { 11 ,      "probability_selector" },
            { 12 ,      "behavior_state_machine" },
            { 13 ,      "link_node" },
        };

        /// <summary>
        /// Behavior Type
        /// </summary>
        [JsonProperty("type")]
        [DefaultValue("")]
        public string Type { get; set; }

        /// <summary>
        /// Animation Selector
        /// </summary>
        [JsonProperty("id")]
        [DefaultValue("")]
        public string ID { get; set; }

        /// <summary>
        /// Behavior Index
        /// </summary>
        [JsonIgnore]
        public int Index { get; set; }

        /// <summary>
        /// Behavior Parent Index
        /// </summary>
        [JsonIgnore]
        public int ParentIndex { get; set; }

        /// <summary>
        /// Action Name
        /// </summary>
        [JsonProperty("ActionName")]
        [DefaultValue("")]
        public string ActionName { get; set; }

        /// <summary>
        /// Animation State Machine State Name
        /// </summary>
        [JsonProperty("ASMStateName")]
        [DefaultValue("")]
        public string ASMStateName { get; set; }

        /// <summary>
        /// Action Notification
        /// </summary>
        [JsonProperty("actionNotify")]
        [DefaultValue("")]
        public string ActionNotify { get; set; }

        /// <summary>
        /// Start Function
        /// </summary>
        [JsonProperty("StartFunction")]
        [DefaultValue("")]
        public string StartFunction { get; set; }

        /// <summary>
        /// Terminate Function
        /// </summary>
        [JsonProperty("TerminateFunction")]
        [DefaultValue("")]
        public string TerminateFunction { get; set; }

        /// <summary>
        /// Update Function
        /// </summary>
        [JsonProperty("UpdateFunction")]
        [DefaultValue("")]
        public string UpdateFunction { get; set; }

        /// <summary>
        /// Looping Action
        /// </summary>
        [JsonProperty("loopingAction")]
        [DefaultValue(0)]
        public int? LoopingAction { get; set; }

        /// <summary>
        /// Max Action Time
        /// </summary>
        [JsonProperty("actionTimeMax")]
        [DefaultValue("")]
        public int? ActionTimeMax { get; set; }

        /// <summary>
        /// Script Function
        /// </summary>
        [JsonProperty("scriptFunction")]
        [DefaultValue("")]
        public string ScriptFunction { get; set; }

        /// <summary>
        /// Interruption Name
        /// </summary>
        [JsonProperty("interruptName")]
        [DefaultValue("")]
        public string InterruptName { get; set; }

        /// <summary>
        /// Chance Percentage
        /// </summary>
        [JsonProperty("percentChance")]
        [DefaultValue(0.0f)]
        public float? PercentChance { get; set; }

        /// <summary>
        /// Min Cool Down
        /// </summary>
        [JsonProperty("cooldownMin")]
        [DefaultValue(0)]
        public int? CoolDownMin { get; set; }

        /// <summary>
        /// Max Cool Down
        /// </summary>
        [JsonProperty("cooldownMax")]
        [DefaultValue(0)]
        public int? CooldDownMax { get; set; }

        /// <summary>
        /// Child Behaviors
        /// </summary>
        [JsonProperty("children", Order = 1)]
        [DefaultValue(null)]
        public List<Behavior> Children;

        /// <summary>
        /// Saves Behavior to a formatted JSON file
        /// </summary>
        /// <param name="path">File Path</param>
        public void Save(string path)
        {
            using (JsonTextWriter output = new JsonTextWriter(new StreamWriter(path)))
            {
                output.Formatting = Formatting.Indented;
                output.Indentation = 4;
                output.IndentChar = ' ';

                JsonSerializer serializer = new JsonSerializer
                {
                    NullValueHandling = NullValueHandling.Ignore
                };

                serializer.Serialize(output, this);

            }
        }

        /// <summary>
        /// Gets Behavior Type By Index, returns Unknown Type String if not found
        /// </summary>
        /// <param name="typeIndex">index of the Behavior Type</param>
        /// <returns>Behavior Type as a String</returns>
        public static string GetBehaviorType(int typeIndex)
        {
            if (Types.ContainsKey(typeIndex))
                return Types[typeIndex];

            return String.Format("BT_UNKNOWN_TYPE_INDEX: {0}", typeIndex);
        }
    }
}

namespace HydraLib.T7.Assets
{
    /// <summary>
    /// Behavior Logic
    /// </summary>
    class BehaviorTree
    {
        /// <summary>
        /// Address Tracker (Used by ExportFromMemory func)
        /// </summary>
        public static long AddressTracker = 0;

        /// <summary>
        /// Loads Behavior Trees from Memory
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
                };

                if (Util.IsNullAsset(asset, poolInfo))
                    continue;


                asset.Path = MemoryUtil.ReadNullTerminatedString(T7Util.ActiveProcess, asset.NameLocation);
                asset.DisplayName = Path.GetFileName(asset.Path);
                asset.AssetType = poolInfo.PoolName;
                asset.Data = BitConverter.ToInt32(assetData, 16);
                asset.Info = String.Format("Behaviors - {0}", (int)asset.Data);
                asset.ExportFunction = ExportFromMemory;
                assetList.Add(asset);
            }

            return assetList;
        }

        /// <summary>
        /// Processes a Behavior and all child behaviors from Memory
        /// </summary>
        public static Behavior ProcessBehavior(long address, Behavior parent = null)
        {
            byte[] buffer = MemoryUtil.ReadBytes(T7Util.ActiveProcess, AddressTracker, 72);

            AddressTracker += 72;

            // New Behavior
            Behavior behavior = new Behavior();

            parent?.Children.Add(behavior);

            behavior.ID = T7Util.GetString(BitConverter.ToInt32(buffer, 0));
            behavior.Type = Behavior.GetBehaviorType(BitConverter.ToInt32(buffer, 4));

            behavior.Index = BitConverter.ToInt32(buffer, 8);
            behavior.ParentIndex = BitConverter.ToInt32(buffer, 12);

            bool hasChildren = BitConverter.ToInt64(buffer, 16) > 0;
            int numChildBehaviors = BitConverter.ToInt32(buffer, 24);
            // Grab all of these as they can be used differently depending on type
            string behaviorString0 = T7Util.GetString(BitConverter.ToInt32(buffer, 28));
            string behaviorString1 = T7Util.GetString(BitConverter.ToInt32(buffer, 32));
            string behaviorString2 = T7Util.GetString(BitConverter.ToInt32(buffer, 36));
            string behaviorString3 = T7Util.GetString(BitConverter.ToInt32(buffer, 40));
            string behaviorString4 = T7Util.GetString(BitConverter.ToInt32(buffer, 44));
            string behaviorString5 = T7Util.GetString(BitConverter.ToInt32(buffer, 48));
            string behaviorString6 = T7Util.GetString(BitConverter.ToInt32(buffer, 52));
            string behaviorString7 = T7Util.GetString(BitConverter.ToInt32(buffer, 56));
            string behaviorString8 = T7Util.GetString(BitConverter.ToInt32(buffer, 60));

            // Print.Debug(String.Format("Behavior {0} - Position 0x{1:X}", behavior.id, fastFile.DecodedStream.BaseStream.Position));

            // Switch Type, as what our strings/ints do depends on type
            switch (behavior.Type)
            {
                // Action Behaviors
                case "action":
                case "behavior_state_machine":
                    behavior.ASMStateName = behaviorString3;
                    behavior.ActionName = behaviorString4;
                    behavior.ActionNotify = behaviorString5;
                    behavior.StartFunction = behaviorString6;
                    behavior.UpdateFunction = behaviorString7;
                    behavior.TerminateFunction = behaviorString8;
                    behavior.LoopingAction = BitConverter.ToInt32(buffer, 64);
                    behavior.ActionTimeMax = BitConverter.ToInt32(buffer, 68);
                    break;
                // Condition Script Behaviors
                case "condition_script":
                case "condition_blackboard":
                case "condition_script_negate":
                case "condition_service_script":
                    behavior.ScriptFunction = behaviorString6;
                    behavior.InterruptName = behaviorString7;
                    behavior.CoolDownMin = BitConverter.ToInt32(buffer, 64);
                    behavior.CooldDownMax = BitConverter.ToInt32(buffer, 68);
                    break;
                case "probability_selector":
                case "decorator_random":
                    behavior.PercentChance = BitConverter.ToSingle(buffer, 64);
                    break;
                default:
                    break;
            }
            // If we have children, create the list
            if (hasChildren)
                behavior.Children = new List<Behavior>();
            // Process Child Nodes
            for (int i = 0; i < numChildBehaviors; i++)
                ProcessBehavior(AddressTracker, behavior);
            // Return to root
            return behavior;
        }

        public static bool ExportFromMemory(Asset asset)
        {
            AddressTracker = asset.StartLocation;
            string assetName = "exported_files\\behavior\\" + asset.Path;
            PathUtil.CreateFilePath(assetName);
            Behavior root = ProcessBehavior(AddressTracker);
            root.Save(assetName);
            return true;
        }

        /// <summary>
        /// Processes a Behavior and all child behaviors from a Fast File
        /// </summary>
        public static Behavior ProcessBehavior(T7Util.FastFile fastFile, Behavior parent = null)
        {
            // New Behavior
            Behavior behavior = new Behavior();

            parent?.Children.Add(behavior);
            behavior.ID = fastFile.GetString(fastFile.DecodedStream.ReadInt32() - 1);
            behavior.Type = Behavior.GetBehaviorType(fastFile.DecodedStream.ReadInt32());

            behavior.Index = fastFile.DecodedStream.ReadInt32();
            behavior.ParentIndex = fastFile.DecodedStream.ReadInt32();

            bool hasChildren = fastFile.DecodedStream.ReadInt64() == -1;
            int numChildBehaviors = fastFile.DecodedStream.ReadInt32();
            // Grab all of these as they can be used differently depending on type
            string behaviorString0 = fastFile.GetString(fastFile.DecodedStream.ReadInt32() - 1);
            string behaviorString1 = fastFile.GetString(fastFile.DecodedStream.ReadInt32() - 1);
            string behaviorString2 = fastFile.GetString(fastFile.DecodedStream.ReadInt32() - 1);
            string behaviorString3 = fastFile.GetString(fastFile.DecodedStream.ReadInt32() - 1);
            string behaviorString4 = fastFile.GetString(fastFile.DecodedStream.ReadInt32() - 1);
            string behaviorString5 = fastFile.GetString(fastFile.DecodedStream.ReadInt32() - 1);
            string behaviorString6 = fastFile.GetString(fastFile.DecodedStream.ReadInt32() - 1);
            string behaviorString7 = fastFile.GetString(fastFile.DecodedStream.ReadInt32() - 1);
            string behaviorString8 = fastFile.GetString(fastFile.DecodedStream.ReadInt32() - 1);

            var data = fastFile.DecodedStream.ReadBytes(4);
            var data2 = fastFile.DecodedStream.ReadBytes(4);
            // Switch Type, as what our strings/ints do depends on type
            switch (behavior.Type)
            {
                // Action Behaviors
                case "action":
                case "behavior_state_machine":
                    behavior.ASMStateName = behaviorString3;
                    behavior.ActionName = behaviorString4;
                    behavior.ActionNotify = behaviorString5;
                    behavior.StartFunction = behaviorString6;
                    behavior.UpdateFunction = behaviorString7;
                    behavior.TerminateFunction = behaviorString8;
                    behavior.LoopingAction = BitConverter.ToInt32(data, 0);
                    behavior.ActionTimeMax = BitConverter.ToInt32(data2, 0);
                    break;
                // Condition Script Behaviors
                case "condition_script":
                case "condition_blackboard":
                case "condition_script_negate":
                case "condition_service_script":
                    behavior.ScriptFunction = behaviorString6;
                    behavior.InterruptName = behaviorString7;
                    behavior.CoolDownMin = BitConverter.ToInt32(data, 0);
                    behavior.CooldDownMax = BitConverter.ToInt32(data2, 0);
                    break;
                case "probability_selector":
                case "decorator_random":
                    behavior.PercentChance = BitConverter.ToSingle(data, 0);
                    break;
                default:
                    break;
            }
            // If we have children, create the list
            if (hasChildren)
                behavior.Children = new List<Behavior>();
            // Process Child Nodes
            for (int i = 0; i < numChildBehaviors; i++)
                ProcessBehavior(fastFile, behavior);
            // Return to root
            return behavior;
        }

        /// <summary>
        /// Loads a Behavior Tree from a Fast File
        /// </summary>
        public static void LoadFromFastFile(T7Util.FastFile fastFile, Asset asset)
        {
            int numBehaviors = fastFile.DecodedStream.ReadInt32();
            fastFile.DecodedStream.Seek(20, SeekOrigin.Current);
            asset.Path = fastFile.DecodedStream.ReadNullTerminatedString();
            asset.DisplayName = Path.GetFileName(asset.Path);
            asset.AssetType = "behaviortree";
            asset.StartLocation = fastFile.DecodedStream.BaseStream.Position;
            asset.Info = String.Format("Behaviors - {0}", numBehaviors);
            asset.ExportFunction = ExportFromFastFile;
        }

        /// <summary>
        /// Exports a Behavior Tree from a Fast File
        /// </summary>
        public static bool ExportFromFastFile(Asset asset)
        {
            T7Util.ActiveFastFile.DecodedStream.Seek(asset.StartLocation, SeekOrigin.Begin);
            string assetName = "exported_files\\behavior\\" + asset.Path;
            PathUtil.CreateFilePath(assetName);
            Behavior root = ProcessBehavior(T7Util.ActiveFastFile);
            root.Save(assetName);
            return true;
        }
    }
}
