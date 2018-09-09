/*
 *  HydraX - Copyright 2018 Philip/Scobalula
 *  
 *  This file is subject to the license terms set out in the
 *  "LICENSE.txt" file. 
 * 
 */
using System.IO;
using PhilUtil;
using HydraLib.T7.Assets;
using HydraLib.T7.Assets.JsonFiles;

namespace HydraLib.GDT
{
    class GDTUtil
    {
        public static void WriteRumble(string name, Rumble rumble)
        {
            string path = "exported_files\\source_data\\rumble_gdts\\" + name + "_gdt.gdt";
            PathUtil.CreateFilePath(path);
            using (StreamWriter streamWriter = new StreamWriter(path))
            {
                streamWriter.WriteLine("{");
                streamWriter.WriteLine("	\"{0}\" ( \"rumble.gdf\" )", name);
                streamWriter.WriteLine("	{");
                streamWriter.WriteLine("		\"configstringFileType\" \"RUMBLE\"");
                streamWriter.WriteLine("		\"highRumbleFile\" \"{0}\"", rumble.HighRumble);
                streamWriter.WriteLine("		\"lowRumbleFile\" \"{0}\"", rumble.LowRumble);
                streamWriter.WriteLine("		\"duration\" \"{0}\"", rumble.Duration);
                streamWriter.WriteLine("		\"range\" \"{0}\"", rumble.Range);
                streamWriter.WriteLine("		\"fadeWithDistance\" \"{0}\"", rumble.FadeWithDistance);
                streamWriter.WriteLine("		\"broadcast\" \"{0}\"", rumble.Broadcast);
                streamWriter.WriteLine("		\"camShakeRange\" \"{0}\"", rumble.CamShakeRange);
                streamWriter.WriteLine("		\"camShakeScale\" \"{0}\"", rumble.CamShakeScale);
                streamWriter.WriteLine("		\"camShakeDuration\" \"{0}\"", rumble.CamShakeDuration);
                streamWriter.WriteLine("		\"pulseScale\" \"{0}\"", rumble.PulseScale);
                streamWriter.WriteLine("		\"pulseRadiusOuter\" \"{0}\"", rumble.PulseRadiusOuter);
                streamWriter.WriteLine("	}");
                streamWriter.WriteLine("}");
                streamWriter.WriteLine();
            }
        }

        public static void WriteXCamGDT(string name, XCam xcam)
        {
            string path = "exported_files\\source_data\\xcam_gdts\\" + name + "_gdt.gdt";
            PathUtil.CreateFilePath(path);
            using (StreamWriter streamWriter = new StreamWriter(path))
            {
                streamWriter.WriteLine("{");
                streamWriter.WriteLine("	\"{0}\" ( \"xcam.gdf\" )", name);
                streamWriter.WriteLine("	{");
                streamWriter.WriteLine("		\"filename\" \"{0}\"", "hydrax_export\\\\" + name + ".XCAM_EXPORT");
                streamWriter.WriteLine("		\"autoMotionBlur\" \"{0}\"", xcam.AutoMotionBlur);
                streamWriter.WriteLine("		\"disableNearDof\" \"{0}\"", xcam.DisableNearFov);
                streamWriter.WriteLine("		\"easeAnimationsOut\" \"{0}\"", xcam.EaseAnimationOut);
                streamWriter.WriteLine("		\"hide_hud\" \"{0}\"", xcam.HideHud);
                streamWriter.WriteLine("		\"hide_local_player\" \"{0}\"", xcam.HideLocalPlayer);
                streamWriter.WriteLine("		\"is_looping\" \"{0}\"", xcam.IsLooping);
                streamWriter.WriteLine("		\"use_firstperson_player\" \"{0}\"", xcam.UseFPSPlayer);
                streamWriter.WriteLine("		\"rightStickRotateOffsetX\" \"{0}\"", xcam.RightStickRotationOffset[0]);
                streamWriter.WriteLine("		\"rightStickRotateOffsetY\" \"{0}\"", xcam.RightStickRotationOffset[1]);
                streamWriter.WriteLine("		\"rightStickRotateOffsetZ\" \"{0}\"", xcam.RightStickRotationOffset[2]);
                streamWriter.WriteLine("		\"rightStickRotateMaxDegreesX\" \"{0}\"", xcam.RightStickRotationDegrees[0]);
                streamWriter.WriteLine("		\"rightStickRotateMaxDegreesY\" \"{0}\"", xcam.RightStickRotationDegrees[1]);
                streamWriter.WriteLine("	}");
                streamWriter.WriteLine("}");
                streamWriter.WriteLine();
            }
        }
    }
}
