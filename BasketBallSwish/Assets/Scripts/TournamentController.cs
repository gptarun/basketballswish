using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

public class TournamentController : MonoBehaviour {

    //This script will play the game of individual teams which is called by  Clicking PLAY button

    [HideInInspector] public string teamAMode;
    [HideInInspector] public string teamBMode;
    private string teamAFullName;
    private string teamBFullName;
    [SerializeField] private Text quater;
    private int totalQuaterCounter;
    [HideInInspector] public int quaterCounter = 1;
    [SerializeField] private Text quaterTimer;
    [HideInInspector] public int qTimer;
    [SerializeField] private Text teamAScore;
    public int scoreA;
    [SerializeField] private Text teamBScore;
    public int scoreB;
    [SerializeField] private Text teamAName;
    [SerializeField] private Text teamBName;
    [SerializeField] GameObject WinnerCanvas;
    [SerializeField] TextMeshProUGUI winnerName;
    [SerializeField] Button pauseButton;
    [HideInInspector] public static bool pauseButtonPressed = false;
    bool gameOver ;
    public GameObject jumpAnim;
    public GameObject FoulAnim;
    public GameObject ExtraTimeAnim;
    float delatTime;
    public GameObject[] player = new GameObject[6];
    Vector3[] playerPosition = new Vector3[6];
    public Transform ballPosition;
    [HideInInspector] public GameObject basketball;
    Vector3 ballInitPos = new Vector3(0.01f,2.4f,0f);
    [SerializeField] TournamentScript tournamentScript;
    private bool newMatch;
    [HideInInspector] public string round;
    [HideInInspector] public Sprite[] faces;
    [HideInInspector] public Sprite[] jersey;
    public bool matchEnded;
    public AudioSource lastTick;
    private bool isTick;
    public AudioSource gameBell;
    public BallControllerTournament ballController;
    private UserDataController userDataController;
    // Use this for initialization
    void Start()
    {
        faces = Resources.LoadAll<Sprite>("Face");
        jersey = Resources.LoadAll<Sprite>("Team");
        basketball = GameObject.Find("basketball");
        ballController = basketball.GetComponent<BallControllerTournament>();
        initializeSharedObjects(0, 0, "quarterFinal");
        userDataController = new UserDataController();
    }

