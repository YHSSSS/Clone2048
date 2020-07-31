using UnityEngine;
using UnityEngine.SceneManagement;

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

    private void Awake()
    {
        //Get player name from playerPrefs.
        playerName = PlayerPrefs.GetString("playerName");
        if (playerName == null || playerName == "") 
            Debug.LogError("Fail to get player name");

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

    /// <summary>
    /// Check each index in the array satisfy the requirement to won the game or 
    /// lose the game. Send the result to the EndGame function to give action to 
    /// the result.
    /// </summary>
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

    /// <summary>
    /// Check if the value of the current index the array is zero or can be able to 
    /// be added by the value of surrounding index in the grid.
    /// </summary>
    /// <param name="_index">the index number that will be checked</param>
    /// <returns>If yes means the value of index in the array is addable, else means 
    /// that is not addable</returns>
    private bool CheckAddable(int _index)
    {
        if (array[_index] == 0)
            return true;
        //Check if the left block of current block is addable.
        if (_index % blockNumEachR > 0 && array[_index - 1] == array[_index] )
            return true;
        //Check if the right block of current block is addable.
        if (_index % blockNumEachR < blockNumEachR -1 && array[_index + 1] == array[_index])
            return true;
        //Check if the up block of current block is addable.
        if (_index >= blockNumEachR && array[_index - blockNumEachR] == array[_index])
            return true;
        //Check if the down block of current block is addable.
        if (_index < GRID_SIZE - blockNumEachR && array[_index + blockNumEachR] == array[_index])
            return true;

        return false;
    }

    /// <summary>
    /// Store player records to playerPrefs and load to next scene.
    /// </summary>
    /// <param name="_wonGame">the result that the game is end or not</param>
    private void EndGame(bool _wonGame)
    {
        if (_wonGame)
        {
            //Set the parameters which will be sent to next scene.
            PlayerPrefs.SetInt("wonGame", 1);
            PlayerPrefs.SetString("playerName", playerName);
            PlayerPrefs.SetInt("score", score);
        }
        else
            PlayerPrefs.SetInt("WonGame", 0);

        //Load the ending scene.
        SceneManager.LoadScene(2);
    }

    public void EndGameNow()
    {
        score = 30;
        playerName = "YHS";
        EndGame(true);
    }
}
