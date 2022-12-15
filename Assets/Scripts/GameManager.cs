using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameState _gameState;

    private void Awake()
    {
        // Resets game state to the needed values for a new game
        _gameState.ResetState();
    }
    private void Start()
    {
        _gameState.CurrentGameState = GameState.GameStateEnum.PlacingPinDeckAndLane;
    }

    private void OnDisable()
    { 
        // Making sure game state gets reset when exiting game
        _gameState.CurrentGameState = GameState.GameStateEnum.TitleScreen;
    }
}
