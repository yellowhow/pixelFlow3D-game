using UnityEngine;
using PixelFlow3D.Data;

namespace PixelFlow3D.Core
{
    /// <summary>
    /// 将材质索引映射到预置的 PixelBlocks 材质
    /// 材质来源为 ColorPalette.asset，在 Inspector 中拖入即可
    /// </summary>
    public class ColorConfig : MonoBehaviour
    {
        public static ColorConfig Instance { get; private set; }

        [SerializeField] private ColorPalette _palette;

        private Material[] _materials;
        private Material[] _variants1;
        private Material[] _variants2;
        public int MaterialCount => _materials?.Length ?? 0;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            BuildMaterials();
        }

        private void BuildMaterials()
        {
            if (_palette == null || _palette.materials == null)
            {
                Debug.LogError("[ColorConfig] ColorPalette 未配置，请在 Inspector 中拖入");
                _materials = new Material[0];
                _variants1 = new Material[0];
                _variants2 = new Material[0];
                return;
            }

            int count = _palette.materials.Length;
            _materials = new Material[count];
            _variants1 = new Material[count];
            _variants2 = new Material[count];
            for (int i = 0; i < count; i++)
            {
                _materials[i] = _palette.materials[i];
                _variants1[i] = _palette.materialsVariant1.Length > i ? _palette.materialsVariant1[i] : null;
                _variants2[i] = _palette.materialsVariant2.Length > i ? _palette.materialsVariant2[i] : null;
            }
            Debug.Log($"[ColorConfig] 从 ColorPalette 加载了 {_materials.Length} 个材质");
        }

        public Material GetMaterial(int index)
        {
            if (_materials == null || index < 0 || index >= _materials.Length)
                return null;
            return _materials[index];
        }

        /// <summary>
        /// 从 Block / Block 1 / Block 2 中随机取一个变体，用于像素视觉多样化
        /// </summary>
        public Material GetRandomVariant(int index)
        {
            if (_materials == null || index < 0 || index >= _materials.Length)
                return null;

            var pool = new System.Collections.Generic.List<Material>();
            if (_materials[index] != null) pool.Add(_materials[index]);
            if (_variants1 != null && index < _variants1.Length && _variants1[index] != null)
                pool.Add(_variants1[index]);
            if (_variants2 != null && index < _variants2.Length && _variants2[index] != null)
                pool.Add(_variants2[index]);

            return pool.Count > 0 ? pool[Random.Range(0, pool.Count)] : null;
        }
    }
}
