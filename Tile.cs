using System.Collections.Generic;

namespace SudokuSolver
{
    public class Tile
    {
        private byte id;
        private byte row;
        private byte column;
        private byte square;
        private byte depth;
        private bool test;
        public static List<byte> TestIds = new List<byte>();
        private List<byte?> digit = new List<byte?>();
        private List<List<byte>> options = new List<List<byte>>();

        public Tile(byte r, byte c, byte s, byte? d = null)
        {
            id = (byte) (r * 9 + c);
            row = r;
            column = c;
            square = s;
            digit.Add(d);
            options.Add(new List<byte> {1, 2, 3, 4, 5, 6, 7, 8, 9});
        }

        public byte? Digit
        {
            get => digit[depth];
            set
            {
                digit[depth] = value;
            }
        }

        public List<byte> Options
        {
            get => options[depth];
            set => options[depth] = value;
        }

        public bool Test
        {
            get => test;
            set => test = value;
        }

        public byte Id => id;

        public static bool operator == (Tile a, Tile b)
        {
            return (a?.row == b?.row || a?.column == b?.column || a?.square == b?.square);
        }
        
        public static bool operator != (Tile a, Tile b)
        {
            return (a?.row != b?.row && a?.column != b?.column && a?.square != b?.square);
        }

        public override string ToString()
        {
            return digit[depth].ToString();
        }

        public void FindTileOptions(Tile[] board)
        {
            if (digit[depth] != null)
            {
                options[depth].Clear();
                return;
            }

            foreach (var tile in board)
            {
                if (this == tile && tile.Digit != null)
                {
                    options[depth].Remove((byte) tile.Digit);
                }
            }
        }

        public bool CheckTile(Tile[] board)
        {
            foreach (var tile in board)
            {
                if (tile == this)
                {
                    if (tile.Digit == Digit)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public void RemoveOption(Tile alteredTile, bool guess)
        {
            if (guess) { depth++; }

            if (options.Count < depth + 1)
            {
                options.Add(new List<byte>());
                options[depth].AddRange(options[depth - 1]);
                digit.Add(digit[depth - 1]);
            }
            
            if (this != alteredTile || !alteredTile.Digit.HasValue) { return; }
            
            options[depth].Remove((byte) alteredTile.Digit);
        }

        public bool SetOptionsBack()
        {
            options.RemoveAt(depth--);

            if (TestIds.Count == 0)
            {
                return false;
            }

            if (id == TestIds[^1])
            {
                options[depth].Remove((byte) digit[depth + 1]);
                test = false;
            }
            
            digit.RemoveAt(digit.Count - 1);
            return true;
        }
    }
}