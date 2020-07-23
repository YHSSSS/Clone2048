using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

[RequireComponent(typeof(XmlMethods))]
public class EndingGame : MonoBehaviour
{
    [SerializeField]
    private GameObject scoreObejct;
    [SerializeField]
    private GameObject wonGame;
    [SerializeField]
    private Text playerNameText;

    [HideInInspector]
    private XmlMethods xml;

    private void Start()
    {
        //SceneManager.LoadScene(1);
        xml = GetComponent<XmlMethods>();
        GameObject rankListObject = Resources.Load(ConstString.RESOURCES_RANK_VIEW_PATH) as GameObject;
  
        //check if loading the object is failed
        if (!rankListObject) Debug.LogError("Fail to load rank list object");

        //instantiate a new block object
        //ScrollView scrollView = Instantiate(rankListObject) as GameObject.transfrom.Find("RankView").GetComponent<ScrollView>;

        //set player name to text
        string playerName = PlayerPrefs.GetString("playerName");
        playerNameText.text = playerName;

        //check if player won the game
        if (PlayerPrefs.GetInt("wonGame") == 1)
        {
            //won game

            //get the score from playerprefs
            int score = PlayerPrefs.GetInt("score");
            //save the record in the xml file
            if (xml.GetCurrentValue(playerName) < score)
                xml.CreateChildInXml(playerName, score);

            //set the score to text
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
