using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceModule : Module
{
    SequenceNode[] nodeGroup;

    int StateNumber = 0;
    public int sequenceLength = 5;
    int[] Sequence;
    Orientation STA;
    Sprite OrientationSymbol;
    public Vector3 OSymbolLocation;

    int seqNumber = 1;
    int countNumber = 0;

    bool isWaiting = false;
    public float waitDelay;

    GameObject symbolObject;

    void Awake()
    {
        symbolObject = new GameObject();
        symbolObject.AddComponent<SpriteRenderer>();
    }


    protected override void Start()
    {
        base.Start();
        isComplete = false;
        nodeGroup = GetComponentsInChildren<SequenceNode>();

        ChooseSymbolSet();
        SelectState();
        CreateSequence();
        AttachSymbolsToNodes();

        int i = 0;
        foreach (SequenceNode n in nodeGroup)
        {
            n.setIndex(i);
            n.DrawSymbol();
            i++;
            n.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    protected override void Update()
    {
        base.Update();

        if(!isComplete)
            StartCoroutine(ExecuteAfterTime(waitDelay));
    }

    protected override void SelectItem()
    {
        base.SelectItem();

        foreach (SequenceNode n in nodeGroup)
        {
            n.GetComponent<BoxCollider2D>().enabled = true;
        }
    }

    protected override void DeselectItem()
    {
        base.DeselectItem();

        foreach (SequenceNode n in nodeGroup)
        {
            n.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
    IEnumerator ExecuteAfterTime(float time)
    {
        if (isWaiting)
            yield break;

        isWaiting = true;

        yield return new WaitForSeconds(time);

        SendSequence();

        isWaiting = false;
    }

    void SendSequence()
    {
        if(!isComplete)
        {
            for (int i = 0; i < seqNumber; i++)
            {
                nodeGroup[Sequence[i]].Glow();
            }
        }        
    }

    public void RecieveButtonPress(SequenceNode n)
    {        
        int press = ToCorrect(n.getIndex());

        if(press == Sequence[countNumber]) // If the button pressed is the correct one
        {
            //Debug.Log("CORRECT!");
            // Increase number of whic the button should be pressed
            countNumber++;
            // Check if the Module has been completed and if it hasnt then move onto the next number in the sequence
            checkComplete();    
        }

        else
        {
            //Debug.Log("INCORRECT!");
            FindObjectOfType<GameManager>().Strike();
            resetSequence();
        }
    }

    void checkComplete()
    {
        if(seqNumber == sequenceLength && countNumber == seqNumber)
        {
            CompleteModule();
            machine.isMachineComplete();
        }

        if(countNumber == seqNumber)
        {
            //Debug.Log("Sequence Complete!");
            seqNumber++;
            countNumber = 0;
        }
    }

    private void resetSequence()
    {
        seqNumber = 1;
        countNumber = 0;
    }

    void SelectState()
    {
        StateNumber = Random.Range(1, 6);
        OrientationSymbol = symbolList.sequenceOrientation[StateNumber];
        if (StateNumber > 3)
            StateNumber -= 2;

        //print("state number: " + s);
        //switch (s)
        //{
        //    case 0:
        //        {
        //            State = "ST";
        //            OrientationSymbol = symbolList.sequenceOrientation[0];
        //            break;
        //        }            
        //    case 1:
        //        {
        //            State = "OP";
        //            OrientationSymbol = symbolList.sequenceOrientation[1];
        //            break;
        //        }
        //    case 2:
        //        {
        //            State = "CW";
        //            int r = Random.Range(2, 4);
        //            OrientationSymbol = symbolList.sequenceOrientation[r];
        //            break;
        //        }
        //    case 3:
        //        {
        //            State = "AC";
        //            int r = Random.Range(4, 6);
        //            OrientationSymbol = symbolList.sequenceOrientation[r];
        //            break;
        //        }            
        //    default:
        //        {
        //            State = "Default";
        //            break;
        //        }
        //}
        Debug.Log("State is: " + StateNumber);
        // Creating and Aligning the orientation Symbol to the top corner of the module
        symbolObject.transform.parent = this.transform;
        symbolObject.transform.localScale = new Vector3(0.012f, 0.012f, 1f);
        symbolObject.transform.localPosition = OSymbolLocation;
        SpriteRenderer sr = symbolObject.GetComponent<SpriteRenderer>();
        sr.sprite = OrientationSymbol;
        sr.sortingLayerName = "symbols";
    }

    int ToCorrect(int n)
    {
        n += StateNumber;

        //switch(State)
        //{
        //    case "CW":
        //        {
        //            n += 3;
        //            break;
        //        }
        //    case "OP":
        //        {
        //            n += 2;
        //            break;
        //        }
        //    case "AC":
        //        {
        //            n += 1;
        //            break;
        //        }
        //    default:
        //        {
        //            Debug.Log("Error! Couldn't Find State: " + State);
        //            break;
        //        }
        //}

        if (n > 3)
            n -= 4;
        if (n < 0)
            n += 4;

        return n;
    }

    void CreateSequence()
    {
        Sequence = new int[sequenceLength];
        for(int i = 0; i < sequenceLength; i++)
        {
            int n = Random.Range(0, 4);
            Sequence[i] = n;
            //Debug.Log("Sequence number " + i + " is " + n);
        }
    }

    void AttachSymbolsToNodes()
    {
        STA.Align(StateNumber);

        nodeGroup[0].AssignSymbol(STA.North);
        nodeGroup[1].AssignSymbol(STA.East);
        nodeGroup[2].AssignSymbol(STA.South);
        nodeGroup[3].AssignSymbol(STA.West);
    }

    void ChooseSymbolSet()
    {
        int num = Random.Range(0, 5);
        Sprite[] SymbolsToAttach;
        switch (num)
        {
            case 0:
                SymbolsToAttach = symbolList.sequenceGroup1;
                break;
            case 1:
                SymbolsToAttach = symbolList.sequenceGroup2;
                break;
            case 2:
                SymbolsToAttach = symbolList.sequenceGroup3;
                break;
            case 3:
                SymbolsToAttach = symbolList.sequenceGroup4;
                break;
            case 4:
                SymbolsToAttach = symbolList.sequenceGroup5;
                break;
            default:
                SymbolsToAttach = symbolList.sequenceGroup1;
                Debug.Log("Number WRONG " + num);
                break;
        }
        STA = new Orientation(SymbolsToAttach[0], SymbolsToAttach[1], SymbolsToAttach[2], SymbolsToAttach[3]);
    }

    struct Orientation
    {
        public Sprite North;
        public Sprite East;
        public Sprite South;
        public Sprite West;

        public Orientation(Sprite n, Sprite e, Sprite s, Sprite w)
        {
            North = n;
            East = e;
            South = s;
            West = w;
        }

        public void Align(int state)
        {
            Shift(state);

            //switch (state)
            //{
            //    case "ST":
            //        break;
            //    case "AC":
            //        Shift(1);
            //        break;
            //    case "OP":
            //        Shift(2);
            //        break;
            //    case "CW":
            //        Shift(3);
            //        break;
            //    default:
            //        Debug.Log("Invalid State " + state);
            //        break;
            //}
        }

        void Shift(int n)
        {
            if (n != 0)
            {
                for (int i = 0; i < n; i++)
                {
                    Sprite temp = West;
                    West = South;
                    South = East;
                    East = North;
                    North = temp;
                }
            }
        }
    }
}
