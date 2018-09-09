/*
 *  HydraX - Copyright 2018 Philip/Scobalula
 *  
 *  This file is subject to the license terms set out in the
 *  "LICENSE.txt" file. 
 * 
 */
using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhilUtil;

namespace HydraLib.T7.Assets
{
    /// <summary>
    /// Sound Pool Data
    /// </summary>
    class SoundPool
    {
        /// <summary>
        /// Sound Pool Language
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// Number of Sound Aliases (Not including multiple entires of the same alias)
        /// </summary>
        public int AliasCount { get; set; }

        /// <summary>
        /// Location of the Sound Alias Pool
        /// </summary>
        public long PoolAddress { get; set; }
    }

    /// <summary>
    /// Sound Logic
    /// </summary>
    class Sound
    {
        /// <summary>
        /// Sound Alias Logic
        /// </summary>
        class Alias
        {
            /// <summary>
            /// Alias Name
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Entries/variations of this alias
            /// </summary>
            public Entry[] Entries { get; set; }

            /// <summary>
            /// Alias Entry
            /// </summary>
            public class Entry
            {
                /// <summary>
                /// Name of this sound alias. A text string to be used throughout the engine to trigger
                /// sounds. Multiple aliases of the same name that are built into a single sound zone will be treated
                /// as a random situation when they are triggered.
                /// </summary>
                public string Name { get; set; }

                /// <summary>
                /// Not used
                /// </summary>
                public string Behavior { get; set; }

                /// <summary>
                /// Specifies whether the sound is loaded (specify “loaded”) into RAM or
                /// streamed(specify “streamed”), from disk.
                /// </summary>
                public string Storage { get; set; }

                /// <summary>
                /// The path and filename of the physical wav file for this sound alias
                /// </summary>
                public string FileSpec { get; set; }

                /// <summary>
                /// If this column is filled out with a looping asset then it will be triggered
                /// when the(one shot) asset specified in FileSpec finishes.
                /// </summary>
                public string FileSpecSustain { get; set; }

                /// <summary>
                /// If this column is filled out then this asset will be triggered when the
                /// looping asset specified in FileSpecSustain is stopped.
                /// </summary>
                public string FileSpecRelease { get; set; }

                /// <summary>
                /// , this field points to the name of a template alias defined in
                /// a template CSV located in <game>\share\raw\sound\templates\. 
                /// Only used at compile, template is "merged" with alias.
                /// </summary>
                public string Template { get; set; }

                /// <summary>
                /// A name of a loadspec contained in
                /// <game>\share\raw\sound\globals\loadspec.csv.
                /// </summary>
                public string Loadspec { get; set; }

                /// <summary>
                /// – specify another sound alias here and it will be triggered immediately after
                /// the “primary” sound alias.
                /// </summary>
                public string Secondary { get; set; }

                /// <summary>
                /// Not used
                /// </summary>
                public string SustainAlias { get; set; }

                /// <summary>
                /// Not used
                /// </summary>
                public string ReleaseAlias { get; set; }

                /// <summary>
                /// This is the bus that the sound belongs to.
                /// </summary>
                public string Bus { get; set; }

                /// <summary>
                /// FOUND
                /// </summary>
                public string VolumeGroup { get; set; }

                /// <summary>
                /// FOUND
                /// </summary>
                public string DuckGroup { get; set; }

                /// <summary>
                /// FOUND
                /// </summary>
                public string Duck { get; set; }

                /// <summary>
                /// FOUND
                /// </summary>
                public string ReverbSend { get; set; }

                /// <summary>
                /// FOUND
                /// </summary>
                public string CenterSend { get; set; }

                /// <summary>
                /// FOUND
                /// </summary>
                public string VolMin { get; set; }

                /// <summary>
                /// FOUND
                /// </summary>
                public string VolMax { get; set; }

                /// <summary>
                /// FOUND
                /// </summary>
                public string DistMin { get; set; }

                /// <summary>
                /// FOUND
                /// </summary>
                public string DistMaxDry { get; set; }

                /// <summary>
                /// FOUND
                /// </summary>
                public string DistMaxWet { get; set; }

                /// <summary>
                /// TBD
                /// </summary>
                public string DryMinCurve { get; set; }

                /// <summary>
                /// TBD
                /// </summary>
                public string DryMaxCurve { get; set; }

                /// <summary>
                /// TBD
                /// </summary>
                public string WetMinCurve { get; set; }

                /// <summary>
                /// TBD
                /// </summary>
                public string WetMaxCurve { get; set; }

                /// <summary>
                /// FOUND
                /// </summary>
                public string LimitCount { get; set; }

                /// <summary>
                /// FOUND
                /// </summary>
                public string LimitType { get; set; }

                /// <summary>
                /// FOUND
                /// </summary>
                public string EntityLimitCount { get; set; }

                /// <summary>
                /// FOUND
                /// </summary>
                public string EntityLimitType { get; set; }

                /// <summary>
                /// FOUND BUT TBD
                /// </summary>
                public string PitchMin { get; set; }

                /// <summary>
                /// FOUND BUT TBD
                /// </summary>
                public string PitchMax { get; set; }

                /// <summary>
                /// FOUND
                /// </summary>
                public string PriorityMin { get; set; }

                /// <summary>
                /// FOUND
                /// </summary>
                public string PriorityMax { get; set; }

                /// <summary>
                /// FOUND
                /// </summary>
                public string PriorityThresholdMin { get; set; }

                /// <summary>
                /// FOUND
                /// </summary>
                public string PriorityThresholdMax { get; set; }

                /// <summary>
                /// FOUND
                /// </summary>
                public string AmplitudePriority { get; set; }

                /// <summary>
                /// FOUND
                /// </summary>
                public string PanType { get; set; }

                /// <summary>
                /// FOUND
                /// </summary>
                public string Pan { get; set; }

                /// <summary>
                /// NOT USED ???
                /// </summary>
                public string Futz { get; set; }

                /// <summary>
                /// FOUND
                /// </summary>
                public string Looping { get; set; }

                /// <summary>
                /// FOUND
                /// </summary>
                public string RandomizeType { get; set; }

                /// <summary>
                /// FOUND
                /// </summary>
                public string Probability { get; set; }

                /// <summary>
                /// FOUND
                /// </summary>
                public string StartDelay { get; set; }

                /// <summary>
                /// FOUND
                /// </summary>
                public string EnvelopMin { get; set; }

                /// <summary>
                /// FOUND
                /// </summary>
                public string EnvelopMax { get; set; }

                /// <summary>
                /// FOUND
                /// </summary>
                public string EnvelopPercent { get; set; }

                /// <summary>
                /// FOUND
                /// </summary>
                public string OcclusionLevel { get; set; }

                /// <summary>
                /// FOUND
                /// </summary>
                public string IsBig { get; set; }

                /// <summary>
                /// FOUND
                /// </summary>
                public string DistanceLpf { get; set; }

