using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireModule : Module
{
    WireNode[] nodeGroup;

    [HideInInspector]
    public WireNode selectedNode;

    bool isConnecting = false;
    int[] Connections;
    Sprite[] SymbolsToAttach;
    WireNode currentConnecting;
    GameObject DrawnWire;

    public Material wireMat;
    public float wireWidth;

    protected override void Start()
    {
        base.Start();
        isComplete = false;
        nodeGroup = GetComponentsInChildren<WireNode>();

        GenerateConnections();

        AttachSymbolsToNodes();

        for (int i = 0; i < nodeGroup.Length; i++) 
        {
            WireNode n = nodeGroup[i];
            n.AssignSymbol(SymbolsToAttach[i]);
            //Debug.Log("symbol to attach: (" + i + ") " + SymbolsToAttach[i]);
            n.DrawSymbol();
            n.GetComponent<CircleCollider2D>().enabled = false;
        }        
    }

    protected override void Update()
    {
        base.Update();

        bool canConnect = true;

        if (!isComplete)
        {

            if (connectionsComplete())
            {
                CompleteModule();
                machine.isMachineComplete();
            }

            if (isConnecting)
            {
                UpdateDrawnWire();

                if (Input.GetMouseButtonUp(0))
                {
                    canConnect = false;
                    if (selectedNode && (selectedNode != currentConnecting))
                    {
                        if (isValidConnection(currentConnecting, selectedNode))
                        {
                            print("VALID CONNECTION");
                            ConnectNodes(currentConnecting, selectedNode);
                        }
                        else
                        {
                            print("INVALID CONNECTION");
                            FindObjectOfType<GameManager>().Strike();
                            currentConnecting.Deselect();
                            selectedNode.Deselect();
                            DrawnWire.SetActive(false);
                            Destroy(DrawnWire);                            
                        }
                    }
                    else
                    {
                        DrawnWire.SetActive(false);
                        Destroy(DrawnWire);
                    }
                    StopConnecting();
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (selectedNode && canConnect)
                    StartConnecting(selectedNode);
            }
        }
    }

    protected override void SelectItem()
    {
        base.SelectItem();

        foreach(WireNode n in nodeGroup)
        {
            n.GetComponent<CircleCollider2D>().enabled = true;
        }
    }

    protected override void DeselectItem()
    {
        base.DeselectItem();

        foreach (WireNode n in nodeGroup)
        {
            n.GetComponent<CircleCollider2D>().enabled = false;
        }
    }

    void ConnectNodes(WireNode a, WireNode b)
    {
        a.Connect();
        b.Connect();

        //Debug.Log("Connected " + a + " to " + b);
    }

    bool isValidConnection(WireNode a, WireNode b)
    {
        return a.isPartner(b);
    }

    void StartConnecting(WireNode n)
    {
        isConnecting = true;
        isEscapable = false;

        currentConnecting = n;

        DrawnWire = new GameObject();
        DrawnWire.AddComponent<LineRenderer>();
        LineRenderer lr = DrawnWire.GetComponent<LineRenderer>();
        lr.material = wireMat;
        lr.startWidth = wireWidth;
        lr.endWidth = wireWidth;
        lr.sortingLayerName = "extras";
        lr.SetPosition(0, currentConnecting.transform.position);
    }

    void StopConnecting()
    {
        isConnecting = false;
        isEscapable = true;

        currentConnecting = null;
    }

    void UpdateDrawnWire()
    {
        var end = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        end = Camera.main.ScreenToWorldPoint(end);

        LineRenderer lr = DrawnWire.GetComponent<LineRenderer>();

        lr.SetPosition(1, end);
        lr.sortingOrder = 3;
    }

    void GenerateConnections()
    {
        int numberConnections = Random.Range(1, 4);
        Connections = new int[numberConnections*2];
        List<int> ConnectedNumbers = new List<int>();

        for(int i = 0; i < numberConnections; i++)
        {
            int x = Random.Range(0, 3);
            while (ConnectedNumbers.Contains(2 * x + 1))
                x = Random.Range(0, 3);
            int Left = 2 * x + 1;

            x = Random.Range(0, 3);
            while (ConnectedNumbers.Contains(2 * x + 2))
                x = Random.Range(0, 3);
            int Right = 2 * x + 2;

            nodeGroup[Left - 1].setPartner(nodeGroup[Right - 1]);
            nodeGroup[Right - 1].setPartner(nodeGroup[Left - 1]);

            ConnectedNumbers.Add(Left);
            ConnectedNumbers.Add(Right);

            Connections[2 * i] = Left;
            Connections[(2 * i) + 1] = Right;

            //Debug.Log("Added Connection between " + Left + " and " + Right);
        }
    }

    void AttachSymbolsToNodes()
    {
        int numCon = Connections.Length / 2;
        //Debug.Log("connections: " + numCon);
        Sprite[] allSymbols = new Sprite[6];
        Sprite[] Pair;

        switch(numCon)
        {
            case 1:
                Pair = chooseSymbolPair(1);
                allSymbols[Connections[0]-1] = Pair[0];
                allSymbols[Connections[1]-1] = Pair[1];
                Pair = chooseRemainingSymbols(4);
                for (int j = 0; j < 4; j++)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        if (allSymbols[i] == null)
                        {
                            allSymbols[i] = Pair[j];
                            break;
                        }
                    }
                }
                break;

            case 2:
                Pair = chooseSymbolPair(1);
                //Debug.Log(Connections[0]);
                allSymbols[Connections[0]-1] = Pair[0];

                //Debug.Log(Connections[1]);
                allSymbols[Connections[1]-1] = Pair[1];

                Pair = chooseSymbolPair(2);
                //Debug.Log(Connections[2]);
                allSymbols[Connections[2]-1] = Pair[0];

                //Debug.Log(Connections[3]);
                allSymbols[Connections[3]-1] = Pair[1];

                Pair = chooseRemainingSymbols(2);
                for (int j = 0; j < 2; j++)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        if (allSymbols[i] == null)
                        {
                            //Debug.Log(Pair[j]);
                            allSymbols[i] = Pair[j];
                            break;
                        }
                    }
                }
                break;

            case 3:
                Pair = chooseSymbolPair(1);
                //Debug.Log(Connections[0]);
                allSymbols[Connections[0]-1] = Pair[0];
                //Debug.Log(Connections[1]);
                allSymbols[Connections[1]-1] = Pair[1];
                Pair = chooseSymbolPair(2);
                //Debug.Log(Connections[2]);
                allSymbols[Connections[2]-1] = Pair[0];
                //Debug.Log(Connections[3]);
                allSymbols[Connections[3]-1] = Pair[1];
                Pair = chooseSymbolPair(3);
                //Debug.Log(Connections[4]);
                allSymbols[Connections[4]-1] = Pair[0];
                //Debug.Log(Connections[5]);
                allSymbols[Connections[5]-1] = Pair[1];
                break;

            default:
                //Debug.Log("No Connection Generated!");
                break;
        }
        //foreach (Sprite s in allSymbols)
        //   //Debug.Log("SymbolName: " + s.name);

        

        SymbolsToAttach = allSymbols;
    }

    int[] swap(int[] arr, int x, int y)
    {
        int[] newArr = arr;
        
        newArr[x] = arr[y];
        newArr[y] = arr[x];

        return newArr;
    }

    Sprite[] chooseSymbolPair(int index)
    {
        Sprite[] Pair = new Sprite[2];
        Sprite[] sArray;
        switch (index)
        {
            case 1:
                sArray = symbolList.wireGroup1;
                break;
            case 2:
                sArray = symbolList.wireGroup2;
                break;
            case 3:
                sArray = symbolList.wireGroup3;
                break;
            default:
                sArray = new Sprite[0];
                //Debug.Log("index -" + index + " not found");
                break;
        }

        int a = Random.Range(0, sArray.Length);
        int b = Random.Range(0, sArray.Length);
        while (a == b)
            b = Random.Range(0, sArray.Length);

        Pair[0] = sArray[a];
        Pair[1] = sArray[b];

        return Pair;
    }

    Sprite[] chooseRemainingSymbols(int number)
    {
        ////Debug.Log("number: " + number);
        Sprite[] symbolSet = new Sprite[number];
        Sprite[] wireGroupN = symbolList.wireGroupN;

        List<int> numbers = new List< int > (6);
        for (int i = 0; i < 6; i++)
        {
            numbers.Add(i);
        }
        int[] randomNumbers = new int[number];
        for (int i = 0; i < number; i++)
        {
            int thisNumber = Random.Range(0, numbers.Count-1);
            randomNumbers[i] = numbers[thisNumber];
            numbers.RemoveAt(thisNumber);
            ////Debug.Log(randomNumbers[i] + " at " + i);
        }
        for (int i = 0; i < randomNumbers.Length; i++)
        {
            ////Debug.Log("randomNumbers: " + randomNumbers[i]);
            ////Debug.Log("wireGroupN[i]: " + wireGroupN[randomNumbers[i]]);
            symbolSet[i] = wireGroupN[randomNumbers[i]];
        }
        return symbolSet;
    }


    bool connectionsComplete()
    {
        int required = 0;
        int completed = 0;
        for(int n = 0; n < nodeGroup.Length; n +=2 )
        {
            if(nodeGroup[n].hasPartner())
            {
                required++;
                if (nodeGroup[n].Connected)
                    completed++;
            }
        }
        if (required == completed)
            return true;
        else
            return false;
    }    
}
