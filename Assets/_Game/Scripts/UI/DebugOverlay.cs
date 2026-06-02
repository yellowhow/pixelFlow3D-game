using PixelFlow3D.Core;
using UnityEngine;

namespace PixelFlow3D.UI
{
    /// <summary>
    /// Debug 信息覆盖层：在屏幕上实时显示游戏状态
    /// </summary>
    public class DebugOverlay : MonoBehaviour
    {
        [SerializeField] private ConveyorBelt _conveyor;
        [SerializeField] private SlotManager _slotManager;

        private void OnGUI()
        {
            if (_conveyor == null) return;

            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.fontSize = 24;
            style.normal.textColor = Color.white;

            float y = 10;
            float lineHeight = 30;

            GUI.Label(new Rect(10, y, 400, lineHeight),
                $"传送带发射器: {_conveyor.ActiveShooterCount} / {_conveyor.MaxSlots}", style);
            y += lineHeight;

            if (_slotManager != null)
            {
                GUI.Label(new Rect(10, y, 400, lineHeight),
                    $"槽位: {_slotManager.Occupied} / {_slotManager.Max}", style);
            }
        }
    }
}
