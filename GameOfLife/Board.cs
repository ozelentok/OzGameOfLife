using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace GameOfLife
{
    public enum CellStatus { dead, dying, born, alive };

    public class Board
    {
        /// <summary>        
        /// Game Grid
        /// 0 - dead, 1 - dying, 2 - born, 3 - alive
        /// </summary>
        public byte[,] Grid { get; private set; }
        public int Size { get; private set; }
        private Random rnd;

        /// <summary>
        /// initialises the game board
        /// </summary>
        public Board()
        {
            Size = 50;
            rnd = new Random();
            Grid = new byte[Size, Size];
            for (int i = 0; i < Grid.GetLength(0); i++)
            {
                for (int j = 0; j < Grid.GetLength(1); j++)
                { Grid[i, j] = (byte)rnd.Next(3); }
            }
        }
        /// <summary>
        /// Advances to the next generation
        /// </summary>
        public void TickNextGen()
        {
            int neightbours;
            // first pass to mark changes
            for (int row = 0; row < Grid.GetLength(0); row++)
            {
                for (int col = 0; col < Grid.GetLength(1); col++)
                {
                    neightbours = CountLiveNeighbours(row, col);
                    if (Grid[row, col] == (int)CellStatus.dead && neightbours == 3)
                        Grid[row, col] = (int)CellStatus.born;
                    else if (Grid[row, col] == (int)CellStatus.alive && neightbours != 2 && neightbours != 3)
                        Grid[row, col] = (int)CellStatus.dying;
                }
            }
            // second pass to make changes
            for (int row = 0; row < Grid.GetLength(0); row++)
            {
                for (int col = 0; col < Grid.GetLength(1); col++)
                {
                    if (Grid[row, col] == (int)CellStatus.dying)
                        Grid[row, col] = (int)CellStatus.dead;
                    else if (Grid[row, col] == (int)CellStatus.born)
                        Grid[row, col] = (int)CellStatus.alive;
                }
            }
        }
        
        /// <summary>
        /// Number of neighbours of a single cell
        /// </summary>
        /// <param name="Width"> Location of cell on Y - Axis</param>
        /// <param name="Height"> Location of cell on X - Axis</param>
        /// <returns> Number of neighbours</returns>
        public int CountLiveNeighbours(int row, int col)
        {
            int count = 0;
            for (int i = row - 1; i <= row + 1; i++)
            {
                for (int j = col - 1; j <= col + 1; j++)
                {
                    if(i >= 0 && i < Size && j >= 0 && j < Size && (i != row || j != col))
                    {
                        if(Grid[i, j] == (int) CellStatus.alive || Grid[i, j] == (int) CellStatus.dying)
                            count++;
                    }
                }
            }
            return count;
        }
    }
}