                /// <summary>
                /// FOUND
                /// </summary>
                public string FluxType { get; set; }

                /// <summary>
                /// FOUND
                /// </summary>
                public string FluxTime { get; set; }

                /// <summary>
                /// FOUND
                /// </summary>
                public string Subtitle { get; set; }

                /// <summary>
                /// FOUND
                /// </summary>
                public string Doppler { get; set; }

                /// <summary>
                /// Used in conjunction with SetSoundContext, etc. to enable this sound
                /// under certain "contexts". (underwater, npc/plr, etc.)
                /// </summary>
                public string ContextType { get; set; }

                /// <summary>
                /// Used in conjunction with SetSoundContext, etc. to enable this sound
                /// under certain "contexts". (underwater, npc/plr, etc.)
                /// </summary>
                public string ContextValue { get; set; }

                /// <summary>
                /// Used in conjunction with SetSoundContext, etc. to enable this sound
                /// under certain "contexts". (underwater, npc/plr, etc.)
                /// </summary>
                public string ContextType1 { get; set; }

                /// <summary>
                /// Used in conjunction with SetSoundContext, etc. to enable this sound
                /// under certain "contexts". (underwater, npc/plr, etc.)
                /// </summary>
                public string ContextValue1 { get; set; }

                /// <summary>
                /// Used in conjunction with SetSoundContext, etc. to enable this sound
                /// under certain "contexts". (underwater, npc/plr, etc.)
                /// </summary>
                public string ContextType2 { get; set; }

                /// <summary>
                /// Used in conjunction with SetSoundContext, etc. to enable this sound
                /// under certain "contexts". (underwater, npc/plr, etc.)
                /// </summary>
                public string ContextValue2 { get; set; }

                /// <summary>
                /// Used in conjunction with SetSoundContext, etc. to enable this sound
                /// under certain "contexts". (underwater, npc/plr, etc.)
                /// </summary>
                public string ContextType3 { get; set; }

                /// <summary>
                /// Used in conjunction with SetSoundContext, etc. to enable this sound
                /// under certain "contexts". (underwater, npc/plr, etc.)
                /// </summary>
                public string ContextValue3 { get; set; }

                /// <summary>
                /// FOUND
                /// </summary>
                public string Timescale { get; set; }

                /// <summary>
                /// FOUND
                /// </summary>
                public string IsMusic { get; set; }

                /// <summary>
                /// FOUND
                /// </summary>
                public string IsCinematic { get; set; }

                /// <summary>
                /// FOUND
                /// </summary>
                public string FadeIn { get; set; }

                /// <summary>
                /// FOUND
                /// </summary>
                public string FadeOut { get; set; }

                /// <summary>
                /// FOUND
                /// </summary>
                public string Pauseable { get; set; }

                /// <summary>
                /// FOUND
                /// </summary>
                public string StopOnEntDeath { get; set; }

                /// <summary>
                /// NOT USED ON PC - CONSOLE ONLY
                /// </summary>
                public string Compression { get; set; }

                /// <summary>
                /// TBD
                /// </summary>
                public string StopOnPlay { get; set; }

                /// <summary>
                /// TBD
                /// </summary>
                public string DopplerScale { get; set; }

                /// <summary>
                /// TBD
                /// </summary>
                public string FutzPatch { get; set; }

                /// <summary>
                /// NOT USED
                /// </summary>
                public string VoiceLimit { get; set; }

                /// <summary>
                /// DONE
                /// </summary>
                public string IgnoreMaxDist { get; set; }

                /// <summary>
                /// TBD
                /// </summary>
                public string NeverPlayTwice { get; set; }

                /// <summary>
                /// TBD
                /// </summary>
                public string ContinuousPan { get; set; }

                /// <summary>
                /// TBD
                /// </summary>
                public string FileSource { get; set; }

                /// <summary>
                /// TBD
                /// </summary>
                public string FileSourceSustain { get; set; }

                /// <summary>
                /// TBD
                /// </summary>
                public string FileSourceRelease { get; set; }

                /// <summary>
                /// TBD
                /// </summary>
                public string FileTarget { get; set; }

                /// <summary>
                /// TBD
                /// </summary>
                public string FileTargetSustain { get; set; }

                /// <summary>
                /// TBD
                /// </summary>
                public string FileTargetRelease { get; set; }

                /// <summary>
                /// TBD
                /// </summary>
                public string Platform { get; set; }

                /// <summary>
                /// TBD
                /// </summary>
                public string Language { get; set; }

                /// <summary>
                /// TBD
                /// </summary>
                public string OutputDevices { get; set; }

                /// <summary>
                /// TBD
                /// </summary>
                public string PlatformMask { get; set; }

                /// <summary>
                /// NOT USED - T6 Wii U Specific?
                /// </summary>
                public string WiiUMono { get; set; }

                /// <summary>
                /// TBD
                /// </summary>
                public string StopAlias { get; set; }

                /// <summary>
                /// TBD
                /// </summary>
                public string DistanceLpfMin { get; set; }

                /// <summary>
                /// TBD
                /// </summary>
                public string DistanceLpfMax { get; set; }

                /// <summary>
                /// TBD
                /// </summary>
                public string FacialAnimationName { get; set; }

                /// <summary>
                /// TBD
                /// </summary>
                public string RestartContextLoops { get; set; }

                /// <summary>
                /// TBD
                /// </summary>
                public string SilentInCPZ { get; set; }

                /// <summary>
                /// TBD
                /// </summary>
                public string ContextFailsafe { get; set; }

                /// <summary>
                /// TBD
                /// </summary>
                public string GPAD { get; set; }

                /// <summary>
                /// TBD
                /// </summary>
                public string GPADOnly { get; set; }

                /// <summary>
                /// TBD
                /// </summary>
                public string MuteVoice { get; set; }

                /// <summary>
                /// TBD
                /// </summary>
                public string MuteMusic { get; set; }

                /// <summary>
                /// TBD
                /// </summary>
                public string RowSourceFileName { get; set; }

                /// <summary>
                /// TBD
                /// </summary>
                public string RowSourceShortName { get; set; }

                /// <summary>
                /// TBD
                /// </summary>
                public string RowSourceLineNumber { get; set; }

