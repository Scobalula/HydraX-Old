/*
 *  HydraX - Copyright 2018 Philip/Scobalula
 *  
 *  This file is subject to the license terms set out in the
 *  "LICENSE.txt" file. 
 * 
 */
using System;
using System.Collections.Generic;
using PhilUtil;
using Newtonsoft.Json;
using System.ComponentModel;
using System.IO;
using HydraLib.T7.Assets.JsonFiles;

// TODO: Memory Export Fails due to realignment among other things in some cases, there are pointers that point to the data to use instead

namespace HydraLib.T7.Assets.JsonFiles
{
    /// <summary>
    /// Animation State Data
    /// </summary>
    class AnimState
    {
        /// <summary>
        /// Number of Sub States 
        /// </summary>
        [JsonIgnore]
        public int SubStateCount { get; set; }

        /// <summary>
        /// Location of the Sub States
        /// </summary>
        [JsonIgnore]
        public long SubStatesLocation { get; set; }

        /// <summary>
        /// Number of Transitions
        /// </summary>
        [JsonIgnore]
        public int TransitionCount { get; set; }



        /// <summary>
        /// Animation Selector
        /// </summary>
        [JsonProperty("animation_selector")]
        [DefaultValue("")]
        public string AnimationSelector { get; set; }

        /// <summary>
        /// Transition Decorator
        /// </summary>
        [JsonProperty("transition_decorator")]
        [DefaultValue("")]
        public string TransitionDecorator { get; set; }

        /// <summary>
        /// Aim Selector
        /// </summary>
        [JsonProperty("aim_selector")]
        [DefaultValue("")]
        public string AimSelector { get; set; }

        /// <summary>
        /// Shoot Selector
        /// </summary>
        [JsonProperty("shoot_selector")]
        [DefaultValue("")]
        public string ShootSelector { get; set; }

        /// <summary>
        /// Delta Layer Function
        /// </summary>
        [JsonProperty("delta_layer_function")]
        [DefaultValue("")]
        public string DeltaLayerFunction { get; set; }

        /// <summary>
        /// Transition Decorator Layer Function
        /// </summary>
        [JsonProperty("transdec_layer_function")]
        [DefaultValue("")]
        public string TransDecLayerFunction { get; set; }

        /// <summary>
        /// AnimStateMachine Client Notify
        /// </summary>
        [JsonProperty("asm_client_notify")]
        [DefaultValue("")]
        public string ASMClientNotify { get; set; }

        /// <summary>
        /// Whether this ASM is Sync Loop or not
        /// </summary>
        [JsonProperty("loopsync")]
        [DefaultValue(false)]
        public bool LoopSync { get; set; }

        /// <summary>
        /// Whether this ASM is Clean Loop or not
        /// </summary>
        [JsonProperty("cleanloop")]
        [DefaultValue(false)]
        public bool CleanLoop { get; set; }

        /// <summary>
        /// Whether this ASM is Multiple Delta or not
        /// </summary>
        [JsonProperty("multipledelta")]
        [DefaultValue(false)]
        public bool MultipleDelta { get; set; }

        /// <summary>
        /// Whether this ASM is Terminal or not
        /// </summary>
        [JsonProperty("terminal")]
        [DefaultValue(false)]
        public bool Terminal { get; set; }

        /// <summary>
        /// Whether this ASM is Parameric 2D or not
        /// </summary>
        [JsonProperty("parametric2d")]
        [DefaultValue(false)]
        public bool Parametric2D { get; set; }

        /// <summary>
        /// Whether this ASM is Animation Driven LocMotion or not
        /// </summary>
        [JsonProperty("animdrivenlocomotion")]
        [DefaultValue(false)]
        public bool AnimDrivenLocmotion { get; set; }

        /// <summary>
        /// Whether this ASM is Coderate or not
        /// </summary>
        [JsonProperty("coderate")]
        [DefaultValue(false)]
        public bool Coderate { get; set; }

        /// <summary>
        /// Whether this ASM is  or not
        /// </summary>
        [JsonProperty("speedblend")]
        [DefaultValue(false)]
        public bool SpeedBlend { get; set; }

        /// <summary>
        /// Whether this ASM allows Transition Decorator Aim  or not
        /// </summary>
        [JsonProperty("allow_transdec_aim")]
        [DefaultValue(false)]
        public bool AllowTransDecAim { get; set; }

        /// <summary>
        /// Whether this ASM is Force Fire or not
        /// </summary>
        [JsonProperty("force_fire")]
        [DefaultValue(false)]
        public bool ForceFire { get; set; }

