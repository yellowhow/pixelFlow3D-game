using PixelFlow3D.Core;
using TMPro;
using UnityEngine;

namespace PixelFlow3D.UI
{
    /// <summary>
    /// 局内 HUD：关卡号、剩余像素、暂停按钮（后续扩展）
    /// </summary>
    public class HUDController : UIPanel
    {
        [Header("UI 引用")]
        [SerializeField] private TMP_Text _levelText;
        [SerializeField] private TMP_Text _remainingText;

        private void Update()
        {
            if (_remainingText != null)
            {
                var gm = FindAnyObjectByType<GridManager>();
                if (gm != null)
                    _remainingText.text = $"Left: {gm.RemainingPixels}";
            }
        }

        protected override void OnShow()
        {
            if (_levelText != null && GameManager.Instance != null)
                _levelText.text = $"level {GameManager.Instance.CurrentLevel}";
        }
    }
}
