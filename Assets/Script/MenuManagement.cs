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

    //Components
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
        //Set the animator bool variable to show a button
        ani.SetBool("ShowTitleButtonIsPressed", true);
    }

    public void StartNewGame()
    {
        if (playerNameInput.text != null && playerNameInput.text != "" && playerNameInput.text.Length > 0)
        {
            //Set animator bool variable.
            ani.SetBool("StartNewGameIsPressed", true);

            //Set the string to player prefabs which will be sent to next scene.
            PlayerPrefs.SetString("playerName", playerName);

            StartCoroutine(GetLoadingScene());

            //Load to next scene.
            SceneManager.LoadScene(1);
        }
        else
        {
            //Load dialog prefab from resource
            GameObject dialogPrefab = Resources.Load(ConstString.RESOURCES_ALERT_DIALOG_PATH) as GameObject;

            //Check if loading the prefab is failed.
            if (dialogPrefab == null) Debug.LogError("Fail to load dialog prefab");

            //Create a dialog object using dialog prefab. 
            GameObject dialog = Instantiate(dialogPrefab) as GameObject;

            //Set the parent of the object. 
            dialog.transform.SetParent(transform);

            dialog.transform.name = "AlertDialog";

            //Change the dialog text. 
            dialog.transform.Find("DialogText").GetComponent<Text>().text = "Please Enter Your Name";
        }
    }

    private IEnumerator GetLoadingScene()
    {
        yield return new WaitForSeconds(3);
    }

    public void SetPlayerName()
    {
        //Get player name string from input field
        playerName = playerNameInput.text;
    }

    public void OpenWebsite()
    {
        string tempUrl = "https://github.com/YHSSSS";
        Application.OpenURL(tempUrl);
    }

}
