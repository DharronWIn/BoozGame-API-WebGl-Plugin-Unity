using UnityEditor;
using UnityEngine;
using System.IO;

[InitializeOnLoad]
public static class PostInstall
{
    static PostInstall()
    {
        string sourcePath = "Packages/com.boozgame.webglclient/RunTime";
        string targetPath = "Assets/BoozClient";

        if (!Directory.Exists(targetPath))
        {
            FileUtil.CopyFileOrDirectory(sourcePath, targetPath);
            AssetDatabase.Refresh();
            Debug.Log("[BoozClient] Prefabs copied to Assets/BoozClient");
        }
    }
}
