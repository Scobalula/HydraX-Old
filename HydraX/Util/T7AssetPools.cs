/*
 *  HydraX T7 Asset Pools - Copyright 2018 Philip/Scobalula
 *  
 *  This file is subject to the license terms set out in the
 *  "LICENSE.txt" file. 
 * 
 */
using HydraLib.T7.Assets;
using System;
using System.Collections.Generic;

namespace HydraLib
{
    public partial class T7Util
    {
        /// <summary>
        /// Class to hold Asset Pool Information
        /// </summary>
        public class AssetPool
        {
            /// <summary>
            /// Asset Pool Name
            /// </summary>
            public string AssetPoolName { get; set; }

            /// <summary>
            /// Asset Pool Load Function
            /// </summary>
            public Func<AssetPoolInformation, List<Asset>> LoadFunction { get; set; }

            /// <summary>
            /// Creates a new Asset Pool
            /// </summary>
            public AssetPool() { }

            /// <summary>
            /// Creates a new Asset Pool with a name
            /// </summary>
            /// <param name="name">Asset Pool Name</param>
            public AssetPool(string name)
            {
                AssetPoolName = name;
            }

            /// <summary>
            /// Creates a new Asset Pool with a name and load function
            /// </summary>
            /// <param name="name">Asset Pool Name</param>
            /// <param name="loadFunction">Asset Pool Load Function</param>
            public AssetPool(string name, Func<AssetPoolInformation, List<Asset>> loadFunction)
            {
                AssetPoolName = name;
                LoadFunction = loadFunction;
            }
        }

        /// <summary>
        /// Asset Pools Address
        /// </summary>
        public static long AssetPoolAddress;

        /// <summary>
        /// Asset Pools Address
        /// </summary>
        public static long StringPoolStartAddress;

        /// <summary>
        /// Asset Pools Address
        /// </summary>
        public static long StringPoolEndAddress;

        /// <summary>
        /// Number of Asset Pools
        /// </summary>
        public const int XASSET_POOLS_COUNT = 0x6B;

        /// <summary>
        /// Known T7 Asset Pools
        /// </summary>
        public static AssetPool[] AssetPools =
        {
            new AssetPool("physpreset", PhysPreset.LoadFromMemory),
            new AssetPool("physconstraints"),
            new AssetPool("destructibledef"),
            new AssetPool("xanim"),
            new AssetPool("xmodel"),
            new AssetPool("xmodelmesh"),
            new AssetPool("material"),
            new AssetPool("computeshaderset"),
            new AssetPool("techset"),
            new AssetPool("image"),
            new AssetPool("sound", Sound.LoadFromMemory),
            new AssetPool("sound_patch"),
            new AssetPool("col_map"),
            new AssetPool("com_map"),
            new AssetPool("game_map"),
            new AssetPool("map_ents", D3DBSP.LoadFromMemory),
            new AssetPool("gfx_map"),
            new AssetPool("lightdef"),
            new AssetPool("lensflaredef"),
            new AssetPool("ui_map"),
            new AssetPool("font"),
            new AssetPool("fonticon"),
            new AssetPool("localize", Localize.LoadFromMemory),
            new AssetPool("weapon"),
            new AssetPool("weapondef"),
            new AssetPool("weaponvariant"),
            new AssetPool("weaponfull"),
            new AssetPool("cgmediatable"),
            new AssetPool("playersoundstable"),
            new AssetPool("playerfxtable"),
            new AssetPool("sharedweaponsounds"),
            new AssetPool("attachment"),
            new AssetPool("attachmentunique"),
            new AssetPool("weaponcamo", WeaponCamo.LoadFromMemory),
            new AssetPool("customizationtable"),
            new AssetPool("customizationtable_feimages"),
            new AssetPool("customizationtablecolor"),
            new AssetPool("snddriverglobals"),
            new AssetPool("fx"),
            new AssetPool("tagfx"),
            new AssetPool("klf"),
            new AssetPool("impactsfxtable"),
            new AssetPool("impactsoundstable"),
            new AssetPool("player_character"),
            new AssetPool("aitype"),
            new AssetPool("character"),
            new AssetPool("xmodelalias"),
            new AssetPool("rawfile", RawFile.LoadFromMemory),
            new AssetPool("stringtable", StringTable.LoadFromMemory),
            new AssetPool("structuredtable", StructuredTableUtil.LoadFromMemory),
            new AssetPool("leaderboarddef"),
            new AssetPool("ddl"),
            new AssetPool("glasses"),
            new AssetPool("texturelist"),
            new AssetPool("scriptparsetree", ScriptParseTree.LoadFromMemory),
            new AssetPool("keyvaluepairs"),
            new AssetPool("vehicle"),
            new AssetPool("addon_map_ents"),
            new AssetPool("tracer"),
            new AssetPool("slug"),
            new AssetPool("surfacefxtable"),
            new AssetPool("surfacesounddef"),
            new AssetPool("footsteptable"),
            new AssetPool("entityfximpacts"),
            new AssetPool("entitysoundimpacts"),
            new AssetPool("zbarrier"),
            new AssetPool("vehiclefxdef"),
            new AssetPool("vehiclesounddef"),
            new AssetPool("typeinfo"),
            new AssetPool("scriptbundle"),
            new AssetPool("scriptbundlelist"),
            new AssetPool("rumble", Rumble.LoadFromMemory),
            new AssetPool("bulletpenetration"),
            new AssetPool("locdmgtable"),
            new AssetPool("aimtable"),
            new AssetPool("animselectortable", AnimSelectorTable.LoadFromMemory),
            new AssetPool("animmappingtable", AnimMappingTable.LoadFromMemory),
            new AssetPool("animstatemachine", AnimStateMachine.LoadFromMemory),
            new AssetPool("behaviortree", BehaviorTree.LoadFromMemory),
            new AssetPool("behaviorstatemachine"),
            new AssetPool("ttf"),
            new AssetPool("sanim"),
            new AssetPool("lightdescription"),
            new AssetPool("shellshock"),
            new AssetPool("xcam", XCamUtil.LoadFromMemory),
            new AssetPool("bgcache"),
            new AssetPool("texturecombo"),
            new AssetPool("flametable"),
            new AssetPool("bitfield"),
            new AssetPool("attachmentcosmeticvariant"),
            new AssetPool("maptable"),
            new AssetPool("maptableloadingimages"),
            new AssetPool("medal"),
            new AssetPool("medaltable"),
            new AssetPool("objective"),
            new AssetPool("objectivelist"),
            new AssetPool("umbra_tome"),
            new AssetPool("navmesh"),
            new AssetPool("navvolume"),
            new AssetPool("binaryhtml"),
            new AssetPool("laser"),
            new AssetPool("beam"),
            new AssetPool("streamerhint"),
            new AssetPool("string"),
            new AssetPool("assetlist"),
            new AssetPool("report"),
            new AssetPool("depend"),
        };
    }
}
