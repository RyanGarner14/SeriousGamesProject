using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module : SelectableObject
{
    protected bool isEscapable = false;
    [HideInInspector]
    public bool isComplete = false;

    Camera cam;
    protected Machine machine;
    protected SymbolList symbolList;

    protected override void Start()
    {
        base.Start();
        machine = GetComponentInParent<Machine>();
        symbolList = machine.GetComponent<SymbolList>();
        cam = machine.getCamera();
    }

    protected override void Update()
    {
        base.Update();

        if(Selected)
        {
            if(Input.GetMouseButtonUp(1))
            {
                if(isEscapable)
                    DeselectItem();
            }
        }
    }

    protected override void SelectItem()
    {
        base.SelectItem();

        // Get the location for the camera to move to
        Vector3 camPos = transform.position;

        cam.GetComponent<CameraScript>().MoveToPos(camPos);
        machine.DisableModules();
        isEscapable = true;
    }

    protected override void DeselectItem()
    {
        base.DeselectItem();

        // Return Camera to it's original position
        cam.GetComponent<CameraScript>().MoveToOrigin();
        machine.EnableModules();
        isEscapable = false;
    }

    protected void CompleteModule()
    {
        Debug.Log(this.name + " COMPLETE");

        AudioSource sound = GetComponent<AudioSource>();
        if (sound != null)
            sound.Play();

        currentColor = pressedColor;

        isComplete = true;
        DeselectItem();
        Selectable = false;
    }
}