                /// <summary>
                /// Pitch Values
                /// </summary>
                public static Dictionary<int, int> PitchValues = new Dictionary<int, int>()
                {
                    { 0x1FFF, -2400 },
                    { 0x21E6, -2300 },
                    { 0x23EA, -2200 },
                    { 0x260D, -2100 },
                    { 0x2850, -2000 },
                    { 0x2AB6, -1900 },
                    { 0x2D40, -1800 },
                    { 0x2FF1, -1700 },
                    { 0x32CB, -1600 },
                    { 0x35D0, -1500 },
                    { 0x3904, -1400 },
                    { 0x3C67, -1300 },
                    { 0x3FFF, -1200 },
                    { 0x43CD, -1100 },
                    { 0x47D5, -1000 },
                    { 0x4C1B, -900  },
                    { 0x50A1, -800  },
                    { 0x556D, -700  },
                    { 0x5A81, -600  },
                    { 0x5FE3, -500  },
                    { 0x6597, -400  },
                    { 0x6BA1, -300  },
                    { 0x7208, -200  },
                    { 0x78CF, -100  },
                    { 0x7FFF, -0    },
                    { 0x879B, 100   },
                    { 0x8FAB, 200   },
                    { 0x9836, 300   },
                    { 0xA143, 400   },
                    { 0xAADA, 500   },
                    { 0xB503, 600   },
                    { 0xBFC7, 700   },
                    { 0xCB2E, 800   },
                    { 0xD743, 900   },
                    { 0xE410, 1000  },
                    { 0xF19F, 1100  },
                    { 0xFFFE, 1200  }
                };

                /// <summary>
                /// Volumne Groups
                /// </summary>
                public static string[] VolumeGroups =
                {
                    "grp_air",
                    "grp_alerts",
                    "grp_ambience",
                    "grp_announcer",
                    "grp_bink",
                    "grp_destructible",
                    "grp_explosion",
                    "grp_foley",
                    "grp_hdrfx",
                    "grp_igc",
                    "grp_impacts",
                    "grp_lfe",
                    "grp_master",
                    "grp_menu",
                    "grp_mp_game",
                    "grp_music",
                    "grp_physics",
                    "grp_player_foley",
                    "grp_player_impacts",
                    "grp_reference",
                    "grp_scripted_moment",
                    "grp_set_piece",
                    "grp_vehicle",
                    "grp_voice",
                    "grp_weapon",
                    "grp_weapon_3p",
                    "grp_weapon_decay_3p",
                    "grp_whizby",
                    "grp_wpn_lfe",
                };

                /// <summary>
                /// Pans
                /// </summary>
                public static string[] Pans =
                {
                    "default",
                    "music",
                    "wpn_all",
                    "wpn_fnt",
                    "wpn_rear",
                    "wpn_left",
                    "wpn_right",
                    "music_all",
                    "fly_foot_all",
                    "front",
                    "back",
                    "front_mostly",
                    "back_mostly",
                    "all",
                    "center",
                    "center_mostly",
                    "front_and_center",
                    "lfe",
                    "quad",
                    "front_mostly_some_center",
                    "front_halfback",
                    "halffront_back",
                    "test",
                    "brass_right",
                    "brass_left",
                    "veh_back",
                    "tst_left",
                    "tst_center",
                    "tst_right",
                    "tst_surround_left",
                    "tst_surround_right",
                    "tst_lfe",
                    "tst_back_left",
                    "tst_back_right",
                    "pip",
                    "movie_vo",
                };

                /// <summary>
                /// Flux Types
                /// </summary>
                public static string[] FluxTypes =
                {
                    "left_player",
                    "center_player",
                    "right_player",
                    "random_player",
                    "left_shot",
                    "center_shot",
                    "right_shot",
                    "random_direction",
                };

                /// <summary>
                /// Sound Duck Groups
                /// </summary>
                public static string[] DuckGroups =
                {
                    "snp_alerts_gameplay",
                    "snp_ambience",
                    "snp_claw",
                    "snp_destructible",
                    "snp_dying",
                    "snp_dying_ice",
                    "snp_evt_2d",
                    "snp_explosion",
                    "snp_foley",
                    "snp_grenade",
                    "snp_hdrfx",
                    "snp_igc",
                    "snp_impacts",
                    "snp_menu",
                    "snp_movie",
                    "snp_music",
                    "snp_never_duck",
                    "snp_player_dead",
                    "snp_player_impacts",
                    "snp_scripted_moment",
                    "snp_set_piece",
                    "snp_special",
                    "snp_vehicle",
                    "snp_vehicle_interior",
                    "snp_voice",
                    "snp_voice_announcer",
                    "snp_weapon_decay_1p",
                    "snp_whizby",
                    "snp_wpn_1p",
                    "snp_wpn_1p_act",
                    "snp_wpn_1p_ads",
                    "snp_wpn_1p_shot",
                    "snp_wpn_3p",
                    "snp_wpn_3p_decay",
                    "snp_wpn_turret",
                    "snp_x2",
                    "snp_x3",
                    "snp_distant_explosions",
                };

                /// <summary>
                /// Randomization Types
                /// </summary>
                public static string[] RandomizeTypes =
                {
                    "variant",
                    "volume",
                    "pitch",
                };

                /// <summary>
                /// Sound Busses (for memu)
                /// </summary>
                public static string[] Buses =
                {
                    "BUS_FX",
                    "BUS_VOICE",
                    "BUS_PFUTZ",
                    "BUS_HDRFX",
                    "BUS_UI",
                    "BUS_MUSIC",
                    "BUS_MOVIE",
                    "BUS_REFERENCE",
                };

