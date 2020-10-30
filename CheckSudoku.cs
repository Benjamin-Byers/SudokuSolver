using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver
{
    public class CheckSudoku
    {
        public (bool, List<int>) CheckBoard(char[] board, bool button = false)
        {
            bool valid;
            List<int> add;
            List<int> duplicates = new List<int>();
            
            if (!CheckFull(board) && !button) return (false, duplicates);

            (Tuple<int, int>[][] squares, Tuple<int, int>[][] columns, Tuple<int, int>[][] rows) = SplitBoard(board);

            (valid, add) = CheckValid(squares, true);
            duplicates.AddRange(add);
            add.Clear();
            
            (valid, add) = CheckValid(columns, valid);
            duplicates.AddRange(add);
            add.Clear();
            
            (valid, add) = CheckValid(rows, valid);
            duplicates.AddRange(add);
            add.Clear();

            return (valid, duplicates);
        }

        public (Tuple<int, int>[][], Tuple<int, int>[][], Tuple<int, int>[][]) SplitBoard(char[] sBoard)
        {
            int[] board = new int[81];
            Tuple<int, int>[][] squares = new Tuple<int, int>[9][];
            Tuple<int, int>[][] columns = new Tuple<int, int>[9][];
            Tuple<int, int>[][] rows    = new Tuple<int, int>[9][];

            for (int i = 0; i < 81; i++)
            {
                board[i] = sBoard[i] - 48;
            }

            for (int i = 0; i < 9; i++)
            {
                //Rows
                rows[i] = new Tuple<int, int>[9];

                for (int j = 0; j < 9; j++)
                {
                    int index = i * 9 + j;
                    rows[i][j] = new Tuple<int, int>(index, board[index]);
                }
                
                //Columns
                columns[i] = new Tuple<int, int>[9];

                for (int j = 0; j < 9; j++)
                {
                    int index = j * 9 + i;
                    columns[i][j] = new Tuple<int, int>(index, board[index]);
                }

            }

            for (int i = 0; i < 7; i += 3)
            {
                //Squares
                squares[i] = new Tuple<int, int>[9];
                squares[i + 1] = new Tuple<int, int>[9];
                squares[i + 2] = new Tuple<int, int>[9];

                for (int j = 0; j < 7; j += 3)
                {
                    int index = i * 9 + 3 * j;

                    for (int k = 0; k < 3; k++)
                    {
                        for (int l = 0; l < 3; l++)
                        {
                            squares[i + k][j + l] = new Tuple<int, int>(index, board[index++]);
                        }
                    }
                }
            }

            _ = 1;
            
            return (squares, columns, rows);
        }

        public bool CheckFull(char[] sBoard)
        {
            foreach (var num in sBoard)
            {
                if (num == ' ') return false;
            }

            return true;
        }

        private (bool, List<int>) CheckValid(Tuple<int, int>[][] groups, bool v)
        {
            List<int> duplicates = new List<int>();
            bool valid = v;
            foreach (var group in groups)
            {
                if (group.Select(i => i.Item2).Distinct().Count(i => i != -16) != 9 - group.Count(i => i.Item2 == -16))
                {
                    valid = false;
                    Dictionary<int, int> checkDuplicates = new Dictionary<int, int>();

                    foreach (var num in group)
                    {
                        if (checkDuplicates.ContainsKey(num.Item2))
                        {
                            checkDuplicates[num.Item2] += 1;
                        }
                        else
                        {
                            checkDuplicates.Add(num.Item2, 1);
                        }
                    }

                    foreach (var num in checkDuplicates.Keys)
                    {
                        if (checkDuplicates[num] > 1)
                        {
                            for (int i = 0; i < group.Length; i++)
                            {
                                if (group[i].Item2 == num)
                                {
                                    duplicates.Add(group[i].Item1); //Add the indices of duplicate numbers.
                                }
                            }
                        }
                    }
                }
            }

            return (valid, duplicates);
        }
    }
}