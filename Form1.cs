using System;
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
        private SudokuBoard board;
        private CheckSudoku check;
        private SudokuSolve solve;
        private Label checkResult;
        private Label solveResult;
        private int BoardSize = 50;
        private List<int> duplicates;
        private int offsetX = 10;
        private int offsetY = 10;
        public Form1()
        {
            InitializeComponent();
            board = new SudokuBoard(offsetX, offsetY);
            check = new CheckSudoku();
            solve = new SudokuSolve(board, check);
            
            
            Button solveButton = new Button()
            {
                Size = new Size(150, 50),
                Font = new Font("Arial", 16, FontStyle.Regular),
                Text = "Solve",
                Location = new Point(50 + offsetX, 500 + offsetY)
            };
            
            Button checkButton = new Button()
            {
                Size = new Size(150, 50),
                Font = new Font("Arial", 16, FontStyle.Regular),
                Text = "Check",
                Location = new Point(250 + offsetX, 500 + offsetY)
            };

            solveResult = new Label()
            {
                Size = new Size(150, 40),
                Font = new Font("Arial", 16, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleCenter,
                Text = "----------",
                Location = new Point(50 + offsetX, 460 + offsetY)
            };

            checkResult = new Label()
            {
                Size = new Size(150, 40),
                Font = new Font("Arial", 16, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleCenter,
                Text = "----------",
                Location = new Point(250 + offsetX, 460 + offsetY)
            };

            solveButton.Click += SolvePuzzle;
            checkButton.Click += CheckPuzzle;

            Controls.Add(solveButton);
            Controls.Add(checkButton);
            Controls.Add(solveResult);
            Controls.Add(checkResult);
            Controls.Add(board);
            
            
            Paint += DrawBoard;
            MouseDown += Deselect;
        }

        private void SolvePuzzle(object sender, EventArgs args)
        {
            (bool valid, List<int> added) = solve.Solve();
            board.SetAdded(added);
            
            if (valid)
            {
                solveResult.ForeColor = Color.DarkGreen;
                solveResult.Text = "Solved";
            }
            else
            {
                solveResult.ForeColor = Color.Red;
                solveResult.Text = "Puzzle";
                CheckPuzzle(null, null);
            }
        }
        
        private void CheckPuzzle(object sender, EventArgs args)
        {
            bool valid;
            (valid, duplicates) = check.CheckBoard(board.GetBoard(), true);
            board.SetDuplicates(duplicates);
            
            if (valid)
            {
                checkResult.ForeColor = Color.DarkGreen;
                checkResult.Text = "Valid";
                if (solveResult.Text == "Puzzle")
                {
                    solveResult.ForeColor = Color.Black;
                    solveResult.Text = "----------";
                }
            }
            else
            {
                checkResult.ForeColor = Color.Red;
                checkResult.Text = "Invalid";
                solveResult.ForeColor = Color.Red;
                solveResult.Text = "Puzzle";
            }
        }

        private void Deselect(object sender, MouseEventArgs args)
        {
            if (args.X > 450 + offsetX || args.Y > 450 + offsetY)
            {
                board.DeselectSquare();
            }
        }

        private void DrawBoard(object sender, PaintEventArgs args)
        {
            args.Graphics.DrawRectangle(new Pen(Color.Black, 3), new Rectangle(new Point(offsetX, offsetY), new Size(450, 450)));
            
            for (int i = 1; i < 9; i++)
            {
                int penSize = (i) % 3 == 0 ? 3 : 1;  
                args.Graphics.DrawLine(new Pen(Color.Black, penSize), new Point(BoardSize * i + offsetX, 0 + offsetY), new Point(BoardSize * i + offsetX, 450 + offsetY));
                args.Graphics.DrawLine(new Pen(Color.Black, penSize), new Point(0 + offsetX, BoardSize * i + offsetY), new Point(450 + offsetX, BoardSize * i + offsetY));
            }
        }
    }
}