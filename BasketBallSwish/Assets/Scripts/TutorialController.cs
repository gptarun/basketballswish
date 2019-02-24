using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class TutorialController : MonoBehaviour {
    public TextMeshProUGUI tutorial1;       // show text on UI
    public GameObject messageScreen;
    private List<string> tutorialMsgs = new List<string>(new string[] { "Hello, Welcome to the Game Tutorial. Tap the screen to Jump", "Good Job! Now tap and hold to jump and to raise your hands. Then release.", "Excellent! To thow the Basketball, tap and hold to Jump and aim towards the Hoop." });
    public GameObject player;
    public GameObject hand;
    int count = 0;
    private bool wait;          //Wait before next tutorial message
    bool tutorialEnded;         //Keep track of tutoial's end
    public bool pauseButtonPressed = false;      // pausing the game
    public GameObject tutorialCompletion;       // End screen text
    private bool rotate;
    private bool antiRotate;
    private float angle;
    private float rotationSpeed;
    public GameObject basketball;

    // Use this for initialization
    void Start () {
        wait = false;
        tutorialEnded = false;
        tutorial1.SetText(tutorialMsgs[count]);
        angle = 0.0f;
        antiRotate = false;
        rotate = false;
        rotationSpeed = 800.0f;
	}

    // Update is called once per frame
    void Update()
    {        
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (!wait)
            {
                if (count < 3)          //Iterating tutorial msgs
                {                    
                    count++;                    
                    StartCoroutine(WaitForNextTutorial());
                    wait = true;
                    Jump();
                    rotate = true;
                }
                else
                {
                    tutorialCompletion.SetActive(true);
                    messageScreen.SetActive(false);
                }
            }
            if (touch.phase.Equals(TouchPhase.Began))
            {
                antiRotate = false;
                //rotate = true;
            }
            if (touch.phase.Equals(TouchPhase.Ended))
            {
                antiRotate = true;
            }
        }
        if (rotate)
        {
            RotateHand();
        }
    }

    private void Jump()
    {             
        player.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 8.0f, 0);
    }

    private void RotateHand()
    {
        if (!antiRotate && angle < 180.0f)
        {
            angle += Time.deltaTime * rotationSpeed;
            if (angle > 180f)
            {
                angle = 180f;
            }
        }
        if (antiRotate)
        {
            angle -= Time.deltaTime * rotationSpeed;
            if (angle <= 0)
            {
                angle = 0;
            }
        }
        hand.transform.localRotation = Quaternion.Euler(0, 0, angle);
        if (angle <= 0)
        {
            rotate = false;
            antiRotate = false;
        }
    }

    IEnumerator WaitForNextTutorial()
    {
        if(count == 4)
        {
            yield return new WaitForSeconds(5f);
        }
        else
        {
            yield return new WaitForSeconds(2f);
        }
        wait = false;
        tutorial1.SetText(tutorialMsgs[count]);
        if (count == 2)
        {
            basketball.SetActive(true);
        }
    }

    public void RestartTutorial()
    {
        SceneManager.LoadScene("TutorialScene");
    }

    public void LoadMainMenu()
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
}
