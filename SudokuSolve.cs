using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            Tuple<int, int, char>[] sBoard = board.GetBoard();
            List<Tuple<int, List<int>>> options = FindSquareOptions(sBoard);

            while (true)
            {
                var apple = options.FindIndex(i => i.Item2.Count == options.Select(j => j.Item2.Count).Min());
                
                if (true)
                {
                    break;
                }
            }

            foreach (var set in options)
            {
                
            }

            //Loop: Find Square with least options
            //Try first value, store index of first value to remove from options if not solved
            //Find options for each square as separate variable.
            //CheckSudoku.CheckValid
        }

        private List<Tuple<int, List<int>>> FindSquareOptions(Tuple<int, int, char>[] sBoard)
        {
            int[] digits = {1, 2, 3, 4, 5, 6, 7, 8, 9};
            List<Tuple<int, List<int>>> options = new List<Tuple<int, List<int>>>();
            (Tuple<int, int>[][] squares, Tuple<int, int>[][] columns, Tuple<int, int>[][] rows) = check.SplitBoard(sBoard);

            foreach (var tile in sBoard)
            {
                if (tile.Item3 != ' ') continue;
                List<int> ops = new List<int>(digits);

                int index = tile.Item1 + tile.Item2 * 9;
                int squareIndex;
                int x, y;
                
                if (tile.Item1 < 3) x = 0;
                else if (tile.Item1 < 6) x = 1;
                else x = 2;
                
                if (tile.Item2 < 3) y = 0;
                else if (tile.Item2 < 6) y = 1;
                else y = 2;

                squareIndex = x + 3 * y;

                foreach (var num in columns[tile.Item1].Select(i => i.Item2))
                {
                    ops.Remove(num);
                }
                
                foreach (var num in rows[tile.Item2].Select(i => i.Item2))
                {
                    ops.Remove(num);
                }

                foreach (var num in squares[squareIndex].Select(i => i.Item2))
                {
                    ops.Remove(num);
                }

                options.Add(new Tuple<int, List<int>>(index, ops));
                
                _ = 1;
                
            }

            return options;
        }
    }
}