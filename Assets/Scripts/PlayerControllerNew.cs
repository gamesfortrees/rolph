using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerControllerNew : MonoBehaviour
{
    public float moveForce = 80f;
    public float minTorque = 50f;
    public float maxTorque = 120f;
    public float maxSpeed = 5f;
    public float jumpForce = 400f;
    public float rotateLift = 5f;
    public float groundedRotateForce = 80f;
    public float airborneRotateForce = 40f;
    public float enemyPushBackFoce = 300f;
    public int seeds = 0;
    public SpriteRenderer bodyRenderer;
    public GameObject enemy;

    public AudioClip jumpClip;
    public AudioClip landClip;
    public AudioClip seedCollectedClip;
    public AudioClip seedPlantedClip;

    private bool grounded = true;
    private bool facingRight = true;
    private bool jump = false;
    private Animator anim;
    private Rigidbody2D rb2d;
    private LayerMask platformLayer;
    private AudioSource audioSource;

    private int GroundLayerInt = 8;

    private bool isOnFlowerBed = false;
    private FlowerBed flowerBed = null;

    private SpriteRenderer firstChildSpriteRenderer;
    public Sprite defaultFace;
    public Sprite jumpingFace;
    public Sprite plantedSeedFace;
    public Sprite enemyCloseFace;
    private Sprite currentFace;

    private ParticleSystem particles;

    void Start()
    {
        currentFace = defaultFace;
        var firstChild = gameObject.transform.GetChild(0).gameObject;
        firstChildSpriteRenderer = firstChild.GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        platformLayer = LayerMask.NameToLayer("Platform");
        audioSource = GetComponent<AudioSource>();
        particles = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (GameController.gameRunning)
        {
            ProcessUserInput();
        }
    }

    private void PlayAudioClip(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
        Debug.Log(clip.name);
    }

    private void ProcessUserInput() { 
        if (Input.GetKeyDown("space") && grounded)
        {
            jump = true;
            PlayAudioClip(jumpClip);
        }
        if (Input.GetKeyDown("s") && seeds > 0 && isOnFlowerBed && flowerBed != null)
        {
            seeds -= 1;
            PlayAudioClip(seedPlantedClip);
            Debug.Log("Seed planted, seeds left: " + seeds);
            flowerBed.SproutSeedling();
            SwitchExpression(plantedSeedFace);
        }
    }

    private bool IsEnemyClose() {
        return (enemy.transform.position - transform.position).magnitude <= 12;
    }

    void FixedUpdate()
    {
        if (!GameController.gameRunning)
        {
            return;
        }

        var wasInTheAir = !grounded;
        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position, Vector2.down, 2.5f, 1 << platformLayer);
        grounded = hit.collider != null;

        if (rb2d.velocity.y <= 0 && wasInTheAir && grounded) {
            SwitchExpression(defaultFace);
        }

        float horizontalAxis = Input.GetAxis("Horizontal");
        //anim.SetFloat("Speed", Mathf.Abs(horizontalAxis));

        if (horizontalAxis > 0 && !facingRight)
            Flip();
        else if (horizontalAxis < 0 && facingRight)
            Flip();

        if (horizontalAxis * rb2d.velocity.x < maxSpeed) {
            rb2d.AddForce(Vector2.right * horizontalAxis * moveForce);
        }

        if (Mathf.Abs(rb2d.velocity.x) > maxSpeed) {
            rb2d.velocity = new Vector2(Mathf.Sign(rb2d.velocity.x) * maxSpeed, rb2d.velocity.y);
        }

        if (IsEnemyClose()) {
            SwitchExpression(enemyCloseFace);
        } else {
            if (currentFace == enemyCloseFace) {
                SwitchExpression(defaultFace);
            }
        }

        // if (rb2d.velocity.magnitude > 0.0f) {
        //     particles.Play();
        // } else {
        //     particles.Stop();
        // }

        anim.SetFloat("Speed", rb2d.velocity.magnitude);

        if (jump)
        {
            //anim.SetTrigger("Jump");
            rb2d.AddForce(new Vector2(0f, jumpForce));
            jump = false;
            grounded = false;
            SwitchExpression(jumpingFace);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == platformLayer)
        {
            audioSource.clip = landClip;
            Debug.Log(audioSource.clip.name);
            audioSource.Play();
        }
        if (other.gameObject.CompareTag("Seed"))
        {
            audioSource.clip = seedCollectedClip;
            audioSource.Play();
            other.gameObject.SetActive(false);
            seeds += 1;
            Debug.Log("Seed collected, seed count: " + seeds);
        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            Vector2 pushBackForce = transform.position.x > other.transform.position.x ? Vector2.right : Vector2.left;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("FlowerBed") && isOnFlowerBed)
        {
            isOnFlowerBed = false;
            Debug.Log("leftFlowerBed");
            flowerBed = null;
            SwitchExpression(defaultFace);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("FlowerBed") && !isOnFlowerBed)
        {
            isOnFlowerBed = true;
            Debug.Log("enteredFlowerBed");
            flowerBed = other.gameObject.GetComponent<FlowerBed>();
        }
        if (other.gameObject.CompareTag("Excavator"))
        {
            SceneManager.LoadScene("Gameover");
        }
        /*if (GameController.gameRunning && other.gameObject.CompareTag("Goal"))
        {
            gameController.EndLevel(gameObject, true);
        }*/
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void Die()
    {
        //gameController.EndLevel(gameObject, false);
        // anim.SetTrigger("Die");
    }

    private void SwitchExpression(Sprite newSprite) {
        firstChildSpriteRenderer.sprite = newSprite;
        currentFace = newSprite;
    }
}