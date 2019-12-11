using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public GameObject player;

    private Vector3 offset;
    private int cameraOffset = 0;
    private float baseYOffset = 0;
    private Camera cameraComponent;

    void Start()
    {
        cameraComponent = gameObject.GetComponent<Camera>();
        offset = transform.position - player.transform.position;
        baseYOffset = offset.y;
    }

    void LateUpdate()
    {
        transform.position = player.transform.position + offset;
    }

    void PhysicsCamera() { 
        var pos = player.transform.position + offset;

        var posWithBaseYOffset = player.transform.position + new Vector3(0, baseYOffset, 0);
        var delta = posWithBaseYOffset - transform.position;
        var direction = delta.normalized;
        var distance = delta.magnitude;

        var rigidBody = gameObject.GetComponent<Rigidbody2D>();
        var scalar = 50.0f;
        var force = direction * scalar;
        rigidBody.AddForce(force);
        
        if (pos.y > 8)
        {
            //cameraOffset++;
        } else
        {
            pos.y = 0.5566f;
            //cameraOffset--;
        }

        //pos.y = cameraOffset;
        
        //transform.position = pos;// player.transform.position + offset;
    }
}
