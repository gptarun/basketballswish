using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour {

    public GameObject basketA;
    public GameObject basketB;
    public Transform ball;
    private GameObject ballGameObject;
    private SpriteRenderer basketballSprite;
    [HideInInspector] public bool attached;
    [HideInInspector] public string attachName;
    [HideInInspector] public string attachParentName;
    private SinglePlayerController singlePlayerController;
    [SerializeField] AudioClip[] ballClip;
    AudioSource playAudio;
    private float screenWidth;
    private bool isThrowBotA = false;
    private bool isThrowBotB = false;
    [HideInInspector] public bool isThrow = false;
    [HideInInspector] public string attachTagName;
    private GameObject gameSceneObject;
    private bool attachedIllusion;
    public HandMovement handMovement;
	[SerializeField] GameObject blurBall;
    private bool waitToThrowA;
    private bool waitToThrowB;
    private bool upperBound;
    public AudioSource scoreAudio;
    public AudioSource swishAudio;
    public GameObject scoreAnim;
    private int modeCounter = 0;
    private float ballHeight = 5;
    private bool routineCallA;
    private bool routineCallB;
    private bool toThrow=false;
    public GameObject shotClockAnim;
    private float botTimer;

    void Start()
    {
        ballGameObject = GameObject.Find("basketball");
        basketballSprite = ballGameObject.GetComponent<SpriteRenderer>();
        gameSceneObject = GameObject.Find("GameSceneObject");
        basketA = GameObject.Find("Basket_Team1");
        basketB = GameObject.Find("Basket_Team2");
        attached = false;
        isThrow = false;
        attachedIllusion = false;
        playAudio = GetComponent<AudioSource>();
        screenWidth = Screen.width;
        singlePlayerController = gameSceneObject.GetComponent<SinglePlayerController>();
        waitToThrowA = false;
        waitToThrowB = false;
        routineCallA = false;
        routineCallA = false;
        if (singlePlayerController.teamAMode.Equals("human"))
        {
            modeCounter++;
        }
        if (singlePlayerController.teamBMode.Equals("human"))
        {
            modeCounter++;
        }
        if (OptionMenuScript.difficultyLevel.Equals("EASY"))
        {
            botTimer = 1.1f;
        }
        else
        {
            botTimer = 0.85f;
        }
    }

    void Update()
    {
        //For Bot movement
        if (singlePlayerController.teamAMode.Equals("bot"))
        {
            if (isThrowBotA && attachTagName.Equals("TeamA"))
            {
                if (!waitToThrowA && !routineCallA)
                {
                    StartCoroutine(BotThrowBallA(botTimer));
                }
                else if (handMovement != null)
                {
                    if (handMovement.botAntiRotateA && waitToThrowA)
                    {
                        StopCoroutine("BallHoldFoul");
                        LaunchBall(basketB, ballHeight);
                        waitToThrowA = false;
                        routineCallA = false;
                    }
                }
            }
            if (attached)
            {
                isThrowBotA = true;
            }
        }

        if (singlePlayerController.teamBMode.Equals("bot"))
        {
            if (isThrowBotB && attachTagName.Equals("TeamB"))
            {
                if (!waitToThrowB && !routineCallB)
                {
                    StartCoroutine(BotThrowBallB(botTimer));
                }
                else if (handMovement != null)
                {
                    if (handMovement.botAntiRotateB && waitToThrowB)
                    {
                        LaunchBall(basketA, ballHeight);
                        StopCoroutine("BallHoldFoul");
                        waitToThrowB = false;
                        routineCallB = false;
                    }
                }
            }
            if (attached)
            {
                isThrowBotB = true;
            }
        }
        
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touchA = Input.GetTouch(i);
            
            if (isThrow && touchA.phase.Equals(TouchPhase.Ended))
            {
                if (attachTagName.Equals("TeamA"))
                {
                    if (modeCounter == 2)           // if double player then Throw should be devided 2
                    {
                        if (touchA.position.x < screenWidth / 2)
                            LaunchBall(basketB, ballHeight);
                            StopCoroutine("BallHoldFoul");
                    }
                    else
                    {
                        LaunchBall(basketB, ballHeight);
                        StopCoroutine("BallHoldFoul");
                    }
                }
                else if (attachTagName.Equals("TeamB"))
                {
                    if (modeCounter == 2)             // if double player then Throw should be devided 2
                    {
                        if (touchA.position.x > screenWidth / 2)
                            LaunchBall(basketA, ballHeight);
                            StopCoroutine("BallHoldFoul");
                    }
                    else
                    {
                        LaunchBall(basketA, ballHeight);
                        StopCoroutine("BallHoldFoul");
                    }
                }
            }
            if (attached && touchA.phase.Equals(TouchPhase.Ended))
            {
                toThrow = true;
            }
            if (toThrow && touchA.phase.Equals(TouchPhase.Began))
            {
                isThrow = true;
                toThrow = false;
            }
        }
        if (!SinglePlayerController.pauseButtonPressed)
        {
            Instantiate(blurBall, transform.position, transform.rotation);//creating motion blur
        }
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag.Equals("handpivot") && !attachedIllusion) //to catch the ball
        {
            attachBall(col);
        }
        if (col.tag.Equals("handpivot") && attachedIllusion && !col.transform.parent.transform.parent.tag.Equals(attachTagName)) //to stole the ball
        {
            StopCoroutine("BallHoldFoul");
            attachBall(col);
        }
        if (col.tag.Contains("outofbound"))
        {
            StartCoroutine(MakeUserReadyFoul());
        }
        if (col.tag.Equals("upperbound"))
        {
            upperBound = true;
        }
        if (col.tag.Equals("lowerbound") && upperBound && !attached)
        {
            if (col.transform.parent.tag.Equals("BasketA"))
            {
                if (scoreAudio != null)
                    scoreAudio.Play();
                if(swishAudio != null)                
                    swishAudio.Play();                
                StartCoroutine("MakeUserScore");
                singlePlayerController.scoreB = singlePlayerController.scoreB + 3;
            }
            if (col.transform.parent.tag.Equals("BasketB"))
            {
                if (scoreAudio != null)
                    scoreAudio.Play();
                if (swishAudio != null)
                    swishAudio.Play();
                StartCoroutine("MakeUserScore");
                singlePlayerController.scoreA = singlePlayerController.scoreA + 3;
            }
            upperBound = false;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Equals("upperbound"))
        {
            upperBound = false;
        }                           
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.name.Contains("Basket"))
        {
            AudioClip clip = ballClip[0];
            playAudio.PlayOneShot(clip);
        }
        if (collision.transform.tag.Equals("ground"))
        {
            AudioClip clip = ballClip[1];
            playAudio.PlayOneShot(clip);
        }
    }

    private void LaunchBall(GameObject basket, float height)
    {
        //ballGameObject.transform.parent = gameSceneObject.transform;
        basketballSprite.sortingOrder = 0;
        ballGameObject.GetComponent<Rigidbody2D>().bodyType = (RigidbodyType2D)0;
        ballGameObject.GetComponent<Rigidbody2D>().velocity = CalculateJumpDistance(basket, height);// launch the projectile!
        ballGameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
        StartCoroutine("MakeBallReady");
        attached = false;
        isThrowBotA = false;
        isThrowBotB = false;
        isThrow = false;
    }

    public Vector3 CalculateJumpDistance(GameObject anyObject, float height)
    {
        Vector3 jumpDis;
        float g = Physics.gravity.magnitude; // get the gravity value
        float vSpeed = Mathf.Sqrt(2 * g * height); // calculate the vertical speed
        float totalTime = 2 * vSpeed / g; // calculate the total time
        float maxDistance = Vector3.Distance(anyObject.transform.position, this.transform.position);
        float hSpeed = maxDistance * 2.3f / totalTime; // calculate the horizontal speed
        if ((anyObject.transform.position.x - this.transform.position.x) < 0)
        {
            hSpeed = -hSpeed;
            hSpeed -= Random.Range(0, 1f);
        }
        else
        {
            hSpeed += Random.Range(0, 1f);
        }
        jumpDis = new Vector3(hSpeed, vSpeed, 0);
        return jumpDis;
    }

    public void attachBall(Collider2D col)
    {
        //ballGameObject.transform.parent = col.transform;
        basketballSprite.sortingOrder = 4;
        ballGameObject.transform.position = col.transform.position;
		ballGameObject.transform.rotation = Quaternion.Euler(0, 0, 0);				 
        ballGameObject.GetComponent<Rigidbody2D>().bodyType = (RigidbodyType2D)2;
        ballGameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        attachName = col.transform.parent.name;     //contains hand name i.e PAHand or PBHand
        attachParentName = col.transform.parent.transform.parent.name; //Contains super-1 parent name i.e Pbody1,2,3
        attachTagName = col.transform.parent.transform.parent.tag; //Contains super parent tag i.e TeamA( of PBody tag name) or TeamB
        ballGameObject.GetComponent<CircleCollider2D>().isTrigger = true;
        attached = true;
        attachedIllusion = true;
        upperBound = false;
        handMovement = GameObject.Find(attachName).GetComponent<HandMovement>();
        StartCoroutine("BallHoldFoul");
    }

    IEnumerator MakeBallReady()
    {
        yield return new WaitForSeconds(0.5f);
        if (!attached)
        {
            ballGameObject.GetComponent<CircleCollider2D>().isTrigger = false;
            attachedIllusion = false;
            attachName = "";
            attachTagName = "";
            attachParentName = "";
        }
    }

    IEnumerator MakeUserReadyFoul()
    {
        //ballGameObject.transform.parent = gameSceneObject.transform;
        if (attached)
        {
            ballGameObject.GetComponent<Rigidbody2D>().bodyType = (RigidbodyType2D)0;
            ballGameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
            StartCoroutine("MakeBallReady");
        }
        ballGameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(0,0,0);
        singlePlayerController.ResetPositions();
        singlePlayerController.FoulAnim.SetActive(true);
        yield return new WaitForSeconds(1);
        singlePlayerController.FoulAnim.SetActive(false);
    }

    IEnumerator BotThrowBallA(float time)
    {
        routineCallA = true;
        yield return new WaitForSeconds(time);
        waitToThrowA = true;
    }

    IEnumerator BotThrowBallB(float time)
    {
        routineCallB = true;
        yield return new WaitForSeconds(time);
        waitToThrowB = true;
    }

    IEnumerator MakeUserScore()
    {        
        singlePlayerController.ResetPositions();
        scoreAnim.SetActive(true);
        yield return new WaitForSeconds(1);
        scoreAnim.SetActive(false);
    }

    IEnumerator BallHoldFoul()
    {
        yield return new WaitForSeconds(6);
        StartCoroutine("ShotClockVoilation");
    }

    IEnumerator ShotClockVoilation()
    {                
        attached = false;
        singlePlayerController.ResetPositions();
        ballGameObject.GetComponent<Rigidbody2D>().bodyType = (RigidbodyType2D)0;
        ballGameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
        StartCoroutine(MakeBallReady());        
        shotClockAnim.SetActive(true);
        yield return new WaitForSeconds(1);
        shotClockAnim.SetActive(false);        
    }
}