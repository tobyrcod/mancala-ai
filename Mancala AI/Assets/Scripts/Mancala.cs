using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mancala : MonoBehaviour
{
    public List<Cell> cells = new List<Cell>();
    public Bank player1Bank;
    public Bank player2Bank;

    private List<int> player1Moves = new List<int>();
    private List<int> player2Moves = new List<int>();

    private CellPair cellPairs = new CellPair();

    public bool isPlayer1Turn { get; private set; }

    public Mancala() {
        for (int i = 0; i < 6; i++) {
            player1Moves.Add(cells.Count);
            cells.Add(new Cell(4));
        }

        player1Bank = new Bank(6);
        cells.Add(player1Bank);

        for (int i = 0; i < 6; i++) {
            player2Moves.Add(cells.Count);
            cells.Add(new Cell(4));
        }

        player2Bank = new Bank(13);
        cells.Add(player2Bank);

        for (int i = 0; i < 6; i++) {
            cellPairs.pairs.Add(new Pair(12 - i, i));
        }

        isPlayer1Turn = true;
    }

    public bool Move(int clickedIndex) {

        if (CanMoveIndex(clickedIndex)) {

            Debug.Log(clickedIndex);

            int seedCount = cells[clickedIndex].seedCount;
            cells[clickedIndex].seedCount = 0;

            int placedSeeds = 0;
            int placeIndex = clickedIndex;

            do {

                placeIndex++;
                placeIndex %= cells.Count;

                int enemyBankIndex = (isPlayer1Turn) ? player2Bank.index : player1Bank.index;
                if (placeIndex == enemyBankIndex) {
                    placeIndex++;
                    placeIndex %= cells.Count;
                }

                cells[placeIndex].seedCount++;
                placedSeeds++;

            } while (placedSeeds < seedCount);

            isPlayer1Turn = !isPlayer1Turn;
            return true;
        }

        return false;
    }

    private bool CanMoveIndex(int index) {
        List<int> moves = (isPlayer1Turn) ? player1Moves : player2Moves;
        if (cells[index].seedCount == 0)
            return false;

        if (!moves.Contains(index))
            return false;

        return true;
    }
}

public class CellPair {
    public List<Pair> pairs;

    public CellPair() {
        this.pairs = new List<Pair>();
    }
}

public class Pair {
    public int TopCellIndex;
    public int BottomCellIndex;

    public Pair(int topCellIndex, int bottomCellIndex) {
        this.TopCellIndex = topCellIndex;
        this.BottomCellIndex = bottomCellIndex;
    }
}

public class Cell {
    public int seedCount;

    public Cell(int seedCount) {
        this.seedCount = seedCount;
    }
}

public class Bank : Cell {
    public int index;

    public Bank(int index) : base(0) {
        this.index = index;
    }
}
