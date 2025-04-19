using UnityEngine;

public class FollowBallRain : MonoBehaviour
{
    public Transform target; 
    private Vector3 positionOffset;

    void Start()
    {
        
        positionOffset = transform.position - target.position;
    }

    void LateUpdate()
    {
        
        transform.position = target.position + positionOffset;
        transform.rotation = Quaternion.identity; 
    }
}