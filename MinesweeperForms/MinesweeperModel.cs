using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MinesweeperForms
{
    class MinesweeperModel
    {
        public int Size { get; }
        public int MineCount { get; }
        public bool IsLost { get; private set; }
        public bool IsWon { get; private set; }

        private Tile[,] _board;
        private List<Tile> _tileList;   //This list will allow me to use LINQ for easy placement of the mines
        private bool _firstClick;
        
        public MinesweeperModel(int size, int mineCount)
        {
            Size = size;
            MineCount = mineCount;
            IsLost = false;
            IsWon = false;
            InitializeBoard();
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

        public Tile GetTile(int row, int col)
        {
            return _board[row, col];
        }

        public void Flag(int row, int col)
        {
            _board[row, col].IsFlagged = !_board[row, col].IsFlagged;
        }

        public void ClickTile(int row, int col)
        {
            if (!_firstClick)
            {
                PlaceMines(row, col);
                SetNeighborCounts();
                _firstClick = true;
            }

            Tile clicked = GetTile(row, col);

            if (clicked.IsRevealed || clicked.IsFlagged)
                return;

            clicked.IsRevealed = true;

            if (clicked.IsMine)
            {
                IsLost = true;
                return;
            }

            if (clicked.AdjacentMines == 0)
            {
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        int rowCheck = row + i, colCheck = col + j;
                        if (CheckRowBounds(rowCheck) && CheckColumnBounds(colCheck))
                        {
                            ClickTile(row + i, col + j);
                        }
                    }
                }
            }

            CheckWinner();
        }

        private void PlaceMines(int row, int col)
        {
            var neighborList = GetNeighborTiles(row, col).ToList();
            _tileList.RemoveAll(x => neighborList.Contains(x));

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

        private void CheckWinner()
        {
            foreach (Tile tile in _board)
            {
                if (!tile.IsMine && !tile.IsRevealed)
                    return;
            }

            IsWon = true;
        }

        private void SetNeighborCount(int row, int col)
        {
            foreach (Tile neighborTile in GetNeighborTiles(row, col))
            {
                if (neighborTile.IsMine)
                {
                    _board[row, col].AdjacentMines++;
                }
            }
        }

        private IEnumerable<Tile> GetNeighborTiles(int row, int col)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    int rowCheck = row + i, colCheck = col + j;
                    if (CheckRowBounds(rowCheck) && CheckColumnBounds(colCheck))
                    {
                        yield return _board[rowCheck, colCheck];
                    }
                }
            }
        }

        private bool CheckRowBounds(int row)
        {
            return row >= 0 && row < Size;
        }

        private bool CheckColumnBounds(int col)
        {
            return col >= 0 && col < Size;
        }
    }
}
