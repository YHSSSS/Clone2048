﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosingDialog : MonoBehaviour
{
    public void ButtonOnClick()
    {
        Destroy(transform.gameObject);
    }
}
