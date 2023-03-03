using System.Collections;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameState _gameState;
    [SerializeField] private TMP_Text _scoreUI;
    [SerializeField] private TMP_Text _nextTurnUI;
    [SerializeField] private TMP_Text _remainingBallsUI;
    [SerializeField] private TMP_Text _finalScoreUI;
    [SerializeField] private GameObject _strikeUI;
    [SerializeField] private GameObject _throwBallInstructions;
    [SerializeField] private GameObject _throwBallInstructions2;
    private bool _throwInstructionShowed;
    [SerializeField] private GameObject _gameOverScreen;
    [SerializeField] private float _turnWaitTime = 3;

    private void OnEnable()
    {
        _gameState.OnScoreChanged.AddListener(UpdateScoreUI);
        _gameState.OnTurnEnded.AddListener(ShowNextTurnUI);
        _gameState.OnGameEnded.AddListener(ShowGameOverScreen);
        _gameState.OnReadyToThrow.AddListener(ShowThrowBallInstructions);
        _gameState.OnBallInPlay.AddListener(UpdateAmountOfBallsUI);
        _gameState.OnGameEnded.AddListener(UpdateFinalScoreUI);
        _gameState.OnStrikeAchieved.AddListener(ShowStrikeUI);
    }

    private void OnDisable()
    {
        _gameState.OnScoreChanged.RemoveListener(UpdateScoreUI);
        _gameState.OnTurnEnded.RemoveListener(ShowNextTurnUI);
        _gameState.OnGameEnded.RemoveListener(ShowGameOverScreen);
        _gameState.OnReadyToThrow.RemoveListener(ShowThrowBallInstructions);
        _gameState.OnBallInPlay.RemoveListener(UpdateAmountOfBallsUI);
        _gameState.OnGameEnded.RemoveListener(UpdateFinalScoreUI);
        _gameState.OnStrikeAchieved.RemoveListener(ShowStrikeUI);
    }

    private void Start()
    {
        UpdateAmountOfBallsUI();
        UpdateScoreUI(0);
    }
    
    void UpdateScoreUI(int newScore)
    {
        _scoreUI.text = $"Score: {newScore}";
    }

    public void ShowStrikeUI()
    {
        _strikeUI.SetActive(true);
    }
    public void ShowNextTurnUI()
    {
        StartCoroutine(ShowNextTurnRoutine());
    }
    
    IEnumerator ShowNextTurnRoutine()
    {
        // Increases the current turn number
        _gameState.CurrentTurn++;
        
        if (_gameState.CurrentTurn <= _gameState.MaxTurns)
        {
            _nextTurnUI.gameObject.SetActive(true);
            _nextTurnUI.text = $"Turn {_gameState.CurrentTurn}";
            yield return new WaitForSeconds(_turnWaitTime);
            _nextTurnUI.gameObject.SetActive(false);
            _gameState.CurrentGameState = GameState.GameStateEnum.ResettingDeck;
        }
        else
        {
            _gameState.CurrentGameState = GameState.GameStateEnum.GameEnded;
        }
    }

    void ShowGameOverScreen()
    {
        _strikeUI.SetActive(false);
        _gameOverScreen.SetActive(true);
    }

    void UpdateFinalScoreUI()
    {
        _finalScoreUI.text = $"Score: {_gameState.Score}";
    }
    void UpdateAmountOfBallsUI()
    {
        _remainingBallsUI.text = $"Remaining balls {_gameState.RemainingBalls}";
    }

    void ShowThrowBallInstructions()
    {
        if(_throwInstructionShowed)
            return;

        _throwInstructionShowed = true;
        _throwBallInstructions.SetActive(true);
        Invoke("HideThrowBallInstructions", 4);
    }

    void HideThrowBallInstructions()
    {
        _throwBallInstructions.SetActive(false);
        _throwBallInstructions2.SetActive(true);
        Invoke("HideThrowBallInstructions2", 4);
    }

    void HideThrowBallInstructions2()
    {
        _throwBallInstructions2.SetActive(false);
    }
}
