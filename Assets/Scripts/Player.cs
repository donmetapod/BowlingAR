using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameState _gameState;
    [SerializeField] private Transform _arCamera;
    [SerializeField] private GameObject _ballPrefab;
    [SerializeField] private Stack<GameObject> _availableBalls = new Stack<GameObject>();
    private Vector2 _touchInitialPosition;
    private Vector2 _touchFinalPosition;
    private float _ySwipeDelta;
    private float _xSwipeDelta;
    
    private void Awake()
    {
        _gameState.OnEnterBallSetup.AddListener(EnableSelf);
    }
    
    private void OnEnable()
    {
        BallInitialSetup();
    }
    
    private void OnDisable()
    {
        _gameState.OnReadyToThrow.RemoveListener(EnableSelf);
    }
    
    void EnableSelf()
    {
        enabled = true;
    }
    
    // Creates the balls to be used for the player and stores them in a Stack
    void BallInitialSetup()
    {
        for (int i = 0; i < _gameState.RemainingBalls; i++)
        {
            _availableBalls.Push(Instantiate(_ballPrefab, new Vector3(0, -1000, 0), Quaternion.identity));// Creating balls far from view
        }
        // Once balls are ready switch to ReadyToThrow
        _gameState.CurrentGameState = GameState.GameStateEnum.ReadyToThrow;
    }
    
    void Update()
    {
        if (_gameState.CurrentGameState != GameState.GameStateEnum.ReadyToThrow)
            return;
        
        DetectScreenSwipe();

        #if UNITY_EDITOR
        if (Input.GetMouseButtonDown(1))
        {
            ThrowBall();
        }
        #endif
    }

    // Detects screen swipe and calls ThrowBall
    void DetectScreenSwipe()
    {
        foreach (var touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                _touchInitialPosition = touch.position;
            }
            if (touch.phase == TouchPhase.Ended)
            {
                _touchFinalPosition = touch.position;
                if (_touchFinalPosition.y > _touchInitialPosition.y)
                {
                    _ySwipeDelta = _touchFinalPosition.y - _touchInitialPosition.y;
                }
                _xSwipeDelta = _touchFinalPosition.x - _touchInitialPosition.x;
                // Messenger.Instance.EnqueueMessage($"x swipe delta {_xSwipeDelta}", 3);
                
                ThrowBall();
            }
        }
    }
    
    // Gets a ball from the top of the ball stack and sets position, rotation and adds force to it
    void ThrowBall()
    {
        GameObject currentBall = _availableBalls.Pop();
        if (currentBall.TryGetComponent(out TurnEnd turnEnd))
        {
            turnEnd.enabled = true;
        }
        if (currentBall.TryGetComponent(out SphereCollider sphereCollider))
        {
            sphereCollider.isTrigger = false;
        }
        if (currentBall.TryGetComponent(out Rigidbody rigidbody))
        {
            rigidbody.useGravity = true;
        }
        if (currentBall.TryGetComponent(out DestroyOnTurnEnd destroyOnTurnEnd))
        {
            destroyOnTurnEnd.enabled = true;
        }
        currentBall.transform.position = _arCamera.position;
        currentBall.transform.rotation = _arCamera.rotation;
        Vector3 forceVector = currentBall.transform.forward * (_ySwipeDelta * _gameState._throwPowerMultiplier);
        rigidbody.AddForce(forceVector, ForceMode.Impulse);

        _gameState.RemainingBalls--;
        // Go to throw
        _gameState.CurrentGameState = GameState.GameStateEnum.BallInPlay;
    }
}
