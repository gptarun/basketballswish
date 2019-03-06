using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    private bool jumpA;
    private bool jumpB;
    [HideInInspector] public bool isGrounded;
    public SinglePlayerController singlePlayerController;
    private float screenWidth;
    private GameObject ballGameObject;
    private BallController ballScript;
    AudioSource jumpPlayerAudio;
    [HideInInspector] public bool routineCall;
    public HandMovement handMovement;
    public GameObject tapAnim;
    private float botTimer;
    private float jumpHeight;
    public Animator playerAnim;     //If used animator
    //public Animation playerAnim;  //If uses legacy animation
    // Use this for initialization
    void Start () {
        //this.playerAnim["Player_Idle"].time = Random.Range(0,0.2f);
        screenWidth = Screen.width;
        jumpHeight = 8.0f;
        ballGameObject = GameObject.Find("basketball");
        ballScript = ballGameObject.GetComponent<BallController>();
        jumpPlayerAudio = GetComponent<AudioSource>();
        isGrounded = true;
        if (OptionMenuScript.difficultyLevel.Equals("EASY"))
        {
            botTimer = 1;
        }
        else
        {
            botTimer = 0.75f;
        }
        routineCall = false;
    }

    void Update()
    {
        if (!singlePlayerController.matchEnded){
            //For Bot movement
            if (singlePlayerController.teamAMode.Equals("bot"))
            {
                if (this.transform.tag.Equals("TeamA") && isGrounded && !routineCall)
                {
                    StartCoroutine(BotJump(botTimer));
                }
            }
            if (singlePlayerController.teamBMode.Equals("bot"))
            {
                if (this.transform.tag.Equals("TeamB") && isGrounded && !routineCall)
                {
                    StartCoroutine(BotJump(botTimer));
                }
            }

            for (int i = 0; i < Input.touchCount; i++)
            {
                if (tapAnim != null)
                {
                    tapAnim.SetActive(false);
                }
                Touch touchA = Input.GetTouch(i);
                if (singlePlayerController.teamAMode.Equals("human") && singlePlayerController.teamBMode.Equals("human"))
                {
                    if (touchA.position.x < screenWidth / 2)
                    {
                        if (touchA.phase.Equals(TouchPhase.Began) && this.transform.tag.Equals("TeamA") && isGrounded)
                        {
                            this.gameObject.GetComponent<Rigidbody2D>().velocity = CalculateJumpDistance(ballGameObject, jumpHeight);
                            isGrounded = false;
                        }
                    }
                    else if (touchA.position.x > screenWidth / 2)
                    {
                        if (touchA.phase.Equals(TouchPhase.Began) && this.transform.tag.Equals("TeamB") && isGrounded)
                        {
                            this.gameObject.GetComponent<Rigidbody2D>().velocity = CalculateJumpDistance(ballGameObject, jumpHeight);
                            isGrounded = false;
                        }
                    }
                }
                else if (singlePlayerController.teamAMode.Equals("human") && singlePlayerController.teamBMode.Equals("bot"))
                {
                    if (touchA.phase.Equals(TouchPhase.Began) && this.transform.tag.Equals("TeamA") && isGrounded)
                    {
                        this.gameObject.GetComponent<Rigidbody2D>().velocity = CalculateJumpDistance(ballGameObject, jumpHeight);
                        isGrounded = false;
                    }
                }
                else if (singlePlayerController.teamAMode.Equals("bot") && singlePlayerController.teamBMode.Equals("human"))
                {
                    if (touchA.phase.Equals(TouchPhase.Began) && this.transform.tag.Equals("TeamB") && isGrounded)
                    {
                        this.gameObject.GetComponent<Rigidbody2D>().velocity = CalculateJumpDistance(ballGameObject, jumpHeight);
                        isGrounded = false;
                    }
                }
            }
        }
    }   

    public Vector3 CalculateJumpDistance(GameObject anyObject, float height)
    {
        if (playerAnim != null)
            playerAnim.SetTrigger("isJump");
        if (jumpPlayerAudio != null)
            jumpPlayerAudio.Play();
        Vector3 jumpDis = new Vector3(0, 0, 0);
        float maxHspeed = 4;
        float g = Physics.gravity.magnitude; // get the gravity value
        float vSpeed = height; // calculate the vertical speed
        float totalTime = 6 * vSpeed / g; // calculate the total time
        float maxDistance = Vector3.Distance(anyObject.transform.position, this.transform.position);
        float hSpeed = maxDistance * 1.50f / totalTime * 2; // calculate the horizontal speed
        if ((anyObject.transform.position.x - this.transform.position.x) < 0)
        {
            if (hSpeed < -maxHspeed)
            {
                hSpeed = -maxHspeed;
            }
            else
            {
                hSpeed = -hSpeed;
            }
        }
        else
        {
            if (hSpeed > maxHspeed)
            {
                hSpeed = maxHspeed;
            }
        }
        /*  
         *  This script will allow only all the players will jump
         *  But anyone of same team has the ball will jump
         *  opponent will jump
         */ 
        if (ballScript.attached)
        {
            if (this.gameObject.transform.parent.name.Equals(ballScript.attachTagName))     
            {
                if (this.gameObject.name.Equals(ballScript.attachParentName))
                {
                    jumpDis = new Vector3(hSpeed, vSpeed, 0);
                }
                else
                {
                    jumpDis = new Vector3(0, 0, 0);
                }
            }
            else
            {
                jumpDis = new Vector3(hSpeed, vSpeed, 0);
            }
        }
        else
        {
            jumpDis = new Vector3(hSpeed, vSpeed, 0);
        }
        return jumpDis;
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.tag.Contains("ground"))
        {
            isGrounded = true;
            if (ballScript.attached && ballScript.attachParentName.Equals(this.gameObject.name))
            {
                ballScript.isThrow = true;
            }
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag.Contains("ground"))
        {
            isGrounded = false;
        }
    }
    
    IEnumerator BotJump(float time)
    {
        routineCall = true;
        yield return new WaitForSeconds(time);
        if (ballScript.attached && singlePlayerController.teamAMode.Equals("bot") && ballScript.attachTagName.Equals("TeamA") && this.transform.tag.Equals("TeamA") && !gameObject.name.Equals(ballScript.attachParentName))
        {
            handMovement.antiRotateA = false;
            handMovement.rotateA = false;
        }
        else if (ballScript.attached && singlePlayerController.teamBMode.Equals("bot") && this.transform.tag.Equals("TeamB") && ballScript.attachTagName.Equals("TeamB") && !gameObject.name.Equals(ballScript.attachParentName))
        {
            handMovement.antiRotateB = false;
            handMovement.rotateB = false;
        }
        else
        {
            handMovement.antiRotateA = false;
            handMovement.rotateA = true;
            handMovement.antiRotateB = false;
            handMovement.rotateB = true;
        }
        if (this.isGrounded)
        {
            this.gameObject.GetComponent<Rigidbody2D>().velocity = CalculateJumpDistance(ballGameObject, jumpHeight);
        }
        routineCall = false;
    }
}