                /// <summary>
                /// Hashed Strings (Duck and Context Values reference this dictionary)
                /// </summary>
                public static Dictionary<uint, string> HashedStrings = new Dictionary<uint, string>()
                {
                    { 0xFDBFA7F2         , "aquifer_cockpit" },
                    { 0xA8E1E3AB         , "active" },
                    { 0x3C962EFD         , "battle" },
                    { 0x59542E3A         , "silent" },
                    { 0x6DFCA373         , "slomo" },
                    { 0xBE1CADD7         , "boot" },
                    { 0x32C00D01         , "wet" },
                    { 0x850E1232         , "foley" },
                    { 0x9D0CDF4C         , "normal" },
                    { 0x2BDD3460         , "igc" },
                    { 0xC81277D0         , "healthstate" },
                    { 0x2489C328         , "human" },
                    { 0x85A08194         , "cyber" },
                    { 0xFE6241D9         , "infected" },
                    { 0x5A2AFE44         , "on" },
                    { 0x770DEBDB         , "laststand" },
                    { 0x31026DAD         , "mature" },
                    { 0x97C90F59         , "explicit" },
                    { 0xCE22AF32         , "safe" },
                    { 0x8D844C74         , "movement" },
                    { 0x8F66D6B7         , "loud" },
                    { 0x85E4DD2F         , "quiet" },
                    { 0xA1060CD7         , "perspective" },
                    { 0xEC996326         , "player" },
                    { 0x2E5C841C         , "npc" },
                    { 0x9147790          , "plr_body" },
                    { 0x390D0114         , "reaper" },
                    { 0x378CB4EF         , "prophet" },
                    { 0xA4EB18AA         , "nomad" },
                    { 0x64AF02D1         , "outrider" },
                    { 0x5ED5591E         , "seraph" },
                    { 0xA978154D         , "ruin" },
                    { 0xD2DCE0A          , "assassin" },
                    { 0x4E6BB40B         , "pyro" },
                    { 0x453270DA         , "grenadier" },
                    { 0x2E25DAEF         , "plr_gender" },
                    { 0xB6FFCC32         , "male" },
                    { 0x55EE5431         , "female" },
                    { 0xD57307F4         , "plr_impact" },
                    { 0x3241FD74         , "veh" },
                    { 0x168AC66          , "pwr_armor" },
                    { 0xDB25FBBE         , "plr_stance" },
                    { 0xDB4F5091         , "stand" },
                    { 0xED47D8DF         , "crouch" },
                    { 0xB9616B1F         , "prone" },
                    { 0x12D993FE         , "plr_vehicle" },
                    { 0xECCB65AD         , "driver" },
                    { 0x80773E11         , "ringoff_plr" },
                    { 0x5FED5218         , "indoor" },
                    { 0xCCAF6B37         , "outdoor" },
                    { 0xCAA43AE4         , "underwater" },
                    { 0xC2290CE3         , "train" },
                    { 0x554BCA71         , "country" },
                    { 0xE9B422D0         , "city" },
                    { 0x82E4D            , "tunnel" },
                    { 0xF8B7C1A7         , "vehicle" },
                    { 0xE9552675         , "interior" },
                    { 0xBABBB083         , "exterior" },
                    { 0x805172D2         , "water" },
                    { 0x4D705673         , "under" },
                    { 0x1E5DB199         , "over" },
                    { 0x57A27B3F         , "amb_battle_ducker" },
                    { 0x62F3D53          , "amb_battle_ducker_half" },
                    { 0xB2CA4596         , "amb_lightning_indoor" },
                    { 0x346DB54F         , "amb_lightning_sewer" },
                    { 0xE7B4A72E         , "cmn_3p_weapon_occ" },
                    { 0x720F1959         , "cmn_aar_screen" },
                    { 0x2A05C2D4         , "cmn_bleedout" },
                    { 0x3473D6FF         , "cmn_bo3_load" },
                    { 0x370FAEF0         , "cmn_cc_securitybreach" },
                    { 0x90B85A39         , "cmn_cc_securitybreach_trans" },
                    { 0x7A6D9156         , "cmn_cc_tacmenu" },
                    { 0xE710269          , "cmn_dead_turret_exp" },
                    { 0xD749D398         , "cmn_death_coop" },
                    { 0x63AAABAB         , "cmn_death_plr" },
                    { 0xBFB3DECA         , "cmn_death_solo" },
                    { 0xEB5226C3         , "cmn_dni_interrupt" },
                    { 0x734D115B         , "cmn_duck_all" },
                    { 0x1F700AAE         , "cmn_duck_all_but_movie" },
                    { 0xFE51655F         , "cmn_duck_music" },
                    { 0x994D3606         , "cmn_duck_music_dist" },
                    { 0xE7179C80         , "cmn_duck_underscore" },
                    { 0xADC513C7         , "cmn_duck_underscore_and_round" },
                    { 0x1E4A10FD         , "cmn_health_laststand" },
                    { 0x9D1BDD11         , "cmn_health_low" },
                    { 0x144AE81E         , "cmn_health_resurrect" },
                    { 0x96BC2038         , "cmn_igc_amb_silent" },
                    { 0xD3041C21         , "cmn_igc_bg_lower" },
                    { 0xAA82869F         , "cmn_igc_foley_lower" },
                    { 0x64A9F439         , "cmn_igc_skip" },
                    { 0xA33BDD93         , "cmn_jump_land_plr" },
                    { 0x8E301056         , "cmn_kill_foley" },
                    { 0xFCE9FB57         , "cmn_level_fadeout" },
                    { 0xDCA66709         , "cmn_level_fade_immediate" },
                    { 0x3FCD9007         , "cmn_level_start" },
                    { 0x9BFDECE1         , "cmn_melee_pain" },
                    { 0xF1DB377A         , "cmn_music_quiet" },
                    { 0x724BE457         , "cmn_no_vo" },
                    { 0xF0FE02EC         , "cmn_override_duck" },
                    { 0xB4F9114D         , "cmn_pain_plr" },
                    { 0x6D5C916F         , "cmn_qtank_railgun_shot" },
                    { 0xD263BA2E         , "cmn_raps_spawn" },
                    { 0xDE71D8AF         , "cmn_robot_behind" },
                    { 0x8389F925         , "cmn_shock_tinitus" },
                    { 0x934DBB8C         , "cmn_sniper_fire_3rd" },
                    { 0x49E144FD         , "cmn_swimming" },
                    { 0x8D3D88E9         , "cmn_time_freeze" },
                    { 0xCECA2457         , "cmn_ui_tac_mode" },
                    { 0x62323141         , "cmn_wallrun" },
                    { 0xF4A1624          , "cod_ads" },
                    { 0xB9DDDA1A         , "cod_alloff" },
                    { 0x628C7954         , "cod_allon" },
                    { 0x892D7D94         , "cod_doa_fps" },
                    { 0xC4738B0D         , "cod_fadein" },
                    { 0x9ED46559         , "cod_hipfire" },
                    { 0xFCF06494         , "cod_hold_breath" },
                    { 0x741C7F4D         , "cod_matureduck" },
                    { 0x82A4A8B          , "cod_menu" },
                    { 0xCC0918EC         , "cod_mpl_combat_awareness" },
                    { 0xD9F53E8F         , "cod_test_alias" },
                    { 0xE996F34C         , "cod_test_env" },
                    { 0x6EC2083          , "cod_xcam" },
                    { 0x5AACC08E         , "cp_aquifer_breach" },
                    { 0xCA84F094         , "cp_aquifer_breach_slowmo" },
                    { 0xB7F92C1F         , "cp_aquifer_drown_evt" },
                    { 0xE5A3B5B2         , "cp_aquifer_int" },
                    { 0xA53C2BF9         , "cp_aquifer_int_deep" },
                    { 0x9A6C086E         , "cp_aquifer_outro" },
                    { 0xF98CB254         , "cp_aquifer_pip_HeroLocation" },
                    { 0x4C7C1142         , "cp_aquifer_script_jet" },
                    { 0xD7B9695C         , "cp_aquifer_underwater" },
                    { 0x984604F7         , "cp_aquifer_veh_dogfight" },
                    { 0x85BF974E         , "cp_aquifer_veh_exp_hit" },
                    { 0x1DDCEF4C         , "cp_aquifer_veh_int" },
                    { 0x2134DD69         , "cp_barge_slowtime" },
                    { 0x21A4FB2F         , "cp_biodome2_slide_prj" },
                    { 0x4432AB71         , "cp_biodomes_2_end_vehicle" },
                    { 0x4BD5458          , "cp_biodomes_dive_duckambience" },
                    { 0xFC4AFD7D         , "cp_biodomes_supertree_collapse" },
                    { 0x19E697FB         , "cp_biodomes_vtol_esc" },
                    { 0x5DCF51D1         , "cp_biodome_acc_drive_igc" },
                    { 0xB08090E4         , "cp_biodome_acc_drive_igc_2" },
                    { 0x2768D6E2         , "cp_biodome_supertree_vtol" },
                    { 0x176AC7F5         , "cp_blackstation_boatride" },
                    { 0x11611B63         , "cp_blackstation_boatride_getoff" },
                    { 0xD23755CB         , "cp_blackstation_boatride_geton" },
                    { 0x124DB396         , "cp_blackstation_data_recorder" },
                    { 0xCF2A4F8          , "cp_blackstation_debris" },
                    { 0x39915100         , "cp_blackstation_debris_small" },
                    { 0xD28979A9         , "cp_blackstation_intro_veh" },
                    { 0x6F44CBC          , "cp_blackstation_intro_veh_2" },
                    { 0x3A75224E         , "cp_blackstation_outro" },
                    { 0xF363D940         , "cp_blackstation_scripted_wind" },
                    { 0xC7CE1E67         , "cp_blackstation_thunder" },
                    { 0xF153BCE6         , "cp_blackstation_warlord_igc" },
                    { 0xEF1E52E7         , "cp_cybercore_activate" },
                    { 0xFAEA02EF         , "cp_cybercore_ready" },
                    { 0x9B874EDB         , "cp_cybercore_unstoppable" },
                    { 0x653D6895         , "cp_dialog" },
                    { 0xD66AA808         , "cp_infection_flyaway" },
                    { 0x8937E10A         , "cp_infection_hideout_amb" },
                    { 0x72687B60         , "cp_infection_interface" },
                    { 0xDE8FDCD3         , "cp_infection_intro" },
                    { 0x44A7BAE6         , "cp_infection_intro_2" },
                    { 0x852F17E0         , "cp_infection_intro_imp" },
                    { 0x9321F700         , "cp_infection_labdeath" },
                    { 0x9FC68C1C         , "cp_infection_qt_birth" },
                    { 0x67AC9732         , "cp_infection_testlab_transition" },
                    { 0xA6691368         , "cp_life_vinetrans" },
                    { 0x17FE024D         , "cp_lotus_delusion_overlay" },
                    { 0x589D6984         , "cp_lotus_hospital_fade" },
                    { 0x37D86E70         , "cp_lotus_vtol_bridge" },
                    { 0x9AE9EF           , "cp_lotus_vtol_hallway" },
                    { 0xBB56E530         , "cp_mi_sing_vengeance_slowmo" },
                    { 0x9349D3E5         , "cp_newworld_pipes" },
                    { 0x1FB644FD         , "cp_prologue_apc_door_close" },
                    { 0x48F17350         , "cp_prologue_apc_duck_explo" },
                    { 0x1CC2DA89         , "cp_prologue_duck_apc_lps" },
                    { 0x82BDB1FE         , "cp_prologue_exit_apc" },
                    { 0x2044053F         , "cp_prologue_outro_rippedapart" },
                    { 0x6F4F0B07         , "cp_prologue_outro_runup" },
                    { 0x8A694EA4         , "cp_prologue_vtolflyby" },
                    { 0x52520787         , "cp_ramses_celing" },
                    { 0xD5C1EE27         , "cp_ramses_demostreet_1" },
                    { 0xD5C1EE28         , "cp_ramses_demostreet_2" },
                    { 0xD5C1EE29         , "cp_ramses_demostreet_3" },
                    { 0x6EED2F63         , "cp_ramses_intro_igc" },
                    { 0x906D66C3         , "cp_ramses_int_dead" },
                    { 0x685528FA         , "cp_ramses_int_veh" },
                    { 0x1B75B840         , "cp_ramses_jeep_drive" },
                    { 0xC9C944D5         , "cp_ramses_nasser_igc" },
                    { 0x40952F1C         , "cp_ramses_outro" },
                    { 0x41FC1B5A         , "cp_ramses_plaza_battle" },
                    { 0x704E778          , "cp_ramses_preplaza" },
                    { 0x5EE6E3C6         , "cp_ramses_pre_vtol" },
                    { 0x397DBE08         , "cp_ramses_qt_vtol" },
                    { 0x87443C50         , "cp_ramses_qt_wallcrash" },
                    { 0x83EA541C         , "cp_ramses_quad_music" },
                    { 0x7CF1828E         , "cp_ramses_raps_intro" },
                    { 0x512C6C1F         , "cp_ramses_streetcollapse" },
                    { 0x13EB7E43         , "cp_ramses_theater_explo" },
                    { 0xFF3F7059         , "cp_ramses_trans" },
                    { 0x48C35A50         , "cp_ramses_vtol_fall" },
                    { 0xB6C87B5B         , "cp_ramses_vtol_impact" },
                    { 0x5FB736FE         , "cp_ramses_vtol_walk" },
                    { 0x867431A          , "cp_ramses_wall_3p_gunfire" },
                    { 0xC9BEC057         , "cp_safehouse_exit" },
                    { 0xA759B0C7         , "cp_sgen_base_explo" },
                    { 0xA4CEBE99         , "cp_sgen_flooding" },
                    { 0x9B96172          , "cp_sgen_flyover" },
                    { 0x5809DF20         , "cp_sgen_ghost_igc" },
                    { 0xD42A525D         , "cp_sgen_steam_duck" },
                    { 0xD9AE7373         , "cp_sgen_twinrevenge_igc" },
                    { 0x22684065         , "cp_sgen_uw_boulder" },
                    { 0x690C6B6E         , "cp_sgen_wave" },
                    { 0xA4C3238          , "cp_sh_cairo_foley_low" },
                    { 0xA972591D         , "cp_vengeance_cafe" },
                    { 0x87D5598F         , "cp_vengeance_int" },
                    { 0x86863C5C         , "cp_vengeance_int_deep" },
                    { 0xC5D09945         , "cp_warlord_melee" },
                    { 0xE176A162         , "cp_zmb_box_3d" },
                    { 0x42AEC0C4         , "cp_zmb_ending" },
                    { 0x700FE2C5         , "cp_zmb_thelooper" },
                    { 0x8B9B50E1         , "cp_zmb_timefreeze" },
                    { 0xDBF3C435         , "cp_zmb_voice" },
                    { 0x19E2FEAE         , "cp_zurich_duckrabbit" },
                    { 0x8B5CB328         , "cp_zurich_end_duckexplo" },
                    { 0xCD080995         , "cp_zurich_movie" },
                    { 0xE5F706           , "cp_zurich_movie_long" },
                    { 0x29BBCF4D         , "cp_zurich_train" },
                    { 0xF680CFBC         , "default" },
                    { 0x14E44A23         , "dummy" },
                    { 0x70EDC08D         , "exp_barrel" },
                    { 0x3BC2AC7          , "exp_grenade" },
                    { 0xE4C9783C         , "exp_medium" },
                    { 0x1CBB3E5C         , "exp_mortar" },
                    { 0x2B7FE0A5         , "exp_quad_rocket" },
                    { 0xB2722054         , "exp_rocket_close" },
                    { 0x89BE6CB          , "exp_rocket_quad" },
                    { 0xBE1A36A0         , "exp_small" },
                    { 0x54265E65         , "exp_vehicle" },
                    { 0x548630DE         , "exp_vehicle_close" },
                    { 0x953869EE         , "mpl_announcer" },
                    { 0x76A6C999         , "mpl_death" },
                    { 0x11AAB05E         , "mpl_duck_announcer" },
                    { 0x120D7241         , "mpl_duck_holoscreen" },
                    { 0x93C7C225         , "mpl_endmatch" },
                    { 0xCADE9E4D         , "mpl_final_killcam" },
                    { 0x9121FD95         , "mpl_final_killcam_slowdown" },
                    { 0x45B83F31         , "mpl_hellstorm" },
                    { 0xC0A241CA         , "mpl_postmatch" },
                    { 0x36B7DBE1         , "mpl_post_match" },
                    { 0x462809D          , "mpl_prematch" },
                    { 0xA0808543         , "mpl_speedboost_run" },
                    { 0xAF7FEE02         , "prj_impact" },
                    { 0x94450659         , "prj_impact_plr" },
                    { 0x58EE3295         , "prj_whizby" },
                    { 0xA19754E6         , "wpn_cmn_burst_3p" },
                    { 0xFCD577F8         , "wpn_cmn_shot_3p" },
                    { 0xCE86373B         , "wpn_cmn_shot_plr" },
                    { 0x36B86475         , "wpn_cmn_suppressed_plr" },
                    { 0x945C16D6         , "wpn_hunter_missile" },
                    { 0x4927A700         , "wpn_jet_rocket_plr" },
                    { 0x60191BA2         , "wpn_melee_knife_plr" },
                    { 0xE66DFA51         , "wpn_rpg_plr" },
                    { 0x2E895CD1         , "wpn_shotgun_sci" },
                    { 0xF41A026C         , "wpn_shoulder_shot_npc" },
                    { 0xD5B16432         , "wpn_sniper_shot_plr" },
                    { 0x330A3541         , "wpn_turret_npc" },
                    { 0x34025356         , "wpn_turret_plr" },
                    { 0x17229E88         , "zmb_apothifight_beam" },
                    { 0x351E8486         , "zmb_beastmode_enter" },
                    { 0xF88C2309         , "zmb_bgb_fearinheadlights" },
                    { 0x5DE9E91A         , "zmb_bgb_killingtime" },
                    { 0xC15BEB77         , "zmb_castle_bossbattle" },
                    { 0xAAC84C32         , "zmb_castle_bossbattle_event" },
                    { 0x83974EC1         , "zmb_castle_duck_evt_3d" },
                    { 0x72F33E79         , "zmb_castle_outro" },
                    { 0x5CDCAEF9         , "zmb_castle_timetravel" },
                    { 0x8A78E805         , "zmb_cmn_mk3_orb" },
                    { 0xAA18BC01         , "zmb_cosmo_intro" },
                    { 0xA475DE02         , "zmb_cp_song_suppress" },
                    { 0x2DAB7127         , "zmb_derriese_start" },
                    { 0x31D6507D         , "zmb_dialog" },
                    { 0x16B36AD4         , "zmb_dialog_2d" },
                    { 0x9784980F         , "zmb_dialog_monty" },
                    { 0x8B0A78DF         , "zmb_duck_close_vehicles" },
                    { 0x866CADBC         , "zmb_duck_music_3d" },
                    { 0x5DB9A18F         , "zmb_easter_egg_song" },
                    { 0x2736A34C         , "zmb_game_over" },
                    { 0xFA69CCAA         , "zmb_game_start" },
                    { 0xFF17DE32         , "zmb_game_start_nofade" },
                    { 0x2E0A7E39         , "zmb_hd_game_start_nofade" },
                    { 0x495426C6         , "zmb_health_low" },
                    { 0x8966D755         , "zmb_island_dopple_scream" },
                    { 0xB9B1301C         , "zmb_island_duck_bg_music" },
                    { 0xB098EEF1         , "zmb_island_hallucinate" },
                    { 0x3B5DF753         , "zmb_island_inside_thrasher" },
                    { 0x1C860C72         , "zmb_island_swimming" },
                    { 0x90A12A93         , "zmb_island_takeo" },
                    { 0xA694E741         , "zmb_island_trial" },
                    { 0xC51DDF6B         , "zmb_laststand" },
                    { 0xC7994A85         , "zmb_margwa_smash" },
                    { 0xFD56227C         , "zmb_moon_gasmask" },
                    { 0xC1563D7D         , "zmb_moon_space" },
                    { 0x6BDC6DF2         , "zmb_radio_duck" },
                    { 0xCDE238E1         , "zmb_sophia" },
                    { 0x9261F1F9         , "zmb_stalingrad_pa_destruct" },
                    { 0x7C27C9FC         , "zmb_stal_boss_fight" },
                    { 0x28F00F42         , "zmb_stal_dragon_fight" },
                    { 0x53ECAD8D         , "zmb_stal_outro" },
                    { 0x297961D7         , "zmb_stal_tbc" },
                    { 0xB3584229         , "zmb_temple_meteor" },
                    { 0x990E817E         , "zmb_temple_radio" },
                    { 0x37C6BFD3         , "zmb_umonkey" },
                    { 0xC2F80042         , "zmb_zhd_laststand" },
                    { 0x9F3BD446         , "zmb_zod_apothibattle_duck" },
                    { 0x59FDD9A2         , "zmb_zod_apothigod" },
                    { 0x4BE4B87D         , "zmb_zod_beastmode" },
                    { 0x7AEE79F7         , "zmb_zod_cursed" },
                    { 0xC1501851         , "zmb_zod_duck_octobomb_lp" },
                    { 0x3B869E6F         , "zmb_zod_endigc" },
                    { 0x102D05D4         , "zmb_zod_scream" },
                    { 0x94489423         , "zmb_zod_shadbattle_duck" },
                    { 0x6FD35478         , "zmb_zod_sword" },
                    { 0x6A7C6699         , "zmb_zod_sword_powerup" },
                    { 0x2E1119D0         , "zmb_zod_teleport" },
                    { 0x61205857         , "zmb_zod_totem_charge" },
                };

