using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace SudokuSolver
{
    public class SudokuBoard : UserControl
    {
        private Tuple<int, int, char>[] Squares = new Tuple<int, int, char>[81];
        private Tuple<int, int> mousePos = new Tuple<int, int>(0, 0);
        private Tuple<int, int> lastPos = new Tuple<int, int>(0, 0);
        private Tuple<int, int> highlight = new Tuple<int, int>(1000, 1000);
        private int[] Digits = {1, 2, 3, 4, 5, 6, 7, 8, 9};
        private Font defFont = new Font("Arial", 42, FontStyle.Regular);
        public SudokuBoard()
        {
            InitializeComponent();
            Paint += DrawBoard;
            MouseMove += HighlightSquare;
            MouseDown += SelectSquare;
            KeyDown += NumPress;
        }

        private void NumPress(object sender, KeyEventArgs args)
        {
            if (!highlight.Equals(new Tuple<int, int>(1000, 1000)) && Digits.Contains(args.KeyValue - 48))
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
            Squares[index] = new Tuple<int, int, char>(x, y, digit);
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
            
            for (int i = 0; i < Squares.Length; i++)
            {
                if (x > 8)
                {
                    y++;
                    x = 0;
                }

                Squares[i] = new Tuple<int, int, char>(x++, y, ' ');
            }
        }

        private void DrawBoard(object sender, PaintEventArgs args)
        {
            foreach (var square in Squares)
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