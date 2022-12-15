using System.Collections;
using UnityEngine;

// Added to the ball, waits for TurnMaxDuration and then checks for strikes
// if a strike is achieved goes to StrikeAchieved state if not goes to
// TurnEnd state
public class TurnEnd : MonoBehaviour
{
    [SerializeField] private GameState _gameState;
    
    IEnumerator Start()
    {
        yield return new WaitForSeconds(_gameState.TurnMaxDuration);
        if (_gameState.StrikeCounter == 10)
        {
            _gameState.CurrentGameState = GameState.GameStateEnum.StrikeAchieved;
            _gameState.Score += 30;
        }
        _gameState.StrikeCounter = 0;
        yield return new WaitForSeconds(3);
        _gameState.CurrentGameState = GameState.GameStateEnum.TurnEnd;
    }
}