                /// <summary>
                /// Returns a string that matches hash from string hashed string table.
                /// </summary>
                /// <param name="hash">Hash Value</param>
                /// <returns>String if in table, otherwise empty string</returns>
                public static string GetHashedString(uint hash)
                {
                    return HashedStrings.ContainsKey(hash) ? HashedStrings[hash] : ""; 
                }

                /// <summary>
                /// Gets Randomize Type from last 2 bits from Alias values int.
                /// </summary>
                /// <param name="value">Int with Alias values</param>
                /// <returns>Randomize Type</returns>
                public static string GetRandomizeType(int value)
                {
                    return RandomizeTypes?.ElementAtOrDefault((value & 128) >> 6);
                }

                /// <summary>
                /// Gets Volume Group as a string from Alias Int.
                /// </summary>
                /// <param name="index">Volume Group Index</param>
                /// <returns>Volume Group Name</returns>
                public static string GetVolumeGroup(int index)
                {
                    return VolumeGroups?.ElementAtOrDefault(index);
                }

                /// <summary>
                /// Returns pan based off index
                /// </summary>
                /// <param name="index">Index of pan</param>
                /// <returns>Pan as a string if in table, otherwise null.</returns>
                public static string GetPan(int index)
                {
                    return Pans?.ElementAtOrDefault(index);
                }

