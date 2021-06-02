using System;
using System.Collections.Generic;
using System.Text;

namespace MinesweeperForms
{
    class Tile
    {
        public int AdjacentMines { get; set; }
        public bool IsMine { get; set; }
        public bool IsFlagged { get; set; }
        public bool IsRevealed { get; set; }

        public Tile()
        {
            AdjacentMines = 0;
            IsMine = false;
            IsFlagged = false;
            IsRevealed = false;
        }
    }
}
