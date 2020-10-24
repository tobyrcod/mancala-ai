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

    private const int extraMoveBonus = 100;

    public bool GameOver;

    internal bool IsGameOver() {
        List<int> moves = GetPossibleMoves();
        if (moves.Count == 0) {
            List<int> oppMoves = GetPossibleOpponentMoves();
            Bank oppBank = GetPlayerBank(!isPlayer1Turn);
            for (int i = 0; i < oppMoves.Count; i++) {
                oppBank.seedCount += cells[oppMoves[i]].seedCount;
                cells[oppMoves[i]].seedCount = 0;
            }

            return true;
        }

        return false;
    }

    internal int StaticEwawvaluation() {
        int bonus = 0;
        if (isPlayer1Turn == _isPlayer1Turn) {
            if (isPlayer1Turn) {
                bonus = 5;
            }
            else {
                bonus = -5;
            }
        }
        return bonus + (player1Bank.seedCount - player2Bank.seedCount);
    }

    internal List<int> GetPossibleMoves() {
        List<int> potentialMoves = GetPotentialMoves();
        List<int> possibleMoves = potentialMoves.FindAll(m => cells[m].seedCount > 0);
        return possibleMoves;
    }

    internal List<int> GetPossibleOpponentMoves() {
        List<int> potentialMoves = GetPotentialOpponentMoves();
        List<int> possibleMoves = potentialMoves.FindAll(m => cells[m].seedCount > 0);
        return possibleMoves;
    }

    private List<int> GetPotentialMoves() {
        return (isPlayer1Turn) ? player1Moves : player2Moves;
    }

    private List<int> GetPotentialOpponentMoves() {
        return (isPlayer1Turn) ? player2Moves : player1Moves;
    }

    private Bank GetPlayerBank(bool isPlayer1) {
        return (isPlayer1) ? player1Bank : player2Bank;
    }

    internal Mancala GetStateAfterMove(int possibleMove) {
        Mancala currentState = this.DeepCopy();
        currentState.ApplyMove(possibleMove);
        return currentState;
    }

    public bool isPlayer1Turn { get; private set; }
    private bool _isPlayer1Turn = false;

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
    public Mancala(List<Cell> player1CellValues, List<Cell> player2CellValues, int player1BankValue, int player2BankValue, bool isPlayer1Turn, bool _isPlayer1Turn) {
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
        this._isPlayer1Turn = _isPlayer1Turn;
    }

    public bool TryMove(int index) {

        if (CanMoveIndex(index)) {
            ApplyMove(index);
            return true;
        }

        return false;
    }

    private Mancala DeepCopy() {
        return new Mancala(cells.GetRange(0, 6), cells.GetRange(7, 6), player1Bank.seedCount, player2Bank.seedCount, isPlayer1Turn, _isPlayer1Turn);
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


        _isPlayer1Turn = isPlayer1Turn;

        int playerBankIndex = (isPlayer1Turn) ? player1Bank.index : player2Bank.index;
        if (playerBankIndex != placeIndex) {

            if (cells[placeIndex].seedCount == 1) {

                if (GetPossibleMoves().Contains(placeIndex)) {

                    int captureIndex = 12 - placeIndex;
                    if (cells[captureIndex].seedCount > 0) { 

                        int captureCount = cells[placeIndex].seedCount + cells[captureIndex].seedCount;
                        cells[placeIndex].seedCount = 0;
                        cells[captureIndex].seedCount = 0;

                        cells[playerBankIndex].seedCount += captureCount;
                    }
                }
            }

            isPlayer1Turn = !isPlayer1Turn;
        }

        GameOver = IsGameOver();
    }

    private bool CanMoveIndex(int index) {
        List<int> moves = GetPossibleMoves();

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
