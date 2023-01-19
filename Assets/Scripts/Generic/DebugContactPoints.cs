using UnityEngine;

public class DebugContactPoints : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"{transform.name} collided with {collision.transform.name}");
    }
}