        /// <summary>
        /// Whether this ASM requires a Ragdoll Notetrack  or not
        /// </summary>
        [JsonProperty("requires_ragdoll_notetrack")]
        [DefaultValue(false)]
        public bool RequiresRagdollNote { get; set; }

        /// <summary>
        /// Sub Anim States
        /// </summary>
        [JsonProperty(Order = 1)]
        public Dictionary<string, AnimState> substates;

        /// <summary>
        /// Transitions
        /// </summary>
        [JsonProperty(Order = 2)]
        public Dictionary<string, Transition> transitions;
    }

    /// <summary>
    /// Transition Data
    /// </summary>
    class Transition
    {
        /// <summary>
        /// Animation Selector
        /// </summary>
        [JsonProperty("animation_selector")]
        [DefaultValue("")]
        public string AnimationSelector { get; set; }
    }
}

namespace HydraLib.T7.Assets
{

    /// <summary>
    /// Anim State Machine Logic
    /// </summary>
    class AnimStateMachine
    {
        /// <summary>
        /// Number of Main States
        /// </summary>
        [JsonIgnore]
        public int MainStateCount { get; set; }

        /// <summary>
        /// Number of Sub States
        /// </summary>
        [JsonIgnore]
        public int SubStateCount { get; set; }

        /// <summary>
        /// Animation States
        /// </summary>
        [JsonProperty("states")]
        public Dictionary<string, AnimState> States = new Dictionary<string, AnimState>();

        /// <summary>
        /// Checks if is enabled or not
        /// </summary>
        public static bool RequiresRagdollNote(int values)
        {
            return (values & 0x2) != 0;
        }

        /// <summary>
        /// Checks if is enabled or note
        /// </summary>
        public static bool IsTerminal(int values)
        {
            return (values & 0x1) != 0;
        }

        /// <summary>
        /// Checks if is enabled or note
        /// </summary>
        public static bool IsLoopSync(int values)
        {
            return (values & 0x2) != 0;
        }

        /// <summary>
        /// Checks if is enabled or note
        /// </summary>
        public static bool IsCleanLoop(int values)
        {
            return (values & 0x80) != 0;
        }

        /// <summary>
        /// Checks if is enabled or note
        /// </summary>
        public static bool IsMultipleDelta(int values)
        {
            return (values & 0x4) != 0;
        }

        /// <summary>
        /// Checks if is enabled or note
        /// </summary>
        public static bool IsParametric2D(int values)
        {
            return (values & 0x8) != 0;
        }

        /// <summary>
        /// Checks if is enabled or note
        /// </summary>
        public static bool IsAnimDrivenLocomotion(int values)
        {
            return (values & 0x100) != 0;
        }

        /// <summary>
        /// Checks if is enabled or note
        /// </summary>
        public static bool IsCoderate(int values)
        {
            return (values & 0x10) != 0;
        }

        /// <summary>
        /// Checks if is enabled or note
        /// </summary>
        public static bool IsSpeedBlend(int values)
        {
            return (values & 0x200) != 0;
        }

        /// <summary>
        /// Checks if is enabled or note
        /// </summary>
        public static bool AllowTransdecAim(int values)
        {
            return (values & 0x20) != 0;
        }

        /// <summary>
        /// Checks if is enabled or note
        /// </summary>
        public static bool ForceFire(int values)
        {
            return (values & 0x40) != 0;
        }

        /// <summary>
        /// Address Tracker for the Memory Exporter
        /// </summary>
        public static long AddressTracker = 0;

        /// <summary>
        /// Loads Animation State Machines from Memory
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
                    NameLocation  = BitConverter.ToInt64(assetData, 0),
                    StartLocation = BitConverter.ToInt64(assetData, 8),
                };

                if (Util.IsNullAsset(asset, poolInfo))
                    continue;

                AnimStateMachine animStateMachine = new AnimStateMachine()
                {
                    MainStateCount = BitConverter.ToInt32(assetData, 16),
                    SubStateCount  = BitConverter.ToInt32(assetData, 32),
                };

