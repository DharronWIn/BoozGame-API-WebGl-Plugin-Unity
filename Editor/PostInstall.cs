using UnityEditor;
using UnityEngine;
using System.IO;

[InitializeOnLoad]
public static class BoozClientPostInstall
{
    static BoozClientPostInstall()
    {
        string sourcePath = Path.Combine("Packages", "com.boozgame.webglclient", "RunTime");
        string targetPath = Path.Combine("Assets", "BoozClient");

        if (!Directory.Exists(targetPath))
        {
            Debug.Log("[BoozClient] Copie initiale des fichiers dans Assets/BoozClient...");
            FileUtil.CopyFileOrDirectory(sourcePath, targetPath);
            AssetDatabase.Refresh();
            Debug.Log("[BoozClient] Copie terminée avec succès.");
        }
    }
}
