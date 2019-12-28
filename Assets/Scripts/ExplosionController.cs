using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour {
    private Animator animator;
    private SpriteRenderer smokeRenderer;
    float smokeLifeTime = 0.7f;
    float currentTime = 0;
    bool hasExploded = false;
    public GameObject rolph;

    void Start() {
        smokeRenderer = GetComponent<SpriteRenderer>();
        smokeRenderer.enabled = false;
        animator = GetComponent<Animator>();
        animator.enabled = false;
    }


    // Update is called once per frame
    void Update() {
        if (hasExploded) {
            currentTime += Time.deltaTime;
            if (currentTime >= smokeLifeTime) {
                smokeRenderer.enabled = false;
            }
        }
    }

    void FixedUpdate() {
        var nextPos = rolph.transform.position;
        nextPos.y += -0.4f;
        transform.position = nextPos;
    }

    public void Explode() {
        rolph.SetActive(false);

        foreach (Transform childTransform in transform) {
            var child = childTransform.gameObject;
            child.GetComponent<SpriteRenderer>().enabled = true;
            var rb = child.GetComponent<Rigidbody2D>();
            rb.gravityScale = 1.0f;
            var randX = Random.Range(-100, 100) / 100.0f;
            var randY = Random.Range(-100, 100) / 100.0f;
            var randomForce = Random.Range(10, 20);
            rb.AddForce(new Vector2(randX, randY) * randomForce, ForceMode2D.Impulse);
        }
        animator.enabled = true;
        smokeRenderer.enabled = true;
        hasExploded = true;
    }
}
