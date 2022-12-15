using UnityEngine;

// Added to objects that need to be destroyed when turn ends
public class DestroyOnTurnEnd : MonoBehaviour
{
    [SerializeField] private GameState _gameState;

    private void OnEnable()
    {
        _gameState.OnTurnEnded.AddListener(DestroySelf);
    }

    private void OnDisable()
    {
        _gameState.OnTurnEnded.RemoveListener(DestroySelf);
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
