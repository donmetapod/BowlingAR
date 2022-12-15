using UnityEngine;
using UnityEngine.Events;

public class OnEnableEvent : MonoBehaviour
{

    [SerializeField] private UnityEvent OnComponentEnabled;

    private void OnEnable()
    {
        OnComponentEnabled?.Invoke();
    }
}
