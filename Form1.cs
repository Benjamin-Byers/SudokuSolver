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
        private int BoardSize = 50;
        public Form1()
        {
            InitializeComponent();
            SudokuBoard board = new SudokuBoard();
            Controls.Add(board);
            Paint += DrawBoard;
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