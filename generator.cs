using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace Sudoku
{
    public class generator
    {
        private int[,] grid = new int[9, 9];
        private int easyBlanks = 30; //add more later
        private int currentBlanks;
        private int[,] tempGrid;
        protected Sudoku receivedObject;
        public generator(Sudoku obj)
        {
            receivedObject = obj;
        }
        public void generatorStart()
        {
            Init(ref grid);
            Update(ref grid, 10);
            removeCells();
            printGrid(grid);
            saveSudoku();
        }
        private void Init(ref int[,] grid)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    grid[i, j] = (i * 3 + i / 3 + j) % 9 + 1; //fantastisk jävla rad
                }
               
            }
        }
        private void Update(ref int[,] grid, int shuffleLevel)
        {
            for (int repeat = 0; repeat < shuffleLevel; repeat++)
            {
                Random rand = new Random(Guid.NewGuid().GetHashCode());
                Random rand2 = new Random(Guid.NewGuid().GetHashCode());
                ChangeTwoCell(ref grid, rand.Next(1, 9), rand2.Next(1, 9));
            }
        }
        private void ChangeTwoCell(ref int[,] grid, int findValue1, int findValue2)
        {
            int xParm1, yParm1, xParm2, yParm2;
            xParm1 = yParm1 = xParm2 = yParm2 = 0;
            for (int i = 0; i < 9; i += 3)
            {
                for (int k = 0; k < 9; k += 3)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        for (int z = 0; z < 3; z++)
                        {
                            if (grid[i + j, k + z] == findValue1)
                            {
                                xParm1 = i + j;
                                yParm1 = k + z;
                            }

                            if (grid[i + j, k + z] == findValue2)
                            {
                                xParm2 = i + j;
                                yParm2 = k + z;
                            }
                        }
                    }

                    grid[xParm1, yParm1] = findValue2;
                    grid[xParm2, yParm2] = findValue1;

                }
            }
        }
        private void removeCells()
        {
            while(currentBlanks < easyBlanks)
            {
                Random rd = new Random();
                int row = rd.Next(0, 8);
                int col = rd.Next(0, 8);
                tempGrid = grid;
                while (tempGrid[row, col] == 0) //behöver ej rensa redan tomma
                {
                    row = rd.Next(0, 8);
                    col = rd.Next(0, 8);
                }
                tempGrid[row, col] = 0;
                currentBlanks++;
            }
        }
        private void saveSudoku()
        {
            saveNload s = new saveNload(receivedObject);
            s.saveSudoku(grid);
        }
        private void printGrid(int[,] grid)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if(grid[i,j] != 0)
                    {
                        receivedObject.SetValueAt(i, j, grid[i, j].ToString());
                    }
                    else
                    {
                        receivedObject.SetValueAt(i, j, "");
                    }
                }
            }
        }

    }
}