                /// <summary>
                /// Returns a duckgroup based off index
                /// </summary>
                /// <param name="index">Index of duckgroup</param>
                /// <returns>DuckGroup as a string if in table, otherwise null.</returns>
                public static string GetDuckGroup(int index)
                {
                    return DuckGroups?.ElementAtOrDefault(index);
                }

                /// <summary>
                /// Gets limit type
                /// </summary>
                /// <param name="input"></param>
                /// <param name="position"></param>
                /// <returns></returns>
                public static string GetLimitType(int input, int position)
                {
                    if (ByteUtil.GetBit(input, position) && ByteUtil.GetBit(input, position + 1))
                    {
                        return "priority";
                    }
                    else if (ByteUtil.GetBit(input, position) && !ByteUtil.GetBit(input, position + 1))
                    {
                        return "oldest";
                    }
                    else if (!ByteUtil.GetBit(input, position) && ByteUtil.GetBit(input, position + 1))
                    {
                        return "reject";
                    }
                    else
                    {
                        return "none";
                    }
                }

                /// <summary>
                /// Gets index from Alias values and returns Flux Type
                /// </summary>
                /// <param name="value">Alias Value @ Byte 111</param>
                /// <returns>Flux Type</returns>
                public static string GetFluxType(int value)
                {
                    return FluxTypes?.ElementAtOrDefault((value & 0x3C0) >> 6);
                }

