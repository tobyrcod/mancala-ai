using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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

        if (mancala.TryMove(index)) {
            UpdateUI();

            if (!mancala.GameOver) {

                if (!mancala.isPlayer1Turn) {
                    StopCoroutine("MakeAIMove");
                    StartCoroutine(MakeAIMove(Random.Range(1f, 2f)));
                }
            }
        }
        else {
            Debug.LogError("AI Cant Move");
        }     
    }

    private IEnumerator MakeAIMove(float waitTime) {

        yield return new WaitForSeconds(waitTime);
        Debug.Log("AI MOVE");

        MancalaAI.Minimax(mancala, 6, int.MinValue, int.MaxValue, false);
        Debug.Log(MancalaAI.bestMove);
        Move(MancalaAI.bestMove);
    }

    private void UpdateUI() {

        for (int i = 0; i < 14; i++) {
            cellsUI[i].text = mancala.cells[i].seedCount.ToString();
        }

        turnUI.text = (mancala.isPlayer1Turn) ? "Player 1" : "Player 2";
    }
}
