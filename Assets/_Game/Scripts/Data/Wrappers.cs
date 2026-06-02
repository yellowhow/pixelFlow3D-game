using System;
using System.Collections.Generic;

namespace PixelFlow3D.Data
{
    /// <summary>
    /// Wrapper for JSON collections like "Walls", "IceBlocks", etc.
    /// </summary>
    [Serializable]
    public class WallsWrapper { public List<WallData> Walls; }
    [Serializable]
    public class IceBlocksWrapper { public List<IceBlockData> IceBlocks; }
    [Serializable]
    public class WoodBlocksWrapper { public List<WoodBlockData> WoodBlocks; }
    [Serializable]
    public class ColorDoorsWrapper { public List<ColorDoorData> Doors; }
    [Serializable]
    public class SnakesWrapper { public List<SnakeData> Snakes; }
    [Serializable]
    public class GatesWrapper { public List<GateData> Gates; }
    [Serializable]
    public class EggBoxesWrapper { public List<EggBoxData> EggBoxes; }
    [Serializable]
    public class PixelPipesWrapper { public List<PixelPipeData> Pipes; }
    [Serializable]
    public class GoldenEggsWrapper { public List<GoldenEggData> Eggs; }
    [Serializable]
    public class BiscuitsWrapper { public List<BiscuitData> Biscuits; }
    [Serializable]
    public class SplitObjectsWrapper { public List<SplitObjectData> SplitObjects; }
    [Serializable]
    public class UfosWrapper { public List<UfoData> Ufos; }
    [Serializable]
    public class KeysWrapper { public List<WallData> Keys; }
    [Serializable]
    public class SurprisePixelsWrapper { public List<GridPointData> Pixels; }
}
