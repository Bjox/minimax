using System;
using System.Collections.Generic;
using System.Text;
using static Minimax.Chess.Piece;
using static Minimax.Chess.Board;

namespace Minimax.Chess
{
    public static class FenParser
    {
        public static void ApplyFenString(Board board, string fen)
        {
            var fenParts = fen.Split(' ');

            var piecePositions = fenParts[0];
            var rank = RANK_8;
            var file = FILE_A;
            foreach (var ch in piecePositions)
            {
                if (ch == '/')
                {
                    rank--;
                    file = FILE_A;
                    continue;
                }
                if (int.TryParse(ch.ToString(), out var emptySquares))
                {
                    for (var i = 0; i < emptySquares; file++, i++)
                    {
                        board.Pieces[file, rank] = NULL;
                    }
                    continue;
                }
                board.Pieces[file, rank] = ch switch
                {
                    'P' => WHITE_PAWN,
                    'p' => BLACK_PAWN,
                    'R' => WHITE_ROOK,
                    'r' => BLACK_ROOK,
                    'N' => WHITE_KNIGHT,
                    'n' => BLACK_KNIGHT,
                    'B' => WHITE_BISHOP,
                    'b' => BLACK_BISHOP,
                    'Q' => WHITE_QUEEN,
                    'q' => BLACK_QUEEN,
                    'K' => WHITE_KING,
                    'k' => BLACK_KING,
                    _ => throw new ArgumentException()
                };
                file++;
            }

            var activeColor = fenParts[1].ToLower();
            if (activeColor == "w")
            {
                board.ActiveColor = Color.WHITE;
            }
            else if (activeColor == "b")
            {
                board.ActiveColor = Color.BLACK;
            }
            else
            {
                throw new ArgumentException();
            }
        }
    }
}
