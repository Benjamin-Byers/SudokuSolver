using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Drawing;
using System.Linq;
using Microsoft.VisualBasic;

namespace SudokuSolver
{
    public class SudokuSolve
    {
        private SudokuBoard board;
        private CheckSudoku check;

        private int[][] sqIndices =
        {
            new[] {00, 01, 02, 09, 10, 11, 18, 19, 20},
            new[] {03, 04, 05, 12, 13, 14, 21, 22, 23},
            new[] {06, 07, 08, 15, 16, 17, 24, 25, 26},
            new[] {27, 28, 29, 36, 37, 38, 45, 46, 47},
            new[] {30, 31, 32, 39, 40, 41, 48, 49, 50},
            new[] {33, 34, 35, 42, 43, 44, 51, 52, 53},
            new[] {54, 55, 56, 63, 64, 65, 72, 73, 74},
            new[] {57, 58, 59, 66, 67, 68, 75, 76, 77},
            new[] {60, 61, 62, 69, 70, 71, 78, 79, 80}
        };
        
        public SudokuSolve(SudokuBoard Board, CheckSudoku Check)
        {
            board = Board;
            check = Check;
        }

        public void Solve()
        {
            char[] sBoard = board.GetBoard();
            List<(int, List<int>)> options = FindSquareOptions(sBoard);
            int index = options.FindIndex(i => i.Item2.Count == options.Select(j => j.Item2.Count).Where(k => k > 0).Min());
            if (index < 0) return;

            bool altered = true;

            while (altered)
            {
                if (options.Count <= 0) return;
                if (options[index].Item2.Count == 1)
                {
                    sBoard[options[index].Item1] = (char) (options[index].Item2.First() + 48);
                    options = FindSquareOptions(sBoard);
                    
                    try
                    {
                        index = options.FindIndex(i => i.Item2.Count == options.Select(j => j.Item2.Count).Where(k => k > 0).Min());
                    }
                    catch (Exception e)
                    {
                        return;
                    }
                    
                    altered = true;
                    continue;
                }

                altered = false;

                foreach (var square in sqIndices)
                {
                    List<(int, List<int>)> sqOps = new List<(int, List<int>)>();
                    foreach (var tile in square)
                    {
                        sqOps.Add(options[tile]);
                    }
                    
                    Dictionary<int, int> sqOpCounts = new Dictionary<int, int>();
                    
                    foreach (var opList in sqOps)
                    {
                        foreach (var num in opList.Item2)
                        {
                            if (sqOpCounts.ContainsKey(num))
                            {
                                sqOpCounts[num] += 1;
                            }
                            else
                            {
                                sqOpCounts.Add(num, 1);
                            }
                        }
                    }

                    foreach (var pair in sqOpCounts)
                    {
                        if (pair.Value == 1)
                        {
                            foreach (var opList in sqOps)
                            {
                                if (opList.Item2.Contains(pair.Key))
                                {
                                    sBoard[opList.Item1] = (char) (pair.Key + 48);
                                    options = FindSquareOptions(sBoard);
                                    
                                    try
                                    {
                                        index = options.FindIndex(i => i.Item2.Count == options.Select(j => j.Item2.Count).Where(k => k > 0).Min());
                                    }
                                    catch (Exception e)
                                    {
                                        return;
                                    }
                                    
                                    altered = true;
                                }
                            }
                        }
                    }
                }
            }
            
            board.SetBoard(sBoard);
            //Loop: Find Square with least options
            //Try first value, store index of first value to remove from options if not solved
            //Find options for each square as separate variable.
            //CheckSudoku.CheckValid
        }

        private List<(int, List<int>)> FindSquareOptions(char[] sBoard)
        {
            int[] digits = {1, 2, 3, 4, 5, 6, 7, 8, 9};
            List<(int, List<int>)> options = new List<(int, List<int>)>();
            (Tuple<int, int>[][] squares, Tuple<int, int>[][] columns, Tuple<int, int>[][] rows) = check.SplitBoard(sBoard);
            
            int X = 0;
            int Y = 0;

            for (int i = 0; i < 81; i++)
            {
                if (X > 8)
                {
                    Y++;
                    X = 0;
                }
                List<int> ops = new List<int>(digits);

                if (sBoard[i].ToString() != " ")
                {
                    X++;
                    options.Add((i, new List<int>()));
                    continue;
                }

                int squareIndex;
                int sqX, sqY;
                
                if (X < 3) sqX = 0;
                else if (X < 6) sqX = 1;
                else sqX = 2;
                
                if (Y < 3) sqY = 0;
                else if (Y < 6) sqY = 1;
                else sqY = 2;

                squareIndex = sqX + 3 * sqY;

                foreach (var num in columns[X].Select(i => i.Item2))
                {
                    ops.Remove(num);
                }
                
                foreach (var num in rows[Y].Select(i => i.Item2))
                {
                    ops.Remove(num);
                }

                foreach (var num in squares[squareIndex].Select(i => i.Item2))
                {
                    ops.Remove(num);
                }

                options.Add((i, ops));
                X++;
            }
            
            return options;
        }
    }
}