                /// <summary>
                /// Gets Bus
                /// </summary>
                /// <param name="index"></param>
                /// <returns></returns>
                public static string GetBus(int index)
                {
                    return Buses?.ElementAtOrDefault(index);
                }

                /// <summary>
                /// Returns closes pitched, accurate to the next 100.
                /// </summary>
                /// <param name="value">Pitch Value</param>
                /// <returns>Pitch Value between -3600 and </returns>
                public static int GetPitch(int value)
                {
                    for(int i = value; i < 0xFFFE; i++)
                        if (PitchValues.ContainsKey(i))
                            return PitchValues[i];

                    return 0;
                }

                /// <summary>
                /// Calculates alias values that are between 0-100 (Vol, Reverb etc)
                /// </summary>
                /// <param name="input">Input Value</param>
                /// <returns>Value between 0-100</returns>
                public static int Calculate100Value(int input)
                {
                    if (input == 0)
                        return 0;

                    // this fucking works i dont give a fuck
                    double x = 0x3F800000 - input;
                    x = x / 140608402.0;
                    x = 1 - x;
                    x = x * 100.0;


                    return (int)x;
                }

                /// <summary>
                /// Loads a sound alias entry
                /// </summary>
                /// <param name="buffer">Alias buffer</param>
                public void LoadEntry(byte[] buffer)
                {
                    using (BinaryReader reader = new BinaryReader(new MemoryStream(buffer)))
                    {
                        // A lot of values here are skipped due to being hashes, etc.
                        reader.Seek(16, SeekOrigin.Current);

                        long subtitlePtr = reader.ReadInt64();
                        if (subtitlePtr > 0)
                            Subtitle = MemoryUtil.ReadNullTerminatedString(T7Util.ActiveProcess, subtitlePtr);

                        long secondaryPtr = reader.ReadInt64();
                        if (secondaryPtr > 0)
                            Secondary = MemoryUtil.ReadNullTerminatedString(T7Util.ActiveProcess, secondaryPtr);

                        reader.Seek(32, SeekOrigin.Current);

                        long fileSpecPtr = reader.ReadInt64();
                        if (fileSpecPtr > 0)
                            FileSpec = MemoryUtil.ReadNullTerminatedString(T7Util.ActiveProcess, fileSpecPtr).Split('.')[0] + ".wav";

                        reader.Seek(8, SeekOrigin.Current);

                        long fileSpecSustainPtr = reader.ReadInt64();
                        if (fileSpecSustainPtr > 0)
                            FileSpecSustain = MemoryUtil.ReadNullTerminatedString(T7Util.ActiveProcess, fileSpecSustainPtr).Split('.')[0] + ".wav";

                        reader.Seek(8, SeekOrigin.Current);

                        long fileSpecReleasePtr = reader.ReadInt64();
                        if (fileSpecReleasePtr > 0)
                            FileSpecRelease = MemoryUtil.ReadNullTerminatedString(T7Util.ActiveProcess, fileSpecReleasePtr).Split('.')[0] + ".wav";

                        int values104 = reader.ReadByte();
                        int values105 = reader.ReadByte();
                        int values106 = reader.ReadByte();
                        int values107 = reader.ReadByte();
                        int values108 = reader.ReadByte();
                        int values109 = reader.ReadByte();
                        int values110 = reader.ReadByte();
                        int values111 = reader.ReadByte();

                        IgnoreMaxDist     = ByteUtil.GetBit(values105, 3) ? "yes" : "no";
                        Pauseable         = ByteUtil.GetBit(values104, 4) ? "yes" : "no";
                        PanType           = ByteUtil.GetBit(values104, 1) ? "3d" : "2d";
                        Storage           = ByteUtil.GetBit(values105, 4)? "loaded" : "streamed";
                        Timescale         = ByteUtil.GetBit(values105, 1) ? "yes" : "no";
                        IsCinematic       = ByteUtil.GetBit(values111, 3) ? "yes" : "no";
                        NeverPlayTwice    = ByteUtil.GetBit(values108, 3) ? "yes" : "no";
                        Looping           = ByteUtil.GetBit(values104, 0) ? "looping" : "nonlooping";
                        IsMusic           = ByteUtil.GetBit(values104, 7) ? "yes" : "no";
                        IsBig             = ByteUtil.GetBit(values111, 4) ? "yes" : "no";
                        AmplitudePriority = ByteUtil.GetBit(values111, 5) ? "yes" : "no";
                        DistanceLpf       = ByteUtil.GetBit(values104, 4) ? "yes" : "no";
                        Doppler           = ByteUtil.GetBit(values104, 5) ? "yes" : "no";
                        StopOnEntDeath    = ByteUtil.GetBit(values106, 0) ? "yes" : "no";

                        LimitType         = GetLimitType(values106, 2);
                        EntityLimitType   = GetLimitType(values106, 4);
                        RandomizeType     = GetRandomizeType(values106);

                        Duck            = GetHashedString(reader.ReadUInt32());
                        ContextType     = GetHashedString(reader.ReadUInt32());
                        ContextValue    = GetHashedString(reader.ReadUInt32());
                        ContextType1    = GetHashedString(reader.ReadUInt32());
                        ContextValue1   = GetHashedString(reader.ReadUInt32());
                        ContextType2    = GetHashedString(reader.ReadUInt32());
                        ContextValue2   = GetHashedString(reader.ReadUInt32());
                        ContextType3    = GetHashedString(reader.ReadUInt32());
                        ContextValue3   = GetHashedString(reader.ReadUInt32());

                        reader.Seek(8, SeekOrigin.Current);

                        ReverbSend     = Calculate100Value(reader.ReadInt32()).ToString();
                        CenterSend     = Calculate100Value(reader.ReadInt32()).ToString();
                        VolMin         = Calculate100Value(reader.ReadInt32()).ToString();
                        VolMax         = Calculate100Value(reader.ReadInt32()).ToString();
                        EnvelopPercent = Calculate100Value(reader.ReadInt32()).ToString();

                        FluxTime   = reader.ReadInt16().ToString();
                        StartDelay = reader.ReadInt16().ToString();

                        PitchMin = GetPitch(reader.ReadUInt16()).ToString();
                        PitchMax = GetPitch(reader.ReadUInt16()).ToString();

                        DistMin = (reader.ReadUInt16() * 2).ToString();
                        DistMaxDry = (reader.ReadUInt16() * 2).ToString();
                        DistMaxWet = (reader.ReadUInt16() * 2).ToString();
                        EnvelopMin = (reader.ReadUInt16() * 2).ToString();
                        EnvelopMax = (reader.ReadUInt16() * 2).ToString();

                        reader.Seek(4, SeekOrigin.Current);

                        FadeIn  = reader.ReadUInt16().ToString();
                        FadeOut = reader.ReadUInt16().ToString();
                        FadeOut = reader.ReadUInt16().ToString();

                        PriorityThresholdMin = Math.Round(reader.ReadByte() / 255.0, 2).ToString();
                        PriorityThresholdMax = Math.Round(reader.ReadByte() / 255.0, 2).ToString();
                        Probability          = Math.Round(reader.ReadByte() / 255.0, 2).ToString();
                        OcclusionLevel       = Math.Round(reader.ReadByte() / 255.0, 2).ToString();

                        PriorityMin      = reader.ReadByte().ToString();
                        PriorityMax      = reader.ReadByte().ToString();
                        Pan              = GetPan(reader.ReadByte());
                        LimitCount       = reader.ReadByte().ToString();
                        EntityLimitCount = reader.ReadByte().ToString();
                        DuckGroup        = GetDuckGroup(reader.ReadByte()).ToLower();
                        Bus              = GetBus(reader.ReadByte()).ToLower();
                        VolumeGroup      = GetVolumeGroup(reader.ReadByte()).ToString();
                    }
                }
            }
        }

