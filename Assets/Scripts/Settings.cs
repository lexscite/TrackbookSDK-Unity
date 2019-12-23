using UnityEngine;
using UnityEditor;

using Trackbook;

public static class Settings
{
    [MenuItem("TrackbookSDK/Create Settings Asset")]
    public static void CreateSettingsAsset()
    {
        var asset = ScriptableObject.CreateInstance<TrackbookSettings>();

        AssetDatabase.CreateAsset(asset, "Assets/TrackbookSDK/Resources/TrackbookSettings.asset");
        AssetDatabase.SaveAssets();
    }

    [MenuItem("TrackbookSDK/Export Package")]
    public static void ExportPackage()
    {
        var files = new string[]
        {
            "Assets/TrackbookSDK",
            "Assets/Plugins/iOS/TrackbookSDK/IOSHelper.mm",
            "Assets/Plugins/iOS/TrackbookSDK/IOSHelper.mm.meta",
        };

        var path = "TrackbookSDK.unitypackage";

        AssetDatabase.ExportPackage(files, path, ExportPackageOptions.Recurse);
    }
}