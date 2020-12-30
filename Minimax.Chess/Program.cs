using System;
using static Minimax.Chess.Piece;
using static Minimax.Chess.Color;
using static Minimax.Chess.Board;
using static Minimax.Chess.BoardExtensions.Action;
using System.Collections.Generic;
using System.Diagnostics;

namespace Minimax.Chess
{
    class Program
    {
        static void Main(string[] args)
        {
            var uci = new UCI("log.txt");
            uci.Start();
        }
    }
}
