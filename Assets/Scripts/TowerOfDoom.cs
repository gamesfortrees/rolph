using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerOfDoom : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    public float forceToAdd = 10;
    public float maxVelocity = 2;
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // var degreesPerSecond = 360.0f;
        // transform.Rotate(new Vector3(0, 0, 1) * degreesPerSecond * Time.deltaTime, Space.Self);
        // rigidBody.isKinematic = true;
    }   

    void FixedUpdate() {
        if (rigidBody.velocity.magnitude < maxVelocity && rigidBody.velocity.x != 0) {
            if (rigidBody.velocity.x > 0) {
                rigidBody.AddForce(new Vector2(forceToAdd, 0));
            } else {
                rigidBody.AddForce(new Vector2(-forceToAdd, 0));
            }
        }
    }
}
