using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace SudokuSolver
{
    public class SudokuBoard : UserControl
    {
        private int offsetX;
        private int offsetY;
        private char[] board = new char[81];
        private Tuple<int, int> mousePos = new Tuple<int, int>(1000, 1000);
        private Tuple<int, int> lastPos = new Tuple<int, int>(0, 0);
        private Tuple<int, int> highlight = new Tuple<int, int>(1000, 1000);
        private List<int> duplicates = new List<int>();
        private List<int> added = new List<int>();
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
        
        private int[][] testGrid =
        {
            new [] {1, 2, -16, -16, 9, 7, -16, -16, -16},
            new [] {8, 7, -16, -16, 5, -16, -16, -16, 1},
            new [] {-16, -16, 4, -16, -16, -16, 7, -16, 5},
            new [] {9, -16, -16, 4, -16, 6, -16, 1, -16},
            new [] {-16, 3, -16, 5, -16, 2, -16, 6, -16},
            new [] {-16, 1, -16, 9, -16, 3, -16, -16, 7},
            new [] {5, -16, 1, -16, -16, -16, 6, -16, -16},
            new [] {7, -16, -16, -16, 2, -16, -16, 9, 8},
            new [] {-16, -16, -16, 1, 3, -16, -16, 7, 4}
        };
        
        private int[][] testGrid2 =
        {
            new [] {-16, -16, -16, 6, 5, -16, -16, -16, 9},
            new [] {8, -16, 4, -16, -16, -16, -16, -16, -16},
            new [] {5, -16, -16, 8, -16, -16, -16, 4, 6},
            new [] {-16, 5, 3, -16, -16, 7, -16, 8, -16},
            new [] {-16, -16, -16, 1, -16, 2, -16, -16, -16},
            new [] {-16, 8, -16, 9, -16, -16, 4, 6, -16},
            new [] {2, 9, -16, -16, -16, 1, -16, -16, 5},
            new [] {-16, -16, -16, -16, -16, -16, 7, -16, 8},
            new [] {7, -16, -16, -16, 2, 8, -16, -16, -16},
        };
        
        private int[][] difficultGrid =
        {
            new [] { 08, -16, -16, -16, -16, -16, -16, -16, -16},
            new [] {-16, -16,  03,  06, -16, -16, -16, -16, -16},
            new [] {-16,  07, -16, -16,  09, -16,  02, -16, -16},
            new [] {-16,  05, -16, -16, -16,  07, -16, -16, -16},
            new [] {-16, -16, -16, -16,  04,  05,  07, -16, -16},
            new [] {-16, -16, -16,  01, -16, -16, -16,  03, -16},
            new [] {-16, -16,  01, -16, -16, -16, -16,  06,  08},
            new [] {-16, -16,  08,  05, -16, -16, -16,  01, -16},
            new [] {-16,  09, -16, -16, -16, -16,  04, -16, -16},
        };
        
        /*private int[][] emptyGrid =
        {
            new [] {-16, -16, -16, -16, -16, -16, -16, -16, -16},
            new [] {-16, -16, -16, -16, -16, -16, -16, -16, -16},
            new [] {-16, -16, -16, -16, -16, -16, -16, -16, -16},
            new [] {-16, -16, -16, -16, -16, -16, -16, -16, -16},
            new [] {-16, -16, -16, -16, -16, -16, -16, -16, -16},
            new [] {-16, -16, -16, -16, -16, -16, -16, -16, -16},
            new [] {-16, -16, -16, -16, -16, -16, -16, -16, -16},
            new [] {-16, -16, -16, -16, -16, -16, -16, -16, -16},
            new [] {-16, -16, -16, -16, -16, -16, -16, -16, -16},
        };*/
            
        public SudokuBoard(int osX, int osY)
        {
            offsetX = osX;
            offsetY = osY;
            
            InitializeComponent();
            Paint += DrawBoard;
            MouseMove += HighlightSquare;
            MouseDown += SelectSquare;
            MouseLeave += (object sender, EventArgs args) =>
            {
                mousePos = new Tuple<int, int>(1000, 1000);
                Invalidate();
            };
            KeyDown += NumPress;
            LostFocus += DeselectSquare;
            SetGrid(difficultGrid);
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

        private void SetGrid(int[][] grid)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    board[j * 9 + i] = (char) (grid[j][i] + 48);
                }
            }
        }

        public char[] GetBoard() { return board;}

        public void SetBoard(char[] Board)
        {
            board = Board;
            Invalidate();
        }

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
            if (!highlight.Equals(new Tuple<int, int>(1000, 1000)) && (_digits.Contains(args.KeyValue - 48) || _digits.Contains(args.KeyValue - 96)))
            {
                char key = args.KeyValue > 90 ? (char) (args.KeyValue - 48) : (char) args.KeyValue;
                
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
                
                UpdateBoard(x, y, ' ');
                duplicates.RemoveAll(i => i == y * 9 + x);
                added.RemoveAll(i => i == y * 9 + x);
            }
        }

        public void UpdateBoard(int x, int y, char digit)
        {
            int index = (y * 9) + x;
            board[index] = digit;
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
                board[i] = ' ';
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

                args.Graphics.DrawString(board[i].ToString(), defFont, brush, new Point(x++ * 50 + 1, y * 50 - 7));
            }
            
            if (!highlight.Equals(new Tuple<int, int>(1000, 1000)))
            {
                args.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(128, Color.Black)), new Rectangle(new Point(highlight.Item1, highlight.Item2), new Size(50, 50)));
            }
            
            args.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(64, Color.Black)), new Rectangle(new Point(mousePos.Item1, mousePos.Item2), new Size(50, 50)));
        }
    }
}