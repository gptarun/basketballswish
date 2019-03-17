using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SinglePlayerController : MonoBehaviour {
    [HideInInspector] public string teamAMode;
    [HideInInspector] public string teamBMode;
    private string teamAFullName;
    private string teamBFullName;
    [SerializeField] Text quater;
    private int totalQuaterCounter;
    [HideInInspector] public int quaterCounter = 1;
    [SerializeField] Text quaterTimer;
    private int qTimer;
    [SerializeField] Text teamAScore;
    public int scoreA;
    [SerializeField] Text teamBScore;
    public int scoreB;
    [SerializeField] Text teamAName;
    [SerializeField] Text teamBName;
    [SerializeField] GameObject canvasObject;
    [SerializeField] TextMeshProUGUI winnerName;
    [SerializeField] Button pauseButton;
    public static bool pauseButtonPressed = false;
    bool gameOver = false;  
    public GameObject jumpAnim;
    public GameObject FoulAnim;
    public GameObject ExtraTimeAnim;
    float delatTime;
    public GameObject[] player = new GameObject[6];
    Vector3[] playerPosition = new Vector3[6];
    public Transform ballPosition;
    Vector3 ballInitPos;
    GameObject basketball;
    public BallController ballController;
    public AudioSource lastTick;
    private bool isTick;
    public AudioSource gameBell;
    public bool matchEnded;
    public GameObject quaterBreak;  //Show pause window after every quater
    /// <summary>
    ///These varaibles will be used for player creation dynamically a
    /// </summary>
    [HideInInspector] public Sprite[] faces;
    [HideInInspector] public Sprite[] jersey;
    private UserDataController userDataController;
    [SerializeField] public TextMeshProUGUI coinsCredit;
    private int coinsWon;
    // Use this for initialization

    void Start () {
        basketball = GameObject.Find("basketball");
        ballController = basketball.GetComponent<BallController>();
        pauseButtonPressed = false;
        totalQuaterCounter = OptionMenuScript.quaterCounter;
        quater.text = "Q" + quaterCounter;
        qTimer = OptionMenuScript.quaterDuration[OptionMenuScript.quaterTimerCounter];
        quaterTimer.text = "" + qTimer;
        scoreA = 0;
        scoreB = 0;
        teamAScore.text = scoreA.ToString();
        teamBScore.text = scoreB.ToString();
        faces = Resources.LoadAll<Sprite>("Face");
        jersey = Resources.LoadAll<Sprite>("Team");        
        int indexA = Array.FindIndex(jersey, cloth => cloth.name == teamAName.text.ToString().ToUpper());
        int indexB = Array.FindIndex(jersey, cloth => cloth.name == teamBName.text.ToString().ToUpper());
        for (int i = 0; i < OptionMenuScript.teamSizeCounter*2; i++)
        {
            playerPosition[i] = player[i].transform.position;
            player[i].SetActive(true);
            if (i % 2 == 0) {
                player[i].GetComponent<SpriteRenderer>().sprite = jersey[indexA];
                int tempFace = UnityEngine.Random.Range(0, 11);
                if( tempFace == 1) { tempFace++; }
                player[i].gameObject.transform.Find("Face").GetComponent<SpriteRenderer>().sprite = faces[tempFace];
            }
            else
            {
                player[i].GetComponent<SpriteRenderer>().sprite = jersey[indexB];
                int tempFace = UnityEngine.Random.Range(0, 11);
                if (tempFace == 1) { tempFace++; }
                player[i].gameObject.transform.Find("Face").GetComponent<SpriteRenderer>().sprite = faces[tempFace];
            }                        
        }
        ballInitPos = ballPosition.position;
        StartCoroutine("LoseTime");
        delatTime = qTimer;
        Time.timeScale = 1;
        isTick = true;
        matchEnded = false;
        StartCoroutine(MakeUserReady());
        userDataController = new UserDataController();
        userDataController.LoadGameData();
        coinsWon = 0;
    }
	
	// Update is called once per frame
	void Update () {
        if (!pauseButtonPressed)
        {
            teamAScore.text = scoreA.ToString();
            teamBScore.text = scoreB.ToString();
            delatTime -= Time.deltaTime;
            if (qTimer > -1)
            {
                GameQuaterCounter(qTimer);

            }
            else if(qTimer < 1 && quaterCounter != totalQuaterCounter)
            {
                pauseButtonPressed = true;          //Showing next quater pause window also
                if(GameObject.Find("BallShadow(Clone)") != null)
                {
                    Debug.Log("Deleted");
                    Destroy(GameObject.Find("BallShadow(Clone)"));
                }                
                qTimer = OptionMenuScript.quaterDuration[OptionMenuScript.quaterTimerCounter];
                quaterCounter++;
                isTick = true;
                quaterBreak.SetActive(true);                
                quater.text = "Q" + quaterCounter;                                
                StartCoroutine(MakeUserReady());
                Time.timeScale = 0f;
            }
            else
            {
                if (!gameOver)
                {
                    gameOver = true;
                    GameWinner();
                }
            }
            if (qTimer < 10 && qTimer >= 0 && lastTick != null && isTick )
            {
                isTick = false;
                lastTick.Play();
            }
            lastTick.UnPause();
        }
        else
        {
            lastTick.Pause();            
        }
    }
    //This is the basic coroutine to reset everytime when you score, foul and timer expires. Also acknowledged the timer to stop
    IEnumerator MakeUserReady()
    {
        ResetPositions();
        jumpAnim.SetActive(true);
        yield return new WaitForSeconds(2);
        jumpAnim.SetActive(false);
    }

    public void ResetPositions()
    {
        StopCoroutine("LoseTime");
        if(basketball.GetComponent<Rigidbody2D>().bodyType == RigidbodyType2D.Static)
        {
            basketball.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            basketball.GetComponent<Rigidbody2D>().gravityScale = 1;
        }
        basketball.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        basketball.GetComponent<Rigidbody2D>().angularVelocity = 0;
        basketball.GetComponent<CircleCollider2D>().isTrigger = false;        
        ballController.attached = false;
        ballController.attachName = "";
        ballController.attachParentName = "";
        ballController.attachTagName = "";
        ballController.attachedIllusion = false;
        for (int i = 0; i < player.Length; i++)
        {
            player[i].transform.position = playerPosition[i];
        }
        ballPosition.transform.position = ballInitPos;
        StartCoroutine("LoseTime");
    }

    public void GameQuaterCounter(int qTimer)
    {
        quaterTimer.text = qTimer.ToString();                        
    }

    public void SinglePlayerTeams()
    {
        Debug.Log("Game Begins");        
    }

    public void SetTeams(string teamANameSelected, string teamBNameSelected, string modeA, string modeB)
    {
        teamAName.text = teamANameSelected.Substring(0, 3);
        teamBName.text = teamBNameSelected.Substring(0, 3);
        teamAMode = modeA;
        teamBMode = modeB;
        teamAFullName = teamANameSelected;
        teamBFullName = teamBNameSelected;
        if(GameObject.Find("MenuBg") != null)
        {
            Destroy(GameObject.Find("MenuBg"));
        }
    }
   
    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenuScene");
    }
   
    public void PauseGame()
    {
        pauseButtonPressed = true;
        Time.timeScale = 0f;
    }
    public void UnPauseGame()
    {
        pauseButtonPressed = false;
        Time.timeScale = 1f;        
    }
    IEnumerator LoseTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            qTimer--;
        }
    }

    public void GameWinner()
    {
        if (scoreA > scoreB)
        {
            Debug.Log("Team A wins");
            winnerName.text = teamAFullName + " Wins!";
            if (teamAMode.Equals("human"))
            {
                coinsWon = (100 + scoreA);
            }
            else if (teamBMode.Equals("human"))
            {
                coinsWon = scoreB;
            }
            AdManager.rewardTwice = coinsWon;
            userDataController.userData.baskyCoins += coinsWon;
            coinsCredit.SetText("Coins : " + coinsWon);
            userDataController.SaveGameData();
            MatchEnded();
        }
        else if (scoreA < scoreB)
        {
            Debug.Log("Team B wins");
            winnerName.text = teamBFullName + " Wins!";
            if (teamBMode.Equals("human"))
            {
                coinsWon = (100 + scoreB);
            }
            else if (teamAMode.Equals("human"))
            {
                coinsWon = scoreA;
            }
            AdManager.rewardTwice = coinsWon;
            userDataController.userData.baskyCoins += coinsWon;
            coinsCredit.SetText("Coins : " + coinsWon);
            userDataController.SaveGameData();
            MatchEnded();
        }
        else
        {
            Debug.Log("Draw");
            gameOver = false;
            StartCoroutine(Draw());
            qTimer = OptionMenuScript.quaterDuration[OptionMenuScript.quaterTimerCounter];
        }
    }
    public void MatchEnded()
    {
        if (gameBell != null)
        {
            gameBell.Play();
        }
        pauseButton.interactable = false;
        isTick = true;        
        basketball.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        StopAllCoroutines();
        canvasObject.SetActive(true);
        if (teamAMode.Equals("human") && teamBMode.Equals("human"))
        {
            if(int.Parse(teamAScore.text) > int.Parse(teamBScore.text))
            {
                GameStats.maxPoints = int.Parse(teamAScore.text);
            }
            else
            {
                GameStats.maxPoints = int.Parse(teamBScore.text);
            }
        }else if (teamAMode.Equals("human"))
        {
            GameStats.maxPoints = int.Parse(teamAScore.text);
        }
        else
        {
            GameStats.maxPoints = int.Parse(teamBScore.text);
        }
        GameStats.steals = UnityEngine.Random.Range(10, 20);        
        matchEnded = true;
        GameStats.matchEnded = matchEnded;
    }

    IEnumerator Draw()
    {
        ResetPositions();        
        ExtraTimeAnim.SetActive(true);
        yield return new WaitForSeconds(1);
        ExtraTimeAnim.SetActive(false);
    }
}
