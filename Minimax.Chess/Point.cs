using System;
using System.Collections.Generic;
using System.Text;

namespace Minimax.Chess
{
    public struct Point
    {
        public byte File { get; set; }
        public byte Rank { get; set; }

        public Point(byte file, byte rank)
        {
            File = file;
            Rank = rank;
        }
    }
}
