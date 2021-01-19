using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderNode : Node
{
    int index;
    OrderModule module;

    Sprite symbol;
    GameObject symbolObject;

    void Awake()
    {
        symbolObject = new GameObject();
        symbolObject.AddComponent<SpriteRenderer>();

        //Debug.Log("Attaching SpriteRenderer Component");
    }

    protected override void Start()
    {
        base.Start();

        symbolObject.transform.parent = this.transform;
        symbolObject.transform.localScale = new Vector3(0.05f, 0.05f, 1f);
        symbolObject.transform.localPosition = Vector3.zero;

        module = GetComponentInParent<OrderModule>();
        Type = "toggle";
    }

    protected override void Update()
    {
        base.Update();        
    }

    protected override void SelectItem()
    {
        module.RecieveButtonPress(index);
    }

    public void DeselectButton()
    {
        isPressed = false;
        currentColor = baseColor;
    }

    public void Press()
    {
        isPressed = true;
        currentColor = pressedColor;
    }

    public void setIndex(int i)
    {
        index = i;
    }

    public void AssignSymbol(Sprite s)
    {
        symbol = s;
    }

    public void DrawSymbol()
    {
        SpriteRenderer sr = symbolObject.GetComponent<SpriteRenderer>();
        sr.sprite = symbol;
        sr.sortingLayerName = "symbols";
    }
}
