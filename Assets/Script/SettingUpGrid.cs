using UnityEngine;
using UnityEngine.UI;

public class SettingUpGrid : MonoBehaviour
{
    [SerializeField]
    private int number = 5;

    [HideInInspector]
    private GameManager manager;
    [HideInInspector]
    private float blockSize;
    [HideInInspector]
    private float blockDis;
    [HideInInspector]
    private int blockNumEachR;
    [HideInInspector]
    private  int gridSize;

    [HideInInspector]
    private Vector2[] blocksPosition;
    [HideInInspector]
    private int[] array;
    [HideInInspector]
    private GameObject[] blocksObject;

    /// <summary>
    /// Set up the blocks and grid by calculating the relative info according to the 
    /// scrren size and intialize the array which will be used to record the block value
    /// </summary>
    public void SetUp()
    {
        //Get game manager component
        manager = GameObject.Find("_GameManager").GetComponent<GameManager>();
        if (manager == null) 
            Debug.LogError("Failed to find game manager!");

        //Check if the block information lists are null.  
        if (manager.GetArrayList() != null) manager.SetArrayList(null);
        if (manager.GetBlockPosition() != null) manager.SetBlockPosition(null);
        
        DestroyObjectInArray();
        manager.SetBlocksObject(null);

        //Get the basic grid size
        gridSize = manager.GetGridSize();
        blockNumEachR = manager.GetBlockNumEachR();
  
        //Initailize new array
        blocksPosition = new Vector2[gridSize];
        array = new int[gridSize];
        blocksObject = new GameObject[gridSize];

        //Get the grid background rect transform
        RectTransform gridBackgroundRect = GetComponent<RectTransform>();   

        //Use the transform information to get the size of the prefab so it fit the background.
        GetBlockSize(gridBackgroundRect.rect.width, gridBackgroundRect.rect.height);

        //Get the position for each blocks in the grid which will be used to move those blocks. 
        GetBlocksPosition(gridBackgroundRect.transform.localPosition.x, gridBackgroundRect.transform.localPosition.y);

        //Initialize the array and randomly get two initial values.
        InitializeArray();

        //Initiial blocks in the grid to create the block object according to the array.
        InitializeArrayInObject();
    }


    /// <summary>
    /// Destroy the objecst stored in the array
    /// </summary>
    private void DestroyObjectInArray()
    {
        for (int i = 0; i < gridSize; i++)
        {
            if (blocksObject[i])
            {
                Destroy(blocksObject[i]);
                blocksObject[i] = null;
            }
        }
    }


    /// <summary>
    /// Get the postion that each block in the grid will locate on the background
    /// </summary>
    /// <param name="_centerX">the grid background position x</param>
    /// <param name="_centerY">the grid background position y</param>
    private void GetBlocksPosition(float _centerX, float _centerY)
    {
        for (int i = 0; i < gridSize; i++)
        {
            float positionx = 0;
            float positiony = 0;

            //According to the number of blocks that each row will have to 
            switch (blockNumEachR)
            {
                case 4:
                    //Get position x
                    if (i % blockNumEachR == 0 || i % blockNumEachR == 3)
                    {
                        positionx = _centerX + (i % blockNumEachR <= 1 ? -1 : 1) * 1.5f * (blockSize + blockDis);
                    }
                    else if (i % blockNumEachR == 1 || i % blockNumEachR == 2)
                    {
                        positionx = _centerX + (i % blockNumEachR <= 1 ? -1 : 1) * 0.5f * (blockSize + blockDis);
                    }
                    //Get position y               
                    if (i / blockNumEachR == 0 || i / blockNumEachR == 3)
                    {
                        positiony = _centerY + (i / blockNumEachR <= 1 ? 1 : -1) * 1.5f * (blockSize + blockDis);
                    }
                    else if (i / blockNumEachR == 1 || i / blockNumEachR == 2)
                    {
                        positiony = _centerY + (i / blockNumEachR <= 1 ? 1 : -1) * 0.5f * (blockSize + blockDis);
                    }
                    break;
                case 5:
                    break;
            }
            //Make the position in the list following the transfrom in world instread of local.
            blocksPosition[i] = new Vector2(positionx - transform.localPosition.x, positiony - transform.localPosition.y);
        }
        //Set the final blocks position array to game manager.
        manager.SetBlockPosition(blocksPosition);
    }


