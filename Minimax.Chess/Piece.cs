using System;

namespace Minimax.Chess
{
    public enum Piece : byte
    {
        NULL = 0,

        WHITE_PAWN = 1,
        WHITE_ROOK = 2,
        WHITE_KNIGHT = 3,
        WHITE_BISHOP = 4,
        WHITE_QUEEN = 5,
        WHITE_KING = 6,

        BLACK_PAWN = 7,
        BLACK_ROOK = 8,
        BLACK_KNIGHT = 9,
        BLACK_BISHOP = 10,
        BLACK_QUEEN = 11,
        BLACK_KING = 12
    }

    public enum Color : byte
    {
        WHITE = 0,
        BLACK = 6
    }

    public static class PieceExtensions
    {
        public static double Value(this Piece piece)
        {
            return piece switch
            {
                Piece.NULL => 0,
                Piece.WHITE_PAWN => 1,
                Piece.BLACK_PAWN => -1,
                Piece.WHITE_ROOK => 5,
                Piece.BLACK_ROOK => -5,
                Piece.WHITE_KNIGHT => 3,
                Piece.BLACK_KNIGHT => -3,
                Piece.WHITE_BISHOP => 3,
                Piece.BLACK_BISHOP => -3,
                Piece.WHITE_QUEEN => 9,
                Piece.BLACK_QUEEN => -9,
                Piece.WHITE_KING => 0,
                Piece.BLACK_KING => 0,
                _ => throw new System.Exception()
            };
        }

        public static Color Color(this Piece piece)
        {
            switch (piece)
            {
                case Piece.WHITE_PAWN:
                case Piece.WHITE_ROOK:
                case Piece.WHITE_KNIGHT:
                case Piece.WHITE_BISHOP:
                case Piece.WHITE_QUEEN:
                case Piece.WHITE_KING:
                    return Chess.Color.WHITE;
                case Piece.BLACK_PAWN:
                case Piece.BLACK_ROOK:
                case Piece.BLACK_KNIGHT:
                case Piece.BLACK_BISHOP:
                case Piece.BLACK_QUEEN:
                case Piece.BLACK_KING:
                    return Chess.Color.BLACK;
                case Piece.NULL:
                    throw new Exception("Cannot get color of empty square.");
            }
            throw new Exception();
        }
    }
}
