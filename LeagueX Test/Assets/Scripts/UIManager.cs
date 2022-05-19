using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public Text gameOverText;
    public Text gameLoseWinDataText;
    public static UIManager instance;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        
    }
    int numberOfTimeLost = 0;
    int numberOfTimeWon = 0;
    public void DisplayPlayerNames()
    {
        gameLoseWinDataText.text = GameManager.instance.myPlayerName + "(" + numberOfTimeWon + ")" + "    Vs    " + GameManager.instance.otherPlayerName+"("+0+")";
    }
    public void GameOverPanel(bool _winOrLose)
    {
        string displayText = "";
        if (_winOrLose)
        {
            displayText = "You win";
            numberOfTimeWon++;
        }
        else
        {
            displayText = "You lost";
            numberOfTimeLost++;
        }
        gameOverText.text = displayText;
        print(displayText +" " + numberOfTimeWon);
        gameOverPanel.SetActive(true);

        gameLoseWinDataText.text = GameManager.instance.myPlayerName + "(" + numberOfTimeWon + ")" + "    Vs    " + GameManager.instance.otherPlayerName+"("+numberOfTimeLost+")";    
    }

    public Text statusText;
    public void StatusText(string message)
    {
        statusText.text = message;
    }
}
