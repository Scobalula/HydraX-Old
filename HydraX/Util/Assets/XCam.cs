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
using Newtonsoft.Json;
using PhilUtil;
using PhilUtil.MathUtil;
using HydraLib.GDT;
using HydraLib.T7.Assets.JsonFiles;

namespace HydraLib.T7.Assets.JsonFiles
{
    /// <summary>
    /// XCam Data
    /// </summary>
    public class XCam
    {
        /// <summary>
        /// XCam Notetrack
        /// </summary>
        public class Notetrack
        {
            [JsonProperty("name")]
            public string Note = "";

            [JsonProperty("frame")]
            public int Frame = 0;
        }

        /// <summary>
        /// XCam Camera Switch
        /// </summary>
        public class CameraSwitch
        {
            [JsonProperty("frame")]
            public string Frame { get; set; }

            [JsonProperty("dissolve")]
            public double Dissolve { get; set; }

            [JsonProperty("cameras")]
            public int[] Cameras { get; set; }
        }

        /// <summary>
        /// XCam Align Node
        /// </summary>
        public class Align
        {
            [JsonProperty("tag")]
            public string Tag = "tag_align";

            [JsonProperty("offset")]
            public double[] Offset = new double[3];

            [JsonProperty("axis")]
            public Dictionary<string, double[]> Axis = new Dictionary<string, double[]>()
            {
                { "x", new double[3] },
                { "y", new double[3] },
                { "z", new double[3] },
            };
        }

        /// <summary>
        /// Target Model Bone Root Frame Data
        /// </summary>
        public class TargetModelBoneRootFrame
        {
            [JsonProperty("frame")]
            public int Frame = 0;

            [JsonProperty("offset")]
            public double[] Offset = new double[3];

            [JsonProperty("axis")]
            public Dictionary<string, double[]> Axis = new Dictionary<string, double[]>()
            {
                { "x", new double[3] },
                { "y", new double[3] },
                { "z", new double[3] },
            };
        }

        /// <summary>
        /// Target Model Bone Root Data
        /// </summary>
        public class TargetModelBoneRoot
        {
            [JsonProperty("name", Order = 1)]
            public string Name { get; set; }

            [JsonProperty("animation", Order = 3)]
            public TargetModelBoneRootFrame[] Animation;

            [JsonProperty("axis", Order = 2)]
            public Dictionary<string, double[]> Axis = new Dictionary<string, double[]>()
            {
                { "x", new double[3] },
                { "y", new double[3] },
                { "z", new double[3] },
            };
        }

        /// <summary>
        /// Camera Animation Data
        /// </summary>
        public class CameraAnimation
        {
            [JsonProperty("frame", Order = -2)]
            public int Frame { get; set; }

            [JsonProperty("origin")]
            public double[] Origin = new double[3];

            [JsonProperty("dir")]
            public double[] Dir = new double[3];

            [JsonProperty("up")]
            public double[] Up = new double[3];

            [JsonProperty("right")]
            public double[] Right = new double[3];

            [JsonProperty("flen")]
            public double FocalLength = 27.0000;

            [JsonProperty("fov")]
            public double FieldOfView = 28.7985;

            [JsonProperty("fdist")]
            public double FDist = 109.4973;

            [JsonProperty("fstop")]
            public double FStop = 1.2000;

            [JsonProperty("lense")]
            public int Lense = 10;
        }

        /// <summary>
        /// Camera Data
        /// </summary>
        public class Camera
        {
            [JsonProperty("name", Order = -3)]
            public string Name { get; set; }

            [JsonProperty("index", Order = -2)]
            public int Index { get; set; }

            [JsonProperty("type")]
            public string Type = "Perspective";

            [JsonProperty("aperture")]
            public string Aperture = "FOCAL_LENGTH";

            [JsonProperty("origin")]
            public double[] Origin = new double[3];

            [JsonProperty("dir")]
            public double[] Dir = new double[3];

            [JsonProperty("up")]
            public double[] Up = new double[3];

            [JsonProperty("right")]
            public double[] Right = new double[3];

