using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : SelectableObject
{
    protected string Type;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void SelectItem()
    {
        if(Type == "press")
        {
            base.SelectItem();
        }
        else if(Type == "toggle")
        {
            ToggleSelected();
        }
    }

    //protected override void DeselectItem()
    //{
    //    Selected = false;
    //    Selectable = true;
    //    Debug.Log(this.name + " Deselected");
    //}

    protected void ToggleSelected()
    {
        if (isPressed)
        {
            isPressed = false;
        }
        else
        {
            isPressed = true;
        }
    }
}
