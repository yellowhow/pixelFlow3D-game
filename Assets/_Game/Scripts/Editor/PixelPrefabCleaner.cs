using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace PixelFlow3D.Editor
{
    /// <summary>
    /// Creates clean Pixel prefabs by loading the broken ones, stripping all
    /// missing scripts, and saving back as clean prefabs.
    /// </summary>
    public class PixelPrefabCleaner : EditorWindow
    {
        [MenuItem("PixelFlow3D/Clean All Prefabs")]
        public static void CleanAllPixelPrefabs()
        {
            if (!EditorUtility.DisplayDialog("Clean All Prefabs",
                "Will clean ALL prefabs in:\n" +
                "- Assets/GameObject/Grid/\n" +
                "- Assets/GameObject/HardPixel/\n" +
                "- Assets/GameObject/Conveyor/\n" +
                "- Assets/GameObject/Shooter/\n\n" +
                "All missing script references will be removed.",
                "Proceed", "Cancel"))
                return;

            CleanDirectory("Assets/GameObject/Grid");
            CleanDirectory("Assets/GameObject/HardPixel");
            CleanDirectory("Assets/GameObject/Conveyor");
            CleanDirectory("Assets/GameObject/Shooter");

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void CleanDirectory(string dirPath)
        {
            string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { dirPath });

            int cleaned = 0;
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                if (CleanPrefabViaAPI(path))
                    cleaned++;
            }

            Debug.Log($"[PrefabCleaner] Cleaned {cleaned}/{guids.Length} prefabs in {dirPath}.");
        }

        private static bool CleanPrefabViaAPI(string assetPath)
        {
            // Load the prefab
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
            if (prefab == null)
            {
                Debug.LogWarning($"[PrefabCleaner] Cannot load: {assetPath}");
                return false;
            }

            // Instantiate to modify
            GameObject instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            if (instance == null) return false;

            bool modified = false;

            // Remove missing components from the instance and all children
            RemoveMissingComponents(instance, ref modified);

            if (modified)
            {
                // Save back as prefab
                PrefabUtility.SaveAsPrefabAsset(instance, assetPath);
                Debug.Log($"[PrefabCleaner] Cleaned: {Path.GetFileName(assetPath)}");
            }

            Object.DestroyImmediate(instance);
            return modified;
        }

        private static void RemoveMissingComponents(GameObject go, ref bool modified)
        {
            // Unity's GameObjectUtility.RemoveMonoBehavioursWithMissingScript
            int removed = GameObjectUtility.RemoveMonoBehavioursWithMissingScript(go);
            if (removed > 0)
                modified = true;

            // Recursively process children
            foreach (Transform child in go.transform)
            {
                RemoveMissingComponents(child.gameObject, ref modified);
            }
        }
    }
}
