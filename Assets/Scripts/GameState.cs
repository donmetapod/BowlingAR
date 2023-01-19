using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "GameState", menuName = "ScriptableObjects/CreateGameStateAsset")]
public class GameState : ScriptableObject
{
    // All game's possible states
    public enum GameStateEnum
    {
        TitleScreen,
        PlacingPinDeckAndLane,
        SetupBalls,
        ReadyToThrow,
        BallInPlay,
        StrikeAchieved,
        TurnEnd,
        ResettingDeck,
        GameEnded
    }

    [SerializeField] private GameStateEnum _currentGameState;
    
    [HideInInspector] public UnityEvent OnEnterBallSetup;
    [HideInInspector] public UnityEvent OnReadyToThrow;
    [HideInInspector] public UnityEvent OnBallInPlay;
    [HideInInspector] public UnityEvent OnStrikeAchieved;
    [HideInInspector] public UnityEvent OnTurnEnded;
    [HideInInspector] public UnityEvent OnResettingDeck;
    [HideInInspector] public UnityEvent OnGameEnded;
    [HideInInspector] public UnityEvent<int> OnScoreChanged;
    [SerializeField] private int _score;
    [SerializeField] private int _remainingBalls;
    [SerializeField] private int _currentTurn;
    [SerializeField] private int _strikeCounter = 0;
    [SerializeField] private int _strikeExtraPoints = 10;
    public int MaxTurns = 10;
    public float _throwPowerMultiplier = 0.0025f;
    public int TurnMaxDuration = 10;

    #region Properties
    public GameStateEnum CurrentGameState
    {
        get => _currentGameState;
        set
        {
            _currentGameState = value;
            switch (value)
            {
                case GameStateEnum.SetupBalls:
                    OnEnterBallSetup?.Invoke();
                    break;
                case GameStateEnum.ReadyToThrow:
                    OnReadyToThrow?.Invoke();
                    break;
                case GameStateEnum.BallInPlay:
                    OnBallInPlay?.Invoke();
                    break;
                case GameStateEnum.StrikeAchieved:
                    OnStrikeAchieved?.Invoke();
                    break;
                case GameStateEnum.TurnEnd:
                    OnTurnEnded?.Invoke();
                    break;
                case GameStateEnum.ResettingDeck:
                    OnResettingDeck?.Invoke();
                    break;
                case GameStateEnum.GameEnded:
                    OnGameEnded?.Invoke();
                    break;
            }
        }
    }
    public int Score
    {
        get => _score;
        set
        {
            _score = value;
            OnScoreChanged?.Invoke(_score);
        }
    }
    public int RemainingBalls
    {
        get => _remainingBalls;
        set => _remainingBalls = value;
    }
    public int CurrentTurn
    {
        get => _currentTurn;
        set => _currentTurn = value;
    }
    public int StrikeCounter
    {
        get => _strikeCounter;
        set
        {
            _strikeCounter = value;
            // Messenger.Instance.EnqueueMessage($"{_strikeCounter}", 5);
        }
    }

    public int StrikeExtraPoints
    {
        get => _strikeExtraPoints;
        set => _strikeExtraPoints = value;
    }
    #endregion
    
    // Resets data of this SO to the initial state
    public void ResetState()
    {
        _currentTurn = 1;
        _score = 0;
        _remainingBalls = MaxTurns;
    }
    
    public void LoadNewScene(string sceneName)
    {
        SceneLoader.LoadScene(sceneName);
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}
