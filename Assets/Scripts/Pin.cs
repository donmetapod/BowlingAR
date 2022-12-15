using System.Collections;
using UnityEngine;

public class Pin : MonoBehaviour
{
    [SerializeField] private GameState _gameState;
    private bool _collided;
    private void OnCollisionEnter(Collision collision)
    {
        if(_collided)
            return;

        if (collision.transform.CompareTag("Ball") || collision.transform.CompareTag("Pin"))
        {
            StartCoroutine(CheckIfPinIsAlive());    
        }
    }

    // After pin is hit by the ball or another pin, check if it was tumbled
    IEnumerator CheckIfPinIsAlive()
    {
        _collided = true;
        yield return new WaitForSeconds(3);
        float angle = Vector3.Angle(transform.up, Vector3.up);
        if (angle != 0) // pin was tumbled
        {
            _gameState.Score++;
            _gameState.StrikeCounter++;
        }
    }
}
