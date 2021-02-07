using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            var game = SudokuGame.NewGame();
            Console.WriteLine(game.ToString());
            game.Crook();
            Console.WriteLine(game.ToString());



        }
    }

    public class SudokuGame
    {
        public List<Cell> Cells { get; set; }
        public SudokuGame()
        {
            Cells = new List<Cell>();
            for (int m = 1; m <= 9; m++)
            {
                for (int n = 1; n <= 9; n++)
                {
                    Cells.Add(new Cell(m, n));
                }
            }

        }
        public Cell this[int m, int n]
        {
            get { return Cells.SingleOrDefault(q => q.M == m && q.N == n); }
            set
            {
                var cell = Cells.SingleOrDefault(q => q.M == m && q.N == n);
                cell = value;
            }
        }

        public static SudokuGame NewGame()
        {
            var game = new SudokuGame();
            game[1, 2].Value = 9;
            game[1, 4].Value = 7;
            game[1, 7].Value = 8;
            game[1, 8].Value = 6;
            game[2, 2].Value = 3;
            game[2, 3].Value = 1;
            game[2, 6].Value = 5;
            game[2, 8].Value = 2;
            game[3, 1].Value = 8;
            game[3, 3].Value = 6;
            game[4, 3].Value = 7;
            game[4, 5].Value = 5;
            game[4, 9].Value = 6;
            game[5, 4].Value = 3;
            game[5, 6].Value = 7;
            game[6, 1].Value = 5;
            game[6, 5].Value = 1;
            game[6, 7].Value = 7;
            game[7, 7].Value = 1;
            game[7, 9].Value = 9;
            game[8, 2].Value = 2;
            game[8, 4].Value = 6;
            game[8, 7].Value = 3;
            game[8, 8].Value = 5;
            game[9, 2].Value = 5;
            game[9, 3].Value = 4;
            game[9, 6].Value = 8;
            game[9, 8].Value = 7;
            return game;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine(new string('-', 19));
            for (int m = 1; m <= 9; m++)
            {
                sb.Append('|');
                for (int n = 1; n <= 9; n++)
                {
                    sb.Append(this[m, n].Value == 0 ? " " : this[m, n].Value.ToString());
                    sb.Append('|');
                }
                sb.AppendLine();
                sb.AppendLine(new string('-', 19));
            }

            return sb.ToString();

        }

        public void RemovePossibilities()
        {
            var firstText = this.ToString();
            for (int m = 1; m <= 9; m++)
            {
                for (int n = 1; n <= 9; n++)
                {
                    var cell = this[m, n];

                    if (cell.Value == 0)
                    {
                        // remove row
                        for (int i = 1; i <= 9; i++)
                        {
                            if (cell == this[m, i])
                                continue;
                            var r = this[m, i].Value;
                            cell.Possibilities.RemoveAll(q => q == r);
                        }
                        // remove column
                        for (int i = 1; i <= 9; i++)
                        {
                            if (cell == this[i, n])
                                continue;
                            var c = this[i, n].Value;
                            cell.Possibilities.RemoveAll(q => q == c);
                        }

                        // remove small square
                        var startM = 1;
                        if (m > 6)
                            startM = 7;
                        else if (m > 3)
                            startM = 4;

                        var startN = 1;
                        if (n > 6)
                            startN = 7;
                        else if (n > 3)
                            startN = 4;

                        for (int i = startM; i < startM + 3; i++)
                        {
                            for (int j = startN; j < startN + 3; j++)
                            {
                                if (cell == this[i, j])
                                    continue;
                                if (this[i, j].Value > 0)
                                    cell.Possibilities.RemoveAll(q => q == this[i, j].Value);
                            }
                        }

                        if (cell.Possibilities.Count == 1)
                            cell.Value = cell.Possibilities[0];
                    }
                }
            }
            if (firstText != ToString())
                RemovePossibilities();
        }

        public void Crook()
        {
            var firstText = this.ToString();
            RemovePossibilities();
            for (int m = 1; m <= 9; m++)
            {
                for (int n = 1; n <= 9; n++)
                {
                    var cell = this[m, n];

                    if (cell.Value == 0)
                    {
                        for (int p = 0; p < cell.Possibilities.Count; p++)
                        {
                            var possibility = cell.Possibilities[p];

                            var possibilityCount = 0;
                            // row
                            for (int i = 1; i <= 9; i++)
                            {
                                if (this[m, i] != cell && this[i, n].Possibilities.Exists(q => q == possibility))
                                    possibilityCount++;
                            }
                            if (possibilityCount == 1)
                            {
                                cell.Value = possibility;
                                Crook();
                            }
                            possibilityCount = 0;
                            // column                           
                            for (int i = 1; i <= 9; i++)
                            {
                                if (this[i, n] != cell && this[i, n].Possibilities.Exists(q => q == possibility))
                                    possibilityCount++;
                            }
                            if (possibilityCount == 1)
                            {
                                cell.Value = possibility;
                                Crook(); ;
                            }

                            var startM = 1;
                            if (m > 6)
                                startM = 7;
                            else if (m > 3)
                                startM = 4;

                            var startN = 1;
                            if (n > 6)
                                startN = 7;
                            else if (n > 3)
                                startN = 4;
                             possibilityCount = 0;
                            for (int i = startM; i < startM + 3; i++)
                            {
                                for (int j = startN; j < startN + 3; j++)
                                {
                                    if (cell == this[i, j])
                                        continue;
                                    if (this[i, j].Possibilities.Exists(q => q == possibility))
                                        possibilityCount++;
                                }
                            }

                            if (possibilityCount == 1)
                            {
                                cell.Value = possibility;
                                Crook(); ;
                            }
                        }

                    }
                }
            }
            if (firstText != ToString())
                RemovePossibilities();
        }
    }

    public class Cell
    {
        public Cell(int m, int n, int value = 0)
        {
            M = m;
            N = n;
            Value = value;
            Possibilities = new List<int>();
            if (Value == 0)
            {
                for (int i = 1; i <= 9; i++)
                {
                    Possibilities.Add(i);
                }
            }
        }
        public int M { get; set; }
        public int N { get; set; }
        private int value = 0;
        public int Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
                if (value != 0)
                    Possibilities.Clear();
            }
        }
        public List<int> Possibilities { get; set; }
    }



}
