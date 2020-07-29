using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SettingUpGrid))]
public class BlocksMovement : MonoBehaviour
{
    //Block objects movement
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

    //Components
    [HideInInspector]
    private SettingUpGrid setUpGrid;
    [HideInInspector]
    private GameManager manager;

    //Blocks information
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

    /// <summary>
    /// Initialize those variables which will be used in the movement script.
    /// Initialize the grid and blocks object and get the grid info from game manager.
    /// </summary>
    public void InitializeMovement()
    {
        //Get the components
        setUpGrid = GetComponent<SettingUpGrid>();
        manager = GameObject.Find("_GameManager").GetComponent<GameManager>();
        if (!manager) 
            Debug.LogError("Fail to find game manager!");

        //Get the information of the grid and array.
        gridSize = manager.GetGridSize();
        blockNumEachR = manager.GetBlockNumEachR();
        blocksPosition = manager.GetBlockPosition();
        array = manager.GetArrayList();
        blocks = manager.GetBlocksObject();

        //Initailize variables
        movingDirection = 0;
        addedCount = 0;
        currentScore = 0.0f;
    }

    /// <summary>
    /// Set the movement of the array and blocks according to the moving direction from users. 
    /// Update the array values in the grid after swiping.
    /// </summary>
    /// <param name="_movingDirection">the direction that user swipes</param>
    public void SettingMovement(int _movingDirection)
    {
        //Set the direction of swiping from user.
        movingDirection = _movingDirection;

        //Get current array and blocks list.
        array = manager.GetArrayList();
        blocks = manager.GetBlocksObject();

        switch (_movingDirection)
        {
            case 1:
                //Moving from left to right.
                for (int row = 0; row < gridSize / blockNumEachR; row++)
                {
                    //Remove zero
                    for (int column = (gridSize - 1) % blockNumEachR; column > 0; column--)
                    {
                        int tempNum = column;
                        CheckZeroHorizontalRecursion(row, column, column, true);
                    }

                    //After removing zero in the list, check if the value for each index is addable.
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
                //Moving from right to left
                for (int row = 0; row < gridSize / blockNumEachR; row++)
                {
                    //Remove zero
                    for (int column = 0; column < (gridSize - 1) % blockNumEachR; column++)
                    {
                        int tempNum = column;
                        CheckZeroHorizontalRecursion(row, column, column, false);
                    }

                    //After removing zero in the list, check if the value for each index is addable.
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
                    //Remove zero
                    for (int row = 0; row < (gridSize / blockNumEachR - 1); row++)
                    {
                        int tempNum = row;
                        CheckZeroVerticalRecursion(row, column, row, true);
                    }

                    //After removing zero in the list, check if the value for each index is addable
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
                    //Remove zero
                    for (int row = (gridSize / blockNumEachR - 1); row > 0; row--)
                    {
                        int tempNum = row;
                        CheckZeroVerticalRecursion(row, column, row, false);
                    }

                    //After removing zero in the list, check if the value for each index is addable.
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

        //Refresh the objects in the grid
        for (int i = 0; i < gridSize; i++)
        {
            if (array[i] != 0)
            {
                //Create new objects for those index in the array with non-zero value.
                if (!blocks[i])
                {
                    blocks[i] = setUpGrid.CreateABlockObject(i);
                    if (!blocks[i]) Debug.LogError("create a null object at: " + i);
                }

                //If the object of this index is exist, only change the text of the object.
                blocks[i].transform.Find("Text").gameObject.GetComponent<Text>().text = array[i].ToString();
            }
            else
            {
                //Destroy the object for those index in the array with zero value.
                if (blocks[i])
                {
                    Destroy(blocks[i]);
                    blocks[i] = null;
                }
            }
        }

        //Update the array value and object list
        manager.SetArrayList(array);
        manager.SetBlocksObject(blocks);

        //Update score
        currentScore += addedCount;
        manager.SetScore((int)currentScore);
        //Debug.Log("current score: " + currentScore);

        //Add a new block object in the grid
        AddOneObjectEachAction();

        //Check if end the game
        manager.CheckEndGame();

        //string temp = "result";
        //foreach (int item in array)
        //{
        //    temp = temp + " " + item;
        //}
        //Debug.Log(temp);
    }

    /// <summary>
    /// Check if the value of current index recursively in the array is zero.
    /// </summary>
    /// <param name="row">the position row of the index in the grid</param>
    /// <param name="column">the position column of the index in the grid</param>
    /// <param name="num">the times that need to check the value of this index</param>
    /// <param name="isUp">if is true which means the moving direction is swiping to up, 
    /// else means is swiping to down</param>
    private void CheckZeroVerticalRecursion(int row, int column, int num, bool isUp)
    {
        //Check if the value of current index is zero.
        if (array[row * blockNumEachR + column] == 0)
        {
            //If user is swiping to up.
            if (isUp)
            {
                ArrayRemoveZeroVertical(column, row, row + 1, isUp);
                if (num < blockNumEachR - 1)
                    CheckZeroVerticalRecursion(row, column, ++num, isUp);
            }
            //If user is swiping to down.
            else
            {
                ArrayRemoveZeroVertical(column, row, row - 1, isUp);
                if (num > 0)
                    CheckZeroVerticalRecursion(row, column, --num, isUp);
            }
        }
    }

    /// <summary>
    /// Check if the value of current index recursively in the array is zero.
    /// </summary>
    /// <param name="row">the position row of the index in the grid</param>
    /// <param name="column">the position column of the index in the grid</param>
    /// <param name="num">the times that need to check the value of this index</param>
    /// <param name="isRight">if is true which means the moving direction is swiping to right, 
    /// else means is swiping to left</param>
    private void CheckZeroHorizontalRecursion(int row, int column, int num, bool isRight)
    {
        //Check if the value of current index is zero
        if (array[row * blockNumEachR + column] == 0)
        {
            //If user is swiping to right
            if (isRight)
            {
                ArrayRemoveZeroHorizontal(row, column, column - 1, isRight);
                if (num > 0)
                    CheckZeroHorizontalRecursion(row, column, --num, isRight);
            }
            //If user is swipting to left
            else
            {
                ArrayRemoveZeroHorizontal(row, column, column + 1, isRight);
                if (num < blockNumEachR - 1)
                    CheckZeroHorizontalRecursion(row, column, ++num, isRight);
            }
        }
    }

    /// <summary>
    /// Move the latter index to the former index on vertical and keep the same action for next pair
    /// of index on this column.
    /// </summary>
    /// <param name="_column">The column of the index that need to be moved</param>
    /// <param name="_forRow">The former row of index that need to be replaced</param>
    /// <param name="_latRow">The latter row of index that will replace the former row of index</param>
    /// <param name="isUp">If is true means the moving direction is swiping to up, else means is 
    /// swiping to down</param>
    private void ArrayRemoveZeroVertical(int _column, int _forRow, int _latRow, bool isUp)
    {
        array[_column + blockNumEachR * _forRow] = array[_column + blockNumEachR * _latRow];
        array[_column + blockNumEachR * _latRow] = 0;

        //Recursive this fuction to move the next index to up or down on current column.
        if (_latRow < (blockNumEachR - 1) && isUp)
            ArrayRemoveZeroVertical(_column, ++_forRow, ++_latRow, isUp);
        else if (_latRow > 0 && !isUp)
            ArrayRemoveZeroVertical(_column, --_forRow, --_latRow, isUp);
    }

    /// <summary>
    /// Add the value of the former index and the latter index, then remove the zero for 
    /// the next two index
    /// </summary>
    /// <param name="_column">The column of the index that need to be moved</param>
    /// <param name="_forRow">The former row of index that need to be replaced</param>
    /// <param name="_latRow">The latter row of index that will replace the former row of index</param>
    /// <param name="isUp">If is true means the moving direction is swiping to up, else means is 
    /// swiping to down</param>
    /// <returns></returns>
    private void ArrayAddVertical(int _column, int _forRow, int _latRow, bool isUp)
    {
        //Count the adding action to calculate current score.
        addedCount++;

        //Add the values of two index and set the value of latter index to zero.
        array[_column + blockNumEachR * _forRow] = 2 * array[_column + blockNumEachR * _forRow];
        array[_column + blockNumEachR * _latRow] = 0;

        //Recursive this function to add the next values of current column.
        if (_latRow < (blockNumEachR - 1) && isUp)
            ArrayRemoveZeroVertical(_column, ++_forRow, ++_latRow, isUp);
        else if (_latRow > 0 && !isUp)
            ArrayRemoveZeroVertical(_column, --_forRow, --_latRow, isUp);
    }

    /// <summary>
    /// Move the latter index to the former index on horizontal and keep the same action for next pair
    /// of index on this row.
    /// </summary>
    /// <param name="_row">The column of the index that need to be moved</param>
    /// <param name="_forColumn">The former column of index that need to be replaced</param>
    /// <param name="_latColumn">The latter column of index that will replace the former row of index</param>
    /// <param name="isRight">If is true means the moving direction is swiping to down, else means is 
    /// swiping to left</param>
    /// <returns></returns>
    private void ArrayRemoveZeroHorizontal(int _row, int _forColumn, int _latColumn, bool isRight)
    {
        array[_row * blockNumEachR + _forColumn] = array[_row * blockNumEachR + _latColumn];
        array[_row * blockNumEachR + _latColumn] = 0;

        //Recursive this fuction to move the next values to left or right of current row.
        if (_latColumn > 0 && isRight)
            ArrayRemoveZeroHorizontal(_row, --_forColumn, --_latColumn, isRight);
        else if (_latColumn < (blockNumEachR - 1) && !isRight)
            ArrayRemoveZeroHorizontal(_row, ++_forColumn, ++_latColumn, isRight);
    }

    /// <summary>
    /// Add the value of the former index and the latter index, then remove the zero for 
    /// the next two index
    /// </summary>
    /// <param name="_row">The row of the index that need to be moved</param>
    /// <param name="_forColumn">The former column of index that need to be replaced</param>
    /// <param name="_latColumn">The latter column of index that will replace the former row of index</param>
    /// <param name="isRight">If is true means the moving direction is swiping to right, else means is 
    /// swiping to left</param>
    /// <returns></returns>
    private void ArrayAddHorizontal(int _row, int _forColumn, int _latColumn, bool isRight)
    {
        //Count the adding action to calculate current score.
        addedCount++;

        //Add the values of two index and set the value of latter index to zero.
        array[_row * blockNumEachR + _forColumn] = 2 *  array[_row * blockNumEachR + _forColumn];
        array[_row * blockNumEachR + _latColumn] = 0;

        //Recursive this function to add the next values of current row.
        if (_latColumn > 0 && isRight)
            ArrayRemoveZeroHorizontal(_row, --_forColumn, --_latColumn, isRight);
        else if (_latColumn < (blockNumEachR - 1) && !isRight)
            ArrayRemoveZeroHorizontal(_row, ++_forColumn, ++_latColumn, isRight);
    }

    /// <summary>
    /// Create a new block object with the value of it is a random number (2 or 4)
    /// and the index of it is one of the blocks in the grid that the value of it is zero. 
    /// </summary>
    private void AddOneObjectEachAction()
    {
        int textValue = Random.Range(0, 9) < 2 ? 2 : 4;
        
        //Get randomly index
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
        int addObjectIndex = tempAddableIndex[Random.Range(0, tempIndex + 1)];
        //Debug.Log("add object index" + addObjectIndex);

        //Create the obejct 
        blocks[addObjectIndex] = setUpGrid.CreateABlockObject(addObjectIndex);
        if (blocks[addObjectIndex])
        {
            //Set the value of this object into object list
            Text text = blocks[addObjectIndex].transform.Find("Text").gameObject.GetComponent<Text>();
            text.text = textValue.ToString();
        }

        //Set the value of this object in array list
        array[addObjectIndex] = textValue;
    }
}
