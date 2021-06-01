using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MinesweeperForms
{
    class MinesweeperModel
    {
        private int Size { get; }
        private int MineCount { get; }
        private Tile[,] _board;
        //This list will allow me to use LINQ for easy placement of the mines
        private List<Tile> _tileList;

        #region Constructor
        public MinesweeperModel(int size, int mineCount)
        {
            Size = size;
            MineCount = mineCount;
            InitializeBoard();
            PlaceMines();
            SetNeighborCounts();
        }

        private void InitializeBoard()
        {
            _board = new Tile[Size, Size];
            _tileList = new List<Tile>();

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    _board[i, j] = new Tile();
                    _tileList.Add(_board[i, j]);
                }
            }
        }

        private void PlaceMines()
        {
            Random rand = new Random();
            var randomList = _tileList.OrderBy(tile => rand.Next());
            var mines = randomList.Take(MineCount).ToList();
            mines.ForEach(tile => tile.IsMine = true);
        }

        private void SetNeighborCounts()
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    SetNeighborCount(i, j);
                }
            }
        }

        private void SetNeighborCount(int row, int col)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {

                    if (row + i < 0 || row + i >= Size)
                    {
                        continue;
                    }

                    if (col + j < 0 || col + j >= Size)
                    {
                        continue;
                    }

                    int rowCheck = row + i,
                        colCheck = col + j;

                    if (_board[rowCheck,colCheck].IsMine)
                    {
                        _board[row, col].AdjacentMines++;
                    }
                }
            }
        }
        #endregion

        public Tile GetTile(int row, int col)
        {
            return _board[row, col];
        }
    }
}
