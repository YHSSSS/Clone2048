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

        //keyboard.active(true);

    }
    public void AwakeKeyboard()
    {
        Debug.Log("opening keyboard");
        keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.ASCIICapable);
        keyboard.active = true;

        Debug.Log(TouchScreenKeyboard.area);
        if (TouchScreenKeyboard.visible) Debug.Log("showed");
        else Debug.Log("fail to show");

        if (keyboard == null) Debug.LogError("keyboard null!");
    }

    public void ShowNewGameTitle()
    {
        ani.SetBool("ShowTitleButtonIsPressed", true);
    }

    public void StartNewGame()
    {
        if (playerNameInput.text != null)
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
