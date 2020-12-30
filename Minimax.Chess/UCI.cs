using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace Minimax.Chess
{
    public class UCI
    {
        private readonly string _logfile;
        private Board _board;

        public UCI(string logfile = null)
        {
            _logfile = logfile;
            WriteToLog("-- UCI LOG --");
        }

        public void Start()
        {
            var run = true;
            while (run)
            {
                var line = GetLine();
                var parts = line.Split(' ');
                var command = parts[0];

                switch (command)
                {
                    case "uci":
                        SendLine("id name Minimax.Chess");
                        SendLine("id author Bjørnar W. Alvestad");
                        SendLine("uciok");
                        break;

                    case "quit":
                        run = false;
                        break;

                    case "isready":
                        SendLine("readyok");
                        break;

                    case "ucinewgame":
                        _board = null;
                        break;

                    case "position":
                        if (parts[1] == "startpos")
                        {
                            _board = Board.CreateStartingPosition();
                        }
                        else if (parts[1] == "fen")
                        {
                            _board = new Board(parts[2]);
                        }
                        foreach (var move in parts.SkipWhile(p => p != "moves").Skip(1))
                        {
                            _board.Move(move);
                        }
                        break;

                    case "go":
                        WriteToLog("Starting search on position:");
                        WriteToLog(_board.ToString());

                        var boardPosition = new BoardPosition(_board);
                        var minimaxResult = Core.Minimax.MinimaxAlphaBeta(
                            boardPosition,
                            depth: 5,
                            _board.ActiveColor == Color.WHITE ? Core.Minimax.Target.Maximize : Core.Minimax.Target.Minimize);

                        var bestmove = minimaxResult.SelectedPosition as BoardPosition;
                        _board.Move(bestmove.From, bestmove.To);

                        var bestmoveString = $"bestmove {BoardExtensions.ToNotation(bestmove.From)}{BoardExtensions.ToNotation(bestmove.To)}";
                        SendLine(bestmoveString);
                        break;

                }
            }
        }

        private string GetLine()
        {
            var line = Console.ReadLine();
            WriteToLog("GUI\t: " + line);
            return line;
        }

        private void SendLine(string command)
        {
            WriteToLog("Engine\t: " + command);
            Console.WriteLine(command);
        }

        private void WriteToLog(string line)
        {
            if (_logfile != null)
            {
                File.AppendAllLines(_logfile, new[] { line });
            }
        }
    }
}
