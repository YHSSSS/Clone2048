using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(XmlMethods))]
[RequireComponent(typeof(Animator))]
public class MenuManagement : MonoBehaviour
{
    [HideInInspector]
    private string playerName;

    [HideInInspector]
    private InputField playerNameInput;

    //components
    [HideInInspector]
    private XmlMethods xml;
    [HideInInspector]
    private Animator ani;

    private void Awake()
    {
        playerNameInput = GameObject.Find("PlayerName").GetComponent<InputField>();
    }

    void Start()
    {
        xml = GetComponent<XmlMethods>();
        xml.CreateXml();

        ani = GetComponent<Animator>();
    }

    public void ShowNewGameTitle()
    {
        //set the animator bool variable to show a button
        ani.SetBool("ShowTitleButtonIsPressed", true);
    }

    public void StartNewGame()
    {
        if (playerNameInput.text != null && playerNameInput.text != "" && playerNameInput.text.Length > 0)
        {
            //set animator bool variable
            ani.SetBool("StartNewGameIsPressed", true);

            //set the string to player prefabs which will be sent to next scene
            PlayerPrefs.SetString("playerName", playerName);

            StartCoroutine(GetLoadingScene());

            //load to next scene
            SceneManager.LoadScene(1);
        }
        else
        {
            //load dialog prefab from resource
            GameObject dialogPrefab = Resources.Load(ConstString.RESOURCES_ALERT_DIALOG_PATH) as GameObject;

            //check if loading the prefab is failed
            if (dialogPrefab == null) Debug.LogError("Fail to load dialog prefab");

            //instantiate the dialog prefab 
            GameObject dialog = Instantiate(dialogPrefab) as GameObject;

            //set the parent of the object 
            dialog.transform.SetParent(transform);

            dialog.transform.name = "AlertDialog";

            //change the dialog text 
            dialog.transform.Find("DialogText").GetComponent<Text>().text = "Please Enter Your Name";
        }
    }

    private IEnumerator GetLoadingScene()
    {
        yield return new WaitForSeconds(3);
    }

    public void SetPlayerName()
    {
        //get the player name string from input field
        playerName = playerNameInput.text;
    }

    //open the website of the developer
    public void OpenWebsite()
    {
        string tempUrl = "https://github.com/YHSSSS";
        Application.OpenURL(tempUrl);
    }

}
