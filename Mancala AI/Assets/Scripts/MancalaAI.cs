using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MancalaAI {

    public static int bestMove = -1;
    //Player 1 is the maximizing player, Player 2 is the minimizing player
    public static int Minimax(Mancala currentState, int depth, int alpha, int beta, bool maximisingPlayer) {
        if (depth == 0 || currentState.IsGameOver())
            return currentState.StaticEvaluation();

        int localBestMove = -1;

        if (maximisingPlayer) {
            int maxEval = int.MinValue;
            List<int> possibleMoves = currentState.GetPossibleMoves();
            foreach (int possibleMove in possibleMoves) {
                Mancala possibleState = currentState.GetStateAfterMove(possibleMove);
                int eval = Minimax(possibleState, depth - 1, alpha, beta, false);
                if (eval > maxEval) {
                    maxEval = eval;
                    localBestMove = possibleMove;
                }
                alpha = Math.Max(alpha, eval);
                if (beta <= alpha)
                    break;
            }

            bestMove = localBestMove;
            return maxEval;
        }
        else {
            int minEval = int.MaxValue;
            List<int> possibleMoves = currentState.GetPossibleMoves();
            if (depth == 3)
                Debug.Log(currentState.isPlayer1Turn);
            foreach (int possibleMove in possibleMoves) {
                Mancala possibleState = currentState.GetStateAfterMove(possibleMove);
                int eval = Minimax(possibleState, depth - 1, alpha, beta, true);
                if (eval < minEval) {
                    minEval = eval;
                    localBestMove = possibleMove;
                }
                beta = Math.Max(beta, eval);
                if (beta <= alpha)
                    break;
            }

            bestMove = localBestMove;
            return minEval;
        }
    }
}

