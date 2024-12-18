using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using URandom = UnityEngine.Random;

public class IAScript : MonoBehaviour
{
    public enum Dificulty { loading, easy, medium, hard };
    public Dificulty dificulty;
    public enum Winner { none, player, ia };
    public Winner winner;

    public bool iaTurn = false;

    bool loaded = false;
    public GameObject dificultySelector, minigame;
    public List<Button> buttons = new List<Button>();
    public List<Image> buttonsColor = new List<Image>();

    int[] numbers = new int[10]{ 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
    bool[] numbersChecked = new bool[10];
    int rightIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        iaTurn = true;
        SetDificulty(Dificulty.loading);
        SetWinner(Winner.none);
        dificultySelector.SetActive(true);
        minigame.SetActive(false);
        for (int i = 0; i < 10; i++)
        {
            numbersChecked[i] = false;
            buttonsColor.Add(buttons[i].GetComponent<Image>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        StateMachine();
    }
    void StateMachine()
    {
        switch (winner)
        {
            case Winner.none:
                if (iaTurn)
                {
                    switch (dificulty)
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
                break;
            case Winner.player:
                break;
            case Winner.ia:
                break;
        }

    }
    public void SetDificulty(Dificulty dif)
    {
        dificulty = dif;
    }
    public void SetWinner(Winner win)
    {
        winner = win;
    }
    public void ClickEasy()
    {
        SetDificulty(Dificulty.easy);
        DificultySelected();
    }
    public void ClickMedium()
    {
        SetDificulty(Dificulty.medium);
        DificultySelected();
    }
    public void ClickHard()
    {
        SetDificulty(Dificulty.hard);
        DificultySelected();
    }
    public void DificultySelected()
    {
        dificultySelector.SetActive(false);
        minigame.SetActive(true);
    }
    public void UITouchable()
    {
        for(int i = 0; i < buttons.Count; i++)
        {
            buttons[i].interactable = true;
        }
    }
    public void UINotTouchable()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].interactable = false;
        }
    }
    void LoadingState()
    {
        if (!loaded)
        {
            rightIndex = URandom.Range(0, 10) ;

            loaded = true;
        }
        iaTurn = false;
    }

    public void SelectedButton(int index)
    {
        if (rightIndex == index)
        {
            buttonsColor[index].color = Color.green;
            numbersChecked[index] = true;
            SetWinner(Winner.player);
        }
        else
        {
            buttonsColor[index].color = Color.red;
            numbersChecked[index] = true;
        }
        iaTurn = true;
    }

    void EasyMode()
    {
        UINotTouchable();
        for (int i = 0; i < numbers.Length; i++)
        {
            if (!numbersChecked[i])
            {
                numbersChecked[i] = true;
                if (numbers[i] == rightIndex)
                {
                    buttonsColor[i].color = Color.green;
                    SetWinner(Winner.ia);
                }
                else
                {
                    buttonsColor[i].color = Color.red;
                }
                
                break;
            }
        }
        UITouchable();
        iaTurn = false;
    }

    void MediumMode()
    {
        UINotTouchable();
        UITouchable();
        iaTurn = false;
    }

    void HardMode()
    {
        UINotTouchable();
        UITouchable();
        iaTurn = false;
    }











}