    // Update is called once per frame
    void Update()
    {
        if (newMatch)
        {
            if (ExtraTimeAnim.activeSelf)
            {
                ExtraTimeAnim.SetActive(false);
            }
            if (FoulAnim.activeSelf)
            {
                FoulAnim.SetActive(false);
            }
            basketball.GetComponent<CircleCollider2D>().isTrigger = false;
            basketball.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            pauseButtonPressed = false;
            totalQuaterCounter = OptionMenuScript.quaterCounter;
            qTimer = OptionMenuScript.quaterDuration[OptionMenuScript.quaterTimerCounter];
            quaterTimer.text = "" + qTimer;
            quater.text = "Q" + quaterCounter;
            pauseButton.interactable = true;
            StartCoroutine("LoseTime");
            int indexA = Array.FindIndex(jersey, cloth => cloth.name == teamAName.text.ToString().ToUpper());
            int indexB = Array.FindIndex(jersey, cloth => cloth.name == teamBName.text.ToString().ToUpper());            
            for (int i = 0; i < OptionMenuScript.teamSizeCounter * 2; i++)
            {
                playerPosition[i] = player[i].transform.position;
                player[i].SetActive(true);
                if (i % 2 == 0)
                {
                    player[i].GetComponent<SpriteRenderer>().sprite = jersey[indexA];
                    int tempFace = UnityEngine.Random.Range(0, 11);
                    if (tempFace == 1) { tempFace++; }
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
            delatTime = qTimer;
            Time.timeScale = 1;
            StartCoroutine(MakeUserReady());
            gameOver = false;
            newMatch = false;
            isTick = true;
            matchEnded = false;
            basketball.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
        }
        if (!pauseButtonPressed)
        {
            teamAScore.text = scoreA.ToString();
            teamBScore.text = scoreB.ToString();
            delatTime -= Time.deltaTime;
            if (qTimer > -1)
            {
                GameQuaterCounter(qTimer);
            }
            else if (qTimer < 1 && quaterCounter != totalQuaterCounter)
            {
                qTimer = OptionMenuScript.quaterDuration[OptionMenuScript.quaterTimerCounter];
                quaterCounter++;
                quater.text = "Q" + quaterCounter;
                isTick = true;
                StartCoroutine(MakeUserReady());
            }
            else
            {
                if (!gameOver)
                {
                    gameOver = true;
                    GameWinner(round);
                }
            }
            if (qTimer < 10 && qTimer >= 0 && lastTick != null && isTick)
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

    IEnumerator MakeUserReady()
    {
        StopCoroutine("LoseTime");
        ResetPositions();
        jumpAnim.SetActive(true);
        yield return new WaitForSeconds(2);
        jumpAnim.SetActive(false);
        StartCoroutine("LoseTime");
    }

    public void ResetPositions()
    {
        basketball.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        basketball.GetComponent<Rigidbody2D>().angularVelocity = 0;
        if (ballController.attached)
        {
            basketball.GetComponent<Rigidbody2D>().bodyType = (RigidbodyType2D)0;
            basketball.GetComponent<Rigidbody2D>().gravityScale = 1;
            ballController.attached = false;
        }
        for (int i = 0; i < player.Length; i++)
        {
            player[i].transform.position = playerPosition[i];
        }        
        ballPosition.transform.position = ballInitPos;
    }

    public void GameQuaterCounter(int qTimer)
    {
        quaterTimer.text = qTimer.ToString();
    }

    public void SinglePlayerTeams()
    {
        Debug.Log("Game Begins");
    }

    public void SetTeams(MatchDay matchDay)
    {
        Debug.Log(matchDay.TeamA.TeamName + " vs " + matchDay.TeamB.TeamName + " in quarter final");
        teamAName.text = matchDay.TeamA.ShortName;
        teamBName.text = matchDay.TeamB.ShortName;
        teamAMode = matchDay.TeamA.Mode;
        teamBMode = matchDay.TeamB.Mode;
        teamAFullName = matchDay.TeamA.TeamName;
        teamBFullName = matchDay.TeamB.TeamName;
    }

    public void SetTeams(string teamANameSelected, string teamBNameSelected, string modeA, string modeB)
    {
        Debug.Log(teamANameSelected +" vs "+ teamBNameSelected);
        teamAName.text = teamANameSelected.Substring(0, 4);
        teamBName.text = teamBNameSelected.Substring(0, 4);
        teamAMode = modeA;
        teamBMode = modeB;
        teamAFullName = teamANameSelected;
        teamBFullName = teamBNameSelected;       
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
    public void GameWinner(string round)
    {
        MatchDay[] matchDays;
        if (round.Equals("quarterFinal"))
        {
            matchDays = tournamentScript.quarterFinal;
        }
        else if (round.Equals("semiFinal"))
        {
            matchDays = tournamentScript.semiFinal;
        }
        else if (round.Equals("final"))
        {
            matchDays = tournamentScript.final;
        }
        else
        {
            matchDays = null;
        }
        if (scoreA > scoreB)
        {
            winnerName.text = teamAFullName + " Wins!";
            setMatchResults(teamAFullName, scoreA, teamBFullName, scoreB, matchDays);
            if (teamAMode.Equals("human"))
            {
                userDataController.userData.baskyCoins += (100 + scoreA);
            }
            else if (teamBMode.Equals("human"))
            {
                userDataController.userData.baskyCoins += (scoreB);
            }
            userDataController.SaveGameData();
            MatchEnded();
        }
        else if (scoreA < scoreB)
        {
            winnerName.text = teamBFullName + " Wins!";
            setMatchResults(teamBFullName, scoreB, teamAFullName, scoreA, matchDays);
            if (teamBMode.Equals("human"))
            {
                userDataController.userData.baskyCoins += (100 + scoreB);
            }
            else if (teamAMode.Equals("human"))
            {
                userDataController.userData.baskyCoins += (scoreA);
            }
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
        Debug.Log(round);
    }
    public void MatchEnded()
    {
        if (gameBell != null)
        {
            gameBell.Play();
        }
        pauseButton.interactable = false;
        StopAllCoroutines();
        WinnerCanvas.SetActive(true);
        matchEnded = true;
    }

    IEnumerator Draw()
    {
        ResetPositions();
        ExtraTimeAnim.SetActive(true);
        yield return new WaitForSeconds(1);
        ExtraTimeAnim.SetActive(false);
    }
    public void SkipCurrentMatch()
    {
        Debug.Log("Current match is skipped");
        ResetPositions();
        matchEnded = true;
        scoreA = (int)Mathf.Round(UnityEngine.Random.Range(150, 300) / 3);
        scoreB = (int)Mathf.Round(UnityEngine.Random.Range(150, 300) / 3);
        GameWinner(round);                
    }   
    public void setMatchResults(string winner,int winScore, string loser, int loseScore,MatchDay[] matchDays)
    {
        for (int i = 0; i < matchDays.Length; i++)
        {
            if (matchDays[i].TeamA.TeamName.Equals(winner))
            {
                matchDays[i].MatchDayResult = new MatchDayResult(matchDays[i].TeamA, winScore, matchDays[i].TeamB, loseScore);
            }
            else if (matchDays[i].TeamB.TeamName.Equals(winner))
            {
                matchDays[i].MatchDayResult = new MatchDayResult(matchDays[i].TeamB, winScore, matchDays[i].TeamA, loseScore);
            }
        }
    }
    public void initializeSharedObjects(int scoreA, int scoreB, string roundPassed)
    {
        this.scoreA = scoreA;
        this.scoreB = scoreB;
        teamAScore.text = scoreA.ToString();
        teamBScore.text = scoreB.ToString();        
        round = roundPassed;
        newMatch = true;
        quaterCounter = 1;
        matchEnded = false;
        isTick = true;        
    }
}