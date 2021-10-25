using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerScript : MonoBehaviour
{
    public float jumpForce;
    private Rigidbody2D rd2d;  
    public float speed;  
    public Text score;  
    private int scoreValue = 0;
    private int livesValue = 3;
    private bool isOnGround;
    public Transform groundcheck;
    public float checkRadius;
    public LayerMask allGround;
    public Text lives;
    public GameObject loseTextObject;
    public GameObject winTextObject;
    public AudioClip musicClipOne;

    public AudioClip musicClipTwo;

    public AudioSource musicSource;

    Animator anim;

    private bool facingRight = true;

    public Text hozText;
    public Text jumpText;
    


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        
        rd2d = GetComponent<Rigidbody2D>();
        score.text = "Score: " + scoreValue.ToString();
        lives.text = "Lives: " + livesValue.ToString();
        winTextObject.SetActive(false);
        loseTextObject.SetActive(false);
        SetCountText();
        musicSource.clip = musicClipOne;
        musicSource.Play();
        musicSource.loop = true;
       
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A))
        {
            anim.SetInteger("State", 1);
        }
        if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A))
            {
            anim.SetInteger("State", 0);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            anim.SetInteger("State", 2);
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            anim.SetInteger("State", 0);
        }
       


     
    }
    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }



    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");
        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));
        isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround);
        if (facingRight == false && hozMovement > 0)
        {
            Flip();
        }
        else if (facingRight == true && hozMovement < 0)
        {
            Flip();
        }
    }


    void SetCountText()
    {
        score.text = "Score: " + scoreValue.ToString();
        if (scoreValue == 8)
        {
            winTextObject.SetActive(true);
           
                
            
        }
        if (livesValue == 0)
        {
            loseTextObject.SetActive(true);
            Destroy(gameObject);
        }
    }
    
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            SetCountText();
            score.text = "Score: " + scoreValue.ToString();
            Destroy(collision.collider.gameObject);
            if(scoreValue == 4)
            {
                transform.position = new Vector2(59.0f, 0.0f);
                livesValue = 3;
                lives.text = "Lives " + livesValue.ToString();
            }
            if(scoreValue == 8)
            {
                musicSource.Pause();
                musicSource.clip = musicClipTwo;
                musicSource.Play();
            }
            
            
            
        }

        else if(collision.collider.tag == "Enemy" && scoreValue <= 7)
        {
            livesValue -= 1;
            lives.text = "Lives: " + livesValue.ToString();
            Destroy(collision.collider.gameObject);
            SetCountText();
        }
        
       

       

    }
    
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.collider.tag == "Ground" && isOnGround)
        {
            if (Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            }
           


        }
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }



}
