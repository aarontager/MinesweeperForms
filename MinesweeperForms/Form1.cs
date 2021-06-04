using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MinesweeperForms
{
    public partial class Form1 : Form
    {
        private MinesweeperModel _model;
        private Button[,] _buttons;
        private TableLayoutPanel _gamePanel;
        private DateTime _tracker;
        public Form1()
        {
            InitializeComponent();
        }

        private void tmrGame_Tick(object sender, EventArgs e)
        {
            TimeSpan elapsedTime = DateTime.Now - _tracker;
            label1.Text = "Time: " + elapsedTime.ToString("mm':'ss");
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            SetupGameBoard();
            SetupTimer();
        }

        private void SetupTimer()
        {
            tmrGame.Enabled = true;
            tmrGame.Start();
            _tracker = DateTime.Now;
        }

        private void SetupGameBoard()
        {
            this.SuspendLayout();
            int boardSize = 0, mineCount = 0;
            switch (cmbDifficulty.SelectedItem)
            {
                case "Easy":
                    boardSize = 10;
                    mineCount = 20;
                    break;
                case "Medium":
                    boardSize = 20;
                    mineCount = 40;
                    break;
                case "Hard":
                    boardSize = 25;
                    mineCount = 50;
                    break;
            }
            _model = new MinesweeperModel(boardSize, mineCount);
            
            SetupGameTable();
            SetupGameButtons();
            this.ResumeLayout(false);
        }

        private void SetupGameTable()
        {
            _gamePanel = new TableLayoutPanel
            {
                ColumnCount = _model.Size,
                RowCount = _model.Size,
                Dock = DockStyle.Fill
            };
            tlpMain.Controls.Add(_gamePanel, 1, 0);

            for (int i = 0; i < _model.Size; i++)
            {
                _gamePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F / _model.Size));
                _gamePanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F / _model.Size));
            }
        }

        private void SetupGameButtons()
        {
            _buttons = new Button[_model.Size, _model.Size];
            for (int i = 0; i < _model.Size; i++)
            {
                for (int j = 0; j < _model.Size; j++)
                {
                    Button tempButton = new Button
                    {
                        Tag = i * _model.Size + j,
                        Margin = new Padding(1),
                        Dock = DockStyle.Fill,
                        TextAlign = ContentAlignment.MiddleCenter,
                        Font = new Font(FontFamily.GenericMonospace, 20)
                    };
                    tempButton.MouseUp += btnGame_Click;

                    _buttons[i, j] = tempButton;
                    _gamePanel.Controls.Add(_buttons[i, j], j, i);
                }
            }
        }

        private void btnGame_Click(object sender, MouseEventArgs e)
        {
            Button tempButton = (Button) sender;
            int tag = (int) tempButton.Tag;

            if (e.Button == MouseButtons.Left)
                _model.ClickTile(tag / _model.Size, tag % _model.Size);
            else
                _model.Flag(tag / _model.Size, tag % _model.Size);

            UpdateGUI();
            ShowMessageIfGameOver();
        }

        private void UpdateGUI()
        {
            for (int i = 0; i < _model.Size; i++)
            {
                for (int j = 0; j < _model.Size; j++)
                {
                    //Reset the button styling in case a tile was unflagged
                    _buttons[i, j].BackColor = DefaultBackColor;
                    _buttons[i, j].UseVisualStyleBackColor = true;

                    Tile tile = _model.GetTileString(i, j);
                    if (tile.IsFlagged)
                    {
                        _buttons[i, j].BackColor = Color.Red;
                    }
                    else if (tile.IsRevealed)
                    {
                        if (tile.IsMine)
                        {
                            _buttons[i, j].BackColor = Color.Black;
                        }
                        else
                        {
                            _buttons[i, j].Text = tile.AdjacentMines.ToString();
                        }
                    }
                }
            }
        }

        private void ShowMessageIfGameOver()
        {
            if (_model.IsLost)
            {
                StopGame();
                MessageBox.Show("You lose!", "Aaron Tager's Minesweeper MVC Project");
            }

            if (_model.IsWon)
            {
                StopGame();
                MessageBox.Show("You win!", "Aaron Tager's Minesweeper MVC Project");
            }
        }

        private void StopGame()
        {
            tmrGame.Stop();
            foreach (Button button in _buttons)
            {
                button.MouseUp -= btnGame_Click;
            }
        }
    }
}