    /// <summary>
    /// Calculate the size of the block so that the blocks can fit in the background
    /// </summary>
    /// <param name="_width">the width of the grid background</param>
    /// <param name="_height">the height of the grid background</param>
    private void GetBlockSize(float _width, float _height)
    {
        if (_height < _width) 
            Debug.LogError("Width is larger than height");
        
        //According to the ratio number to adjust the length of block size and block distance.
        blockSize = _width * (number - 1) / (float)(blockNumEachR * number);
        blockDis = _width / (float)(blockNumEachR * number);
    }


    /// <summary>
    /// Initialize the array which stores the value of the blocks and two of
    /// blocks will be given random value (2 or 4)
    /// </summary>
    private void InitializeArray()
    {
        //Set the array to zero 
        for (int i = 0; i < gridSize; i++)
        {
            array[i] = 0;
        }

        //Get two random indexs
        int num1 = Random.Range(0, gridSize);
        int num2;
        do
        {
            num2 = Random.Range(0, gridSize);
        } while (num1 == num2);

        //Debug.Log("initial num 1:" + num1 + " num 2:" + num2);

        //Get two random numbers
        int[] iniNum = new int[2];
        for (int j = 0; j < Mathf.Sqrt(blockNumEachR); j++)
        {
            iniNum[j] = Random.Range(0, 9) <2 ? 2 : 4;
        }

        //Set numbers to indexs
        array[num1] = iniNum[0];
        array[num2] = iniNum[1];

        //Set the array value to game manager
        manager.SetArrayList(array);
    }


    /// <summary>
    /// Create a new block object using block prefab and make the block locate at 
    /// the position that stored in the position array of the index 
    /// </summary>
    /// <param name="_index">a new block will be created with this index</param>
    /// <returns>a new block object</returns>
    public GameObject CreateABlockObject(int _index)
    {
        //Check if the object of current index exist
        GameObject blockTemp = GameObject.Find("Block" + _index);
        if (blockTemp)
        {
            Destroy(blockTemp);
            //Debug.Log("Destroy exist object");
        }  

        //Create a game object 
        //Load the block prefab object from resource
        GameObject blockPrefab = Resources.Load(ConstString.RESOURCES_BLOCK_PATH) as GameObject;

        //Check if loading the object is failed
        if (!blockPrefab) Debug.LogError("Failed to load block prefab");

        //Create a new block object
        GameObject block = Instantiate(blockPrefab) as GameObject;
        
        //Create a rectangle transform 
        RectTransform rt = block.GetComponent<RectTransform>();
        
        //Set the parent object of block to a game object and adjust the transform according to this game object.
        block.transform.SetParent(transform, false);
        rt.transform.localPosition = blocksPosition[_index];
        
        //Set the name of block object and the size of block object.
        rt.name = "Block" + _index;
        rt.sizeDelta = new Vector2(blockSize, blockSize);
        
        return block;
    }


    /// <summary>
    /// Clear the objects in the object array to clean the blocks in the grid 
    /// and create the default objects that has a value larger than zero.
    /// </summary>
    private void InitializeArrayInObject()
    {
        for (int i = 0; i < gridSize; i++)
        {
            if (array[i] > 0)
            {
                //Create a block object if the value of current index.
                blocksObject[i] = CreateABlockObject(i);

                //Set the text of the object using the value of current array index.
                if (blocksObject[i] != null)
                {
                    Text text = blocksObject[i].transform.Find("Text").gameObject.GetComponent<Text>();
                    text.text = array[i].ToString();
                }
                else
                {
                    Debug.Log("Failed to create object " + i);
                }
            }
        }

        //Update the blocks object to game manger.
        manager.SetBlocksObject(blocksObject);
    }
}
