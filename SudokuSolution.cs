using System;
using System.Drawing;
using System.Windows.Forms;

namespace SudokuSolver
{
    public partial class SudokuSolution : Form
    {
        private Board board;
        private Solver solve;
        private int BoardSize = 50;
        //private List<int> duplicates;
        private int offsetX = 10;
        private int offsetY = 10;
        public SudokuSolution()
        {
            InitializeComponent();
            board = new Board(offsetX, offsetY);
            solve = new Solver();
            
            
            Button solveButton = new Button()
            {
                Size = new Size(150, 50),
                Font = new Font("Arial", 16, FontStyle.Regular),
                Text = @"Solve",
                Location = new Point(50 + offsetX, 500 + offsetY)
            };
            
            Button checkButton = new Button()
            {
                Size = new Size(150, 50),
                Font = new Font("Arial", 16, FontStyle.Regular),
                Text = @"Check",
                Location = new Point(250 + offsetX, 500 + offsetY)
            };

            var solveResult = new Label()
            {
                Size = new Size(150, 40),
                Font = new Font("Arial", 16, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleCenter,
                Text = @"----------",
                Location = new Point(50 + offsetX, 460 + offsetY)
            };

            var checkResult = new Label()
            {
                Size = new Size(150, 40),
                Font = new Font("Arial", 16, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleCenter,
                Text = @"----------",
                Location = new Point(250 + offsetX, 460 + offsetY)
            };

            solveButton.Click += SolvePuzzle;

            Controls.Add(solveButton);
            Controls.Add(checkButton);
            Controls.Add(solveResult);
            Controls.Add(checkResult);
            Controls.Add(board);
            
            
            Paint += DrawBoard;
            MouseDown += Deselect;
        }

        private async void SolvePuzzle(object sender, EventArgs args)
        {
            await solve.MainMethod(board.SBoard);
            board.SetGrid(solve.Solution);
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