using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SettingUpGrid))]
public class BlocksMovement : MonoBehaviour
{
    //block objects movement
    [SerializeField]
    private float blockMovingSpeed = 20f;
    [HideInInspector]
    private Vector2[] blocksPosition;
    [HideInInspector]
    private int movingDirection;
    [HideInInspector]
    private int addedCount;
    [HideInInspector]
    private float currentScore;

    //components
    [HideInInspector]
    private SettingUpGrid setUpGrid;
    [HideInInspector]
    private GameManager manager;

    //blocks information
    [HideInInspector]
    private GameObject[] blocks;
    [HideInInspector]
    private int[] array;
    [HideInInspector]
    private int gridSize;
    [HideInInspector]
    private int blockNumEachR;

    private void Start()
    {
        InitializeMovement();
    }

    public void InitializeMovement()
    {
        //get components
        setUpGrid = GetComponent<SettingUpGrid>();
        manager = GameObject.Find("_GameManager").GetComponent<GameManager>();
        if (manager == null) Debug.LogError("Fail to find game manager!");

        //get the information of the grid and array
        gridSize = manager.GetGridSize();
        blockNumEachR = manager.GetBlockNumEachR();
        blocksPosition = manager.GetBlockPosition();
        array = manager.GetArrayList();
        blocks = manager.GetBlocksObject();

        //initailize variable
        movingDirection = 0;
        addedCount = 0;
        currentScore = 0.0f;
    }

    private void FixedUpdate()
    {
    }

    public void SettingMovement(int _movingDirection)
    {
        //set the direction of swiping from user
        movingDirection = _movingDirection;

        //get current array and blocks list
        array = manager.GetArrayList();
        blocks = manager.GetBlocksObject();

        switch (_movingDirection)
        {
            case 1:
                //moving from left to right
                for (int row = 0; row < gridSize / blockNumEachR; row++)
                {
                    //remove zero
                    for (int column = (gridSize - 1) % blockNumEachR; column > 0; column--)
                    {
                        int tempNum = column;
                        CheckZeroHorizontalRecursion(row, column, tempNum, true);
                    }

                    //after removing zero in the list, check if the value for each index is addable
                    for (int column = (gridSize - 1) % blockNumEachR; column > 0; column--)
                    {
                        if (array[row * blockNumEachR + column] == array[row * blockNumEachR + column - 1] && array[row * blockNumEachR + column] != 0)
                        {
                            ArrayAddHorizontal(row, column, column - 1, true);
                        }
                    }
                }
                break;
            case 2:
                //movign from right to left
                for (int row = 0; row < gridSize / blockNumEachR; row++)
                {
                    //remove zero
                    for (int column = 0; column < (gridSize - 1) % blockNumEachR; column++)
                    {
                        int tempNum = column;
                        CheckZeroHorizontalRecursion(row, column, tempNum, false);
                    }

                    //after removing zero in the list, check if the value for each index is addable
                    for (int column = 0; column < (gridSize - 1) % blockNumEachR; column++)
                    {
                        if (array[row * blockNumEachR + column] == array[row * blockNumEachR + column + 1] && array[row * blockNumEachR + column] != 0)
                        {
                            ArrayAddHorizontal(row, column, column + 1, false);
                        }
                    }
                }
                break;
            case -1:
                for (int column = 0; column <= (gridSize - 1) % blockNumEachR; column++)
                {
                    //remove zero
                    for (int row = 0; row < (gridSize / blockNumEachR - 1); row++)
                    {
                        int tempNum = row;
                        CheckZeroVerticalRecursion(row, column, tempNum, true);
                    }

                    //after removing zero in the list, check if the value for each index is addable
                    for (int row = 0; row < (gridSize / blockNumEachR - 1); row++)
                    {
                        if (array[column + blockNumEachR * row] == array[column + blockNumEachR * (row + 1)] && array[column + blockNumEachR * row] != 0)
                        {
                            ArrayAddVertical(column, row, row + 1, true);
                        }
                    }
                }
                break;
            case -2:
                for (int column = 0; column < blockNumEachR; column++)
                {
                    //remove zero
                    for (int row = (gridSize / blockNumEachR - 1); row > 0; row--)
                    {
                        int tempNum = row;
                        CheckZeroVerticalRecursion(row, column, tempNum, false);
                    }

                    //after removing zero in the list, check if the value for each index is addable
                    for (int row = (gridSize / blockNumEachR - 1); row > 0; row--)
                    {
                        if (array[column + blockNumEachR * row] == array[column + blockNumEachR * (row - 1)] && array[column + blockNumEachR * row] != 0)
                        {
                            ArrayAddVertical(column, row, row - 1, false);
                        }
                    }
                }
                break;
        }

        //refresh the objects in the grid
        for (int i = 0; i < gridSize; i++)
        {
            if (array[i] != 0)
            {
                //create new objects for those index in the array with non-zero value
                if (!blocks[i])
                {
                    blocks[i] = setUpGrid.CreateBlocksObject(i);
                    if (!blocks[i]) Debug.LogError("create a null object at: " + i);
                }

                //if the object of this index is exist, only change the text of the object
                blocks[i].transform.Find("Text").gameObject.GetComponent<Text>().text = array[i].ToString();
            }
            else
            {
                //destroy the object for those index in the array with zero value
                if (blocks[i])
                {
                    Destroy(blocks[i]);
                    blocks[i] = null;
                }
            }
        }

        //update the array value and object list
        manager.SetArrayList(array);
        manager.SetBlocksObject(blocks);

        //update score
        currentScore += addedCount;
        manager.SetScore((int)currentScore);
        Debug.Log("current score: " + currentScore);

        //add a new block object in the grid
        AddOneObjectEachAction();

        //check if end the game
        manager.CheckEndGame();

        string temp = "result";
        foreach (int item in array)
        {
            temp = temp + " " + item;
        }
        Debug.Log(temp);
    }

