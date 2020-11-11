using System.Collections.Generic;
using System.Threading.Tasks;

namespace SudokuSolver
{
    public class Solver
    {
        private Tile[] sBoard = new Tile[81];
        private byte?[] solution = new byte?[81];
        public Solver()
        {
            InitializeGrid();
        }
        public async Task MainMethod(byte?[] board)
        {
            SetGrid(board);
            await Task.Run(SolveGrid);
            SetSolution();
        }

        public byte?[] Solution => solution;

        private void InitializeGrid()
        {
            int index = 0;
            byte r = 0, c = 0;
            for (byte square = 0; square < 9; square++)
            {
                for (byte row = (byte) (0 + r); row < (byte) (3 + r); row++)
                {
                    for (byte column = (byte) (0 + c); column < (byte) (3 + c); column++)
                    {
                        sBoard[index++] = new Tile(row, column, square);
                    }
                }

                c += 3;

                if (c <= 6)
                {
                    continue;
                }
                
                c = 0;
                r += 3;
            }
        }
        
        private void SetGrid(byte?[] grid)
        {
            int i = 0;
            foreach (var tile in sBoard)
            {
                sBoard[tile.Id].Digit = (byte?) (grid[i++] - 48);
            }

            foreach (var tile in sBoard)
            {
                tile.FindTileOptions(sBoard);
            }
        }

        private void SetSolution()
        {
            foreach (var tile in sBoard)
            {
                solution[tile.Id] = (byte?) (tile.Digit + 48);
            }
        }
        
        private void SolveGrid()
        {
            while (!CheckFull())
            {
                bool reset = false;
                bool altered = CheckTileOnlyOption();
                if (altered) { continue; }
                altered = CheckSquareOnlyOption();
                if (altered) { continue; }

                int min = 9;
                Tile minTile = null;

                while (true)
                {
                    foreach (var tile in sBoard)
                    {
                        if (tile.Options.Count != 0 || tile.Digit != null)
                        {
                            continue;
                        }
                        
                        foreach (var t in sBoard)
                        {
                            if (!t.SetOptionsBack())
                            {
                                return;
                            }
                        }
                            
                        Tile.TestIds.RemoveAt(Tile.TestIds.Count - 1);

                        reset = true;
                    }

                    if (reset) { break;}
                    
                    foreach (var tile in sBoard)
                    {
                        if (tile.Options.Count >= min || tile.Options.Count <= 0 || tile.Test)
                        {
                            continue;
                        }
                        
                        min = tile.Options.Count;
                        minTile = tile;

                        if (min < 3) { break; }
                    }

                    if (minTile != null)
                    {
                        break;
                    }

                    if (CheckFull())
                    {
                        continue;
                    }
                    foreach (var tile in sBoard)
                    {
                        tile.SetOptionsBack();
                    }
                }

                if (reset)
                {
                    continue;
                }

                minTile.Digit = minTile.Options[0];
                minTile.Test = true;
                Tile.TestIds.Add(minTile.Id);
                _ = 1;

                foreach (var tile in sBoard)
                {
                    tile.RemoveOption(minTile, true);
                }

                if (CheckFull() && !CheckGrid())
                {
                    foreach (var t in sBoard)
                    {
                        if (!t.SetOptionsBack())
                        {
                            return;
                        }
                    }
                            
                    Tile.TestIds.RemoveAt(Tile.TestIds.Count - 1);
                }
            }
        }
        
        private bool CheckFull()
        {
            foreach (var tile in sBoard)
            {
                if (tile.Digit == null)
                {
                    return false;
                }
            }

            return true;
        }

        private bool CheckGrid()
        {
            foreach (var tile in sBoard)
            {
                if (!tile.CheckTile(sBoard))
                {
                    return false;
                }
            }
            return true;
        }

        private bool CheckTileOnlyOption()
        {
            bool altered = false;
            foreach (var selectedTile in sBoard)
            {
                if (selectedTile.Options.Count == 1 && !selectedTile.Test)
                {
                    selectedTile.Digit = selectedTile.Options[0];
                    selectedTile.Options = new List<byte>();
                    altered = true;
                    
                    foreach (var tile in sBoard)
                    {
                        tile.RemoveOption(selectedTile, false);
                    }
                }
            }

            return altered;
        }

        private bool CheckSquareOnlyOption()
        {
            Tile selectedTile = null;
            bool altered = false;

            for (int square = 0; square < 81; square += 9)
            {
                for (byte num = 1; num < 10; num++)
                {
                    for (int i = square; i < square + 9; i++)
                    {
                        if (!sBoard[i].Options.Contains(num))
                        {
                            continue;
                        }
                        
                        if (selectedTile == null)
                        {
                            selectedTile = sBoard[i];
                            continue;
                        }

                        selectedTile = null;
                        break;
                    }

                    if (selectedTile == null) continue;
                    
                    selectedTile.Digit = num;
                    selectedTile.Options = new List<byte>();
                    altered = true;

                    foreach (var tile in sBoard)
                    {
                        tile.RemoveOption(selectedTile, false);
                    }
                    
                    selectedTile = null;
                }
            }

            return altered;
        }
    }
}