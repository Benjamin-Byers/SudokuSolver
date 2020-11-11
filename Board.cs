using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SudokuSolver
{
    public class Board : UserControl
    {
        private int offsetX;
        private int offsetY;
        private byte?[] board = new byte?[81];
        private Tuple<int, int> mousePos = new Tuple<int, int>(1000, 1000);
        private Tuple<int, int> lastPos = new Tuple<int, int>(0, 0);
        private Tuple<int, int> highlight = new Tuple<int, int>(1000, 1000);
        private List<int> duplicates = new List<int>();
        private List<int> added = new List<int>();
        private readonly int[] digits = {1, 2, 3, 4, 5, 6, 7, 8, 9};
        private Font defFont = new Font("Arial", 42, FontStyle.Regular);
#if DEBUG
        private byte?[] difficultGrid =
        {
            56, null, null, null, null, null, null, null, null, 
            null, null, 51, 54, null, null, null, null, null, 
            null, 55, null, null, 57, null, 50, null, null, 
            null, 53, null, null, null, 55, null, null, null, 
            null, null, null, null, 52, 53, 55, null, null, 
            null, null, null, 49, null, null, null, 51, null, 
            null, null, 49, null, null, null, null, 54, 56, 
            null, null, 56, 53, null, null, null, 49, null, 
            null, 57, null, null, null, null, 52, null, null
        };
#endif
        

        public Board(int osX, int osY)
        {
            offsetX = osX;
            offsetY = osY;
            
            InitializeComponent();
            Paint += DrawBoard;
            MouseMove += HighlightSquare;
            MouseDown += SelectSquare;
            MouseLeave += (sender, args) =>
            {
                mousePos = new Tuple<int, int>(1000, 1000);
                Invalidate();
            };
            KeyDown += NumPress;
            LostFocus += DeselectSquare;
#if DEBUG
            SetGrid(difficultGrid, true);
#endif
            Invalidate();
        }

        private void InitializeComponent()
        {
            BackColor = Color.Transparent;
            Size = new Size(450, 450);
            Location = new Point(offsetX, offsetY);
            DoubleBuffered = true;
            CreateBoard();
        }

        public void SetGrid(byte?[] grid, bool initial)
        {
            if (!initial)
            {
                for (int i = 0; i < board.Length; i++)
                {
                    if (!board[i].HasValue)
                    {
                        added.Add(i);
                    }
                }
            }
            
            board = grid;
            Invalidate();
        }

        public byte?[] SBoard => board;

        public void SetDuplicates(List<int> dups)
        {
            duplicates = dups;
            added.Clear();
            Invalidate();
        }
        
        public void SetAdded(List<int> add)
        {
            added = add;
            duplicates.Clear();
            Invalidate();
        }

        private void NumPress(object sender, KeyEventArgs args)
        {
            if (!highlight.Equals(new Tuple<int, int>(1000, 1000)) && (digits.Contains(args.KeyValue - 48) || digits.Contains(args.KeyValue - 96)))
            {
                byte? key = args.KeyValue > 90 ? (byte?) (args.KeyValue - 48) : (byte?) args.KeyValue;
                
                int y = highlight.Item2 / 50;
                int x = highlight.Item1 / 50;
                
                UpdateBoard(x, y, key);
                duplicates.RemoveAll(i => i == y * 9 + x);
                added.RemoveAll(i => i == y * 9 + x);
            }
            else if (!highlight.Equals(new Tuple<int, int>(1000, 1000)) && (args.KeyCode == Keys.Delete || args.KeyCode == Keys.Back))
            {
                int y = highlight.Item2 / 50;
                int x = highlight.Item1 / 50;
                
                UpdateBoard(x, y, null);
                duplicates.RemoveAll(i => i == y * 9 + x);
                added.RemoveAll(i => i == y * 9 + x);
            }
        }

        public void UpdateBoard(int x, int y, byte? digit)
        {
            board[y * 9 + x] = digit;
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

        public void DeselectSquare(object sender = null, EventArgs args = null)
        {
            highlight = new Tuple<int, int>(1000, 1000);
            Invalidate();
        }

        private void CreateBoard()
        {
            for (int i = 0; i < board.Length; i++)
            {
                board[i] = null;
            }
        }

        private void DrawBoard(object sender, PaintEventArgs args)
        {
            int x = 0;
            int y = 0;
            SolidBrush brush;

            for (int i = 0; i < 81; i++)
            {
                if (x > 8)
                {
                    y++;
                    x = 0;
                }

                if (duplicates.Contains(i))
                {
                    brush = new SolidBrush(Color.Red);
                }
                else if (added.Contains(i))
                {
                    brush = new SolidBrush(Color.DarkGreen);
                }
                else
                {
                    brush = new SolidBrush(Color.Black);
                }

                args.Graphics.DrawString((board[i] - 48).ToString(), defFont, brush, new Point(x++ * 50 + 1, y * 50 - 7));
            }
            
            if (!highlight.Equals(new Tuple<int, int>(1000, 1000)))
            {
                args.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(128, Color.Black)), new Rectangle(new Point(highlight.Item1, highlight.Item2), new Size(50, 50)));
            }
            
            args.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(64, Color.Black)), new Rectangle(new Point(mousePos.Item1, mousePos.Item2), new Size(50, 50)));
        }
    }
}