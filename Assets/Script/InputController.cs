using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField]
    private float minSwipeNumber = 4.0f;

    [HideInInspector]
    private Vector2 startPos;
    [HideInInspector]
    private bool isSwiped;
    [HideInInspector]
    private float minSwipeDistX;
    [HideInInspector]
    private float minSwipeDistY;

    [HideInInspector]
    private BlocksMovement movement;

    private void Start()
    {
        movement = GameObject.Find("GridPart").GetComponent<BlocksMovement>();
        if (!movement)
            Debug.LogError("Failed to find grid part and get blocks movement component");

        isSwiped = false;

        //Check if the screen is portrait.
        if (Screen.width < Screen.height)
        {
            //Using the width of the screen to calculate the minimum length of swiping distance.
            minSwipeDistX = Screen.width / minSwipeNumber;
            minSwipeDistY = Screen.width / minSwipeNumber;
        }
        else
        {
            //Using the height of the screen to calculate the minimum length of swiping distance.
            minSwipeDistX = Screen.height / minSwipeNumber;
            minSwipeDistY = Screen.height / minSwipeNumber;
        }
    }

    void Update()
    {
        //Check if player touch the screen.
        if (Input.touchCount > 0)
        {
            //get the first touch 
            Touch touch = Input.touches[0];

            //Phase the touch to run different action.
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startPos = touch.position;
                    break;

                case TouchPhase.Moved: 
                    if (!isSwiped)
                    {
                        //Get the distance between current touch position and the position of start touch.
                        float temp_swipeDistHorizontal = touch.position.x - startPos.x;
                        float temp_swipeDistVertical = touch.position.y - startPos.y;
                        
                        //Check the direction of the distance and check if the longest swiping distance is on
                        //horizontal or vertical.
                        //Check if the distance satisfy the minimum distance. 
                        if (temp_swipeDistHorizontal > 0 && Mathf.Abs(temp_swipeDistHorizontal) > Mathf.Abs(temp_swipeDistVertical) && Mathf.Abs(temp_swipeDistHorizontal) > minSwipeDistX)
                        {
                            //Debug.Log("Right");
                            isSwiped = true;
                            movement.SettingMovement(1);
                        }
                        else if (temp_swipeDistHorizontal < 0 && Mathf.Abs(temp_swipeDistHorizontal) > Mathf.Abs(temp_swipeDistVertical) && Mathf.Abs(temp_swipeDistHorizontal) > minSwipeDistX)
                        {
                            //Debug.Log("Left");
                            isSwiped = true;
                            movement.SettingMovement(2);
                        }
                        else if (temp_swipeDistVertical > 0 && Mathf.Abs(temp_swipeDistHorizontal) < Mathf.Abs(temp_swipeDistVertical) && Mathf.Abs(temp_swipeDistVertical) > minSwipeDistY)
                        {
                            //Debug.Log("Up");
                            isSwiped = true;
                            movement.SettingMovement(-1);
                        }
                        else if (temp_swipeDistVertical < 0 && Mathf.Abs(temp_swipeDistHorizontal) < Mathf.Abs(temp_swipeDistVertical) && Mathf.Abs(temp_swipeDistVertical) > minSwipeDistY)
                        {
                            //Debug.Log("Down");
                            isSwiped = true;
                            movement.SettingMovement(-2);
                        }
                    }
                    break;

                case TouchPhase.Ended:
                    isSwiped = false;
                    break;
            }
        }
    }
}