            [JsonProperty("flen")]
            public double FocalLength = 27.0000;

            [JsonProperty("fov")]
            public double FieldOfView = 28.7985;

            [JsonProperty("fdist")]
            public double FDist = 109.4973;

            [JsonProperty("fstop")]
            public double FStop = 1.2000;

            [JsonProperty("lense")]
            public int Lense = 10;

            [JsonProperty("aspectratio")]
            public double AspectRatio = 1.7786;

            [JsonProperty("nearz")]
            public double Nearz = 3.9370;

            [JsonProperty("farz")]
            public double Farz = 3937.0079;

            [JsonProperty("animation")]
            public CameraAnimation[] Animations { get; set; }
        }


        [JsonIgnore]
        public int HideHud = 0;

        [JsonIgnore]
        public int IsLooping = 0;

        [JsonIgnore]
        public int HideLocalPlayer = 0;

        [JsonIgnore]
        public int UseFPSPlayer = 0;

        [JsonIgnore]
        public int DisableNearFov = 0;

        [JsonIgnore]
        public int AutoMotionBlur = 0;

        [JsonIgnore]
        public int EaseAnimationOut = 0;

        [JsonIgnore]
        public float[] RightStickRotationOffset = new float[3];

        [JsonIgnore]
        public float[] RightStickRotationDegrees = new float[2];

        [JsonIgnore]
        public long NotetrackLocation { get; set; }

        [JsonIgnore]
        public long TargetModelBoneRootLocation { get; set; }

        [JsonIgnore]
        public long CamerasLocation { get; set; }

        [JsonIgnore]
        public long CameraSwitchLocation { get; set; }

        [JsonProperty("version")]
        public int Version = 1;

        [JsonProperty("scene")]
        public string Scene = "exported_via_HydraX_by_Scobalula.fbx";

        [JsonProperty("align")]
        public Align AlignNode = new Align();

        [JsonProperty("framerate")]
        public int FrameRate = 30;

        [JsonProperty("numframes")]
        public int FrameCount { get; set; }

        [JsonProperty("targetModelBoneRoots", Order = 1)]
        public TargetModelBoneRoot[] TargetModelBoneRoots = new TargetModelBoneRoot[0];

        [JsonProperty("cameras", Order = 2)]
        public Camera[] Cameras = new Camera[0];

        [JsonProperty("notetracks", Order = 4)]
        public Notetrack[] Notetracks = new Notetrack[0];

        [JsonProperty("cameraSwitch", Order = 3)]
        public CameraSwitch[] CameraSwitches = new CameraSwitch[0];

        /// <summary>
        /// Saves XCam to a formatted JSON file
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
                    NullValueHandling = NullValueHandling.Ignore,
                };

                serializer.Serialize(output, this);
            }
        }
    }
}

