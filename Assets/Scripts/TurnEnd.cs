using System.Collections;
using UnityEngine;

// Added to the ball, waits for TurnMaxDuration and then checks for strikes
// if a strike is achieved goes to StrikeAchieved state if not goes to
// TurnEnd state
public class TurnEnd : MonoBehaviour, ICanBeKilledByDeathBox
{
    [SerializeField] private GameState _gameState;

    private void OnEnable()
    {
        //SubscribeToDeathBoxEvent();
        StartCoroutine(EndTurn(_gameState.TurnMaxDuration));
    }

    IEnumerator EndTurn(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        if (_gameState.StrikeCounter == 10)
        {
            _gameState.CurrentGameState = GameState.GameStateEnum.StrikeAchieved;
            _gameState.Score += _gameState.StrikeExtraPoints;
        }
        _gameState.StrikeCounter = 0;
        yield return new WaitForSeconds(3);
        _gameState.CurrentGameState = GameState.GameStateEnum.TurnEnd;
    }
    void ForceTurnEnd()
    {
        //StopAllCoroutines();
        // StartCoroutine(EndTurn(0));
    }
    
    public void SubscribeToDeathBoxEvent()
    {
        DeathBox deathBox = FindObjectOfType<DeathBox>();
        deathBox.OnObjectEnteredDeathBox.AddListener(ForceTurnEnd);
    }
}
