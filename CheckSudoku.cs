using System;
using System.Collections.Generic;
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
            List<Tuple<int, int>> duplicates = new List<Tuple<int, int>>();
            
            if (!CheckFull(board)) return false;
            
            (int[][] Squares, int[][] Columns, int[][] Rows) = SplitBoard(board);
            
            if (!CheckValid(Squares)) return false;
            foreach (var group in duplicates)
            {
                //Item1 
            }
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

            for (int i = 0; i < 7; i += 3)
            {
                //Squares
                squares[i] = new int[9];
                squares[i + 1] = new int[9];
                squares[i + 2] = new int[9];

                for (int j = 0; j < 7; j += 3)
                {
                    squares[i][j] = board[i * 9 + 3 * j];
                    squares[i][j + 1] = board[i * 9 + 3 * j + 1];
                    squares[i][j + 2] = board[i * 9 + 3 * j + 2];
                    
                    squares[i + 1][j] = board[i * 9 + 3 * j + 3];
                    squares[i + 1][j + 1] = board[i * 9 + 3 * j + 4];
                    squares[i + 1][j + 2] = board[i * 9 + 3 * j + 5];
                    
                    squares[i + 2][j] = board[i * 9 + 3 * j + 6];
                    squares[i + 2][j + 1] = board[i * 9 + 3 * j + 7];
                    squares[i + 2][j + 2] = board[i * 9 + 3 * j + 8];
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
            bool valid = true;
            int index = 0;
            foreach (var group in groups)
            {
                if (group.Distinct().Count() != 9)
                {
                    valid = false;
                    List<Tuple<int, int>> duplicates = new List<Tuple<int, int>>();
                    Dictionary<int, int> checkDuplicates = new Dictionary<int, int>();

                    foreach (var num in group)
                    {
                        if (checkDuplicates.ContainsKey(num))
                        {
                            checkDuplicates[num] += 1;
                        }
                        else
                        {
                            checkDuplicates.Add(num, 1);
                        }
                    }

                    foreach (var num in checkDuplicates.Keys)
                    {
                        if (checkDuplicates[num] > 1)
                        {
                            for (int i = 0; i < group.Length; i++)
                            {
                                if (group[i] == num)
                                {
                                    duplicates.Add(new Tuple<int, int>(index, i)); //Find the indices of duplicate numbers.
                                }
                            }
                        }
                    }
                    
                    return false;
                }
                
                index++;

            }

            return true;
        }
    }
}