using UnityEngine;
using UnityEngine.UI;

public class SettingUpGrid : MonoBehaviour
{
    [SerializeField]
    private int number = 5;
    [SerializeField]
    private GameObject singleBlock;

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

    public void SetUp()
    {
        manager = GameObject.Find("_GameManager").GetComponent<GameManager>();
        if (manager == null) Debug.LogError("Fail to find game manager!");
        //manager = _manager;

        //check if the block information lists are null  
        if (manager.GetArrayList() != null) manager.SetArrayList(null);
        if (manager.GetBlockPosition() != null) manager.SetBlockPosition(null);
        DestroyArray();
        //if (manager.GetBlocksObject() != null) manager.SetBlocksObject(null);

        //get the basic grid size
        gridSize = manager.GetGridSize();
        blockNumEachR = manager.GetBlockNumEachR();
  
        //initailize new array
        blocksPosition = new Vector2[gridSize];
        array = new int[gridSize];
        blocksObject = new GameObject[gridSize];

        //get the grid background rect transform
        RectTransform gridBackgroundRect = GetComponent<RectTransform>();   

        //use the transform information to get the size of the prefab so it fit the background
        GetBlockSize(gridBackgroundRect.rect.width, gridBackgroundRect.rect.height);

        //get the position for each blocks in the grid which will be used to move those blocks 
        GetBlocksPosition(gridBackgroundRect.transform.localPosition.x, gridBackgroundRect.transform.localPosition.y);

        //initialize the array and randomly get two initial values
        InitializeArray();

        //initiail blocks in the grid to create the block object according to the array
        InitializeArrayInObject();
    }

    private void DestroyArray()
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
    private void GetBlocksPosition(float centerX, float centerY)
    {
        for (int i = 0; i < gridSize; i++)
        {
            float positionx = 0;
            float positiony = 0;

            switch (blockNumEachR)
            {
                case 4:
                    //get position x
                    if (i % blockNumEachR == 0 || i % blockNumEachR == 3)
                    {
                        positionx = centerX + (i % blockNumEachR <= 1 ? -1 : 1) * 1.5f * (blockSize + blockDis);
                    }
                    else if (i % blockNumEachR == 1 || i % blockNumEachR == 2)
                    {
                        positionx = centerX + (i % blockNumEachR <= 1 ? -1 : 1) * 0.5f * (blockSize + blockDis);
                    }
                    //get position y               
                    if (i / blockNumEachR == 0 || i / blockNumEachR == 3)
                    {
                        positiony = centerY + (i / blockNumEachR <= 1 ? 1 : -1) * 1.5f * (blockSize + blockDis);
                    }
                    else if (i / blockNumEachR == 1 || i / blockNumEachR == 2)
                    {
                        positiony = centerY + (i / blockNumEachR <= 1 ? 1 : -1) * 0.5f * (blockSize + blockDis);
                    }
                    break;
                case 5:
                    break;
            }
            //make the position in the list following the transfrom in world instread of local
            blocksPosition[i] = new Vector2(positionx - transform.localPosition.x, positiony - transform.localPosition.y);
        }
        //set the final blocks position array to game manager
        manager.SetBlockPosition(blocksPosition);
    }

    private void GetBlockSize(float width, float height)
    {
        if (height < width) Debug.LogError("width is larger than height");
        
        //according to the ratio number to adjust the length of block size and block distance
        blockSize = width * (number - 1) / (float)(blockNumEachR * number);
        blockDis = width / (float)(blockNumEachR * number);
    }

    private void InitializeArray()
    {
        //set the array to zero 
        for (int i = 0; i < gridSize; i++)
        {
            array[i] = 0;
        }

        //get two random indexs
        int num1 = Random.Range(0, gridSize);
        int num2;
        do
        {
            num2 = Random.Range(0, gridSize);
        } while (num1 == num2);

        //get two random numbers
        int[] iniNum = new int[2];
        for (int j = 0; j < Mathf.Sqrt(blockNumEachR); j++)
        {
            iniNum[j] = Random.Range(0, 9) <3 ? 2 : 4;
        }

        //set numbers to indexs
        array[num1] = iniNum[0];
        array[num2] = iniNum[1];

        //set the array value to game manager
        manager.SetArrayList(array);
    }

    public GameObject CreateBlocksObject(int i)
    {
        //check if the object of current index exist
        if (transform.Find("Block" + i)) return null;

        //create a game object 
        GameObject block = Instantiate(singleBlock, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        
        //create a rectangle transform 
        RectTransform rt = block.GetComponent<RectTransform>();
        
        //set the parent object of block to a game object and adjust the transform according to this game object
        block.transform.SetParent(transform, false);
        rt.transform.localPosition = blocksPosition[i];
        
        //set the name of block object and the size of block object
        rt.name = "Block" + i;
        rt.sizeDelta = new Vector2(blockSize, blockSize);
        
        return block;
    }

    private void InitializeArrayInObject()
    {
        for (int i = 0; i < gridSize; i++)
        {
            if (array[i] != 0)
            {
                //create a block object if the value of current index
                blocksObject[i] = CreateBlocksObject(i);

                //set the text of the object using the value of current array index
                if (blocksObject[i] != null)
                {
                    Text text = blocksObject[i].transform.Find("Text").gameObject.GetComponent<Text>();
                    text.text = array[i].ToString();
                }
            }
        }

        //update the blocks object to game manger
        manager.SetBlocksObject(blocksObject);
    }
}
