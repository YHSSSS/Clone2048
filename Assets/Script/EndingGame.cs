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
        GameObject wonGame = GameObject.Find("wonGameTitle");
        if (!wonGame) 
            Debug.LogError("Fail to find won game title object");

        //Check if player won the game.
        if (PlayerPrefs.GetInt("wonGame") == 1)
        {
            //Won game.

            //Get the score from playerprefs.
            int score = PlayerPrefs.GetInt("score");

            //Check if current score is higher than best score.
            if (xml.GetCurrentValue(playerName) < score)
            {
                //Save the beset record in the xml file.
                xml.CreateChildInXml(playerName, score);
                Debug.Log("Updated best score");
            }

            //Get the title text object.
            GameObject scoreObject = GameObject.Find("ScoreTitle");
            if (!scoreObject)
                Debug.LogError("Fail to find score object title object");

            //Set the text to the text object.
            scoreObject.transform.Find("ScoreTextBlack").GetComponent<Text>().text = score.ToString();
            scoreObject.transform.Find("ScoreTextWhite").GetComponent<Text>().text = score.ToString();

            //Set won game title.
            wonGame.transform.Find("TitleBlack").GetComponent<Text>().text = "CONGRATULATIONS";
            wonGame.transform.Find("TitleBlack").GetComponent<Text>().text = "CONGRATULATIONS";
        }
        else if (PlayerPrefs.GetInt("wonGame") == 2)
        {
            //Lose game.

            //Set won game title.
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
        //Load the rank view prefab object. 
        GameObject rankListObject = Resources.Load(ConstString.RESOURCES_RANK_VIEW_PATH) as GameObject;

        //Check if loading the object is failed.
        if (!rankListObject) Debug.LogError("Fail to load rank list object");

        //Create a new scroll view object.
        GameObject scrollObject = Instantiate(rankListObject) as GameObject;

        scrollObject.transform.SetParent(transform);

        //Get the scroll view component from the game object.
        Transform viewContent = scrollObject.transform.Find("Content");

        //Load the rank memeber object from rescource. 
        GameObject rankMemberObject = Resources.Load(ConstString.RESOURCES_RANK_MEMBER_PATH) as GameObject;

        //Get sorted rank list from xml file.
        List<players> playerList = xml.GetSortedPlayerList();

        foreach (players element in playerList)
        {
            //Create a new member object to show the record in the rank.
            GameObject rankMember = Instantiate(rankMemberObject) as GameObject;

            //Set the information in the list to the text in the member object.
            rankMember.transform.Find("PlayerName").GetComponent<Text>().text = element.playerName;
            rankMember.transform.Find("PlayerScore").GetComponent<Text>().text = element.bestScore;

            //Set the view content object as the parent of member object. 
            rankMember.transform.SetParent(viewContent);
        }
    }
}
