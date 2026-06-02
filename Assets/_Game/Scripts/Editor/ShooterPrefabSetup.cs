using UnityEngine;
using UnityEditor;
using TMPro;

namespace PixelFlow3D.Editor
{
    /// <summary>
    /// 挂载自定义脚本到清理后的 Shooter.prefab（重复运行安全）
    /// </summary>
    public class ShooterPrefabSetup : EditorWindow
    {
        [MenuItem("PixelFlow3D/Setup Shooter Prefab")]
        public static void Setup()
        {
            string prefabPath = "Assets/GameObject/Shooter/Shooter.prefab";
            GameObject prefab = PrefabUtility.LoadPrefabContents(prefabPath);
            if (prefab == null)
            {
                Debug.LogError("[ShooterSetup] 未找到 Shooter.prefab！");
                return;
            }

            // 1. 主 Shooter 脚本（避免重复）
            var shooter = GetOrAddComponent<Shooter>(prefab);

            // 2. Model_Shooter 的子节点引用
            var modelShooter = prefab.transform.Find("Scale Pivot/Model Pivot/Model_Shooter");
            if (modelShooter != null)
            {
                var renderer = modelShooter.GetComponent<MeshRenderer>();
                var so = new SerializedObject(shooter);
                so.FindProperty("_bodyRenderer").objectReferenceValue = renderer;
                so.ApplyModifiedProperties();
                Debug.Log("[ShooterSetup] 已赋值 body renderer");
            }

            // 3. Ammo Text 赋值
            var ammoText = prefab.transform.Find("Scale Pivot/Model Pivot/Model_Shooter/Ammo Text Pivot/Ammo Text");
            if (ammoText != null)
            {
                var tmp = ammoText.GetComponent<TextMeshPro>();
                if (tmp == null)
                {
                    tmp = ammoText.gameObject.AddComponent<TextMeshPro>();
                    tmp.text = "0";
                    tmp.fontSize = 5;
                    tmp.alignment = TMPro.TextAlignmentOptions.Center;
                    Debug.Log("[ShooterSetup] 补建了 TextMeshPro");
                }
                var so = new SerializedObject(shooter);
                so.FindProperty("_ammoText").objectReferenceValue = tmp;
                so.ApplyModifiedProperties();
                Debug.Log("[ShooterSetup] 已赋值 Ammo Text");
            }
            else
                Debug.LogWarning("[ShooterSetup] 未找到 Ammo Text");

            // 4. 占位脚本（避免重复）
            GetOrAddComponent<ShooterAnimationController>(prefab);
            GetOrAddComponent<ShooterModelController>(prefab);
            GetOrAddComponent<ShooterWeaponController>(prefab);
            Debug.Log("[ShooterSetup] 已挂载占位脚本");

            // 5. Animator（Idle 动画，避免重复）
            if (modelShooter != null)
            {
                var animator = GetOrAddComponent<Animator>(modelShooter.gameObject);
                var controller = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>(
                    "Assets/AnimatorController/Model_Shooter.controller");
                if (controller != null)
                {
                    animator.runtimeAnimatorController = controller;
                    Debug.Log("[ShooterSetup] 已挂载 Animator");
                }
                else
                    Debug.LogWarning("[ShooterSetup] 未找到 AnimatorController");
            }

            // 6. 删除 Level Design（编辑器预览）
            var levelDesign = prefab.transform.Find("Level Design");
            if (levelDesign != null)
            {
                DestroyImmediate(levelDesign.gameObject);
                Debug.Log("[ShooterSetup] 已删除 Level Design");
            }

            PrefabUtility.SaveAsPrefabAsset(prefab, prefabPath);
            PrefabUtility.UnloadPrefabContents(prefab);

            Debug.Log("[ShooterSetup] 完成！");
            AssetDatabase.Refresh();
        }

        private static T GetOrAddComponent<T>(GameObject go) where T : Component
        {
            var c = go.GetComponent<T>();
            return c != null ? c : go.AddComponent<T>();
        }
    }
}
