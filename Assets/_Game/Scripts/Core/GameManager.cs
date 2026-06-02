using PixelFlow3D;
using PixelFlow3D.Core;
using PixelFlow3D.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 游戏状态管理器：控制关卡加载、胜利/失败流程
/// </summary>
public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Initializing,
        Playing,
        Paused,
        Won,
        Lost
    }

    public static GameManager Instance { get; private set; }

    [SerializeField] private GameState _currentState = GameState.Initializing;
    [SerializeField] private int _currentLevel = 1;

    public GameState CurrentState => _currentState;
    public int CurrentLevel => _currentLevel;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        _currentLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        GameEvents.AllPixelsCleared += OnAllPixelsCleared;
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Gameplay")
        {
            // 每次进 Gameplay 都从存档读最新关卡
            _currentLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
            SetState(GameState.Playing);
        }
    }

    /// <summary>
    /// 加载指定关卡
    /// </summary>
    public void LoadLevel(int levelNumber)
    {
        _currentLevel = levelNumber;
        SetState(GameState.Playing);

        // 重新加载场景（简单粗暴的方式）
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);

        Debug.Log($"[GameManager] 加载第 {levelNumber} 关");
    }

    public void SetState(GameState newState)
    {
        if (_currentState == newState) return;

        Debug.Log($"[GameManager] State: {_currentState} -> {newState}");

        switch (_currentState)
        {
            case GameState.Paused:
                Time.timeScale = 1f;
                break;
        }

        _currentState = newState;

        switch (newState)
        {
            case GameState.Won:
                ShowVictoryPanel();
                break;
            case GameState.Lost:
                Debug.Log("[GameManager] 失败！");
                break;
            case GameState.Paused:
                Time.timeScale = 0f;
                break;
        }
    }

    private void ShowVictoryPanel()
    {
        int nextLevel = _currentLevel + 1;
        if (nextLevel > PlayerPrefs.GetInt("UnlockedLevel", 1))
            PlayerPrefs.SetInt("UnlockedLevel", nextLevel);

        UIManager.Instance?.ShowVictory(_currentLevel);
    }

    private void OnAllPixelsCleared()
    {
        SetState(GameState.Won);
    }
}