                asset.Path           = MemoryUtil.ReadNullTerminatedString(T7Util.ActiveProcess, asset.NameLocation);
                asset.DisplayName    = Path.GetFileName(asset.Path);
                asset.AssetType      = poolInfo.PoolName;
                asset.Data           = animStateMachine;
                asset.Info           = String.Format("States - {0}", animStateMachine.SubStateCount);
                asset.ExportFunction = ExportFromMemory;
                assetList.Add(asset);
            }

            return assetList;
        }

        /// <summary>
        /// Exports an Animation State Machine from Memory
        /// </summary>
        public static bool ExportFromMemory(Asset asset)
        {
            AddressTracker = asset.StartLocation;

            // Total Transitions from all states - Max 127?
            int totalTransitions = 0;

            AnimStateMachine animStateMachine = (AnimStateMachine)asset.Data;

            string[] mainStates = new string[animStateMachine.MainStateCount];

            string assetName = "exported_files\\animstatemachines\\" + asset.Path;
            PathUtil.CreateFilePath(assetName);


            for (int i = 0; i < mainStates.Length; i++)
            {
                // Name from Fast File String Table
                string stateName = T7Util.GetString(MemoryUtil.ReadInt32(T7Util.ActiveProcess, AddressTracker));



                AddressTracker += 16;

                // Sub states this state has
                int numSubStates = MemoryUtil.ReadInt32(T7Util.ActiveProcess, AddressTracker);

                Console.WriteLine(numSubStates);

                AddressTracker += 8;

                animStateMachine.States[stateName] = new AnimState
                {
                    SubStateCount = numSubStates
                };

                mainStates[i] = stateName;
            }

            // Indexes for in-game possibly?
            int bytes = 4 * animStateMachine.SubStateCount;

            AddressTracker += bytes + (bytes % 8);

            for (int i = 0; i < mainStates.Length; i++)
            {
                AnimState state = animStateMachine.States[mainStates[i]];

                state.substates = new Dictionary<string, AnimState>();

                for (int j = 0; j < state.SubStateCount; j++)
                {

                    string stateName = T7Util.GetString(MemoryUtil.ReadInt32(T7Util.ActiveProcess, AddressTracker));

                    AddressTracker += 8;

                    state.substates[stateName] = new AnimState();

                    int boolValues1 = MemoryUtil.ReadInt32(T7Util.ActiveProcess, AddressTracker);
                    int boolValues2 = MemoryUtil.ReadInt32(T7Util.ActiveProcess, AddressTracker + 4);

                    AddressTracker += 12;

                    state.substates[stateName].AnimationSelector   = T7Util.GetString(MemoryUtil.ReadInt32(T7Util.ActiveProcess, AddressTracker));
                    state.substates[stateName].AimSelector         = T7Util.GetString(MemoryUtil.ReadInt32(T7Util.ActiveProcess, AddressTracker + 4));
                    state.substates[stateName].ShootSelector       = T7Util.GetString(MemoryUtil.ReadInt32(T7Util.ActiveProcess, AddressTracker + 8));
                    state.substates[stateName].TransitionDecorator = T7Util.GetString(MemoryUtil.ReadInt32(T7Util.ActiveProcess, AddressTracker + 12));
                    state.substates[stateName].DeltaLayerFunction  = T7Util.GetString(MemoryUtil.ReadInt32(T7Util.ActiveProcess, AddressTracker + 16));
                    state.substates[stateName].TransitionDecorator = T7Util.GetString(MemoryUtil.ReadInt32(T7Util.ActiveProcess, AddressTracker + 20));
                    state.substates[stateName].ASMClientNotify     = T7Util.GetString(MemoryUtil.ReadInt32(T7Util.ActiveProcess, AddressTracker + 24));

                    AddressTracker += 28;

                    // Transitions are stored at end of ASM, assuming in order of each state
                    bool hasTransitions = MemoryUtil.ReadInt64(T7Util.ActiveProcess, AddressTracker) > 0;
                    int transitionCount = MemoryUtil.ReadInt32(T7Util.ActiveProcess, AddressTracker + 8);

                    AddressTracker += 16;

                    totalTransitions += transitionCount;

                    if (hasTransitions)
                        state.substates[stateName].transitions = new Dictionary<string, Transition>();

                    state.substates[stateName].RequiresRagdollNote = RequiresRagdollNote(boolValues1);
                    state.substates[stateName].Terminal            = IsTerminal(boolValues2);
                    state.substates[stateName].LoopSync            = IsLoopSync(boolValues2);
                    state.substates[stateName].CleanLoop           = IsCleanLoop(boolValues2);
                    state.substates[stateName].MultipleDelta       = IsMultipleDelta(boolValues2);
                    state.substates[stateName].Parametric2D        = IsParametric2D(boolValues2);
                    state.substates[stateName].AnimDrivenLocmotion = IsAnimDrivenLocomotion(boolValues2);
                    state.substates[stateName].Coderate            = IsCoderate(boolValues2);
                    state.substates[stateName].SpeedBlend          = IsSpeedBlend(boolValues2);
                    state.substates[stateName].AllowTransDecAim    = AllowTransdecAim(boolValues2);
                    state.substates[stateName].ForceFire           = ForceFire(boolValues2);

                    state.substates[stateName].TransitionCount = transitionCount;
                }
            }

            // Indexes for in-game possibly?
            bytes = 4 * totalTransitions;

            AddressTracker += bytes + (bytes % 8);
            // Process Transitions
            foreach (KeyValuePair<string, AnimState> states in animStateMachine.States)
            {
                foreach (KeyValuePair<string, AnimState> subStates in states.Value.substates)
                {
                    for (int i = 0; i < subStates.Value.TransitionCount; i++)
                    {
                        string transitionName = T7Util.GetString(MemoryUtil.ReadInt32(T7Util.ActiveProcess, AddressTracker));

                        AddressTracker += 28;

                        subStates.Value.transitions[transitionName] = new Transition
                        {
                            AnimationSelector = T7Util.GetString(MemoryUtil.ReadInt32(T7Util.ActiveProcess, AddressTracker))
                        };

                        AddressTracker += 20;
                    }
                }
            }

            // Save AI_ASM
            animStateMachine.Save(assetName);

            return true;
        }

        /// <summary>
        /// Loads an Anim State Machine from a Fast File
        /// </summary>
        public static void LoadFromFastFile(T7Util.FastFile fastFile, Asset asset)
        {
            AnimStateMachine animStateMachine = new AnimStateMachine
            {
                // Main State count - Max 24?
                MainStateCount = fastFile.DecodedStream.ReadInt32()
            };
            fastFile.DecodedStream.Seek(12, SeekOrigin.Current);
            // Total States - Max 127?
            animStateMachine.SubStateCount = fastFile.DecodedStream.ReadInt32();
            fastFile.DecodedStream.Seek(20, SeekOrigin.Current);
            asset.Data = animStateMachine;
            asset.Path = fastFile.DecodedStream.ReadNullTerminatedString();
            asset.DisplayName = Path.GetFileName(asset.Path);
            asset.ExportFunction = ExportFromFastFile;
            asset.AssetType = "animstatemachine";
            asset.StartLocation = fastFile.DecodedStream.BaseStream.Position;
            asset.Info = String.Format("States - {0}", animStateMachine.SubStateCount);
        }

        /// <summary>
        /// Exports an Animation State Machine from a Fast File
        /// </summary>
        public static bool ExportFromFastFile(Asset asset)
        {
            // Total Transitions from all states - Max 127?
            int totalTransitions = 0;

            AnimStateMachine animStateMachine = (AnimStateMachine)asset.Data;

            string[] mainStates = new string[animStateMachine.MainStateCount];

            string assetName = "exported_files\\animstatemachines\\" + asset.Path;

            PathUtil.CreateFilePath(assetName);
            T7Util.ActiveFastFile.DecodedStream.Seek(asset.StartLocation, SeekOrigin.Begin);

            for (int i = 0; i < mainStates.Length; i++)
            {
                // Name from Fast File String Table
                string stateName = T7Util.ActiveFastFile.GetString(T7Util.ActiveFastFile.DecodedStream.ReadInt32() - 1);
                T7Util.ActiveFastFile.DecodedStream.Seek(12, SeekOrigin.Current);
                // Sub states this state has
                int numSubStates = T7Util.ActiveFastFile.DecodedStream.ReadInt32();
                // ???
                T7Util.ActiveFastFile.DecodedStream.Seek(4, SeekOrigin.Current);
                animStateMachine.States[stateName] = new AnimState
                {
                    SubStateCount = numSubStates
                };
                mainStates[i] = stateName;
            }

            // Indexes for in-game possibly?
            T7Util.ActiveFastFile.DecodedStream.Seek(4 * animStateMachine.SubStateCount, SeekOrigin.Current);

            for (int i = 0; i < mainStates.Length; i++)
            {
                AnimState state = animStateMachine.States[mainStates[i]];

                state.substates = new Dictionary<string, AnimState>();

                for (int j = 0; j < state.SubStateCount; j++)
                {

                    string stateName = T7Util.ActiveFastFile.GetString(T7Util.ActiveFastFile.DecodedStream.ReadInt32() - 1);

                    state.substates[stateName] = new AnimState();

                    T7Util.ActiveFastFile.DecodedStream.ReadInt32();
                    int boolValues1 = T7Util.ActiveFastFile.DecodedStream.ReadInt32();
                    int boolValues2 = T7Util.ActiveFastFile.DecodedStream.ReadInt32();

                    T7Util.ActiveFastFile.DecodedStream.ReadInt32();

                    state.substates[stateName].AnimationSelector        = T7Util.ActiveFastFile.GetString(T7Util.ActiveFastFile.DecodedStream.ReadInt32() - 1);
                    state.substates[stateName].AimSelector              = T7Util.ActiveFastFile.GetString(T7Util.ActiveFastFile.DecodedStream.ReadInt32() - 1);
                    state.substates[stateName].ShootSelector            = T7Util.ActiveFastFile.GetString(T7Util.ActiveFastFile.DecodedStream.ReadInt32() - 1);
                    state.substates[stateName].TransitionDecorator      = T7Util.ActiveFastFile.GetString(T7Util.ActiveFastFile.DecodedStream.ReadInt32() - 1);
                    state.substates[stateName].DeltaLayerFunction       = T7Util.ActiveFastFile.GetString(T7Util.ActiveFastFile.DecodedStream.ReadInt32() - 1);
                    state.substates[stateName].TransitionDecorator      = T7Util.ActiveFastFile.GetString(T7Util.ActiveFastFile.DecodedStream.ReadInt32() - 1);
                    state.substates[stateName].ASMClientNotify          = T7Util.ActiveFastFile.GetString(T7Util.ActiveFastFile.DecodedStream.ReadInt32() - 1);

                    // Transitions are stored at end of ASM, assuming in order of each state
                    bool hasTransitions = T7Util.ActiveFastFile.DecodedStream.ReadUInt64() == ulong.MaxValue;
                    int transitionCount = T7Util.ActiveFastFile.DecodedStream.ReadInt32();

                    T7Util.ActiveFastFile.DecodedStream.ReadInt32();

                    totalTransitions += transitionCount;

                    if (hasTransitions)
                        state.substates[stateName].transitions = new Dictionary<string, Transition>();

                    state.substates[stateName].RequiresRagdollNote      = RequiresRagdollNote(boolValues1);
                    state.substates[stateName].Terminal                 = IsTerminal(boolValues2);
                    state.substates[stateName].LoopSync                 = IsLoopSync(boolValues2);
                    state.substates[stateName].CleanLoop                = IsCleanLoop(boolValues2);
                    state.substates[stateName].MultipleDelta            = IsMultipleDelta(boolValues2);
                    state.substates[stateName].Parametric2D             = IsParametric2D(boolValues2);
                    state.substates[stateName].AnimDrivenLocmotion      = IsAnimDrivenLocomotion(boolValues2);
                    state.substates[stateName].Coderate                 = IsCoderate(boolValues2);
                    state.substates[stateName].SpeedBlend               = IsSpeedBlend(boolValues2);
                    state.substates[stateName].AllowTransDecAim         = AllowTransdecAim(boolValues2);
                    state.substates[stateName].ForceFire                = ForceFire(boolValues2);

                    state.substates[stateName].TransitionCount = transitionCount;
                }
            }
            // Like with states, these might be indexes?
            T7Util.ActiveFastFile.DecodedStream.Seek(totalTransitions * 4, SeekOrigin.Current);
            // Process Transitions
            foreach (KeyValuePair<string, AnimState> states in animStateMachine.States)
            {
                foreach (KeyValuePair<string, AnimState> subStates in states.Value.substates)
                {
                    for (int i = 0; i < subStates.Value.TransitionCount; i++)
                    {
                        string transitionName = T7Util.ActiveFastFile.GetString(T7Util.ActiveFastFile.DecodedStream.ReadInt32() - 1);

                        subStates.Value.transitions[transitionName] = new Transition();

                        /*
                         * Rest of these values may just point
                         * parent, etc.
                         */
                        T7Util.ActiveFastFile.DecodedStream.Seek(24, SeekOrigin.Current);

                        subStates.Value.transitions[transitionName].AnimationSelector = T7Util.ActiveFastFile.GetString(T7Util.ActiveFastFile.DecodedStream.ReadInt32() - 1);

                        // Seek to next Transition
                        T7Util.ActiveFastFile.DecodedStream.Seek(16, SeekOrigin.Current);
                    }
                }
            }
            // Save AI_ASM
            animStateMachine.Save(assetName);

            return true;
        }

        /// <summary>
        /// Saves the Animation State Machine to a formatted JSON file
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
                    DefaultValueHandling = DefaultValueHandling.Ignore
                };

                serializer.Serialize(output, this);

            }
        }
    }
}
