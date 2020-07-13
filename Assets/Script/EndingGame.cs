using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndingGame : MonoBehaviour
{
    [SerializeField]
    private GameObject scoreObejct;
    [SerializeField]
    private GameObject wonGame;
    [SerializeField]
    private Text playerNameText;

    private void Start()
    {
        SceneManager.LoadScene(1);

        //set player name to text
        string playerName = PlayerPrefs.GetString("playerName");
        playerNameText.text = playerName;

        if (PlayerPrefs.GetInt("wonGame") == 1)
        {
            //won game
            //set score to text
            int score = PlayerPrefs.GetInt("score");
            scoreObejct.transform.Find("ScoreTextBlack").GetComponent<Text>().text = score.ToString();
            scoreObejct.transform.Find("ScoreTextWhite").GetComponent<Text>().text = score.ToString();

            //set won game title
            wonGame.transform.Find("TitleBlack").GetComponent<Text>().text = "CONGRATULATIONS";
            wonGame.transform.Find("TitleBlack").GetComponent<Text>().text = "CONGRATULATIONS";
        }
        else
        {
            //lose game
            //set won game title
            wonGame.transform.Find("TitleBlack").GetComponent<Text>().text = "LOSE";
            wonGame.transform.Find("TitleBlack").GetComponent<Text>().text = "LOSE";
        }
    }

    public void GoBackMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ShowRankList()
    {

    }
}
