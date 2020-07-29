using UnityEngine;

public class ClosingDialog : MonoBehaviour
{
    /// <summary>
    /// Destroy the object which obtains this script when press the button
    /// </summary>
    public void ButtonOnClick()
    {
        Destroy(transform.gameObject);
    }
}
