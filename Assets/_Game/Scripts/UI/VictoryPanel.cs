using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace PixelFlow3D.UI
{
    /// <summary>
    /// 胜利结算面板：显示通关信息 + 继续按钮
    /// </summary>
    public class VictoryPanel : UIPanel
    {
        [Header("UI 引用")]
        [SerializeField] private TMP_Text _titleText;
        [SerializeField] private TMP_Text _levelText;
        [SerializeField] private Button _continueButton;

        private int _currentLevel;

        protected override void OnShow()
        {
            if (_levelText != null)
                _levelText.text = $"level {_currentLevel}";

            _continueButton?.onClick.AddListener(OnContinueClicked);
        }

        protected override void OnHide()
        {
            _continueButton?.onClick.RemoveListener(OnContinueClicked);
        }

        public void SetLevel(int levelNumber)
        {
            _currentLevel = levelNumber;
        }

        private void OnContinueClicked()
        {
            UIManager.Instance?.TransitionToMainMenu();
        }
    }
}
