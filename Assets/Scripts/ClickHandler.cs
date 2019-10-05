using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickHandler : MonoBehaviour
{
    public Action WasClicked;    

    private void OnMouseDown()
    {
        if (this.WasClicked != null)
        {
            this.WasClicked();
        }
    }
}
