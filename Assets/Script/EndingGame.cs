using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

[RequireComponent(typeof(XmlMethods))]
public class EndingGame : MonoBehaviour
{
    [HideInInspector]
    private XmlMethods xml;

    private void Start()
    {
        //SceneManager.LoadScene(1);
        xml = GetComponent<XmlMethods>();

        //Set player name to text.
        string playerName = PlayerPrefs.GetString("playerName");
        GameObject.Find("PlayerName").GetComponent<Text>().text = playerName;

        //Get won game title object.
        GameObject wonGame = GameObject.Find("WonGameTitle");
        if (!wonGame) 
            Debug.LogError("Failed to find the won game title object");

        //Check if player won the game.
        if (PlayerPrefs.GetInt("wonGame") == 1)
        {
            //Won game.

            //Get the score from playerprefs.
            int score = PlayerPrefs.GetInt("score");

            //Check if current score is higher than best score.
            int bestScore = xml.GetCurrentValue(playerName);
            if (bestScore < score && bestScore !=0)
            {
                xml.UpdateScore(playerName, score);
            }
            else if(bestScore == 0)
            {
                xml.CreateChildInXml(playerName, score);
            }

            //Get the title text object.
            GameObject scoreObject = GameObject.Find("ScoreTitle");
            if (!scoreObject)
                Debug.LogError("Failed to find the score title object");

            //Set the text to the text object.
            scoreObject.transform.Find("ScoreTextBlack").GetComponent<Text>().text = score.ToString();
            scoreObject.transform.Find("ScoreTextWhite").GetComponent<Text>().text = score.ToString();

            //Set won game title.
            wonGame.transform.Find("TitleBlack").GetComponent<Text>().text = "CONGRATULATIONS";
            wonGame.transform.Find("TitleWhite").GetComponent<Text>().text = "CONGRATULATIONS";
        }
        else if (PlayerPrefs.GetInt("wonGame") == 2)
        {
            //Lose game.

            //Set won game title.
            wonGame.transform.Find("TitleBlack").GetComponent<Text>().text = "LOSE";
            wonGame.transform.Find("TitleWhite").GetComponent<Text>().text = "LOSE";
        }
    }

    public void GoBackMenu()
    {
        //Load the loading dialog prefab
        GameObject loadingDialogPrefab = Resources.Load(ConstString.RESOURCES_LOADING_DIALOG_PATH) as GameObject;
        if (!loadingDialogPrefab)
            Debug.LogError("Failed to load the loading dialog prefab");

        //Create a loading dialog to inform player for waiting
        GameObject loadingDialog = Instantiate(loadingDialogPrefab) as GameObject;
        loadingDialog.transform.SetParent(transform);

        SceneManager.LoadScene(0);
    }

    public void ShowRankList()
    {
        //Load the rank view prefab object. 
        GameObject rankListObject = Resources.Load(ConstString.RESOURCES_RANK_VIEW_PATH) as GameObject;
        if (!rankListObject) 
            Debug.LogError("Failed to load rank list object");

        //Create a new scroll view object.
        GameObject scrollObject = Instantiate(rankListObject) as GameObject;

        scrollObject.transform.SetParent(transform.parent.transform);

        //Get the scroll view component from the game object.
        Transform viewContent = scrollObject.transform.Find("RankView/Viewport/Content");

        //Load the rank memeber object from rescource. 
        GameObject rankMemberObject = Resources.Load(ConstString.RESOURCES_RANK_MEMBER_PATH) as GameObject;

        //Get sorted rank list from xml file.
        List<players> playerList = xml.GetSortedPlayerList();

        foreach (players element in playerList)
        {
            //Create a new member object to show the record in the rank.
            GameObject rankMember = Instantiate(rankMemberObject, viewContent) as GameObject;

            //Set the information in the list to the text in the member object.
            rankMember.transform.Find("PlayerName").GetComponent<Text>().text = element.playerName;
            rankMember.transform.Find("PlayerScore").GetComponent<Text>().text = element.bestScore;

        }
    }
}
