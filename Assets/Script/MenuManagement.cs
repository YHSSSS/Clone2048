using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//[RequireComponent(typeof(XmlMethods))]
[RequireComponent(typeof(Animator))]
public class MenuManagement : MonoBehaviour
{
    [SerializeField]
    private InputField playerNameInput;

    [HideInInspector]
    private string playerName;

    private TouchScreenKeyboard keyboard;

    //components
    //[HideInInspector]
    //private XmlMethods xml;
    [HideInInspector]
    private Animator ani;

    void Start()
    {
        //xml = GetComponent<XmlMethods>();
        ani = GetComponent<Animator>();
    }

    public void ShowNewGameTitle()
    {
        ani.SetBool("ShowTitleButtonIsPressed", true);
    }

    public void StartNewGame()
    {
        if (playerNameInput.text != null || playerNameInput.text == "")
        {
            ani.SetBool("StartNewGameIsPressed", true);
            PlayerPrefs.SetString("playerName", playerName);
            //xml.CreateXML(playerName);
            StartCoroutine(GetLoadingScene());
            SceneManager.LoadScene(1);
        }
        else Debug.Log("Please enter player name!");
    }

    private IEnumerator GetLoadingScene()
    {
        yield return new WaitForSeconds(3);
    }

    public void SetPlayerName()
    {
        playerName = playerNameInput.text;
    }

    public void OpenWebsite()
    {
        string tempUrl = "https://github.com/YHSSSS";
        Application.OpenURL(tempUrl);
    }
}
