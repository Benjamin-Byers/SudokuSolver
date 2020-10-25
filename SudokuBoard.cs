using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace SudokuSolver
{
    public class SudokuBoard : UserControl
    {
        private Tuple<int, int, char>[] board = new Tuple<int, int, char>[81];
        private Tuple<int, int> mousePos = new Tuple<int, int>(0, 0);
        private Tuple<int, int> lastPos = new Tuple<int, int>(0, 0);
        private Tuple<int, int> highlight = new Tuple<int, int>(1000, 1000);
        private readonly int[] _digits = {1, 2, 3, 4, 5, 6, 7, 8, 9};
        private Font defFont = new Font("Arial", 42, FontStyle.Regular);

        private int[][] validGrid =
        {
            new [] {1, 2, 3, 7, 8, 9, 4, 5, 6},
            new [] {4, 5, 6, 1, 2, 3, 7, 8, 9},
            new [] {7, 8, 9, 4, 5, 6, 1, 2, 3},
            new [] {2, 3, 1, 8, 9, 7, 5, 6, 4},
            new [] {5, 6, 4, 2, 3, 1, 8, 9, 7},
            new [] {8, 9, 7, 5, 6, 4, 2, 3, 1},
            new [] {3, 1, 2, 9, 7, 8, 6, 4, 5},
            new [] {6, 4, 5, 3, 1, 2, 9, 7, 8},
            new [] {9, 7, 8, 6, 4, 5, 3, 1, 2}
        };
            
        public SudokuBoard()
        {
            InitializeComponent();
            Paint += DrawBoard;
            MouseMove += HighlightSquare;
            MouseDown += SelectSquare;
            KeyDown += NumPress;
            SetGrid(validGrid);
            Invalidate();
        }

        private void SetGrid(int[][] grid)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    board[j * 9 + i] = new Tuple<int, int, char>(i, j, (char) (grid[j][i] + 48));
                }
            }
        }

        public Tuple<int, int, char>[] GetBoard() { return board;}

        private void NumPress(object sender, KeyEventArgs args)
        {
            if (!highlight.Equals(new Tuple<int, int>(1000, 1000)) && _digits.Contains(args.KeyValue - 48))
            {
                UpdateBoard(highlight.Item1 / 50, (int) highlight.Item2 / 50, (char) args.KeyValue);
            }
            else if (!highlight.Equals(new Tuple<int, int>(1000, 1000)) && args.KeyCode == Keys.Delete)
            {
                UpdateBoard(highlight.Item1 / 50, (int) highlight.Item2 / 50, ' ');
            }
        }

        private void InitializeComponent()
        {
            BackColor = Color.Transparent;
            Size = new Size(450, 450);
            DoubleBuffered = true;
            CreateBoard();
        }

        public void UpdateBoard(int x, int y, char digit)
        {
            int index = (y * 9) + x;
            board[index] = new Tuple<int, int, char>(x, y, digit);
            Invalidate();
        }

        private void HighlightSquare(object sender, MouseEventArgs args)
        {
            mousePos = new Tuple<int, int>((int) Math.Floor(args.X / 50.0) * 50, (int) Math.Floor(args.Y / 50.0) * 50);
            if (!mousePos.Equals(lastPos))
            {
                lastPos = mousePos;
                Invalidate();
            }
        }

        private void SelectSquare(object sender, MouseEventArgs args)
        {
            highlight = mousePos;
            Invalidate();
        }

        private void CreateBoard()
        {
            int x = 0;
            int y = 0;
            
            for (int i = 0; i < board.Length; i++)
            {
                if (x > 8)
                {
                    y++;
                    x = 0;
                }

                board[i] = new Tuple<int, int, char>(x++, y, ' ');
            }
        }

        private void DrawBoard(object sender, PaintEventArgs args)
        {
            foreach (var square in board)
            {
                args.Graphics.DrawString(square.Item3.ToString(), defFont, new SolidBrush(Color.Black), new Point(square.Item1 * 50 + 1, square.Item2 * 50 - 7));
            }
            
            if (!highlight.Equals(new Tuple<int, int>(1000, 1000)))
            {
                args.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(128, Color.Black)), new Rectangle(new Point(highlight.Item1, highlight.Item2), new Size(50, 50)));
            }
            
            args.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(64, Color.Black)), new Rectangle(new Point(mousePos.Item1, mousePos.Item2), new Size(50, 50)));
        }
    }
}