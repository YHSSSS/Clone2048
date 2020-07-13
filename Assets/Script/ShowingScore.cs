using UnityEngine;
using UnityEngine.UI;

public class ShowingScore : MonoBehaviour
{
    [HideInInspector]
    private GameManager manager;

    [SerializeField]
    private Text scoreText;

    private int score;

    private void Start()
    {
        manager = GameObject.Find("_GameManager").GetComponent<GameManager>();
        if (manager == null) Debug.LogError("Fail to find game manager!");

        score = 0;
    }

    private void FixedUpdate()
    {
        score = manager.GetScore();
        Debug.Log("score : " + score);
        scoreText.text = score.ToString();
    }
}
