using System;
using System.Collections.Generic;

namespace PixelFlow3D.Data
{
    /// <summary>
    /// JSON-serializable grid point (matches JSON "X"/"Y" keys).
    /// </summary>
    [Serializable]
    public class GridPointData
    {
        public int X;
        public int Y;
    }

    /// <summary>
    /// Single pixel entry in the grid.
    /// </summary>
    [Serializable]
    public class PixelData
    {
        public int x;
        public int y;
        public int material;
        public int areaX;
        public int areaY;
    }

    /// <summary>
    /// Pixel health override (requires multiple hits).
    /// </summary>
    [Serializable]
    public class PixelHealthData
    {
        public int x;
        public int y;
        public int health;
    }

    /// <summary>
    /// Wall obstacle: blocks projectile paths.
    /// </summary>
    [Serializable]
    public class WallData
    {
        public List<GridPointData> GridPoints;
        public int BlockSize;
    }

    /// <summary>
    /// Ice block covering pixels.
    /// </summary>
    [Serializable]
    public class IceBlockData
    {
        public List<GridPointData> GridPoints;
        public int BlockSize;
        public int Health;
    }

    /// <summary>
    /// Wood block covering pixels.
    /// </summary>
    [Serializable]
    public class WoodBlockData
    {
        public List<GridPointData> GridPoints;
        public int BlockSize;
        public int Health;
    }

    /// <summary>
    /// Color door obstacle.
    /// </summary>
    [Serializable]
    public class ColorDoorData
    {
        public List<GridPointData> GridPoints;
        public int Material;
        public int Count;
    }

    /// <summary>
    /// Snake path feature.
    /// </summary>
    [Serializable]
    public class SnakeSegmentData
    {
        public List<GridPointData> GridPoints;
    }

    [Serializable]
    public class SnakeData
    {
        public List<GridPointData> MainGridPoints;
        public List<SnakeSegmentData> SnakeGridPoints;
        public int Material;
        public int Count;
    }

    /// <summary>
    /// Gate obstacle.
    /// </summary>
    [Serializable]
    public class GateData
    {
        public List<GridPointData> GridPoints;
        public int Direction;
        public int Length;
        public int Material;
        public int Count;
    }

    /// <summary>
    /// Egg in an egg box.
    /// </summary>
    [Serializable]
    public class EggData
    {
        public int Material;
        public int Count;
    }

    /// <summary>
    /// Egg box container.
    /// </summary>
    [Serializable]
    public class EggBoxData
    {
        public List<GridPointData> GridPoints;
        public GridPointData EggArea;
        public List<EggData> Eggs;
    }

    /// <summary>
    /// Pixel pipe feature.
    /// </summary>
    [Serializable]
    public class PixelPipeData
    {
        public List<GridPointData> GridPoints;
    }

    /// <summary>
    /// Golden egg feature.
    /// </summary>
    [Serializable]
    public class GoldenEggData
    {
        public List<GridPointData> GridPoints;
    }

    /// <summary>
    /// Biscuit feature.
    /// </summary>
    [Serializable]
    public class BiscuitData
    {
        public List<GridPointData> GridPoints;
        public int Count;
    }

    /// <summary>
    /// Split object feature.
    /// </summary>
    [Serializable]
    public class SplitObjectData
    {
        public List<GridPointData> GridPoints;
    }

    /// <summary>
    /// UFO feature.
    /// </summary>
    [Serializable]
    public class UfoData
    {
        public List<GridPointData> GridPoints;
    }
}
