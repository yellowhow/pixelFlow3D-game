using UnityEngine;
using Newtonsoft.Json;

namespace PixelFlow3D.Core
{
    /// <summary>
    /// Loads level data from JSON files in Resources.
    /// Usage: LevelData data = LevelLoader.LoadLevel(1);
    /// </summary>
    public static class LevelLoader
    {
        private const string ResourcePath = "Level_{0}";

        /// <summary>
        /// Load a single level by number. Returns null if file not found or parse error.
        /// </summary>
        public static Data.LevelData LoadLevel(int levelNumber)
        {
            string path = string.Format(ResourcePath, levelNumber);
            TextAsset jsonAsset = Resources.Load<TextAsset>(path);

            if (jsonAsset == null)
            {
                Debug.LogError($"[LevelLoader] Level {levelNumber} not found at Resources/{path}");
                return null;
            }

            try
            {
                Data.LevelData data = JsonConvert.DeserializeObject<Data.LevelData>(jsonAsset.text);
                if (data != null)
                {
                    Debug.Log($"[LevelLoader] Loaded Level {levelNumber}: {data.levelName}, " +
                              $"grid {data.pixelGrid?.width}x{data.pixelGrid?.height}, " +
                              $"pixels={data.pixelGrid?.pixels?.Count ?? 0}, " +
                              $"queues={data.queueGroup?.shooterQueues?.Count ?? 0}");
                }
                return data;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[LevelLoader] Failed to parse Level {levelNumber}: {e.Message}");
                return null;
            }
        }
    }
}
