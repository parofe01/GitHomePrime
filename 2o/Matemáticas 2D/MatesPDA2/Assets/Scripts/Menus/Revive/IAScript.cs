using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using URandom = UnityEngine.Random;

public class IAScript : MonoBehaviour
{
    public enum Dificulty { loading, easy, medium, hard };
    public Dificulty dificulty;
    bool loaded = false;

    int[] numbers = new int[10];
    int rightNumber = 0;
    int rightIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        dificulty = Dificulty.loading;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void StateMachine()
    {
        switch(dificulty)
        {
            case Dificulty.loading:
                LoadingState();
                break;
            case Dificulty.easy:
                EasyMode();
                break;
            case Dificulty.medium:
                MediumMode();
                break;
            case Dificulty.hard:
                HardMode();
                break;
        }
    }
    void LoadingState()
    {
        if (!loaded)
        {
            for (int i = 0; i < numbers.Length; i++)
            {
                numbers[i] = URandom.Range(0, 100);
            }
            Array.Sort(numbers);

            rightIndex = URandom.Range(0, 9) ;
            rightNumber = numbers[rightIndex];

            loaded = true;
        }
    }

    void EasyMode()
    {
        
    }

    void MediumMode()
    {

    }

    void HardMode()
    {

    }











}
