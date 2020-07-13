using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField]
    private float minSwipeNumver = 4.0f;

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

        isSwiped = false;

        //check if portrait
        if (Screen.width < Screen.height)
        {
            minSwipeDistX = Screen.width / minSwipeNumver;
            minSwipeDistY = Screen.width / minSwipeNumver;
        }
        else
        {
            minSwipeDistX = Screen.height / minSwipeNumver;
            minSwipeDistY = Screen.height / minSwipeNumver;
        }
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startPos = touch.position;
                    break;
                case TouchPhase.Moved:
                    if (!isSwiped)
                    {
                        float temp_swipeDistHorizontal = touch.position.x - startPos.x;
                        float temp_swipeDistVertical = touch.position.y - startPos.y;
                        if (temp_swipeDistHorizontal > 0 && Mathf.Abs(temp_swipeDistHorizontal) > Mathf.Abs(temp_swipeDistVertical) && Mathf.Abs(temp_swipeDistHorizontal) > minSwipeDistX)
                        {
                            Debug.Log("Right");
                            isSwiped = true;
                            movement.SettingMovement(1);
                        }
                        else if (temp_swipeDistHorizontal < 0 && Mathf.Abs(temp_swipeDistHorizontal) > Mathf.Abs(temp_swipeDistVertical) && Mathf.Abs(temp_swipeDistHorizontal) > minSwipeDistX)
                        {
                            Debug.Log("Left");
                            isSwiped = true;
                            movement.SettingMovement(2);
                        }
                        else if (temp_swipeDistVertical > 0 && Mathf.Abs(temp_swipeDistHorizontal) < Mathf.Abs(temp_swipeDistVertical) && Mathf.Abs(temp_swipeDistVertical) > minSwipeDistY)
                        {
                            Debug.Log("Up");
                            isSwiped = true;
                            movement.SettingMovement(-1);
                        }
                        else if (temp_swipeDistVertical < 0 && Mathf.Abs(temp_swipeDistHorizontal) < Mathf.Abs(temp_swipeDistVertical) && Mathf.Abs(temp_swipeDistVertical) > minSwipeDistY)
                        {
                            Debug.Log("Down");
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
