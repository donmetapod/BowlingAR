using UnityEngine;
using UnityEngine.Events;

public class DeathBox : MonoBehaviour
{
    public UnityEvent OnObjectEnteredDeathBox;
    private void OnTriggerEnter(Collider other)
    {
        OnObjectEnteredDeathBox?.Invoke();
    }
}