namespace HydraLib.T7.Assets
{
    /// <summary>
    /// XCam Logic
    /// </summary>
    class XCamUtil
    {
        /// <summary>
        /// Loads XCams from Memory
        /// </summary>
        /// <param name="poolInfo">Asset Pool Information</param>
        /// <returns>List of Assets Found</returns>
        public static List<Asset> LoadFromMemory(AssetPoolInformation poolInfo)
        {
            List<Asset> assetList = new List<Asset>();

            for (int i = 0; assetList.Count < poolInfo.AssetCount; i++)
            {
                byte[] assetData = MemoryUtil.ReadBytes(T7Util.ActiveProcess, poolInfo.StartLocation + (poolInfo.EntrySize * i), poolInfo.EntrySize);

                XCam xCam = new XCam();

                Asset asset = new Asset
                {
                    NameLocation = BitConverter.ToInt64(assetData, 0)
                };

                if (Util.IsNullAsset(asset, poolInfo))
                    continue;

                // Rotations are stored as Quaternions, but in XCAM_EXPORT are Matrices
                Matrix matrix = new Quaternion()
                {
                    X = BitConverter.ToSingle(assetData, 40),
                    Y = BitConverter.ToSingle(assetData, 44) * -1,
                    Z = BitConverter.ToSingle(assetData, 48) * -1,
                    W = BitConverter.ToSingle(assetData, 52)
                }.ToMatrix();

                xCam.HideHud          = assetData[16];
                xCam.IsLooping        = assetData[17];
                xCam.HideLocalPlayer  = assetData[18];
                xCam.UseFPSPlayer     = assetData[19];
                xCam.DisableNearFov   = assetData[20];
                xCam.AutoMotionBlur   = assetData[21];
                xCam.EaseAnimationOut = assetData[22];

                xCam.AlignNode.Axis["x"] = matrix.X.ToArray();
                xCam.AlignNode.Axis["y"] = matrix.Y.ToArray();
                xCam.AlignNode.Axis["z"] = matrix.Z.ToArray();

                xCam.AlignNode.Offset[0] = Math.Round(BitConverter.ToSingle(assetData, 56), 4);
                xCam.AlignNode.Offset[1] = Math.Round(BitConverter.ToSingle(assetData, 60), 4);
                xCam.AlignNode.Offset[2] = Math.Round(BitConverter.ToSingle(assetData, 64), 4);

                xCam.RightStickRotationOffset[0] = BitConverter.ToSingle(assetData, 148);
                xCam.RightStickRotationOffset[1] = BitConverter.ToSingle(assetData, 152);
                xCam.RightStickRotationOffset[2] = BitConverter.ToSingle(assetData, 156);

                xCam.RightStickRotationDegrees[0] = BitConverter.ToSingle(assetData, 160);
                xCam.RightStickRotationDegrees[1] = BitConverter.ToSingle(assetData, 164);

                xCam.FrameRate  = BitConverter.ToInt32(assetData, 32);
                xCam.FrameCount = BitConverter.ToInt32(assetData, 28);

                xCam.NotetrackLocation = BitConverter.ToInt64(assetData, 96);
                xCam.CamerasLocation = BitConverter.ToInt64(assetData, 80);
                xCam.CameraSwitchLocation = BitConverter.ToInt64(assetData, 88);

                if (xCam.NotetrackLocation > 0)
                    xCam.Notetracks = new XCam.Notetrack[BitConverter.ToInt32(assetData, 36)];


                if (xCam.CamerasLocation > 0)
                    xCam.Cameras = new XCam.Camera[BitConverter.ToInt32(assetData, 24)];

                if (xCam.CameraSwitchLocation > 0)
                    xCam.CameraSwitches = new XCam.CameraSwitch[xCam.Cameras.Length];


                xCam.TargetModelBoneRootLocation = BitConverter.ToInt64(assetData, 72);

                asset.AssetType      = poolInfo.PoolName;
                asset.Path           = MemoryUtil.ReadNullTerminatedString(T7Util.ActiveProcess, asset.NameLocation);
                asset.DisplayName    = Path.GetFileName(asset.Path);
                asset.Data           = xCam;
                asset.ExportFunction = ExportFromMemory;

                asset.Info = String.Format("Frames - {0}, Framerate - {1}", xCam.FrameCount, xCam.FrameRate);

                assetList.Add(asset);
            }

            return assetList;
        }

        /// <summary>
        /// Exports XCams from Memory
        /// </summary>
        public static bool ExportFromMemory(Asset asset)
        {
            XCam xCam = (XCam)asset.Data;

            string assetPath = String.Format
                (
                    "exported_files\\\\xanim_export\\\\hydrax_export\\\\{0}.XCAM_EXPORT", 
                    asset.Path
                );

            GDTUtil.WriteXCamGDT(asset.Path, xCam);

            LoadCameras(xCam);
            LoadNotetracks(xCam);
            LoadTargetModelBoneRoots(xCam);


            PathUtil.CreateFilePath(assetPath);

            xCam.Save(assetPath);

            return true;
        }

