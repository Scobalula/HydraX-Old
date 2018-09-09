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
    /// Animation Selector Table Lo9gic
    /// </summary>
    class AnimSelectorTable
    {
        /// <summary>
        /// Animation Selectors
        /// </summary>
        public AnimationSelector[] Selectors { get; set; }

        /// <summary>
        /// Animation Selector Class
        /// </summary>
        public class AnimationSelector
        {
            /// <summary>
            /// Animation Selector Row
            /// </summary>
            public class Row
            {
                public string[] Columns { get; set; }
            }

            /// <summary>
            /// Animation Selector Name
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Animation Selector Headers (Each selector has its own headers)
            /// </summary>
            public string[] Headers { get; set; }

            /// <summary>
            /// Animation Selector Row
            /// </summary>
            public Row[] Rows { get; set; }
        };

        /// <summary>
        /// AST Definitions, from AST Definitions JSON File included in the Mod Tools
        /// </summary>
        public static Dictionary<string, Func<T7Util.FastFile, string>> ASTDefinitions = new Dictionary<string, Func<T7Util.FastFile, string>>()
        {
            { "_context",                                     ReadString },
            { "_context2",                                    ReadString },
            { "_cover_concealed",                             ReadEnum },
            { "_cover_direction",                             ReadEnum },
            { "_previous_cover_direction",                    ReadEnum },
            { "_desired_cover_direction",                     ReadEnum },
            { "_cover_mode",                                  ReadEnum },
            { "_previous_cover_mode",                         ReadEnum },
            { "_cover_type",                                  ReadEnum },
            { "_current_location_cover_type",                 ReadEnum },
            { "_previous_cover_type",                         ReadEnum },
            { "_exposed_type",                                ReadEnum },
            { "_stance",                                      ReadEnum },
            { "_desired_stance",                              ReadEnum },
            { "_arrival_stance",                              ReadEnum },
            { "_locomotion_should_turn",                      ReadEnum },
            { "_arrival_type",                                ReadEnum },
            { "_locomotion_speed",                            ReadEnum },
            { "_grapple_direction",                           ReadEnum },
            { "_run_n_gun_variation",                         ReadEnum },
            { "_has_legs",                                    ReadEnum },
            { "_idgun_damage_direction",                      ReadEnum },
            { "_which_board_pull",                            ReadInt },
            { "_variant_type",                                ReadInt },
            { "_low_gravity_variant",                         ReadInt },
            { "_board_attack_spot",                           ReadFloat },
            { "_weapon_class",                                ReadEnum },
            { "_weapon_type",                                 ReadEnum },
            { "_damage_weapon",                               ReadEnum },
            { "_damage_direction",                            ReadEnum },
            { "_damage_location",                             ReadEnum },
            { "_fatal_damage_location",                       ReadEnum },
            { "_damage_taken",                                ReadEnum },
            { "_damage_weapon_class",                         ReadEnum },
            { "_tracking_turn_yaw_min",                       ReadFloat },
            { "_tracking_turn_yaw_max",                       ReadFloat },
            { "_melee_distance_min",                          ReadFloat },
            { "_melee_distance_max",                          ReadFloat },
            { "_throw_distance_min",                          ReadFloat },
            { "_throw_distance_max",                          ReadFloat },
            { "_locomotion_exit_yaw_min",                     ReadFloat },
            { "_locomotion_exit_yaw_max",                     ReadFloat },
            { "_locomotion_motion_angle_min",                 ReadFloat },
            { "_locomotion_motion_angle_max",                 ReadFloat },
            { "_lookahead_angle_min",                         ReadFloat },
            { "_lookahead_angle_max",                         ReadFloat },
            { "_locomotion_turn_yaw_min",                     ReadFloat },
            { "_locomotion_turn_yaw_max",                     ReadFloat },
            { "_locomotion_arrival_yaw_min",                  ReadFloat },
            { "_locomotion_arrival_yaw_max",                  ReadFloat },
            { "_tactical_arrival_facing_yaw_min",             ReadFloat },
            { "_tactical_arrival_facing_yaw_max",             ReadFloat },
            { "_locomotion_arrival_distance_min",             ReadFloat },
            { "_locomotion_arrival_distance_max",             ReadFloat },
            { "_enemy_yaw_min",                               ReadFloat },
            { "_enemy_yaw_max",                               ReadFloat },
            { "_perfect_enemy_yaw_min",                       ReadFloat },
            { "_perfect_enemy_yaw_max",                       ReadFloat },
            { "_react_yaw_min",                               ReadFloat },
            { "_react_yaw_max",                               ReadFloat },
            { "_speed_min",                                   ReadFloat },
            { "_speed_max",                                   ReadFloat },
            { "_locomotion_face_enemy_quadrant",              ReadEnum },
            { "_locomotion_face_enemy_quadrant_previous",     ReadEnum },
            { "_traversal_type",                              ReadString },
            { "_locomotion_pain_type",                        ReadString },
            { "_human_locomotion_movement_type",              ReadEnum },
            { "_animation_alias",                             ReadString },
            { "_aim_up_alias",                                ReadString },
            { "_aim_down_alias",                              ReadString },
            { "_aim_left_alias",                              ReadString },
            { "_aim_right_alias",                             ReadString },
            { "_animation_alias_semi",                        ReadString },
            { "_animation_alias_singleshot",                  ReadString },
            { "_animation_alias_burst3",                      ReadString },
            { "_animation_alias_burst4",                      ReadString },
            { "_animation_alias_burst5",                      ReadString },
            { "_animation_alias_burst6",                      ReadString },
            { "_animation_alias_param_f",                     ReadString },
            { "_animation_alias_param_b",                     ReadString },
            { "_animation_alias_param_l",                     ReadString },
            { "_animation_alias_param_r",                     ReadString },
            { "_animation_alias_param_balance",               ReadString },
            { "_animation_alias_turn_r",                      ReadString },
            { "_animation_alias_turn_l",                      ReadString },
            { "_param_idle_blend_dropoff",                    ReadFloat },
            { "_param_turn_blend_min_ratio",                  ReadFloat },
            { "_param_turn_blend_scale",                      ReadFloat },
            { "_blend_in_time",                               ReadFloat },
            { "_blend_out_time",                              ReadFloat },
            { "_animation_mocomp",                            ReadEnum },
            { "_aim_table",                                   ReadEnum },
            { "_gib_location",                                ReadEnum },
            { "_yaw_to_cover_min",                            ReadFloat },
            { "_yaw_to_cover_max",                            ReadFloat },
            { "_should_run",                                  ReadEnum },
            { "_should_howl",                                 ReadEnum },
            { "_arms_position",                               ReadEnum },
            { "_mind_control",                                ReadEnum },
            { "_move_mode",                                   ReadEnum },
            { "_fire_mode",                                   ReadEnum },
            { "_special_death",                               ReadEnum },
            { "_juke_direction",                              ReadEnum },
            { "_juke_distance",                               ReadEnum },
            { "_panic",                                       ReadEnum },
            { "_gibbed_limbs",                                ReadEnum },
            { "_human_cover_flankability",                    ReadEnum },
            { "_robot_step_in",                               ReadEnum },
            { "_awareness",                                   ReadEnum },
            { "_awareness_prev",                              ReadEnum },
            { "_robot_jump_direction",                        ReadEnum },
            { "_robot_wallrun_direction",                     ReadEnum },
            { "_robot_locomotion_type",                       ReadEnum },
            { "_robot_traversal_type",                        ReadEnum },
            { "_staircase_exit_type",                         ReadEnum },
            { "_staircase_type",                              ReadEnum },
            { "_staircase_state",                             ReadEnum },
            { "_staircase_direction",                         ReadEnum },
            { "_staircase_skip_num",                          ReadEnum },
            { "_melee_enemy_type",                            ReadEnum },
            { "_zombie_damageweapon_type",                    ReadEnum },
            { "_parasite_firing_rate",                        ReadEnum },
            { "_margwa_head",                                 ReadEnum },
            { "_margwa_teleport",                             ReadEnum },
            { "_enemy",                                       ReadEnum },
            { "_patrol",                                      ReadEnum },
            { "_knockdown_direction",                         ReadEnum },
            { "_getup_direction",                             ReadEnum },
            { "_push_direction",                              ReadEnum },
            { "_human_locomotion_variation",                  ReadEnum },
            { "_robot_mode",                                  ReadEnum },
            { "_low_gravity",                                 ReadEnum },
            { "_knockdown_type",                              ReadEnum },
            { "_mechz_part",                                  ReadEnum },
            { "_apothicon_bamf_distance_min",                 ReadFloat },
            { "_apothicon_bamf_distance_max",                 ReadFloat },
            { "_keeper_protector_attack",                     ReadEnum },
            { "_keeper_protector_attack_type",                ReadEnum },
            { "_whirlwind_speed",                             ReadEnum },
            { "_quad_wall_crawl",                             ReadEnum },
            { "_quad_phase_direction",                        ReadEnum },
            { "_quad_phase_distance",                         ReadEnum },
            { "_zombie_blackholebomb_pull_state",             ReadEnum },
        };

        /// <summary>
        /// AST Definitions, from AST Definitions JSON File included in the Mod Tools
        /// </summary>
        public static Dictionary<string, Func<long, string>> ASTDefinitionsMemory = new Dictionary<string, Func<long, string>>()
        {
            { "_context",                                     ReadString },
            { "_context2",                                    ReadString },
            { "_cover_concealed",                             ReadEnum },
            { "_cover_direction",                             ReadEnum },
            { "_previous_cover_direction",                    ReadEnum },
            { "_desired_cover_direction",                     ReadEnum },
            { "_cover_mode",                                  ReadEnum },
            { "_previous_cover_mode",                         ReadEnum },
            { "_cover_type",                                  ReadEnum },
            { "_current_location_cover_type",                 ReadEnum },
            { "_previous_cover_type",                         ReadEnum },
            { "_exposed_type",                                ReadEnum },
            { "_stance",                                      ReadEnum },
            { "_desired_stance",                              ReadEnum },
            { "_arrival_stance",                              ReadEnum },
            { "_locomotion_should_turn",                      ReadEnum },
            { "_arrival_type",                                ReadEnum },
            { "_locomotion_speed",                            ReadEnum },
            { "_grapple_direction",                           ReadEnum },
            { "_run_n_gun_variation",                         ReadEnum },
            { "_has_legs",                                    ReadEnum },
            { "_idgun_damage_direction",                      ReadEnum },
            { "_which_board_pull",                            ReadInt },
            { "_variant_type",                                ReadInt },
            { "_low_gravity_variant",                         ReadInt },
            { "_board_attack_spot",                           ReadFloat },
            { "_weapon_class",                                ReadEnum },
            { "_weapon_type",                                 ReadEnum },
            { "_damage_weapon",                               ReadEnum },
            { "_damage_direction",                            ReadEnum },
            { "_damage_location",                             ReadEnum },
            { "_fatal_damage_location",                       ReadEnum },
            { "_damage_taken",                                ReadEnum },
            { "_damage_weapon_class",                         ReadEnum },
            { "_tracking_turn_yaw_min",                       ReadFloat },
            { "_tracking_turn_yaw_max",                       ReadFloat },
            { "_melee_distance_min",                          ReadFloat },
            { "_melee_distance_max",                          ReadFloat },
            { "_throw_distance_min",                          ReadFloat },
            { "_throw_distance_max",                          ReadFloat },
            { "_locomotion_exit_yaw_min",                     ReadFloat },
            { "_locomotion_exit_yaw_max",                     ReadFloat },
            { "_locomotion_motion_angle_min",                 ReadFloat },
            { "_locomotion_motion_angle_max",                 ReadFloat },
            { "_lookahead_angle_min",                         ReadFloat },
            { "_lookahead_angle_max",                         ReadFloat },
            { "_locomotion_turn_yaw_min",                     ReadFloat },
            { "_locomotion_turn_yaw_max",                     ReadFloat },
            { "_locomotion_arrival_yaw_min",                  ReadFloat },
            { "_locomotion_arrival_yaw_max",                  ReadFloat },
            { "_tactical_arrival_facing_yaw_min",             ReadFloat },
            { "_tactical_arrival_facing_yaw_max",             ReadFloat },
            { "_locomotion_arrival_distance_min",             ReadFloat },
            { "_locomotion_arrival_distance_max",             ReadFloat },
            { "_enemy_yaw_min",                               ReadFloat },
            { "_enemy_yaw_max",                               ReadFloat },
            { "_perfect_enemy_yaw_min",                       ReadFloat },
            { "_perfect_enemy_yaw_max",                       ReadFloat },
            { "_react_yaw_min",                               ReadFloat },
            { "_react_yaw_max",                               ReadFloat },
            { "_speed_min",                                   ReadFloat },
            { "_speed_max",                                   ReadFloat },
            { "_locomotion_face_enemy_quadrant",              ReadEnum },
            { "_locomotion_face_enemy_quadrant_previous",     ReadEnum },
            { "_traversal_type",                              ReadString },
            { "_locomotion_pain_type",                        ReadString },
            { "_human_locomotion_movement_type",              ReadEnum },
            { "_animation_alias",                             ReadString },
            { "_aim_up_alias",                                ReadString },
            { "_aim_down_alias",                              ReadString },
            { "_aim_left_alias",                              ReadString },
            { "_aim_right_alias",                             ReadString },
            { "_animation_alias_semi",                        ReadString },
            { "_animation_alias_singleshot",                  ReadString },
            { "_animation_alias_burst3",                      ReadString },
            { "_animation_alias_burst4",                      ReadString },
            { "_animation_alias_burst5",                      ReadString },
            { "_animation_alias_burst6",                      ReadString },
            { "_animation_alias_param_f",                     ReadString },
            { "_animation_alias_param_b",                     ReadString },
            { "_animation_alias_param_l",                     ReadString },
            { "_animation_alias_param_r",                     ReadString },
            { "_animation_alias_param_balance",               ReadString },
            { "_animation_alias_turn_r",                      ReadString },
            { "_animation_alias_turn_l",                      ReadString },
            { "_param_idle_blend_dropoff",                    ReadFloat },
            { "_param_turn_blend_min_ratio",                  ReadFloat },
            { "_param_turn_blend_scale",                      ReadFloat },
            { "_blend_in_time",                               ReadFloat },
            { "_blend_out_time",                              ReadFloat },
            { "_animation_mocomp",                            ReadEnum },
            { "_aim_table",                                   ReadEnum },
            { "_gib_location",                                ReadEnum },
            { "_yaw_to_cover_min",                            ReadFloat },
            { "_yaw_to_cover_max",                            ReadFloat },
            { "_should_run",                                  ReadEnum },
            { "_should_howl",                                 ReadEnum },
            { "_arms_position",                               ReadEnum },
            { "_mind_control",                                ReadEnum },
            { "_move_mode",                                   ReadEnum },
            { "_fire_mode",                                   ReadEnum },
            { "_special_death",                               ReadEnum },
            { "_juke_direction",                              ReadEnum },
            { "_juke_distance",                               ReadEnum },
            { "_panic",                                       ReadEnum },
            { "_gibbed_limbs",                                ReadEnum },
            { "_human_cover_flankability",                    ReadEnum },
            { "_robot_step_in",                               ReadEnum },
            { "_awareness",                                   ReadEnum },
            { "_awareness_prev",                              ReadEnum },
            { "_robot_jump_direction",                        ReadEnum },
            { "_robot_wallrun_direction",                     ReadEnum },
            { "_robot_locomotion_type",                       ReadEnum },
            { "_robot_traversal_type",                        ReadEnum },
            { "_staircase_exit_type",                         ReadEnum },
            { "_staircase_type",                              ReadEnum },
            { "_staircase_state",                             ReadEnum },
            { "_staircase_direction",                         ReadEnum },
            { "_staircase_skip_num",                          ReadEnum },
            { "_melee_enemy_type",                            ReadEnum },
            { "_zombie_damageweapon_type",                    ReadEnum },
            { "_parasite_firing_rate",                        ReadEnum },
            { "_margwa_head",                                 ReadEnum },
            { "_margwa_teleport",                             ReadEnum },
            { "_enemy",                                       ReadEnum },
            { "_patrol",                                      ReadEnum },
            { "_knockdown_direction",                         ReadEnum },
            { "_getup_direction",                             ReadEnum },
            { "_push_direction",                              ReadEnum },
            { "_human_locomotion_variation",                  ReadEnum },
            { "_robot_mode",                                  ReadEnum },
            { "_low_gravity",                                 ReadEnum },
            { "_knockdown_type",                              ReadEnum },
            { "_mechz_part",                                  ReadEnum },
            { "_apothicon_bamf_distance_min",                 ReadFloat },
            { "_apothicon_bamf_distance_max",                 ReadFloat },
            { "_keeper_protector_attack",                     ReadEnum },
            { "_keeper_protector_attack_type",                ReadEnum },
            { "_whirlwind_speed",                             ReadEnum },
            { "_quad_wall_crawl",                             ReadEnum },
            { "_quad_phase_direction",                        ReadEnum },
            { "_quad_phase_distance",                         ReadEnum },
            { "_zombie_blackholebomb_pull_state",             ReadEnum },
        };

        /// <summary>
        /// Reads a float from a compiled Animation Selector Table
        /// </summary>
        /// <param name="fastFile"></param>
        /// <returns></returns>
        public static string ReadFloat(T7Util.FastFile fastFile)
        {
            string str = fastFile.GetString(fastFile.DecodedStream.ReadInt32() - 1);

            float data = fastFile.DecodedStream.ReadSingle();

            // If this points to a valid string (which should only return * if value)
            // we're using the string.
            return string.IsNullOrEmpty(str) ? data.ToString() : str;
        }

        /// <summary>
        /// Reads a float from a compiled Animation Selector Table
        /// </summary>
        /// <param name="fastFile"></param>
        /// <returns></returns>
        public static string ReadFloat(long address)
        {
            string str = T7Util.GetString(MemoryUtil.ReadInt32(T7Util.ActiveProcess, address));

            float data = MemoryUtil.ReadSingle(T7Util.ActiveProcess, address + 4);

            // If this points to a valid string (which should only return * if value)
            // we're using the string.
            return string.IsNullOrEmpty(str) ? data.ToString() : str;
        }

        /// <summary>
        /// Reads an integer from a compiled Animation Selector Table
        /// </summary>
        /// <param name="fastFile"></param>
        /// <returns></returns>
        public static string ReadInt(T7Util.FastFile fastFile)
        {
            string str = fastFile.GetString(fastFile.DecodedStream.ReadInt32() - 1);

            int data = fastFile.DecodedStream.ReadInt32();

            // If this points to a valid string (which should only return * if value)
            // we're using the string.
            return string.IsNullOrEmpty(str) ? data.ToString() : str;
        }

        /// <summary>
        /// Reads an integer from a compiled Animation Selector Table
        /// </summary>
        /// <param name="fastFile"></param>
        /// <returns></returns>
        public static string ReadInt(long address)
        {
            string str = T7Util.GetString(MemoryUtil.ReadInt32(T7Util.ActiveProcess, address));

            int data = MemoryUtil.ReadInt32(T7Util.ActiveProcess, address + 4);

            // If this points to a valid string (which should only return * if value)
            // we're using the string.
            return string.IsNullOrEmpty(str) ? data.ToString() : str;
        }

        /// <summary>
        /// Reads a string from compiled Animation Selector Table
        /// </summary>
        /// <param name="fastFile"></param>
        /// <returns></returns>
        public static string ReadString(T7Util.FastFile fastFile)
        {
            int stringIndex = fastFile.DecodedStream.ReadInt32();

            fastFile.DecodedStream.Seek(4, SeekOrigin.Current);

            string str = fastFile.GetString(stringIndex - 1);

            return string.IsNullOrEmpty(str) ? "*" : str;
        }

        /// <summary>
        /// Reads an Enumerator from compiled Animation Selector Table
        /// </summary>
        /// <param name="fastFile"></param>
        /// <returns></returns>
        public static string ReadString(long address)
        {
            int stringIndex = MemoryUtil.ReadInt32(T7Util.ActiveProcess, address);

            string str = T7Util.GetString(stringIndex);

            // Make them upper case, more for maintaining
            // naming conventions, doesn't matter once in
            // the FF whether it's lower or upper case.
            return string.IsNullOrEmpty(str) ? "*" : str.ToUpper();
        }

        /// <summary>
        /// Reads an Enumerator from compiled Animation Selector Table
        /// </summary>
        /// <param name="fastFile"></param>
        /// <returns></returns>
        public static string ReadEnum(T7Util.FastFile fastFile)
        {
            int stringIndex = fastFile.DecodedStream.ReadInt32();

            fastFile.DecodedStream.Seek(4, SeekOrigin.Current);

            string str = fastFile.GetString(stringIndex - 1);

            // Make them upper case, more for maintaining
            // naming conventions, doesn't matter once in
            // the FF whether it's lower or upper case.
            return string.IsNullOrEmpty(str) ? "*" : str.ToUpper();
        }

        /// <summary>
        /// Reads an Enumerator from compiled Animation Selector Table
        /// </summary>
        /// <param name="fastFile"></param>
        /// <returns></returns>
        public static string ReadEnum(long address)
        {
            int stringIndex = MemoryUtil.ReadInt32(T7Util.ActiveProcess, address);

            string str = T7Util.GetString(stringIndex);

            // Make them upper case, more for maintaining
            // naming conventions, doesn't matter once in
            // the FF whether it's lower or upper case.
            return string.IsNullOrEmpty(str) ? "*" : str.ToUpper();
        }

        public static long AddressTracker = 0;

        /// <summary>
        /// Loads ASTs from Memory
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

                AnimSelectorTable animSelectorTable = new AnimSelectorTable()
                {
                    Selectors = new AnimationSelector[BitConverter.ToInt32(assetData, 16)]
                };


                asset.Path = MemoryUtil.ReadNullTerminatedString(T7Util.ActiveProcess, asset.NameLocation);
                asset.DisplayName = Path.GetFileName(asset.Path);
                asset.AssetType = poolInfo.PoolName;
                asset.Data = animSelectorTable;
                asset.Info = String.Format("Selectors - {0}", animSelectorTable.Selectors.Length);
                asset.ExportFunction = ExportFromMemory;
                assetList.Add(asset);
            }

            return assetList;
        }

        /// <summary>
        /// Exports an Animation Selector Table from Memory
        /// </summary>
        public static bool ExportFromMemory(Asset asset)
        {
            long addressTracker = asset.StartLocation;

            AnimSelectorTable selectorTable = (AnimSelectorTable)asset.Data;

            string assetName = "exported_files\\animtables\\" + asset.Path;
            PathUtil.CreateFilePath(assetName);

            for (int i = 0; i < selectorTable.Selectors.Length; i++)
            {
                selectorTable.Selectors[i] = new AnimationSelector
                {
                    Name = T7Util.GetString(MemoryUtil.ReadInt32(T7Util.ActiveProcess, addressTracker))
                };

                addressTracker += 16;

                selectorTable.Selectors[i].Headers = new string[MemoryUtil.ReadInt32(T7Util.ActiveProcess, addressTracker)];

                addressTracker += 16;

                selectorTable.Selectors[i].Rows = new AnimationSelector.Row[MemoryUtil.ReadInt32(T7Util.ActiveProcess, addressTracker)];

                for (int j = 0; j < selectorTable.Selectors[i].Rows.Length; j++)
                    selectorTable.Selectors[i].Rows[j] = new AnimationSelector.Row();

                addressTracker += 56;

            }

            for (int i = 0; i < selectorTable.Selectors.Length; i++)
            {
                for (int j = 0; j < selectorTable.Selectors[i].Headers.Length; j++)
                {
                    selectorTable.Selectors[i].Headers[j] = T7Util.GetString(MemoryUtil.ReadInt32(T7Util.ActiveProcess, addressTracker));

                    addressTracker += 24;

                }

                addressTracker += 16 * selectorTable.Selectors[i].Rows.Length;

                int numHeaders = selectorTable.Selectors[i].Headers.Length;
                int numRows = selectorTable.Selectors[i].Rows.Length;

                for (int j = 0; j < numRows; j++)
                {
                    selectorTable.Selectors[i].Rows[j].Columns = new string[numHeaders];

                    for (int k = 0; k < numHeaders; k++)
                    {
                        selectorTable.Selectors[i].Rows[j].Columns[k] = "";

                        if (ASTDefinitions.ContainsKey(selectorTable.Selectors[i].Headers[k]))
                        {
                            selectorTable.Selectors[i].Rows[j].Columns[k] = ASTDefinitionsMemory[selectorTable.Selectors[i].Headers[k]](addressTracker);
                        }

                        addressTracker += 8;
                    }
                }
            }

            // Build new AST String
            StringBuilder output = new StringBuilder();

            for (int i = 0; i < selectorTable.Selectors.Length; i++)
            {
                output.AppendLine(selectorTable.Selectors[i].Name + ",");

                for (int k = 0; k < selectorTable.Selectors[i].Headers.Length; k++)
                {
                    output.Append(selectorTable.Selectors[i].Headers[k] + ",");
                }

                output.AppendLine();

                for (int k = 0; k < selectorTable.Selectors[i].Rows.Length; k++)
                {
                    for (int j = 0; j < selectorTable.Selectors[i].Rows[k].Columns.Length; j++)
                    {
                        output.Append(selectorTable.Selectors[i].Rows[k].Columns[j] + ",");
                    }

                    output.AppendLine();
                }

                output.AppendLine(",");
            }

            // Dump
            File.WriteAllText(assetName, output.ToString());

            return true;

        }

        /// <summary>
        /// Loads an Animation Selector Table from a Fast File
        /// </summary>
        public static void LoadFromFastFile(T7Util.FastFile fastFile, Asset asset)
        {
            AnimSelectorTable animSelectorTable = new AnimSelectorTable()
            {
                Selectors = new AnimationSelector[fastFile.DecodedStream.ReadInt32()]
            };

            fastFile.DecodedStream.Seek(4, SeekOrigin.Current);

            asset.AssetType = "animselectortable";
            asset.Path = fastFile.DecodedStream.ReadNullTerminatedString();
            asset.DisplayName = Path.GetFileName(asset.Path);
            asset.StartLocation = fastFile.DecodedStream.BaseStream.Position;
            asset.Data = animSelectorTable;
            asset.Info = String.Format("Selectors - {0}", animSelectorTable.Selectors.Length);
            asset.ExportFunction = ExportFromFastFile;
        }

        /// <summary>
        /// Exports an Animation Selector Table from a Fast File
        /// </summary>
        public static bool ExportFromFastFile(Asset asset)
        {
            T7Util.ActiveFastFile.DecodedStream.Seek(asset.StartLocation, SeekOrigin.Begin);

            AnimSelectorTable selectorTable = (AnimSelectorTable)asset.Data;

            string assetName = "exported_files\\animtables\\" + asset.Path;
            PathUtil.CreateFilePath(assetName);

            for (int i = 0; i < selectorTable.Selectors.Length; i++)
            {
                selectorTable.Selectors[i] = new AnimationSelector
                {
                    Name = T7Util.ActiveFastFile.GetString(T7Util.ActiveFastFile.DecodedStream.ReadInt32() - 1)
                };

                T7Util.ActiveFastFile.DecodedStream.Seek(12, SeekOrigin.Current);

                selectorTable.Selectors[i].Headers = new string[T7Util.ActiveFastFile.DecodedStream.ReadInt32()];

                T7Util.ActiveFastFile.DecodedStream.Seek(12, SeekOrigin.Current);

                selectorTable.Selectors[i].Rows = new AnimationSelector.Row[T7Util.ActiveFastFile.DecodedStream.ReadInt32()];

                for (int j = 0; j < selectorTable.Selectors[i].Rows.Length; j++)
                    selectorTable.Selectors[i].Rows[j] = new AnimationSelector.Row();

                T7Util.ActiveFastFile.DecodedStream.Seek(52, SeekOrigin.Current);

            }

            for (int i = 0; i < selectorTable.Selectors.Length; i++)
            {
                for (int j = 0; j < selectorTable.Selectors[i].Headers.Length; j++)
                {
                    selectorTable.Selectors[i].Headers[j] = T7Util.ActiveFastFile.GetString(T7Util.ActiveFastFile.DecodedStream.ReadInt32() - 1);

                    T7Util.ActiveFastFile.DecodedStream.Seek(20, SeekOrigin.Current);

                }

                T7Util.ActiveFastFile.DecodedStream.Seek(16 * selectorTable.Selectors[i].Rows.Length, SeekOrigin.Current);

                int numHeaders = selectorTable.Selectors[i].Headers.Length;
                int numRows = selectorTable.Selectors[i].Rows.Length;

                for (int j = 0; j < numRows; j++)
                {
                    selectorTable.Selectors[i].Rows[j].Columns = new string[numHeaders];

                    for (int k = 0; k < numHeaders; k++)
                    {
                        selectorTable.Selectors[i].Rows[j].Columns[k] = "";

                        if (ASTDefinitions.ContainsKey(selectorTable.Selectors[i].Headers[k]))
                        {
                            selectorTable.Selectors[i].Rows[j].Columns[k] = ASTDefinitions[selectorTable.Selectors[i].Headers[k]](T7Util.ActiveFastFile);
                        }
                        // Unknown Entry
                        else
                        {
                            T7Util.ActiveFastFile.DecodedStream.Seek(8, SeekOrigin.Current);
                        }
                    }
                }
            }

            // Build new AST String
            StringBuilder output = new StringBuilder();

            for (int i = 0; i < selectorTable.Selectors.Length; i++)
            {
                output.AppendLine(selectorTable.Selectors[i].Name + ",");

                for (int k = 0; k < selectorTable.Selectors[i].Headers.Length; k++)
                {
                    output.Append(selectorTable.Selectors[i].Headers[k] + ",");
                }

                output.AppendLine();

                for (int k = 0; k < selectorTable.Selectors[i].Rows.Length; k++)
                {
                    for (int j = 0; j < selectorTable.Selectors[i].Rows[k].Columns.Length; j++)
                    {
                        output.Append(selectorTable.Selectors[i].Rows[k].Columns[j] + ",");
                    }

                    output.AppendLine();
                }

                output.AppendLine(",");
            }

            // Dump
            File.WriteAllText(assetName, output.ToString());

            return true;

        }
    }
}
