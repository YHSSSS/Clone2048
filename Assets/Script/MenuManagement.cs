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

    bool ShowTitleButtonIsPressed;
    private void Awake()
    {
        playerNameInput = GameObject.Find("PlayerName").GetComponent<InputField>();
    }

    void Start()
    {
        xml = GetComponent<XmlMethods>();
        xml.CreateXml();

        ani = GetComponent<Animator>();
        ani.SetBool("StartNewGameIsPressed", false);
    }

    public void StartNewGame()
    {
        if (playerNameInput.text != null && playerNameInput.text != "" && playerNameInput.text.Length > 0)
        {
            //Set animator bool variable.
            //ani.SetBool("StartNewGameIsPressed", true);

            //Load the loading dialog prefab
            GameObject loadingDialogPrefab = Resources.Load(ConstString.RESOURCES_LOADING_DIALOG_PATH) as GameObject;
            if (!loadingDialogPrefab)
                Debug.LogError("Failed to load the loading dialog prefab");

            //Create a loading dialog to inform player for waiting
            GameObject loadingDialog = Instantiate(loadingDialogPrefab) as GameObject;
            loadingDialog.transform.SetParent(transform);

            //Set the string to player prefabs which will be sent to next scene.
            PlayerPrefs.SetString("playerName", playerName);

            //StartCoroutine(GetLoadingScene());

            //Load to next scene.
            SceneManager.LoadScene(1);
        }
        else
        {
            //Load dialog prefab from resource
            GameObject alertDialogPrefab = Resources.Load(ConstString.RESOURCES_ALERT_DIALOG_PATH) as GameObject;
            if (!alertDialogPrefab) 
                Debug.LogError("Failed to load the alert dialog prefab");

            //Create a dialog object using dialog prefab. 
            GameObject alertDialog = Instantiate(alertDialogPrefab) as GameObject;
            alertDialog.transform.SetParent(transform);
        }
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