        /// <summary>
        /// Loads Cameras for this XCam
        /// </summary>
        public static void LoadCameras(XCam xCam)
        {
            if (xCam.Cameras?.Length > 0 && xCam.CamerasLocation > 0)
            {
                long addressTracker = xCam.CamerasLocation;

                for(int i = 0; i < xCam.Cameras.Length; i++)
                {
                    byte[] buffer              = MemoryUtil.ReadBytes(T7Util.ActiveProcess, addressTracker, 32);

                    xCam.Cameras[i] = new XCam.Camera
                    {
                        Name       = T7Util.GetString(BitConverter.ToInt32(buffer, 0)),
                        Index      = BitConverter.ToInt32(buffer, 4),
                        Animations = new XCam.CameraAnimation[BitConverter.ToInt32(buffer, 8)],
                        Farz       = BitConverter.ToSingle(buffer, 24)
                    };

                    addressTracker += 32;
                }

                for (int i = 0; i < xCam.Cameras.Length; i++)
                {
                    for(int j = 0; j < xCam.Cameras[i].Animations.Length; j++)
                    {

                        

                        byte[] buffer = MemoryUtil.ReadBytes(T7Util.ActiveProcess, addressTracker, 48);

                        xCam.Cameras[i].Animations[j] = new XCam.CameraAnimation();

                        // Rotations are stored as Quaternions, but in XCAM_EXPORT are Matrices
                        Matrix matrix = new Quaternion()
                        {
                            X = BitConverter.ToSingle(buffer, 16),
                            Y = BitConverter.ToSingle(buffer, 20),
                            Z = BitConverter.ToSingle(buffer, 24),
                            W = BitConverter.ToSingle(buffer, 28)
                        }.ToMatrix();

                        xCam.Cameras[i].Animations[j].Frame = BitConverter.ToInt32(buffer, 0);

                        xCam.Cameras[i].Animations[j].Dir[0]   = matrix.X.X;
                        xCam.Cameras[i].Animations[j].Dir[1]   = matrix.Y.X;
                        xCam.Cameras[i].Animations[j].Dir[2]   = matrix.Z.X;
                        xCam.Cameras[i].Animations[j].Up[0]    = matrix.X.Z;
                        xCam.Cameras[i].Animations[j].Up[1]    = matrix.Y.Z;
                        xCam.Cameras[i].Animations[j].Up[2]    = matrix.Z.Z;
                        xCam.Cameras[i].Animations[j].Right[0] = matrix.X.Y * -1;
                        xCam.Cameras[i].Animations[j].Right[1] = matrix.Y.Y * -1;
                        xCam.Cameras[i].Animations[j].Right[2] = matrix.Z.Y * -1;

                        xCam.Cameras[i].Animations[j].Origin[0] = Math.Round(BitConverter.ToSingle(buffer, 4), 4);
                        xCam.Cameras[i].Animations[j].Origin[1] = Math.Round(BitConverter.ToSingle(buffer, 8), 4);
                        xCam.Cameras[i].Animations[j].Origin[2] = Math.Round(BitConverter.ToSingle(buffer, 12), 4);

                        xCam.Cameras[i].Animations[j].FieldOfView = Math.Round(BitConverter.ToSingle(buffer, 32), 4);
                        xCam.Cameras[i].Animations[j].FocalLength = Math.Round(BitConverter.ToSingle(buffer, 36), 4);
                        xCam.Cameras[i].Animations[j].FDist       = Math.Round(BitConverter.ToSingle(buffer, 40), 4);
                        xCam.Cameras[i].Animations[j].FStop       = Math.Round(BitConverter.ToSingle(buffer, 44), 4);

                        if(j == 0)
                        {
                            xCam.Cameras[i].Origin = xCam.Cameras[i].Animations[j].Origin;
                            xCam.Cameras[i].Dir    = xCam.Cameras[i].Animations[j].Dir;
                            xCam.Cameras[i].Up     = xCam.Cameras[i].Animations[j].Up;
                            xCam.Cameras[i].Right  = xCam.Cameras[i].Animations[j].Right;

                            xCam.Cameras[i].FieldOfView = xCam.Cameras[i].Animations[j].FieldOfView;
                            xCam.Cameras[i].FocalLength = xCam.Cameras[i].Animations[j].FocalLength;
                            xCam.Cameras[i].FDist       = xCam.Cameras[i].Animations[j].FDist;
                            xCam.Cameras[i].FStop       = xCam.Cameras[i].Animations[j].FStop;

                        }

                        addressTracker += 48;
                    }
                }
            }
        }

