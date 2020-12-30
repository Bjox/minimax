using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Minimax.Chess.Piece;
using static Minimax.Chess.Color;
using static Minimax.Chess.Board;

namespace Minimax.Chess
{
    public static class BoardExtensions
    {
        public static bool IsKingCheck(this Board board, Color kingColor)
        {
            var kingPosition = board.FindFirstPiece(WHITE_KING + (byte)kingColor);
            if (kingPosition == (-1, -1))
            {
                return false;
            }

            var isCheck = false;

            IterateStraights(board, kingPosition, (file, rank, piece) =>
            {
                if (piece == NULL)
                {
                    return default;
                }
                if (piece.Color() == kingColor)
                {
                    return Action.NextDirection;
                }
                if (piece == WHITE_ROOK || piece == BLACK_ROOK || piece == WHITE_QUEEN || piece == BLACK_QUEEN)
                {
                    isCheck = true;
                    return Action.Break;
                }
                return Action.NextDirection;
            });
            if (isCheck)
            {
                return true;
            }

            IterateDiagonals(board, kingPosition, (file, rank, piece) =>
            {
                if (piece == NULL)
                {
                    return default;
                }
                if (piece.Color() == kingColor)
                {
                    return Action.NextDirection;
                }
                if (piece == WHITE_BISHOP || piece == BLACK_BISHOP || piece == WHITE_QUEEN || piece == BLACK_QUEEN)
                {
                    isCheck = true;
                    return Action.Break;
                }
                return Action.NextDirection;
            });
            if (isCheck)
            {
                return true;
            }

            IterateKnightPositions(board, kingPosition, (file, rank, piece) =>
            {
                if (piece == NULL)
                {
                    return default;
                }
                if (piece.Color() == kingColor)
                {
                    return Action.NextDirection;
                }
                if (piece == WHITE_KNIGHT || piece == BLACK_KNIGHT)
                {
                    isCheck = true;
                    return Action.Break;
                }
                return Action.NextDirection;
            });
            if (isCheck)
            {
                return true;
            }

            IterateKingPositions(board, kingPosition, (file, rank, piece) =>
            {
                if (piece == NULL)
                {
                    return default;
                }
                if (piece.Color() == kingColor)
                {
                    return Action.NextDirection;
                }
                if (piece == WHITE_KING || piece == BLACK_KING)
                {
                    isCheck = true;
                    return Action.Break;
                }
                return Action.NextDirection;
            });
            if (isCheck)
            {
                return true;
            }

            // Pawn
            (int file, int rank) checkPos;
            if (kingColor == WHITE)
            {
                checkPos = (kingPosition.file - 1, kingPosition.rank + 1);
                if (IsValidPosition(checkPos) && board[checkPos.file, checkPos.rank] == BLACK_PAWN)
                {
                    return true;
                }
                checkPos = (kingPosition.file + 1, kingPosition.rank + 1);
                if (IsValidPosition(checkPos) && board[checkPos.file, checkPos.rank] == BLACK_PAWN)
                {
                    return true;
                }
            }
            else // kingColor == BLACK
            {
                checkPos = (kingPosition.file - 1, kingPosition.rank - 1);
                if (IsValidPosition(checkPos) && board[checkPos.file, checkPos.rank] == WHITE_PAWN)
                {
                    return true;
                }
                checkPos = (kingPosition.file + 1, kingPosition.rank - 1);
                if (IsValidPosition(checkPos) && board[checkPos.file, checkPos.rank] == WHITE_PAWN)
                {
                    return true;
                }
            }

            return false;
        }

        public static (int file, int rank) FindFirstPiece(this Board board, Piece piece)
        {
            for (int file = FILE_A; file <= FILE_H; file++)
            {
                for (int rank = RANK_1; rank <= RANK_8; rank++)
                {
                    if (board.Pieces[file, rank] == piece)
                    {
                        return (file, rank);
                    }
                }
            }
            return (-1, -1);
        }

        public static void IterateStraights(this Board board, (int file, int rank) origin, Func<int, int, Piece, Action> action)
        {
            (int file, int rank) checkPos;

            // north
            for (checkPos = (origin.file, origin.rank + 1); IsValidPosition(checkPos); checkPos.rank++)
            {
                var actionResult = action(checkPos.file, checkPos.rank, board.Pieces[checkPos.file, checkPos.rank]);
                if (actionResult == Action.NextDirection) break;
                if (actionResult == Action.Break) return;
            }

            // east
            for (checkPos = (origin.file + 1, origin.rank); IsValidPosition(checkPos); checkPos.file++)
            {
                var actionResult = action(checkPos.file, checkPos.rank, board.Pieces[checkPos.file, checkPos.rank]);
                if (actionResult == Action.NextDirection) break;
                if (actionResult == Action.Break) return;
            }

            // south
            for (checkPos = (origin.file, origin.rank - 1); IsValidPosition(checkPos); checkPos.rank--)
            {
                var actionResult = action(checkPos.file, checkPos.rank, board.Pieces[checkPos.file, checkPos.rank]);
                if (actionResult == Action.NextDirection) break;
                if (actionResult == Action.Break) return;
            }

            // west
            for (checkPos = (origin.file - 1, origin.rank); IsValidPosition(checkPos); checkPos.file--)
            {
                var actionResult = action(checkPos.file, checkPos.rank, board.Pieces[checkPos.file, checkPos.rank]);
                if (actionResult == Action.NextDirection) break;
                if (actionResult == Action.Break) return;
            }
        }

