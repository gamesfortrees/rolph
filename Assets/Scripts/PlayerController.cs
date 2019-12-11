using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
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
    public Sprite oneBiteSprite;
    public Sprite twoBiteSprite;
    public Sprite threeBiteSprite;
    public GameObject enemy;
    public Sprite defaultFace;
    public Sprite jumpingFace;
    public Sprite plantedSeedFace;
    public Sprite enemyCloseFace;

    public AudioClip jumpClip;
    public AudioClip landClip;
    public AudioClip seedCollectedClip;
    public AudioClip seedPlantedClip;
    public Text seedCounter;

    private bool grounded = true;
    private bool facingRight = true;
    private bool jump = false;
    private Animator anim;
    private Rigidbody2D rb2d;
    private LayerMask platformLayer;
    private int bites = 0;
    private AudioSource audioSource;

    private bool isOnFlowerBed = false;
    private FlowerBed flowerBed = null;
    private DialogController dialogController;
    private GameController gameController;
    private Sprite currentFace;
    private SpriteRenderer firstChildSpriteRenderer;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        platformLayer = LayerMask.NameToLayer("Platform");
        audioSource = GetComponent<AudioSource>();
        dialogController = GameObject.FindWithTag("Dialog").GetComponent<DialogController>();
        gameController = GameObject.FindWithTag("GameMaster").GetComponent<GameController>();

        var firstChild = gameObject.transform.GetChild(0).gameObject;
        firstChildSpriteRenderer = firstChild.GetComponent<SpriteRenderer>();
        currentFace = defaultFace;
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
            seedCounter.text = "" + seeds;
            PlayAudioClip(seedPlantedClip);
            flowerBed.SproutSeedling();
        }
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

        if (rb2d.velocity.y <= 0 && wasInTheAir && grounded)
        {
            SwitchExpression(defaultFace);
        }

        float horizontalAxis = Input.GetAxis("Horizontal");
        //anim.SetFloat("Speed", Mathf.Abs(horizontalAxis));

        if (horizontalAxis > 0 && !facingRight)
            Flip();
        else if (horizontalAxis < 0 && facingRight)
            Flip();

        if (horizontalAxis * rb2d.velocity.x < maxSpeed)
            rb2d.AddForce(Vector2.right * horizontalAxis * moveForce);

        if (Mathf.Abs(rb2d.velocity.x) > maxSpeed)
            rb2d.velocity = new Vector2(Mathf.Sign(rb2d.velocity.x) * maxSpeed, rb2d.velocity.y);

        if (IsEnemyClose())
        {
            SwitchExpression(enemyCloseFace);
        }
        else
        {
            if (currentFace == enemyCloseFace)
            {
                SwitchExpression(defaultFace);
            }
        }
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
            audioSource.Play();
        }
        if (other.gameObject.CompareTag("Seed"))
        {
            audioSource.clip = seedCollectedClip;
            audioSource.Play();
            other.gameObject.SetActive(false);
            seeds += 1;
            seedCounter.text = "" + seeds;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("FlowerBed") && isOnFlowerBed)
        {
            isOnFlowerBed = false;
            flowerBed = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("FlowerBed") && !isOnFlowerBed)
        {
            isOnFlowerBed = true;
            flowerBed = other.gameObject.GetComponent<FlowerBed>();
        }
        if (
            other.gameObject.CompareTag("Excavator") ||
            other.gameObject.CompareTag("Fire") ||
            other.gameObject.CompareTag("Blade")
            )
        {
            gameController.EndLevel();
        }
        /*if (GameController.gameRunning && other.gameObject.CompareTag("Goal"))
        {
            gameController.EndLevel(gameObject, true);
        }*/
        if (other.gameObject.CompareTag("DialogTrigger2"))
        {
            dialogController.Dialog1_2();
            dialogController.Activate();
        }
        if (other.gameObject.CompareTag("DialogTrigger3"))
        {
            dialogController.Dialog1_3();
            dialogController.Activate();
        }
        if (other.gameObject.CompareTag("DialogTrigger4"))
        {
            dialogController.Dialog1_4();
            dialogController.Activate();
        }
        if (other.gameObject.CompareTag("DialogTrigger5"))
        {
            dialogController.Dialog2_1();
            dialogController.Activate();
        }
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

    private bool IsEnemyClose()
    {
        return (enemy.transform.position - transform.position).magnitude <= 12;
    }

    private void SwitchExpression(Sprite newSprite)
    {
        firstChildSpriteRenderer.sprite = newSprite;
        currentFace = newSprite;
    }
}