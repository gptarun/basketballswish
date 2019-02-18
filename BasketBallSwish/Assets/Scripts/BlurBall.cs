using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlurBall : MonoBehaviour {

    SpriteRenderer ballSprite;
    float timer = 0.08f;
    BallController ball;
    BallControllerTournament ballTour;
    SpriteRenderer basketball;
    // Use this for initialization
    void Start () {
        ballSprite = GetComponent<SpriteRenderer>();
        if (FindObjectOfType<BallController>() != null)
        {
            ball = FindObjectOfType<BallController>();
            basketball = ball.GetComponent<SpriteRenderer>();
            transform.position = ball.transform.position;
            transform.localScale = ball.transform.localScale;
        }
        else if (FindObjectOfType<BallControllerTournament>() != null)
        {
            ballTour = FindObjectOfType<BallControllerTournament>();
            basketball = ballTour.GetComponent<SpriteRenderer>();
            transform.position = ballTour.transform.position;
            transform.localScale = ballTour.transform.localScale;
        }
        ballSprite.sprite = basketball.sprite;
        ballSprite.sortingLayerName = basketball.sortingLayerName;
        ballSprite.sortingOrder = basketball.sortingOrder;
        ballSprite.color = new Vector4(50,50,50,0.2f);
    }
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Destroy(gameObject);
        } 
		
	}
}
