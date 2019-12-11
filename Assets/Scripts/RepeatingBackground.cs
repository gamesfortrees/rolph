﻿using UnityEngine;
using System.Collections;

public class RepeatingBackground : MonoBehaviour
{
    private GameObject player;
    private SpriteRenderer spriteRenderer;
    private float backgroundHorizontalLength;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        backgroundHorizontalLength = spriteRenderer.bounds.size.x;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        float distance = transform.position.x - player.transform.position.x;
        if (distance < -backgroundHorizontalLength)
        {
            RepositionBackground(true);
        }
        if (distance > backgroundHorizontalLength)
        {
            RepositionBackground(false);
        }
    }

    private void RepositionBackground(bool forward)
    {
        Vector3 groundOffSet = new Vector3(backgroundHorizontalLength * 2f, 0, 0);
        groundOffSet.x -= 0.1f;
        if (!forward) groundOffSet *= -1;
        transform.position = transform.position + groundOffSet;
    }
}