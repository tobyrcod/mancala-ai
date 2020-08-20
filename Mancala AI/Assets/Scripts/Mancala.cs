using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mancala
{
    public List<Cell> cells = new List<Cell>();
    public Bank player1Bank;
    public Bank player2Bank;

    public List<int> player1Moves = new List<int>();
    public List<int> player2Moves = new List<int>();

    private CellPair cellPairs = new CellPair();

    private const int extraMoveBonus = 5;

    internal bool IsGameOver() {
        List<int> moves = GetPossibleMoves();
        for (int i = 0; i < moves.Count; i++) {
            if (CanMoveIndex(moves[i]))
                return false;
        }
        return true;
    }

    internal int StaticEvaluation() {
        int bonus = (isPlayer1Turn) ? 0 : extraMoveBonus;
        return (player1Bank.seedCount - player2Bank.seedCount) + bonus;
    }

    internal List<int> GetPossibleMoves() {
        return (isPlayer1Turn) ? player1Moves : player2Moves;
    }

    internal Mancala GetStateAfterMove(int possibleMove) {
        Mancala currentState = this.DeepCopy();
        currentState.ApplyMove(possibleMove);
        return currentState;
    }

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
    public Mancala(List<Cell> player1CellValues, List<Cell> player2CellValues, int player1BankValue, int player2BankValue, bool isPlayer1Turn) {
        for (int i = 0; i < 6; i++) {
            player1Moves.Add(cells.Count);
            cells.Add(new Cell(player1CellValues[i].seedCount));
        }

        player1Bank = new Bank(6, player1BankValue);
        cells.Add(player1Bank);

        for (int i = 0; i < 6; i++) {
            player2Moves.Add(cells.Count);
            cells.Add(new Cell(player2CellValues[i].seedCount));
        }

        player2Bank = new Bank(13, player2BankValue);
        cells.Add(player2Bank);

        for (int i = 0; i < 6; i++) {
            cellPairs.pairs.Add(new Pair(12 - i, i));
        }

        this.isPlayer1Turn = isPlayer1Turn;
    }

    public bool TryMove(int index) {

        if (CanMoveIndex(index)) {
            ApplyMove(index);
            return true;
        }

        return false;
    }

    private Mancala DeepCopy() {
        return new Mancala(cells.GetRange(0, 6), cells.GetRange(7, 6), player1Bank.seedCount, player2Bank.seedCount, isPlayer1Turn);
    }

    private void ApplyMove(int clickedIndex) {
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

        int playerBankIndex = (isPlayer1Turn) ? player1Bank.index : player2Bank.index;
        if (playerBankIndex != placeIndex)
            isPlayer1Turn = !isPlayer1Turn;
    }

    private bool CanMoveIndex(int index) {
        List<int> moves = GetPossibleMoves();
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

    public Bank(int index, int seedCount = 0) : base(seedCount) {
        this.index = index;
    }
}