        public static void IterateDiagonals(this Board board, (int file, int rank) origin, Func<int, int, Piece, Action> action)
        {
            (int file, int rank) checkPos;

            // north-east
            for (checkPos = (origin.file + 1, origin.rank + 1); IsValidPosition(checkPos); checkPos.file++, checkPos.rank++)
            {
                var actionResult = action(checkPos.file, checkPos.rank, board.Pieces[checkPos.file, checkPos.rank]);
                if (actionResult == Action.NextDirection) break;
                if (actionResult == Action.Break) return;
            }

            // south-east
            for (checkPos = (origin.file + 1, origin.rank - 1); IsValidPosition(checkPos); checkPos.file++, checkPos.rank--)
            {
                var actionResult = action(checkPos.file, checkPos.rank, board.Pieces[checkPos.file, checkPos.rank]);
                if (actionResult == Action.NextDirection) break;
                if (actionResult == Action.Break) return;
            }

            // south-west
            for (checkPos = (origin.file - 1, origin.rank - 1); IsValidPosition(checkPos); checkPos.file--, checkPos.rank--)
            {
                var actionResult = action(checkPos.file, checkPos.rank, board.Pieces[checkPos.file, checkPos.rank]);
                if (actionResult == Action.NextDirection) break;
                if (actionResult == Action.Break) return;
            }

            // north-west
            for (checkPos = (origin.file - 1, origin.rank + 1); IsValidPosition(checkPos); checkPos.file--, checkPos.rank++)
            {
                var actionResult = action(checkPos.file, checkPos.rank, board.Pieces[checkPos.file, checkPos.rank]);
                if (actionResult == Action.NextDirection) break;
                if (actionResult == Action.Break) return;
            }
        }

        public static void IterateKnightPositions(this Board board, (int file, int rank) origin, Func<int, int, Piece, Action> action)
        {
            Action DoCheck((int file, int rank) checkPos)
            {
                if (IsValidPosition(checkPos))
                {
                    return action(checkPos.file, checkPos.rank, board.Pieces[checkPos.file, checkPos.rank]);
                }
                return default;
            }

            // north
            if (DoCheck((origin.file - 1, origin.rank + 2)) == Action.Break) return;
            if (DoCheck((origin.file + 1, origin.rank + 2)) == Action.Break) return;

            // east
            if (DoCheck((origin.file + 2, origin.rank + 1)) == Action.Break) return;
            if (DoCheck((origin.file + 2, origin.rank - 1)) == Action.Break) return;

            // south
            if (DoCheck((origin.file - 1, origin.rank - 2)) == Action.Break) return;
            if (DoCheck((origin.file + 1, origin.rank - 2)) == Action.Break) return;

            // west
            if (DoCheck((origin.file - 2, origin.rank + 1)) == Action.Break) return;
            if (DoCheck((origin.file - 2, origin.rank - 1)) == Action.Break) return;
        }

        public static void IterateKingPositions(this Board board, (int file, int rank) origin, Func<int, int, Piece, Action> action)
        {
            Action DoCheck((int file, int rank) checkPos)
            {
                if (IsValidPosition(checkPos))
                {
                    return action(checkPos.file, checkPos.rank, board.Pieces[checkPos.file, checkPos.rank]);
                }
                return default;
            }

            if (DoCheck((origin.file + 1, origin.rank + 1)) == Action.Break) return;
            if (DoCheck((origin.file + 1, origin.rank)) == Action.Break) return;
            if (DoCheck((origin.file + 1, origin.rank - 1)) == Action.Break) return;
            if (DoCheck((origin.file, origin.rank - 1)) == Action.Break) return;
            if (DoCheck((origin.file - 1, origin.rank - 1)) == Action.Break) return;
            if (DoCheck((origin.file - 1, origin.rank)) == Action.Break) return;
            if (DoCheck((origin.file - 1, origin.rank + 1)) == Action.Break) return;
            if (DoCheck((origin.file, origin.rank + 1)) == Action.Break) return;
        }

        public static void IteratePawnMovementPositions(this Board board, (int file, int rank) origin, Func<int, int, Piece, Action> action)
        {
            Action DoCheck((int file, int rank) checkPos)
            {
                if (IsValidPosition(checkPos))
                {
                    return action(checkPos.file, checkPos.rank, board.Pieces[checkPos.file, checkPos.rank]);
                }
                return default;
            }

            var pawnColor = board.Pieces[origin.file, origin.rank].Color();
            var rankMovement = pawnColor == WHITE ? 1 : -1;

            if (DoCheck((origin.file, origin.rank + rankMovement)) == Action.Break) return;
            if ((pawnColor == WHITE && origin.rank == RANK_2) || (pawnColor == BLACK && origin.rank == RANK_7))
            {
                DoCheck((origin.file, origin.rank + rankMovement * 2));
            }
        }

        public static void IteratePawnCapturePositions(this Board board, (int file, int rank) origin, Func<int, int, Piece, Action> action)
        {
            Action DoCheck((int file, int rank) checkPos)
            {
                if (IsValidPosition(checkPos))
                {
                    return action(checkPos.file, checkPos.rank, board.Pieces[checkPos.file, checkPos.rank]);
                }
                return default;
            }

            var pawnColor = board.Pieces[origin.file, origin.rank].Color();
            var rankMovement = pawnColor == WHITE ? 1 : -1;

            if (DoCheck((origin.file + 1, origin.rank + rankMovement)) == Action.Break) return;
            DoCheck((origin.file - 1, origin.rank + rankMovement));
        }

        public static bool IsValidPosition((int file, int rank) pos)
        {
            return
                pos.file >= FILE_A &&
                pos.file <= FILE_H &&
                pos.rank >= RANK_1 &&
                pos.rank <= RANK_8;
        }

        public static string ToNotation((int file, int rank) pos)
        {
            return $"{(char)('a' + pos.file)}{pos.rank + 1}";
        }

        public enum Action : byte
        {
            Iterate = 0,
            NextDirection = 1,
            Break = 2
        }
    }
}
