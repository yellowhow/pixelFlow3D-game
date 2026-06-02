using UnityEngine;

namespace PixelFlow3D.Data
{
    /// <summary>
    /// 颜色映射表：material 索引 → Unity Material 资产
    /// 在 Inspector 里拖入 PixelBlocks 的预置材质即可
    /// </summary>
    [CreateAssetMenu(fileName = "ColorPalette", menuName = "PixelFlow3D/Color Palette")]
    public class ColorPalette : ScriptableObject
    {
        public Material[] materials = new Material[34];
        public Material[] materialsVariant1 = new Material[34];
        public Material[] materialsVariant2 = new Material[34];
    }
}
