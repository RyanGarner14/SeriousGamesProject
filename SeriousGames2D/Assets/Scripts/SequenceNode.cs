using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceNode : Node
{
    SequenceModule module;
    int index;

    GameObject symbolObject;
    Sprite symbol;

    void Awake()
    {
        symbolObject = new GameObject();
        symbolObject.AddComponent<SpriteRenderer>();
    }

    protected override void Start()
    {
        base.Start();

        symbolObject.transform.parent = this.transform;
        symbolObject.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        symbolObject.transform.localPosition = Vector3.zero;

        module = GetComponentInParent<SequenceModule>();
        Type = "press";
    }

    protected override void Update()
    {
        base.Update();             
    }

    protected override void SelectItem()
    {
        //Glow();
        module.RecieveButtonPress(this);
    }

    public void Glow()
    {
        Debug.Log(this.name + " Glowing");
    }

    public void setIndex(int i)
    {
        index = i;
    }
    public int getIndex()
    {
        return index;
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
