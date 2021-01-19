using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderModule : Module
{
    OrderNode[] nodeGroup;
    int[] buttonOrder;
    int currentPlace = 0;

    Sprite[] symbolGroup;
    Sprite[] symbolsToPlace;

    protected override void Start()
    {
        base.Start();
        isComplete = false;
        nodeGroup = GetComponentsInChildren<OrderNode>();

        GenerateOrder();
        ChooseSymbolGroup();
        AssignSymbols();

        int i = 0;
        foreach (OrderNode n in nodeGroup)
        {
            n.setIndex(i);
            n.DrawSymbol();
            i++;
        }
    }

    protected override void Update()
    {
        base.Update();
    }

    public void RecieveButtonPress(int press)
    {
        if(press == buttonOrder[currentPlace]) // Correct Button Press
        {
            nodeGroup[press].Press();
            currentPlace++;
            checkComplete();
        }
        else
        {
            FindObjectOfType<GameManager>().Strike();
            resetSequence();
        }
    }

    void resetSequence()
    {
        currentPlace = 0;

        //Deselect all buttons
        foreach(OrderNode n in nodeGroup)
        {
            n.DeselectButton();
        }
    }

    void checkComplete()
    {
        if(currentPlace >= 4)
        {
            CompleteModule();
            machine.isMachineComplete();
            resetSequence();
        }
    }

    void GenerateOrder()
    {
        buttonOrder = new int[] { 0, 1, 2, 3 };
        for (var i = buttonOrder.Length - 1; i > 0; i--)
        {
            var r = Random.Range(0, i);
            var tmp = buttonOrder[i];
            buttonOrder[i] = buttonOrder[r];
            buttonOrder[r] = tmp;
        }
        //foreach (int i in buttonOrder)
        //    Debug.Log(i);

        Debug.Log("Button Order is: " + buttonOrder[0] + buttonOrder[1] + buttonOrder[2] + buttonOrder[3]);
    }

    void ChooseSymbolGroup()
    {
        int groupNo = Random.Range(0, 5);
        switch(groupNo) // select a random group from the 5 symbol groups to use
        {
            case 0:
                symbolGroup = symbolList.orderGroup1;
                break;
            case 1:
                symbolGroup = symbolList.orderGroup2;
                break;
            case 2:
                symbolGroup = symbolList.orderGroup3;
                break;
            case 3:
                symbolGroup = symbolList.orderGroup4;
                break;
            case 4:
                symbolGroup = symbolList.orderGroup5;
                break;
            default:
                Debug.Log("Group number not found: " + groupNo);
                symbolGroup = new Sprite[7];
                break;
        }
        Debug.Log("Symbol Group Number is: " + groupNo);
    }

    int[] createRandomUnique(int sizeIn, int sizeOut)
    {
        List<int> from = new List<int>();
        int[] numbers = new int[sizeOut];

        for(int i = sizeIn; i > 0; i--)
        {
            from.Add(i-1);
        }

        for(int i = 0; i < numbers.Length; i++)
        {
            numbers[i] = from[Random.Range(0, from.Count)];
            from.Remove(numbers[i]);
        }        

        //foreach (int i in numbers)
        //    Debug.Log(i);

        return numbers;
    }

    int[] Sort(int[] a)
    {
        List<int> list = new List<int>();
        list.AddRange(a);
        list.Sort();
        int[] arr = list.ToArray();        

        return arr;
    }

    void AssignSymbols()
    {
        // Choose the 4 symbols from the group
        int[] numbersToChoose = createRandomUnique(7, 4);
        numbersToChoose = Sort(numbersToChoose);
        Debug.Log("Symbol Order is: " + numbersToChoose[0] + numbersToChoose[1] + numbersToChoose[2] + numbersToChoose[3]);
        symbolsToPlace = new Sprite[4];
        for(int i = 0; i < 4; i++)
        {
            symbolsToPlace[i] = symbolGroup[numbersToChoose[i]];
            nodeGroup[buttonOrder[i]].AssignSymbol(symbolsToPlace[i]);
        }
    }
}
