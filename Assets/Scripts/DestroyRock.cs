using UnityEngine;

public class DestroyRock : MonoBehaviour
{
    private Rigidbody rb;
    private float velocityThreshold = 0f; // Almost Stopped

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (rb.linearVelocity.magnitude <= velocityThreshold && rb.angularVelocity.magnitude <= velocityThreshold)
        {
            Destroy(gameObject);
        }
    }
}
