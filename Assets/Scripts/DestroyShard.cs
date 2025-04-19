using UnityEngine;

public class DestroyShard : MonoBehaviour
{
    private Rigidbody rb;
    private float velocityThreshold = 0f; // Almost Stopped

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player")){
            ShieldLogic playerShields = collision.gameObject.GetComponent<ShieldLogic>();
            if(!playerShields.IsFullShields) Destroy(gameObject);
        }
    }
}
