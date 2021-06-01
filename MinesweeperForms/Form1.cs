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
                    mineCount = 10;
                    break;
                case "Medium":
                    boardSize = 15;
                    mineCount = 15;
                    break;
                case "Hard":
                    boardSize = 20;
                    mineCount = 20;
                    break;
                case "Expert":
                    boardSize = 30;
                    mineCount = 30;
                    break;
            }
            _model = new MinesweeperModel(boardSize, mineCount);
            
            SetupGameTable(boardSize);
            SetupGameButtons(boardSize);
            this.ResumeLayout(false);
        }

        private void SetupGameTable(int boardSize)
        {
            _gamePanel = new TableLayoutPanel();
            _gamePanel.ColumnCount = boardSize;
            _gamePanel.RowCount = boardSize;
            _gamePanel.Dock = DockStyle.Fill;
            tlpMain.Controls.Add(_gamePanel, 1, 0);

            for (int i = 0; i < boardSize; i++)
            {
                _gamePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F / boardSize));
                _gamePanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F / boardSize));
            }
        }

        private void SetupGameButtons(int boardSize)
        {
            _buttons = new Button[boardSize, boardSize];
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    Button tempButton = new Button();
                    tempButton.Margin = new Padding(1);
                    tempButton.Dock = DockStyle.Fill;
                    tempButton.TextAlign = ContentAlignment.MiddleCenter;
                    tempButton.Font = new Font(FontFamily.GenericMonospace, 16);

                    Tile temp = _model.GetTile(i, j);
                    if (temp.IsMine)
                    {
                        tempButton.Text = "M";
                    }
                    else
                    {
                        tempButton.Text = temp.AdjacentMines.ToString();
                    }

                    _buttons[i, j] = tempButton;
                    _gamePanel.Controls.Add(_buttons[i, j], i, j);
                }
            }
        }
    }
}
