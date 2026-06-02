using DG.Tweening;
using UnityEngine;

namespace PixelFlow3D.UI
{
    /// <summary>
    /// 面板层级：Back=背景层 Game=游戏层 Popup=弹窗层（后渲染的在上层）
    /// </summary>
    public enum PanelLayer { Background = 0, Game = 1, Popup = 2 }

    /// <summary>
    /// UI 面板基类。子类重写 OnShow/OnHide 处理逻辑。
    /// </summary>
    public class UIPanel : MonoBehaviour
    {
        [SerializeField] private PanelLayer _layer = PanelLayer.Background;
        [SerializeField] private CanvasGroup _canvasGroup;

        public PanelLayer Layer => _layer;

        private void Awake()
        {
            if (_canvasGroup == null)
                _canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        /// <summary>
        /// 显示面板（淡入动画）
        /// </summary>
        public void Show(float duration = 0.3f)
        {
            gameObject.SetActive(true);
            _canvasGroup.alpha = 0f;
            _canvasGroup.DOFade(1f, duration).SetEase(Ease.OutCubic);
            OnShow();
        }

        /// <summary>
        /// 隐藏面板（淡出动画）
        /// </summary>
        public void Hide(float duration = 0.3f)
        {
            _canvasGroup.DOFade(0f, duration)
                .SetEase(Ease.InCubic)
                .OnComplete(() =>
                {
                    gameObject.SetActive(false);
                    OnHide();
                });
        }

        /// <summary>
        /// 面板显示时回调（子类重写）
        /// </summary>
        protected virtual void OnShow() { }

        /// <summary>
        /// 面板隐藏时回调（子类重写）
        /// </summary>
        protected virtual void OnHide() { }
    }
}
