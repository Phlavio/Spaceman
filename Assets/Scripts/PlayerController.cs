using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed = 4f;
    public float jumpspeed = 5f;
    private float direction = 0f;
    private Rigidbody2D player;

    public Transform groundcheck;
    public float groundcheckRadius;
    public LayerMask groundlayer;
    private bool isTouchingGround;

    private Animator playerAnimation;

    private Vector3 respawnPoint;
    public GameObject spikes;
    public GameObject camera;
    public GameObject falldetector;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Rigidbody2D>();
        playerAnimation = GetComponent<Animator>();
        respawnPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        isTouchingGround = Physics2D.OverlapCircle(groundcheck.position, groundcheckRadius, groundlayer);
        direction = Input.GetAxis("Horizontal");
        Debug.Log(direction);
        //player going right
        if (direction > 0f)
        {
            player.velocity = new Vector2(direction * speed, player.velocity.y);
            transform.localScale = new Vector2(0.2204f, 0.22913f);
        }
        //player going left
        else if (direction < 0f)
        {
            player.velocity = new Vector2(direction * speed, player.velocity.y);
            transform.localScale = new Vector2(-0.2204f, 0.22913f);
        }
        // player idle
        else
        {
            player.velocity = new Vector2(0, player.velocity.y);
        }
        //jumping
        if (Input.GetButtonDown("Jump") && isTouchingGround)
        {
            player.velocity = new Vector2(player.velocity.x, jumpspeed);
        }

        playerAnimation.SetFloat("Speed", Mathf.Abs(player.velocity.x));
        playerAnimation.SetBool("OnGround", isTouchingGround);

        camera.transform.position = new Vector3(transform.position.x, camera.transform.position.y, camera.transform.position.z);
        falldetector.transform.position = new Vector3(transform.position.x, falldetector.transform.position.y, falldetector.transform.position.z);
    }

    //respawn if falls or hit spikes
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Spikes")
        {
            transform.position = respawnPoint;
        }
        else if (collision.tag == "NextLevel") {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else if (collision.tag == "PrevLevel")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
    }
}
