using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MancalaUI : MonoBehaviour
{
    Mancala mancala = new Mancala();

    [SerializeField] private List<TextMeshProUGUI> cellsUI = new List<TextMeshProUGUI>();

    [Space]

    [SerializeField] private TextMeshProUGUI turnUI;

    // Start is called before the first frame update
    void Start()
    {
       UpdateUI();
    }

    public void Move(int index) {
        if (mancala.Move(index)) {
            UpdateUI();
        }
    }

    private void UpdateUI() {

        for (int i = 0; i < 14; i++) {
            cellsUI[i].text = mancala.cells[i].seedCount.ToString();
        }

        turnUI.text = (mancala.isPlayer1Turn) ? "Player 1" : "Player 2";
    }
}
