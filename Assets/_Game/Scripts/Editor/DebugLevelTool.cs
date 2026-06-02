using UnityEditor;
using UnityEngine;

namespace PixelFlow3D.Editor
{
    /// <summary>
    /// 开发测试用：重置或跳转关卡（仅 Editor，打包后不存在）
    /// </summary>
    public class DebugLevelTool : EditorWindow
    {
        [MenuItem("PixelFlow3D/关卡调试/重置到第1关")]
        private static void ResetLevel()
        {
            PlayerPrefs.SetInt("UnlockedLevel", 1);
            Debug.Log("[DebugLevel] 关卡已重置到第1关");
        }

        [MenuItem("PixelFlow3D/关卡调试/跳到第10关")]
        private static void JumpTo10() => JumpTo(10);
        [MenuItem("PixelFlow3D/关卡调试/跳到第50关")]
        private static void JumpTo50() => JumpTo(50);
        [MenuItem("PixelFlow3D/关卡调试/跳到第100关")]
        private static void JumpTo100() => JumpTo(100);
        [MenuItem("PixelFlow3D/关卡调试/跳到第200关")]
        private static void JumpTo200() => JumpTo(200);

        [MenuItem("PixelFlow3D/关卡调试/自定义跳转...")]
        private static void CustomJump()
        {
            var window = GetWindow<DebugLevelTool>(true, "跳转关卡");
            window.Show();
        }

        private int _targetLevel = 1;

        private void OnGUI()
        {
            GUILayout.Label("输入目标关卡号：", EditorStyles.boldLabel);
            _targetLevel = EditorGUILayout.IntField("关卡", _targetLevel);
            _targetLevel = Mathf.Max(1, _targetLevel);

            if (GUILayout.Button("确认跳转", GUILayout.Height(30)))
            {
                JumpTo(_targetLevel);
                Close();
            }
        }

        private static void JumpTo(int level)
        {
            PlayerPrefs.SetInt("UnlockedLevel", level);
            Debug.Log($"[DebugLevel] 已跳转到第{level}关（重启场景后生效）");
        }
    }
}
