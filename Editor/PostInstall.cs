using UnityEditor;
using UnityEngine;
using System.IO;

[InitializeOnLoad]
public static class BoozClientPostInstall
{
    static BoozClientPostInstall()
    {
        string packagePath = "Packages/com.boozgame.webglclient";
        string targetPath = "Assets/BoozClient";

        if (!Directory.Exists(targetPath))
        {
            Debug.Log("[BoozClient] Installation détectée. Copie des fichiers dans Assets/BoozClient...");
            FileUtil.CopyFileOrDirectory(packagePath, targetPath);
            AssetDatabase.Refresh();
            Debug.Log("[BoozClient] Fichiers copiés avec succès !");
        }
        else
        {
            Debug.Log("[BoozClient] Le dossier Assets/BoozClient existe déjà. Aucune action.");
        }
    }
}
