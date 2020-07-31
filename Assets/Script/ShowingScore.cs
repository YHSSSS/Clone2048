using UnityEngine;
using UnityEngine.UI;

public class ShowingScore : MonoBehaviour
{
    [HideInInspector]
    private GameManager manager;

    [HideInInspector]
    private Text scoreText;

    [HideInInspector]
    private int score;

    private void Start()
    {
        //Get the text component aftering finding the score title object
        scoreText = GameObject.Find("ScoreNum").GetComponent<Text>();
        if (!scoreText)
            Debug.LogError("Failed to find score title object");

        manager = GetComponent<GameManager>();
        if (!manager) 
            Debug.LogError("Failed to find game manager!");

        score = 0;
    }

    private void FixedUpdate()
    {
        //Get current score from game manager
        score = manager.GetScore();
        //Debug.Log("score : " + score);

        //Update the score text 
        scoreText.text = score.ToString();
    }
}
