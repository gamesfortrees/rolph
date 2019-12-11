using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallTruck : MonoBehaviour
{
    public float Speed = 0.03f;
    public float vibration = 0.1f;
    private int frameCount = 0;
    public int framesPerVibration = 3;
    public int seedlingPushBack = 5;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("FlowerBed"))
        {
            var flowerBed = other.gameObject.GetComponent<FlowerBed>();
            if (flowerBed.HasSeedling())
            {
                var pos = transform.position;
                pos.x -= seedlingPushBack;
                gameObject.transform.position = pos;
                flowerBed.CutDown();
            }
        }
    }

    private void FixedUpdate()
    {
        if(!GameController.gameRunning)
        {
            return;
        }
        var pos = transform.position;
        vibration *= -1;
        pos.x += Speed;
        if (frameCount > framesPerVibration)
        {
            frameCount = 0;
            pos.y += vibration;
        } else
        {
            frameCount++;
        }
        gameObject.transform.position = pos;
    }
}
