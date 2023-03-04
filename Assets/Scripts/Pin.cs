using System.Collections;
using UnityEngine;

public class Pin : MonoBehaviour
{
    [SerializeField] private GameState _gameState;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _hitUpForce = 5;
    private bool _collided;
    private void OnCollisionEnter(Collision collision)
    {
        if(_collided)
            return;

        if (collision.transform.CompareTag("Ball") || collision.transform.CompareTag("Pin"))
        {
            _collided = true;
            StartCoroutine(CheckIfPinIsAlive());   
            _rb.AddForce(Vector3.up * _hitUpForce, ForceMode.Impulse);
        }
    }

    // After pin is hit by the ball or another pin, check if it was tumbled
    IEnumerator CheckIfPinIsAlive()
    {
        yield return new WaitForSeconds(4);
        float angle = Vector3.Angle(transform.up, Vector3.up);
        if (angle != 0.5f) // pin was tumbled
        {
            _gameState.Score++;
            _gameState.StrikeCounter++;
        }
    }
}
