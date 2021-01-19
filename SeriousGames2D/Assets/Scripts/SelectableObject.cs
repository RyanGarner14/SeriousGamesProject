using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableObject : MonoBehaviour
{
    [HideInInspector]
    public bool Highlighted = false;
    [HideInInspector]
    public bool Selected = false;
    [HideInInspector]
    public bool Selectable = true;
    [HideInInspector]
    public bool isPressed = false;

    protected Color currentColor;
    protected Color baseColor;
    public Color highlightColor;
    public Color pressedColor;

    private SpriteRenderer sRenderer;

    protected virtual void Start()
    {
        sRenderer = GetComponent<SpriteRenderer>();
        baseColor = sRenderer.color;
        currentColor = baseColor;
    }

    protected virtual void Update()
    {
        if(Highlighted && Selectable && Input.GetMouseButtonDown(0))
        {
            SelectItem();
        }
        sRenderer.color = currentColor;
    }

    void OnMouseEnter()
    {
        if(Selectable)
            HighlightItem();
    }

    protected virtual void OnMouseExit()
    {        
        DeHighlightItem();
    }

    protected virtual void HighlightItem()
    {
        Highlighted = true;
        if(!isPressed)
            currentColor = highlightColor;
        //Debug.Log("Highlighting " + this.name);
    }

    protected virtual void DeHighlightItem()
    {
        Highlighted = false;
        if(!isPressed)
            currentColor = baseColor;
        //Debug.Log("DeHighlighting " + this.name);
    }

    protected virtual void SelectItem()
    {
        Selected = true;
        Selectable = false;
        //Debug.Log(this.name + " Selected");
    }

    protected virtual void DeselectItem()
    {
        Selected = false;
        Selectable = true;
        //Debug.Log(this.name + " Deselected");
    }
}
