using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using URandom = UnityEngine.Random;

public class IAScript : MonoBehaviour
{
    public enum Dificulty { loading, easy, medium, hard };
    public Dificulty dificulty;
    public enum Winner { none, player, ia };
    public Winner winner;

    public bool iaTurn = false;

    bool loaded = false;
    public GameObject dificultySelector, minigame, buttonRevive, buttonMainMenu;
    public List<Button> buttons = new List<Button>();
    public List<Image> buttonsColor = new List<Image>();

    int[] numbers = new int[10]{ 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
    public bool[] numbersChecked = new bool[10];
    public int rightIndex = 0, minNum = 0,
        maxNum = 9, centerNum = 0, marcadorMin = 0, marcadorMax = 0;

    // Start is called before the first frame update
    void Start()
    {
        iaTurn = true;
        SetDificulty(Dificulty.loading);
        SetWinner(Winner.none);
        dificultySelector.SetActive(true);
        minigame.SetActive(false);
        buttonRevive.SetActive(false);
        buttonMainMenu.SetActive(false);
        for (int i = 0; i < 10; i++)
        {
            numbersChecked[i] = false;
            buttonsColor.Add(buttons[i].GetComponent<Image>());
            buttonsColor[i].color = Color.white;
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
                UINotTouchable();
                buttonRevive.SetActive(true);
                break;
            case Winner.ia:
                UINotTouchable();
                buttonMainMenu.SetActive(true);
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
        for (int i = 0; i < buttons.Count; i++)
        {
            if (!numbersChecked[i])
            {
                buttons[i].interactable = true;
            }
        }
    }
    public void UINotTouchable()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].interactable = false;
        }
    }

    void OnWin()
    {
        SceneManager.LoadScene("Runner");
    }
    void OnLose()
    {
        SceneManager.LoadScene("MainMenu");
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
            UINotTouchable();
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

        centerNum = (int)( maxNum + minNum ) / 2;
        // Este while es para prevenir que el codigo que bloqueé
        // si la ia intenta comprobar una casilla que ya esté
        // comprobada por mi
        while (numbersChecked[centerNum])
        {
            if (centerNum < rightIndex)
            {
                centerNum++;
            }
            if (centerNum > rightIndex)
            {
                centerNum--;
            }
        }


        numbersChecked[centerNum] = true;
        if (centerNum < rightIndex)
        {
            minNum = centerNum + 1;
            buttonsColor[centerNum].color = Color.red;
        }
        else if (centerNum > rightIndex)
        {
            maxNum = centerNum - 1;
            buttonsColor[centerNum].color = Color.red;
        }
        else
        {
            buttonsColor[centerNum].color = Color.green;
            SetWinner(Winner.ia);
        }
        UITouchable();
        iaTurn = false;
    }

    void HardMode()
    {
        UINotTouchable();

        if (minNum < maxNum)
        {
            marcadorMin = minNum ( maxNum - minNum );
        }



        UITouchable();
        iaTurn = false;
    }











}
