using Minimax.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using static Minimax.Chess.Piece;
using static Minimax.Chess.Color;
using static Minimax.Chess.Board;

namespace Minimax.Chess
{
    public class BoardPosition : IPosition
    {
        public Board Board { get; set; }
        public (int file, int rank) From { get; set; }
        public (int file, int rank) To { get; set; }
        public bool GameOver { get; set; }

        private List<BoardPosition> ChildPositions { get; set; }

        public BoardPosition(Board board)
        {
            Board = board;
        }

        public void Init()
        {
            ChildPositions = new List<BoardPosition>(100);

            for (int file = FILE_A; file <= FILE_H; file++)
            {
                for (int rank = RANK_1; rank <= RANK_8; rank++)
                {
                    var piece = Board.Pieces[file, rank];
                    if (piece == NULL || piece.Color() != Board.ActiveColor)
                    {
                        continue;
                    }
                    MoveGenerator.GeneratePossibleMoves(Board, (file, rank), ChildPositions);
                }
            }

            GameOver = ChildPositions.Count == 0 && Board.IsKingCheck(Board.ActiveColor);
        }

        public double Evaluate()
        {
            if (GameOver)
            {
                return Board.ActiveColor == WHITE ? double.NegativeInfinity : double.PositiveInfinity;
            }

            double total = 0;
            foreach (var piece in Board.Pieces)
            {
                total += piece.Value();
            }
            return total;
        }

        public IEnumerable<IPosition> GenerateChildPositions()
        {
            if (ChildPositions == null)
            {
                Init();
            }
            return ChildPositions;
        }

        public override string ToString()
        {
            var str = new StringBuilder();
            str.AppendLine($"'{BoardExtensions.ToNotation(From)}' to '{BoardExtensions.ToNotation(To)}':");
            str.AppendLine(Board.ToString());
            return str.ToString();
        }
    }
}
