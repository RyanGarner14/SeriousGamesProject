using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireNode : Node
{
    [HideInInspector]
    public bool Connected = false;

    WireNode Partner = null;
    WireModule module;

    GameObject symbolObject;
    Sprite symbol;
    public Vector3 symbolLocation;

    void Awake()
    {
        symbolObject = new GameObject();
        symbolObject.AddComponent<SpriteRenderer>();

        //Debug.Log("Attaching SpriteRenderer Component");
    }

    protected override void Start()
    {
        base.Start();

        //Debug.Log("Creating Symbol Object");
        symbolObject.transform.parent = this.transform;
        symbolObject.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
        symbolObject.transform.localPosition = symbolLocation;
        //Debug.Log(symbolObject.transform.position);

        module = GetComponentInParent<WireModule>();        
        Type = "toggle";
    }

    protected override void HighlightItem()
    {
        base.HighlightItem();

        module.selectedNode = this;
    }

    protected override void DeHighlightItem()
    {
        base.DeHighlightItem();

        module.selectedNode = null;
    }

    protected override void SelectItem()
    {
        base.SelectItem();
        isPressed = true;
        currentColor = pressedColor;
    }

    protected override void DeselectItem()
    {
        base.DeselectItem();
        isPressed = false;
        currentColor = baseColor;
    }

    public void Connect()
    {
        Connected = true;
        Selectable = false;
    }

    public void Deselect()
    {
        DeselectItem();
    }

    public void setPartner(WireNode p)
    {
        Partner = p;
    }

    public bool isPartner(WireNode p)
    {
        return Partner == p;
    }

    public bool hasPartner()
    {
        return Partner;
    }

    public void AssignSymbol(Sprite s)
    {
        symbol = s;
    }

    public void DrawSymbol()
    {
        //SpriteRenderer sr = symbolObject.GetComponent<SpriteRenderer>();
        //Debug.Log("Symbol Name: " + symbol.name);
        SpriteRenderer sr = symbolObject.GetComponent<SpriteRenderer>();
        sr.sprite = symbol;
        sr.sortingLayerName = "symbols";
    }
}
