using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(XmlMethods))]
public class GameManager : MonoBehaviour
{
    [SerializeField]
    private static int GRID_SIZE = 16;
    [SerializeField]
    private static int blockNumEachR = 4;

    [SerializeField]
    private string playerName;

    [HideInInspector]
    private Vector2[] blocksPosition;
    [HideInInspector]
    private GameObject[] blocksObject;
    [HideInInspector]
    private int[] array;
    [HideInInspector]
    private int score;

    [HideInInspector]
    private XmlMethods xml;

    private void Awake()
    {
        playerName = PlayerPrefs.GetString("playerName");
        if (playerName == null || playerName == "") Debug.LogError("Fail to get player name");

        xml = GetComponent<XmlMethods>();
        SettingUpGrid setUpGrid = GameObject.Find("GridPart").GetComponent<SettingUpGrid>();
        setUpGrid.SetUp();
    }

    public void SetBlockPosition(Vector2[] _blocksPosition)
    {
        blocksPosition = _blocksPosition;
    }

    public void SetArrayList(int[] _array)
    {
        array = _array;
    }

    public void SetScore(int _score)
    {
        score = _score;
    }

    public void SetBlocksObject(GameObject[] _object)
    {
        blocksObject = _object;
    }

    public Vector2[] GetBlockPosition()
    {
        return blocksPosition;
    }

    public int[] GetArrayList()
    {
        return array;
    }

    public int GetScore()
    {
        return score;
    }

    public int GetGridSize()
    {
        return GRID_SIZE;
    }

    public int GetBlockNumEachR()
    {
        return blockNumEachR;
    }

    public GameObject[] GetBlocksObject()
    {
        return blocksObject;
    }

    public void CheckEndGame()
    {
        //check if win the game
        for (int i = 0; i < GRID_SIZE; i++)
        {
            if (array[i] == 2048)
            {
                EndGame(true);
                //Debug.Log("won game");
                return;
            }
        }

        //check if lose the game
        for (int i = 0; i < GRID_SIZE; i++)
        {
            if (CheckAddable(i)) return;
        }
        EndGame(false);
    }

    private bool CheckAddable(int index)
    {
        if (array[index] == 0)
            return true;
        //check if the left block of current block is addable
        if (index % blockNumEachR > 0 && array[index - 1] == array[index] )
            return true;
        //check if the right block of current block is addable
        if (index % blockNumEachR < blockNumEachR -1 && array[index + 1] == array[index])
            return true;
        //check if the up block of current block is addable
        if (index >= blockNumEachR && array[index - blockNumEachR] == array[index])
            return true;
        //check if the down block of current block is addable
        if (index < GRID_SIZE - blockNumEachR && array[index + blockNumEachR] == array[index])
            return true;

        return false;
    }

    private void EndGame(bool _wonGame)
    {
        if (_wonGame)
        {
            //update the score value recorded in xml file
            xml.CreateChildInXml(playerName, score);

            //set the parameters which will be sent to next scene
            PlayerPrefs.SetInt("wonGame", 1);
            PlayerPrefs.SetString("playerName", playerName);
            PlayerPrefs.SetInt("score", score);
        }
        else
            PlayerPrefs.SetInt("WonGame", 0);

        //jump to ending scene
        SceneManager.LoadScene(2);

        Debug.Log("you won the game!");
    }

}
