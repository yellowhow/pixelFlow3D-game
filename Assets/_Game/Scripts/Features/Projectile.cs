using DG.Tweening;
using UnityEngine;
using UnityEngine.Pool;

namespace PixelFlow3D
{
    /// <summary>
    /// 子弹：从发射器飞向目标像素造成伤害
    /// 使用对象池，池在 ConveyorBelt.Start 中通过 InitPool 初始化
    /// </summary>
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float _speed = 8f;

        private Vector3 _targetPos;
        private int _targetX, _targetY;
        private System.Action<int, int, int> _onHit;
        private bool _launched;
        private TrailRenderer _trail;

        private static ObjectPool<GameObject> _pool;

        public static void InitPool(GameObject prefab, int capacity = 20, int max = 50)
        {
            if (_pool != null) return;
            _pool = new ObjectPool<GameObject>(
                () => { var go = Instantiate(prefab); foreach (var c in go.GetComponentsInChildren<Collider>()) Destroy(c); go.AddComponent<Projectile>(); return go; },
                go => { go.SetActive(true); go.GetComponent<Projectile>()._trail?.Clear(); },
                go => { go.transform.DOKill(); var p = go.GetComponent<Projectile>(); p._launched = false; p._onHit = null; var t = p._trail; if (t != null) { t.Clear(); t.emitting = false; } go.SetActive(false); },
                go => Destroy(go),
                false, capacity, max);
        }

        public static Projectile Create(Vector3 startPos, int targetX, int targetY,
            System.Action<int, int, int> onHit)
        {
            if (_pool == null) return null;

            GameObject go = _pool.Get();
            go.transform.position = startPos;

            var p = go.GetComponent<Projectile>();
            // 位置设好后清拖尾再启动，避免闪烁
            if (p._trail != null) { p._trail.Clear(); p._trail.emitting = true; }
            p._targetX = targetX;
            p._targetY = targetY;
            p._onHit = onHit;
            p._launched = true;
            return p;
        }

        private void Awake()
        {
            _trail = GetComponentInChildren<TrailRenderer>();
        }

        private void Update()
        {
            if (!_launched) return;

            Vector3 toTarget = _targetPos - transform.position;
            float step = _speed * Time.deltaTime;

            if (toTarget.magnitude <= step)
            {
                _onHit?.Invoke(_targetX, _targetY, 1);
                _pool.Release(gameObject);
            }
            else
            {
                transform.position += toTarget.normalized * step;
            }
        }

        public void SetTarget(Vector3 targetWorldPos)
        {
            _targetPos = targetWorldPos;
        }
    }
}