        /// <summary>
        /// Loads Sound Pools from Memory
        /// </summary>
        /// <param name="poolInfo">Asset Pool Data</param>
        /// <returns>Asset List</returns>
        public static List<Asset> LoadFromMemory(AssetPoolInformation poolInfo)
        {
            List<Asset> assetList = new List<Asset>();

            for (int i = 0; assetList.Count < poolInfo.AssetCount; i++)
            {
                byte[] soundPoolData = MemoryUtil.ReadBytes(T7Util.ActiveProcess, poolInfo.StartLocation + (poolInfo.EntrySize * i), poolInfo.EntrySize);

                Asset asset = new Asset();

                SoundPool soundPool = new SoundPool();

                asset.AssetType = poolInfo.PoolName;
                asset.NameLocation = BitConverter.ToInt64(soundPoolData, 0);

                if (asset.NameLocation >= poolInfo.StartLocation && asset.NameLocation <= poolInfo.MaxEndLocation)
                    continue;

                asset.Path            = MemoryUtil.ReadNullTerminatedString(T7Util.ActiveProcess, asset.NameLocation);
                asset.DisplayName     = Path.GetFileName(asset.Path);
                asset.ExportFunction  = ExportSoundPool;
                soundPool.AliasCount  = BitConverter.ToInt32(soundPoolData, 32);
                soundPool.Language    = MemoryUtil.ReadNullTerminatedString(T7Util.ActiveProcess, BitConverter.ToInt64(soundPoolData, 16));
                soundPool.PoolAddress = BitConverter.ToInt64(soundPoolData, 40);

                asset.Data = soundPool;

                asset.Info = String.Format("Aliases - {0} - Language - {1}", soundPool.AliasCount, soundPool.Language.First().ToString().ToUpper() + soundPool.Language.Substring(1));

                assetList.Add(asset);
            }

            return assetList;
        }

        /// <summary>
        /// Exports a Sound Pool to an Alias CSV File
        /// </summary>
        public static bool ExportSoundPool(Asset asset)
        {
            SoundPool soundPool = (SoundPool)asset.Data;

            string assetPath = "exported_files\\sound\\aliases\\" + Path.GetFileName(asset.Path) + ".csv";

            PathUtil.CreateFilePath(assetPath);

            StringBuilder output = new StringBuilder();

            PropertyInfo[] properties = typeof(Alias.Entry).GetProperties();

            foreach (PropertyInfo property in properties)
            {
                output.Append(String.Format("{0},", property.Name));
            }
            output.AppendLine();

            output.AppendLine("# Exported via HydraX by Scobalula");
            output.AppendLine("# Please credit if used");

            for (int i = 0; i < soundPool.AliasCount; i++)
            {
                byte[] data = MemoryUtil.ReadBytes(T7Util.ActiveProcess, soundPool.PoolAddress + (40 * i), 40);

                string aliasName = MemoryUtil.ReadNullTerminatedString(T7Util.ActiveProcess, BitConverter.ToInt64(data, 0));
                long aliasAddress = BitConverter.ToInt64(data, 16);
                int numEntries = BitConverter.ToInt32(data, 24);

                Alias alias = new Alias()
                {
                    Name = aliasName,
                    Entries = new Alias.Entry[numEntries]
                };

                for (int j = 0; j < numEntries; j++)
                {
                    byte[] entryData = MemoryUtil.ReadBytes(T7Util.ActiveProcess, aliasAddress + (216 * j), 216);

                    alias.Entries[j] = new Alias.Entry()
                    {
                        Name = aliasName
                    };

                    alias.Entries[j].LoadEntry(entryData);

                    foreach (PropertyInfo property in properties)
                    {
                        output.Append(String.Format("{0},", property.GetValue(alias.Entries[j])));
                    }

                    output.AppendLine();

                }
            }

            File.WriteAllText(assetPath, output.ToString());

            return true;
        }
    }
}
