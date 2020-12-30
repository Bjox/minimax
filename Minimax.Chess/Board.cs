using System;
using System.Collections.Generic;
using System.Text;
using static Minimax.Chess.Piece;

namespace Minimax.Chess
{
    public class Board
    {
        public static Board CreateStartingPosition()
        {
            var board = new Board();
            FenParser.ApplyFenString(board, "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
            return board;
        }

        public const int FILE_A = 0;
        public const int FILE_B = 1;
        public const int FILE_C = 2;
        public const int FILE_D = 3;
        public const int FILE_E = 4;
        public const int FILE_F = 5;
        public const int FILE_G = 6;
        public const int FILE_H = 7;
        public const int RANK_1 = 0;
        public const int RANK_2 = 1;
        public const int RANK_3 = 2;
        public const int RANK_4 = 3;
        public const int RANK_5 = 4;
        public const int RANK_6 = 5;
        public const int RANK_7 = 6;
        public const int RANK_8 = 7;

        /// <summary>
        /// [file, rank], eg [A, 1] or [H, 8]
        /// </summary>
        public Piece[,] Pieces { get; } = new Piece[8, 8];

        public Color ActiveColor { get; set; }

        public Board()
        {
        }

        public Board(string fen)
        {
            FenParser.ApplyFenString(this, fen);
        }

        public Board(Board original)
        {
            for (int file = FILE_A; file <= FILE_H; file++)
            {
                for (int rank = RANK_1; rank <= RANK_8; rank++)
                {
                    Pieces[file, rank] = original.Pieces[file, rank];
                }
            }
        }

        public Piece this[int file, int rank]
        {
            get => Pieces[file, rank];
            set => Pieces[file, rank] = value;
        }

        public void Move((int file, int rank) from, (int file, int rank) to)
        {
            if (BoardExtensions.IsValidPosition(from) && BoardExtensions.IsValidPosition(to))
            {
                Pieces[to.file, to.rank] = Pieces[from.file, from.rank];
                Pieces[from.file, from.rank] = NULL;
                ActiveColor ^= Color.BLACK; // flip color
            }
            else
            {
                throw new Exception("Invalid move");
            }
        }

        public void Move(string algebraicNotation)
        {
            var fromFile = algebraicNotation[0] - 'a';
            var fromRank = algebraicNotation[1] - '1';
            var toFile = algebraicNotation[2] - 'a';
            var toRank = algebraicNotation[3] - '1';
            Move((fromFile, fromRank), (toFile, toRank));
        }

        public override string ToString()
        {
            var str = new StringBuilder();
            for (var rank = RANK_8; rank >= RANK_1; rank--)
            {
                str.Append((rank + 1).ToString());
                str.Append("   ");

                for (var file = FILE_A; file <= FILE_H; file++)
                {
                    var piece = Pieces[file, rank] switch
                    {
                        NULL => '.',
                        WHITE_PAWN => 'P',
                        WHITE_ROOK => 'R',
                        WHITE_KNIGHT => 'N',
                        WHITE_BISHOP => 'B',
                        WHITE_QUEEN => 'Q',
                        WHITE_KING => 'K',
                        BLACK_PAWN => 'p',
                        BLACK_ROOK => 'r',
                        BLACK_KNIGHT => 'n',
                        BLACK_BISHOP => 'b',
                        BLACK_QUEEN => 'q',
                        BLACK_KING => 'k',
                        _ => throw new Exception()
                    };
                    str.Append(piece);
                    str.Append(" ");
                }

                str.AppendLine();
            }
            str.AppendLine();
            str.AppendLine("    a b c d e f g h");
            str.AppendLine();
            str.AppendLine($"Active: {ActiveColor}");

            return str.ToString();
        }
    }
}
