using UnityEditor;
using UnityEngine;
using System.IO;

namespace PixelFlow3D.Editor
{
    /// <summary>
    /// Fixes all Pixel prefabs by applying correct materials from Pixel 1x1 reference.
    /// Handles both direct Model_Pixel child and nested HardPixel prefab instances.
    /// </summary>
    public class PixelShaderFixer : EditorWindow
    {
        [MenuItem("PixelFlow3D/Fix All Pixel Shaders")]
        public static void FixAll()
        {
            string gridPath = "Assets/GameObject/Grid";

            // 1. Get reference material from Pixel 1x1's Model_Pixel
            Material refMaterial = GetReferenceMaterial(gridPath);
            if (refMaterial == null)
            {
                Debug.LogError("[ShaderFixer] Cannot get reference material from Pixel 1x1!");
                return;
            }
            Debug.Log($"[ShaderFixer] Reference material: {refMaterial.name}, Shader: {refMaterial.shader.name}");

            // 2. Fix all Pixel prefabs in Grid/
            string[] pixelGuids = AssetDatabase.FindAssets("t:Prefab", new[] { gridPath });
            int pixelFixed = 0;

            foreach (string guid in pixelGuids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                string filename = Path.GetFileNameWithoutExtension(path);
                if (filename == "Pixel 1x1") continue;

                if (FixPrefabMaterial(path, refMaterial))
                    pixelFixed++;
            }

            // 3. Fix all HardPixel prefabs
            string hardPixelPath = "Assets/GameObject/HardPixel";
            string[] hardGuids = AssetDatabase.FindAssets("t:Prefab", new[] { hardPixelPath });
            int hardFixed = 0;

            foreach (string guid in hardGuids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                if (FixPrefabMaterial(path, refMaterial))
                    hardFixed++;
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"[ShaderFixer] Done! Fixed {pixelFixed} Pixel + {hardFixed} HardPixel prefabs.");
        }

        private static Material GetReferenceMaterial(string gridPath)
        {
            string refPath = gridPath + "/Pixel 1x1.prefab";
            GameObject refPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(refPath);
            if (refPrefab == null) return null;

            Transform refModel = refPrefab.transform.Find("Model_Pixel");
            if (refModel == null) return null;

            MeshRenderer mr = refModel.GetComponent<MeshRenderer>();
            if (mr == null) return null;

            return mr.sharedMaterial;
        }

        private static bool FixPrefabMaterial(string assetPath, Material refMaterial)
        {
            GameObject prefab = PrefabUtility.LoadPrefabContents(assetPath);
            if (prefab == null) return false;

            bool modified = false;

            // Fix all MeshRenderers in this prefab and its children
            MeshRenderer[] renderers = prefab.GetComponentsInChildren<MeshRenderer>(true);
            foreach (var mr in renderers)
            {
                if (mr.sharedMaterial == null ||
                    mr.sharedMaterial.name == "Default-Material" ||
                    mr.sharedMaterial.shader.name == "Standard")
                {
                    Material[] mats = mr.sharedMaterials;
                    for (int i = 0; i < mats.Length; i++)
                    {
                        mats[i] = refMaterial;
                    }
                    mr.sharedMaterials = mats;
                    modified = true;
                }
            }

            if (modified)
            {
                PrefabUtility.SaveAsPrefabAsset(prefab, assetPath);
                Debug.Log($"[ShaderFixer] Fixed: {Path.GetFileName(assetPath)}");
            }

            PrefabUtility.UnloadPrefabContents(prefab);
            return modified;
        }
    }
}
