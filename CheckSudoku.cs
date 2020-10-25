using System;
using System.Linq;

namespace SudokuSolver
{
    public class CheckSudoku
    {
        public CheckSudoku()
        {
            
        }

        public bool CheckBoard(Tuple<int, int, char>[] board)
        {
            if (!CheckFull(board)) return false;
            
            (int[][] Squares, int[][] Columns, int[][] Rows) = SplitBoard(board);
            
            if (!CheckValid(Squares)) return false;
            if (!CheckValid(Columns)) return false;
            if (!CheckValid(Rows)) return false;

            return true;
        }

        private (int[][], int[][], int[][]) SplitBoard(Tuple<int, int, char>[] sBoard)
        {
            int[] board = new int[81];
            int[][] squares = new int[9][];
            int[][] columns = new int[9][];
            int[][] rows    = new int[9][];

            for (int i = 0; i < 81; i++)
            {
                board[i] = sBoard[i].Item3 - 48;
            }

            for (int i = 0; i < 9; i++)
            {
                //Squares
                squares[i] = new int[9];
                
                for (int j = 0; j < 3; j++)
                {
                    int index = i * 9 + j * 3;
                    squares[i][(j * 3)] = board[index];
                    squares[i][(j * 3) + 1] = board[index + 1];
                    squares[i][(j * 3) + 2] = board[index + 2];
                }
                
                //Rows
                rows[i] = new int[9];

                for (int j = 0; j < 9; j++)
                {
                    int index = i * 9 + j;
                    rows[i][j] = board[index];
                }
                
                //Columns
                columns[i] = new int[9];

                for (int j = 0; j < 9; j++)
                {
                    int index = j * 9 + i;
                    columns[i][j] = board[index];
                }
            }
            
            return (squares, columns, rows);
        }

        private bool CheckFull(Tuple<int, int, char>[] sBoard)
        {
            foreach (var num in sBoard)
            {
                if (num.Item3 == ' ') return false;
            }

            return true;
        }

        private bool CheckValid(int[][] groups)
        {
            foreach (var group in groups)
            {
                if (group.Distinct().Count() != 9)
                {
                    return false;
                }
            }

            return true;
        }
    }
}