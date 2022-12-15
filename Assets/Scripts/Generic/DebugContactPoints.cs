using UnityEngine;

public class DebugContactPoints : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        //Messenger.Instance.EnqueueMessage($"{transform.name} scollided with {collision.transform.name}", 5);
        Debug.Log($"{transform.name} collided with {collision.transform.name}");
    }
}