        /// <summary>
        /// Loads Camera Switches for this XCam TODO: Add this
        /// </summary>
        public static void LoadCameraSwitches(XCam cam)
        {

        }

        /// <summary>
        /// Loads Notetracks for this XCam
        /// </summary>
        public static void LoadNotetracks(XCam cam)
        {
            if(cam.Notetracks?.Length > 0 && cam.NotetrackLocation > 0)
            {
                long addressTracker = cam.NotetrackLocation;

                for(int i = 0; i < cam.Notetracks.Length; i++)
                {
                    cam.Notetracks[i] = new XCam.Notetrack()
                    {
                        Note = T7Util.GetString(MemoryUtil.ReadInt32(T7Util.ActiveProcess, addressTracker)),
                        Frame = MemoryUtil.ReadInt32(T7Util.ActiveProcess, addressTracker + 4)
                    };

                    addressTracker += 8;
                }
            }
        }

        /// <summary>
        /// Loads Camera Switches for this XCam TODO: Add this
        /// </summary>
        public static void LoadTargetModelBoneRoots(XCam cam)
        {
            if(cam.TargetModelBoneRootLocation > 0)
            {
                cam.TargetModelBoneRoots = new XCam.TargetModelBoneRoot[1];
                cam.TargetModelBoneRoots[0] = new XCam.TargetModelBoneRoot();

                long addressTracker = cam.TargetModelBoneRootLocation;


                string name = T7Util.GetString(MemoryUtil.ReadInt32(T7Util.ActiveProcess, addressTracker));
                int numFrames = MemoryUtil.ReadInt32(T7Util.ActiveProcess, addressTracker + 4);

                addressTracker += 16;

                cam.TargetModelBoneRoots[0].Name = name;
                cam.TargetModelBoneRoots[0].Animation = new XCam.TargetModelBoneRootFrame[numFrames];

                for(int i = 0; i < numFrames; i++)
                {
                    byte[] buffer = MemoryUtil.ReadBytes(T7Util.ActiveProcess, addressTracker, 28);


                    cam.TargetModelBoneRoots[0].Animation[i] = new XCam.TargetModelBoneRootFrame
                    {
                        Frame = i
                    };

                    // Y values is inverted
                    cam.TargetModelBoneRoots[0].Animation[i].Offset[0] = Math.Round(BitConverter.ToSingle(buffer, 0), 4);
                    cam.TargetModelBoneRoots[0].Animation[i].Offset[1] = Math.Round(BitConverter.ToSingle(buffer, 4), 4) * - 1;
                    cam.TargetModelBoneRoots[0].Animation[i].Offset[2] = Math.Round(BitConverter.ToSingle(buffer, 8), 4);

                    // Rotations are stored as Quaternions, but in XCAM_EXPORT are Matrices
                    Matrix matrix = new Quaternion()
                    {
                        X = Math.Round(BitConverter.ToSingle(buffer, 12), 4),
                        Y = Math.Round(BitConverter.ToSingle(buffer, 16), 4) * - 1,
                        Z = Math.Round(BitConverter.ToSingle(buffer, 20), 4) * - 1,
                        W = Math.Round(BitConverter.ToSingle(buffer, 24), 4)
                    }.ToMatrix();

                    cam.TargetModelBoneRoots[0].Animation[i].Axis["x"] = matrix.X.ToArray();
                    cam.TargetModelBoneRoots[0].Animation[i].Axis["y"] = matrix.Y.ToArray();
                    cam.TargetModelBoneRoots[0].Animation[i].Axis["z"] = matrix.Z.ToArray();

                    addressTracker += 28;

                }


            }
        }

    }
}
