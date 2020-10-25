﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SudokuSolver
{
    public partial class Form1 : Form
    {
        private CheckSudoku check;
        private SudokuBoard board;
        private Label checkResult;
        private int BoardSize = 50;
        public Form1()
        {
            InitializeComponent();
            board = new SudokuBoard();
            check = new CheckSudoku();
            
            Button checkButton = new Button()
            {
                Size = new Size(150, 50),
                Font = new Font("Arial", 16, FontStyle.Regular),
                Text = "Check",
                Location = new Point(475, 400)
            };

            checkResult = new Label()
            {
                Size = new Size(150, 50),
                Font = new Font("Arial", 16, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleCenter,
                Text = "----------",
                Location = new Point(475, 350)
            };

            checkButton.Click += CheckPuzzle;

            Controls.Add(checkButton);
            Controls.Add(board);
            Controls.Add(checkResult);
            
            Paint += DrawBoard;
        }

        private void CheckPuzzle(object sender, EventArgs args)
        {
            if (check.CheckBoard(board.GetBoard()))
            {
                checkResult.ForeColor = Color.Green;
                checkResult.Text = "Valid";
            }
            else
            {
                checkResult.ForeColor = Color.Red;
                checkResult.Text = "Invalid";
            }
        }

        private void DrawBoard(object sender, PaintEventArgs args)
        {
            args.Graphics.DrawRectangle(new Pen(Color.Black, 3), new Rectangle(Point.Empty, new Size(450, 450)));
            
            for (int i = 1; i < 9; i++)
            {
                int penSize = (i) % 3 == 0 ? 3 : 1;  
                args.Graphics.DrawLine(new Pen(Color.Black, penSize), new Point(BoardSize * i, 0), new Point(BoardSize * i, 450));
                args.Graphics.DrawLine(new Pen(Color.Black, penSize), new Point(0, BoardSize * i), new Point(450, BoardSize * i));
            }
        }
    }
}