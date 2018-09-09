using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;


class Settings
{
    /// <summary>
    /// Export Options
    /// </summary>
    public Dictionary<string, bool> ExportOptions = new Dictionary<string, bool>()
    {
        { "sound",                      true },
        { "map_ents",                   true },
        { "localize",                   true },
        { "rawfile",                    true },
        { "stringtable",                true },
        { "scriptparsetree",            true },
        { "rumble",                     true },
        { "animselectortable",          true },
        { "animmappingtable",           true },
        { "animstatemachine",           true },
        { "behaviortree",               true },
        { "xcam",                       true },
        { "physpreset",                 true },
        { "weaponcamo",                 true },
        { "structuredtable",                 true },
    };

    /// <summary>
    /// Fast File Options
    /// </summary>
    public Dictionary<string, bool> FastFileOptions = new Dictionary<string, bool>()
    {
        { "DeleteDecodedFile",          true },
    };

    /// <summary>
    /// Current Active Settings
    /// </summary>
    public static Settings ActiveSettings;

    /// <summary>
    /// Loads JSON Settings File
    /// </summary>
    /// <param name="file">File Path</param>
    public static void Load(string file)
    {
        ActiveSettings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(file));
    }

    /// <summary>
    /// Writes Settings to JSON File
    /// </summary>
    /// <param name="file">File Path</param>
    public static void Write(string file)
    {
        if (ActiveSettings == null)
            ActiveSettings = new Settings();

        File.WriteAllText(file, JsonConvert.SerializeObject(ActiveSettings));
    }
}
