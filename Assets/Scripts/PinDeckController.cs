
using System.Collections;
using UnityEngine;

public class PinDeckController : MonoBehaviour
{
    [SerializeField] private Transform _pinDeckPlacer;
    [SerializeField] private Transform _defaultPlaneIndcator;
    [SerializeField] private GameObject _pinDeckPrefab;
    [SerializeField] private Transform _arCamera;
    [SerializeField] private GameObject _pinDeckBasePrefab;
    [SerializeField] private GameState _gameState;
    private Vector3 _pinDeckSpawnPosition;
    private Quaternion _pinDeckRotation;

    #if UNITY_EDITOR
    private bool _pinDeckCreated;
    #endif
    
    private void OnEnable()
    {
        _gameState.OnResettingDeck.AddListener(PlaceNewDeckOnLane);
    }

    private void OnDisable()
    {
        _gameState.OnResettingDeck.RemoveListener(PlaceNewDeckOnLane);
    }
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1);
        // Finds the object vuforia uses to indicate a plane surface
        _defaultPlaneIndcator = GameObject.Find("DefaultPlaneIndicator(Clone)").transform;
    }

    private void Update()
    {
        if (_defaultPlaneIndcator != null)
        {
            _pinDeckPlacer.position = _defaultPlaneIndcator.position;
        }
        
        #if UNITY_EDITOR
        if (!_pinDeckCreated)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _pinDeckCreated = true;
                CreatePinDeck();
            }
        }
        #endif
    }
    
    // Called from content positioning behaviour (a Vuforia component inside Plane Finder)
    public void CreatePinDeck()
    {
        StartCoroutine(SetupBowlingLaneRoutine());
    }

    IEnumerator SetupBowlingLaneRoutine()
    {
        // Gets camera direction for rotation
        Vector3 directionTowardsCamera = _pinDeckPlacer.position - _arCamera.position;
        directionTowardsCamera.y = 0;
        
        // Create a new pin deck base
        GameObject pinDeckBaseClone = Instantiate(_pinDeckBasePrefab, _pinDeckPlacer.position, Quaternion.LookRotation(directionTowardsCamera, Vector3.up));
        float zDistance = Mathf.Abs((pinDeckBaseClone.transform.position - _arCamera.position).z);
        
        // Increases the size of pin deck base until reaches camera position
        while (pinDeckBaseClone.transform.localScale.z > -zDistance)
        {
            pinDeckBaseClone.transform.localScale = new Vector3(pinDeckBaseClone.transform.localScale.x, 
                pinDeckBaseClone.transform.localScale.y, 
                pinDeckBaseClone.transform.localScale.z - Time.deltaTime);
            yield return new WaitForSeconds(.01f);
        }
        
        // BoxCollider baseCollider = pinDeckBaseClone.transform.Find("PinDeckBase").GetComponent<BoxCollider>();
        //_throwingLine.position = baseCollider.center - baseCollider.bounds.extents;
        
        // Creates position and rotation for new pin deck 
        _pinDeckSpawnPosition = _pinDeckPlacer.position - _pinDeckPlacer.forward;
        _pinDeckRotation = Quaternion.LookRotation(directionTowardsCamera, Vector3.up);
        
        //Creates a new pin deck
        Instantiate(_pinDeckPrefab, _pinDeckSpawnPosition, _pinDeckRotation);

        // Go to next state after first pin deck is created
        _gameState.CurrentGameState = GameState.GameStateEnum.SetupBalls;
        
    }

    // Creates a new deck for each throw
    void PlaceNewDeckOnLane()
    {
        // Vector3 directionTowardsCamera = _pinDeckPlacer.position - _arCamera.position;
        Instantiate(_pinDeckPrefab, _pinDeckSpawnPosition, _pinDeckRotation);
        _gameState.CurrentGameState = GameState.GameStateEnum.ReadyToThrow;
    }
}