    private void CheckZeroVerticalRecursion(int row, int column, int num, bool isUp)
    {
        //check if the value of current index is zero
        if (array[row * blockNumEachR + column] == 0)
        {
            //if user is swiping to up
            if (isUp)
            {
                ArrayRemoveZeroVertical(column, row, row + 1, isUp);
                if (num < blockNumEachR - 1)
                    CheckZeroVerticalRecursion(row, column, ++num, isUp);
            }
            //if user is swipting to down
            else
            {
                ArrayRemoveZeroVertical(column, row, row - 1, isUp);
                if (num > 0)
                    CheckZeroVerticalRecursion(row, column, --num, isUp);
            }
        }
    }

    private void CheckZeroHorizontalRecursion(int row, int column, int num, bool isRight)
    {
        //check if the value of current index is zero
        if (array[row * blockNumEachR + column] == 0)
        {
            //if user is swiping to right
            if (isRight)
            {
                ArrayRemoveZeroHorizontal(row, column, column - 1, isRight);
                if (num > 0)
                    CheckZeroHorizontalRecursion(row, column, --num, isRight);
            }
            //if user is swipting to left
            else
            {
                ArrayRemoveZeroHorizontal(row, column, column + 1, isRight);
                if (num < blockNumEachR - 1)
                    CheckZeroHorizontalRecursion(row, column, ++num, isRight);
            }
        }
    }

    private void ArrayRemoveZeroVertical(int _column, int _forIndex, int _latIndex, bool isUp)
    {
        array[_column + blockNumEachR * _forIndex] = array[_column + blockNumEachR * _latIndex];
        array[_column + blockNumEachR * _latIndex] = 0;

        //recursive this fuction to move the next values to up or down of current column
        if (_latIndex < (blockNumEachR - 1) && isUp)
            ArrayRemoveZeroVertical(_column, ++_forIndex, ++_latIndex, isUp);
        else if (_latIndex > 0 && !isUp)
            ArrayRemoveZeroVertical(_column, --_forIndex, --_latIndex, isUp);
    }

    private void ArrayAddVertical(int _column, int _forIndex, int _latIndex, bool isUp)
    {
        //count the adding action to calculate current score
        addedCount++;

        //add the values of two index and set the value of latter index to zero
        array[_column + blockNumEachR * _forIndex] = 2 * array[_column + blockNumEachR * _forIndex];
        array[_column + blockNumEachR * _latIndex] = 0;

        //recursive this function to add the next values of current column
        if (_latIndex < (blockNumEachR - 1) && isUp)
            ArrayRemoveZeroVertical(_column, ++_forIndex, ++_latIndex, isUp);
        else if (_latIndex > 0 && !isUp)
            ArrayRemoveZeroVertical(_column, --_forIndex, --_latIndex, isUp);
    }

    private void ArrayRemoveZeroHorizontal(int _row, int _forIndex, int _latIndex, bool isRight)
    {
        array[_row * blockNumEachR + _forIndex] = array[_row * blockNumEachR + _latIndex];
        array[_row * blockNumEachR + _latIndex] = 0;

        //recursive this fuction to move the next values to left or right of current row
        if (_latIndex > 0 && isRight)
            ArrayRemoveZeroHorizontal(_row, --_forIndex, --_latIndex, isRight);
        else if (_latIndex < (blockNumEachR - 1) && !isRight)
            ArrayRemoveZeroHorizontal(_row, ++_forIndex, ++_latIndex, isRight);
    }

    private void ArrayAddHorizontal(int _row, int _forIndex, int _latIndex, bool isRight)
    {
        //count the adding action to calculate current score
        addedCount++;

        //add the values of two index and set the value of latter index to zero
        array[_row * blockNumEachR + _forIndex] = 2 *  array[_row * blockNumEachR + _forIndex];
        array[_row * blockNumEachR + _latIndex] = 0;

        //recursive this function to add the next values of current row
        if (_latIndex > 0 && isRight)
            ArrayRemoveZeroHorizontal(_row, --_forIndex, --_latIndex, isRight);
        else if (_latIndex < (blockNumEachR - 1) && !isRight)
            ArrayRemoveZeroHorizontal(_row, ++_forIndex, ++_latIndex, isRight);
    }

    private void AddOneObjectEachAction()
    {
        int textValue = Random.Range(0, 9) < 3 ? 2 : 4;
        
        //get randomly index
        int[] tempAddableIndex = new int[gridSize];
        int tempIndex = 0;
        for (int i = 0; i < gridSize; i++)
        {
            if (array[i] == 0)
            {
                tempAddableIndex[tempIndex] = i;
                tempIndex++;
            }
        }
        int addObjectIndex = tempAddableIndex[Random.Range(0, tempIndex)];
        Debug.Log("add object index" + addObjectIndex);

        //create the obejct 
        blocks[addObjectIndex] = setUpGrid.CreateBlocksObject(addObjectIndex);
        if (blocks[addObjectIndex])
        {
            //set the value of this object into object list
            Text text = blocks[addObjectIndex].transform.Find("Text").gameObject.GetComponent<Text>();
            text.text = textValue.ToString();
        }

        //set the value of this object in array list
        array[addObjectIndex] = textValue;
    }
}
