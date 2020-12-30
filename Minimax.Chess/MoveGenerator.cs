using System;
using System.Collections.Generic;
using System.Text;
using static Minimax.Chess.Piece;
using static Minimax.Chess.Color;
using static Minimax.Chess.Board;
using static Minimax.Chess.BoardExtensions.Action;

namespace Minimax.Chess
{
    public static class MoveGenerator
    {
        public static void GeneratePossibleMoves(Board board, (int file, int rank) piecePos, List<BoardPosition> boardPositionsBuffer)
        {
            var piece = board.Pieces[piecePos.file, piecePos.rank];

            switch (piece)
            {
                case WHITE_PAWN:
                case BLACK_PAWN:
                    Pawn(board, piecePos, boardPositionsBuffer);
                    break;

                case WHITE_ROOK:
                case BLACK_ROOK:
                    Rook(board, piecePos, boardPositionsBuffer);
                    break;

                case WHITE_KNIGHT:
                case BLACK_KNIGHT:
                    Knight(board, piecePos, boardPositionsBuffer);
                    break;

                case WHITE_BISHOP:
                case BLACK_BISHOP:
                    Bishop(board, piecePos, boardPositionsBuffer);
                    break;

                case WHITE_QUEEN:
                case BLACK_QUEEN:
                    Rook(board, piecePos, boardPositionsBuffer);
                    Bishop(board, piecePos, boardPositionsBuffer);
                    break;

                case WHITE_KING:
                case BLACK_KING:
                    King(board, piecePos, boardPositionsBuffer);
                    break;

                case NULL:
                    break;
            }
        }

        private static void Rook(Board board, (int file, int rank) rookPos, List<BoardPosition> boardPositionsBuffer)
        {
            var rookColor = board.Pieces[rookPos.file, rookPos.rank].Color();
            board.IterateStraights(rookPos, (file, rank, piece) =>
            {
                if (piece != NULL && piece.Color() == rookColor)
                {
                    return NextDirection;
                }
                PerformMoveAndCreateBoardPosition(board, rookPos, (file, rank), boardPositionsBuffer);
                if (piece != NULL)
                {
                    return NextDirection;
                }
                return default;
            });
        }

        private static void Bishop(Board board, (int file, int rank) bishopPos, List<BoardPosition> boardPositionsBuffer)
        {
            var bishopColor = board.Pieces[bishopPos.file, bishopPos.rank].Color();
            board.IterateDiagonals(bishopPos, (file, rank, piece) =>
            {
                if (piece != NULL && piece.Color() == bishopColor)
                {
                    return NextDirection;
                }
                PerformMoveAndCreateBoardPosition(board, bishopPos, (file, rank), boardPositionsBuffer);
                if (piece != NULL)
                {
                    return NextDirection;
                }
                return default;
            });
        }

        private static void King(Board board, (int file, int rank) kingPos, List<BoardPosition> boardPositionsBuffer)
        {
            var kingColor = board.Pieces[kingPos.file, kingPos.rank].Color();
            board.IterateKingPositions(kingPos, (file, rank, piece) =>
            {
                if (piece != NULL && piece.Color() == kingColor)
                {
                    return default;
                }
                PerformMoveAndCreateBoardPosition(board, kingPos, (file, rank), boardPositionsBuffer);
                return default;
            });
        }

        private static void Knight(Board board, (int file, int rank) knightPos, List<BoardPosition> boardPositionsBuffer)
        {
            var knightColor = board.Pieces[knightPos.file, knightPos.rank].Color();
            board.IterateKnightPositions(knightPos, (file, rank, piece) =>
            {
                if (piece != NULL && piece.Color() == knightColor)
                {
                    return default;
                }
                PerformMoveAndCreateBoardPosition(board, knightPos, (file, rank), boardPositionsBuffer);
                return default;
            });
        }

        private static void Pawn(Board board, (int file, int rank) pawnPos, List<BoardPosition> boardPositionsBuffer)
        {
            var pawnColor = board.Pieces[pawnPos.file, pawnPos.rank].Color();
            board.IteratePawnMovementPositions(pawnPos, (file, rank, piece) =>
            {
                if (piece != NULL)
                {
                    return Break;
                }
                PerformMoveAndCreateBoardPosition(board, pawnPos, (file, rank), boardPositionsBuffer);
                return default;
            });
            board.IteratePawnCapturePositions(pawnPos, (file, rank, piece) =>
            {
                if (piece != NULL && piece.Color() != pawnColor)
                {
                    PerformMoveAndCreateBoardPosition(board, pawnPos, (file, rank), boardPositionsBuffer);
                }
                return default;
            });
        }

        private static void PerformMoveAndCreateBoardPosition(Board board, (int file, int rank) from, (int file, int rank) to, List<BoardPosition> boardPositionsBuffer)
        {
            var boardCopy = new Board(board);
            boardCopy.Move(from, to);

            if (!boardCopy.IsKingCheck(board.ActiveColor))
            {
                var newBoardPosition = new BoardPosition(boardCopy)
                {
                    From = from,
                    To = to
                };
                boardPositionsBuffer.Add(newBoardPosition);
            }
        }
    }
}
