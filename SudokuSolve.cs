using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Drawing;
using System.Linq;

namespace SudokuSolver
{
    public class SudokuSolve
    {
        private SudokuBoard board;
        private CheckSudoku check;
        
        public SudokuSolve(SudokuBoard Board, CheckSudoku Check)
        {
            board = Board;
            check = Check;
        }

        public void Solve()
        {
            char[] sBoard = board.GetBoard();
            List<Tuple<int, List<int>>> options = FindSquareOptions(sBoard);
            int index = options.FindIndex(i => (i.Item2.Count == options.Select(j => j.Item2.Count).Min() && i.Item2.Count != 0));
            if (index < 0) return;

            while (true)
            {
                if (options.Count <= 0) return;
                if (options[index].Item2.Count == 1)
                {
                    sBoard[options[index].Item1] = (char) (options[index].Item2.First() + 48);
                    options = FindSquareOptions(sBoard);
                    index = options.FindIndex(i => (i.Item2.Count == options.Select(j => j.Item2.Count).Min() && i.Item2.Count != 0));
                    continue;
                }

                break;
            }

            char[] testBoard = sBoard;
            List<Tuple<int, List<int>>> testOptions = options; //FindSquareOptions(testBoard);
            List<int> testSquares = new List<int>();
            List<Tuple<int, int>> testValues = new List<Tuple<int, int>>();

            while(!check.CheckFull(sBoard))
            {
                void Undo()
                {
                    testBoard[testSquares.Last()] = ' ';
                    testOptions[testValues[^1].Item1].Item2.RemoveAt(0);
                    testValues.Remove(testValues.Last());
                    testSquares.Remove(testSquares.Last());
                }
                
                if (testOptions[index].Item2.Count > 0)
                {
                    testSquares.Add(testOptions[index].Item1);
                    testValues.Add(new Tuple<int, int>(index, testOptions[index].Item2.First()));
                    testBoard[testOptions[index].Item1] = (char) (testOptions[index].Item2.First() + 48);
                    
                    if (!check.CheckBoard(testBoard).Item1)
                    {
                        Undo();
                    }
                    
                }
                else if (!check.CheckFull(testBoard))
                {
                    if (testSquares.Count == 0)
                    {
                        Solve();
                        return;
                    }
                    Undo();
                }
                else
                {
                    board.SetBoard(sBoard);
                }

                index = testOptions.FindIndex(i => i.Item2.Count == testOptions.Select(j => j.Item2.Count).Min()  && !testSquares.Contains(i.Item1));

                _ = 1;
            }
            
            //Loop: Find Square with least options
            //Try first value, store index of first value to remove from options if not solved
            //Find options for each square as separate variable.
            //CheckSudoku.CheckValid
        }

        private List<Tuple<int, List<int>>> FindSquareOptions(char[] sBoard)
        {
            int[] digits = {1, 2, 3, 4, 5, 6, 7, 8, 9};
            List<Tuple<int, List<int>>> options = new List<Tuple<int, List<int>>>();
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

                if (sBoard[i].ToString() != " ")
                {
                    X++;
                    continue;
                }
                List<int> ops = new List<int>(digits);

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

                options.Add(new Tuple<int, List<int>>(i, ops));
                X++;
                
                _ = 1;
                
            }

            _ = 1;

            return options;
        }
    }
}