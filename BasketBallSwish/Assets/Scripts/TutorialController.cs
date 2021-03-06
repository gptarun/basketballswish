﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class TutorialController : MonoBehaviour {
    public TextMeshProUGUI tutorial1;       // show text on UI
    public GameObject messageScreen;
    private List<string> tutorialMsgs = new List<string>(new string[] { "Hello, Welcome to the Game Tutorial. Tap the screen to Jump", "Good Job! Now tap and hold to jump and raise your hands. Then release.", "Excellent! To throw the Basketball, tap and hold to Jump and aim towards the Hoop.","Thanks for playing the tutorial!" });
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
    private GameObject handPivot;
    private bool attached;
    public TextMeshProUGUI onScreenTapText;
    private List<string> onScreenTapList = new List<string>(new string[] {"Tap Here!", "Tap and Hold!", "Tap to jump and Throw!"});
    public GameObject tapFinger;

    // Use this for initialization
    void Start () {
        wait = false;
        tutorialEnded = false;
        tutorial1.SetText(tutorialMsgs[count]);
        angle = 0.0f;
        antiRotate = false;
        rotate = false;
        rotationSpeed = 800.0f;
        basketball.SetActive(false);
        handPivot = GameObject.Find("handPivot");
        attached = false;
        if (GameObject.Find("MenuBg") != null)
        {
            Destroy(GameObject.Find("MenuBg"));
        }
    }

    // Update is called once per frame
    void Update()
    {        
        if (!tutorialEnded)
        {            
            if (Input.touchCount > 0)
            {
                Debug.Log(count);
                Touch touch = Input.GetTouch(0);
                if (!wait)
                {
                    if (count < 3)
                    {
                        if (tutorialMsgs[count].Contains("Hello, Welcome"))
                        {
                            Jump();
                            tapFinger.SetActive(false);         //hiding the finger tap animation
                            basketball.SetActive(false);
                        }
                        else if (tutorialMsgs[count].Contains("Good Job! Now tap"))
                        {
                            Jump();
                            rotate = true;
                            tapFinger.SetActive(false);         //hiding the finger tap animation
                            basketball.SetActive(false);
                            if (touch.phase.Equals(TouchPhase.Began))
                            {
                                antiRotate = false;
                            }
                            if (touch.phase.Equals(TouchPhase.Ended))
                            {
                                antiRotate = true;
                            }
                        }
                        else if (tutorialMsgs[count].Contains("Excellent! To throw"))
                        {
                            Jump();
                            rotate = true;
                            tapFinger.SetActive(false);         //hiding the finger tap animation
                            //basketball.SetActive(true);
                        }
                        count++;
                        StartCoroutine(WaitForNextTutorial());
                        wait = true;
                    }                   
                }
                if (tutorialMsgs[count - 1].Contains("Good Job! Now tap"))
                {
                    if (touch.phase.Equals(TouchPhase.Began))
                    {
                        antiRotate = false;
                    }
                    if (touch.phase.Equals(TouchPhase.Ended))
                    {
                        antiRotate = true;
                    }
                }
                if (tutorialMsgs[count - 1].Contains("Excellent! To throw"))
                {
                    if (touch.phase.Equals(TouchPhase.Began))
                    {
                        antiRotate = false;
                        attached = true;
                    }
                    if (touch.phase.Equals(TouchPhase.Ended))
                    {
                        antiRotate = true;
                        basketball.GetComponent<Rigidbody2D>().bodyType = (RigidbodyType2D)0;
                        basketball.GetComponent<Rigidbody2D>().velocity = CalculateJumpDistance(GameObject.Find("Basket_Team2"), 6);// launch the projectile!
                        basketball.GetComponent<Rigidbody2D>().gravityScale = 1;
                        StartCoroutine(MakeBallReady());
                        attached = false;
                    }
                    if (attached)
                    {
                        basketball.transform.position = handPivot.transform.position;
                        basketball.transform.rotation = Quaternion.Euler(0, 0, 0);
                        basketball.GetComponent<Rigidbody2D>().bodyType = (RigidbodyType2D)2;
                        basketball.GetComponent<Rigidbody2D>().gravityScale = 0;
                        basketball.GetComponent<CircleCollider2D>().isTrigger = true;
                    }
                }
            }

            if (rotate)
            {
                RotateHand();
            }
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
        if(count == 3)
        {
            tapFinger.SetActive(false);            
            yield return new WaitForSeconds(3f);
            tutorialEnded = true;
            tutorialCompletion.SetActive(true);            
            messageScreen.SetActive(false);            
        }
        else
        {
            yield return new WaitForSeconds(2f);
        }
        wait = false;       
        if (count < 3)
        {
            tapFinger.SetActive(true);         //showing the finger tap animation
            onScreenTapText.SetText(onScreenTapList[count]);
            tutorial1.SetText(tutorialMsgs[count]);
        }
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

    public Vector3 CalculateJumpDistance(GameObject anyObject, float height)
    {
        Vector3 jumpDis;
        float g = Physics.gravity.magnitude; // get the gravity value
        float vSpeed = Mathf.Sqrt(2 * g * height); // calculate the vertical speed
        float totalTime = 2 * vSpeed / g; // calculate the total time
        float maxDistance = Vector3.Distance(anyObject.transform.position, this.transform.position);
        float hSpeed = 1.7f *maxDistance * 1.75f / totalTime; // calculate the horizontal speed
        if ((anyObject.transform.position.x - this.transform.position.x) < 0)
        {
            hSpeed = -hSpeed;
        }
        jumpDis = new Vector3(hSpeed, vSpeed, 0);
        return jumpDis;
    }

    IEnumerator MakeBallReady()
    {
        yield return new WaitForSeconds(0.5f);
        basketball.GetComponent<CircleCollider2D>().isTrigger = false;
    }
}
