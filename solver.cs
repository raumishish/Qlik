using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Collections;

namespace Sudoku
{
    public class solver
    {
        protected ArrayList[,] tempGridValues;
        protected ArrayList unique;
        protected ArrayList alternatives;
        protected string[,] snapshotValues;
        protected int altRow;
        protected int altCol;
        protected int altIndex;
        protected ArrayList inconsistentCells;
        protected Sudoku receivedObject;
        public solver(Sudoku obj)
        {
            receivedObject = obj;
            tempGridValues = new ArrayList[9, 9];
            unique = new ArrayList();
            alternatives = new ArrayList();
            snapshotValues = new string[9, 9];
            altRow = 0;
            altCol = 0;
            altIndex = 0;
            inconsistentCells = new ArrayList();
        }
        public void solve()
        {
            if (solveSudoku())
            {
                solve();
            }
        }
        public bool solveSudoku()
        {
            if (!IsGridConsistent())
            {
                string cell = "";
                for (int i = 0; i < inconsistentCells.Count; i++)
                {
                    cell += inconsistentCells[i] + " ";
                    if (i > 1 && i % 10 == 0)
                    {
                        cell += "\n";
                    }
                }
                MessageBox.Show("Grid is not consistent:\n" + inconsistentCells.Count + " redundant entries in rows, columns or 3x3 boxes\nAffected cells: " + cell, "Sudoku Solver", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false; //avsluta
            }
            findPossibleCandidates();
            findUniqueCandidates();
            resolveVeryUnique();
            if (Done())
            {
                if (alternatives.Count > 0)
                {
                    SetCellFinal(altRow, altCol, alternatives[altIndex].ToString());
                }
                reset();
                return false;
            }

            else if (unique.Count == 0)
            {
                return resolveByTrying();
            }
            
            return true;
        }
        private void findPossibleCandidates()
        {
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if (receivedObject.GetValueAt(row, col).Equals(string.Empty)) //empty grid 
                    {
                        tempGridValues[row, col] = findCandidates(row, col);
                    }
                    else
                    {
                        tempGridValues[row, col] = null;
                    }
                }

            }
        }

        private ArrayList findCandidates(int myRow, int myCol)
        {
            ArrayList rowExcludes = new ArrayList();
            ArrayList colExcludes = new ArrayList();
            ArrayList gridExcludes = new ArrayList();
            ArrayList finalCandidates = new ArrayList();
            if (myRow < 3 && myCol < 3)
            {
                for (int row = 0; row < 3; row++)
                {
                    for (int col = 0; col < 3; col++)
                    {
                        string value = receivedObject.GetValueAt(row, col);
                        if (!value.Equals(string.Empty))
                        {
                            gridExcludes.Add(value);
                        }
                    }
                }
            }
            else if (myRow < 3 && myCol >= 3 && myCol < 6)
            {
                for (int row = 0; row < 3; row++)
                {
                    for (int col = 3; col < 6; col++)
                    {
                        string value = receivedObject.GetValueAt(row, col);
                        if (!value.Equals(string.Empty))
                        {
                            gridExcludes.Add(value);
                        }
                    }
                }
            }
            else if (myRow < 3 && myCol >= 6)
            {
                for (int row = 0; row < 3; row++)
                {
                    for (int col = 6; col < 9; col++)
                    {
                        string value = receivedObject.GetValueAt(row, col);
                        if (!value.Equals(string.Empty))
                        {
                            gridExcludes.Add(value);
                        }
                    }
                }
            }
            else if (myRow >= 3 && myRow < 6 && myCol < 3) //middle left 3x3 box
            {
                for (int row = 3; row < 6; row++)
                {
                    for (int col = 0; col < 3; col++)
                    {
                        string value = receivedObject.GetValueAt(row, col);
                        if (!value.Equals(string.Empty))
                        {
                            gridExcludes.Add(value);
                        }
                    }
                }
            }
            else if (myRow >= 3 && myRow < 6 && myCol >= 3 && myCol < 6) //middle 3x3 box
            {
                for (int row = 3; row < 6; row++)
                {
                    for (int col = 3; col < 6; col++)
                    {
                        string value = receivedObject.GetValueAt(row, col);
                        if (!value.Equals(string.Empty))
                        {
                            gridExcludes.Add(value);
                        }
                    }
                }
            }
            else if (myRow >= 3 && myRow < 6 && myCol >= 6) //middle right 3x3 box
            {
                for (int row = 3; row < 6; row++)
                {
                    for (int col = 6; col < 9; col++)
                    {
                        string value = receivedObject.GetValueAt(row, col);
                        if (!value.Equals(string.Empty))
                        {
                            gridExcludes.Add(value);
                        }
                    }
                }
            }
            else if (myRow >= 6 && myCol < 3) //bottom left 3x3 box
            {
                for (int row = 6; row < 9; row++)
                {
                    for (int col = 0; col < 3; col++)
                    {
                        string value = receivedObject.GetValueAt(row, col);
                        if (!value.Equals(string.Empty))
                        {
                            gridExcludes.Add(value);
                        }
                    }
                }
            }
            else if (myRow >= 6 && myCol >= 3 && myCol < 6) //bottom middle 3x3 box
            {
                for (int row = 6; row < 9; row++)
                {
                    for (int col = 3; col < 6; col++)
                    {
                        string value = receivedObject.GetValueAt(row, col);
                        if (!value.Equals(string.Empty))
                        {
                            gridExcludes.Add(value);
                        }
                    }
                }
            }
            else if (myRow >= 6 && myCol >= 6) //bottom right 3x3 box
            {
                for (int row = 6; row < 9; row++)
                {
                    for (int col = 6; col < 9; col++)
                    {
                        string value = receivedObject.GetValueAt(row, col);
                        if (!value.Equals(string.Empty))
                        {
                            gridExcludes.Add(value);
                        }
                    }
                }
            }
            for (int col = 0; col < 9; col++)
            {
                string value = receivedObject.GetValueAt(myRow, col);
                if (!value.Equals(string.Empty))
                {
                    rowExcludes.Add(value);
                }
            }
            for (int row = 0; row < 9; row++)
            {
                string value = receivedObject.GetValueAt(row, myCol);
                if (!value.Equals(string.Empty))
                {
                    colExcludes.Add(value);
                }

            }
            //any number not listed in row, col or grid is a candidate
            for (int cand = 1; cand <= 9; cand++)
            {
                string candidate = cand.ToString();
                if (!(rowExcludes.Contains(candidate) || colExcludes.Contains(candidate) || gridExcludes.Contains(candidate)))
                {
                    finalCandidates.Add(candidate);
                }
            }
            return finalCandidates;
        }

        private void findUniqueCandidates()
        {
            unique.Clear();
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if (tempGridValues[row, col] != null && tempGridValues[row, col].Count == 1)
                    {
                        string uniqueSolution = (string)tempGridValues[row, col][0];
                        SetCellFinal(row, col, uniqueSolution);
                        unique.Add(uniqueSolution);
                    }
                }
            }
        }
        private void SetCellFinal(int myRow, int myCol, string value)
        {
            receivedObject.SetValueAt(myRow, myCol, value);
            findPossibleCandidates();
        }
        private void resolveVeryUnique()
        {
            for (int row = 0; row < 9; row++)
            {
                oneTimeRowResolve(row);
            }


            for (int col = 0; col < 9; col++)
            {
                oneTimeColResolve(col);
            }
            onceTimeInBox(0, 3, 0, 3);
            onceTimeInBox(0, 3, 3, 6);
            onceTimeInBox(0, 3, 6, 9);
            onceTimeInBox(3, 6, 0, 3);
            onceTimeInBox(3, 6, 3, 6);
            onceTimeInBox(3, 6, 6, 9);
            onceTimeInBox(6, 9, 0, 3);
            onceTimeInBox(6, 9, 3, 6);
            onceTimeInBox(6, 9, 6, 9);
        }
        private void oneTimeRowResolve(int myRow)
        {
            for (int number = 0; number < 9; number++)
            {
                if (countNumberInRow(number, myRow) == 1)
                {
                    for (int col = 0; col < 9; col++)
                    {
                        if (tempGridValues[myRow, col] != null && tempGridValues[myRow, col].Contains(number.ToString()))
                        {
                            string veryUnique = number.ToString();
                            SetCellFinal(myRow, col, veryUnique);
                        }
                    }
                }
            }
        }
        private void oneTimeColResolve(int myCol)
        {
            for (int i = 0; i < 9; i++)
            {
                if (countNumberInCol(i, myCol) == 1)
                {
                    for (int row = 0; row < 9; row++)
                    {
                        if (tempGridValues[row, myCol] != null && tempGridValues[row, myCol].Contains(i.ToString()))
                        {
                            string veryUnique = i.ToString();
                            SetCellFinal(row, myCol, veryUnique);
                        }
                    }
                }
            }
        }
        private void onceTimeInBox(int startRow, int endRow, int startCol, int endCol)
        {
            for (int number = 0; number < 9; number++)
            {
                if (CountOccurencesInBox(number, startRow, endRow, startCol, endCol) == 1)
                {
                    for (int row = startRow; row < endRow; row++)
                    {
                        for (int col = startCol; col < endCol; col++)
                        {
                            if (tempGridValues[row, col] != null && tempGridValues[row, col].Contains(number.ToString()))
                            {
                                string uniqueSolution = number.ToString();
                                SetCellFinal(row, col, uniqueSolution);
                            }
                        }
                    }
                }
            }
        }
        private int CountOccurencesInBox(int number, int startRow, int endRow, int startCol, int endCol)
        {
            int repetitions = 0;
            for (int row = startRow; row < endRow; row++)
            {
                for (int col = startCol; col < endCol; col++)
                {
                    if (tempGridValues[row, col] != null)
                    {
                        foreach (string candidate in tempGridValues[row, col])
                        {
                            if (Convert.ToInt32(candidate) == number)
                            {
                                repetitions++;
                            }
                        }
                    }
                }
            }
            return repetitions;
        }

        private int countNumberInRow(int number, int myRow)
        {
            int repetitions = 0;
            for (int col = 0; col < 9; col++)
            {
                if (tempGridValues[myRow, col] != null)
                {
                    foreach (string candidate in tempGridValues[myRow, col])
                    {
                        if (Convert.ToInt32(candidate) == number)
                        {
                            repetitions++;
                        }
                    }
                }
            }
            return repetitions;
        }
        private int countNumberInCol(int number, int myCol)
        {
            int repetitions = 0;
            for (int row = 0; row < 9; row++)
            {
                if (tempGridValues[row, myCol] != null)
                {
                    foreach (string candidate in tempGridValues[row, myCol])
                    {
                        if (Convert.ToInt32(candidate) == number)
                        {
                            repetitions++;
                        }
                    }
                }
            }
            return repetitions;
        }
        private bool Done()
        {
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if (receivedObject.GetValueAt(row, col).Equals(string.Empty)) //empty cell
                    {
                        return false;
                    }
                }
            }
            return IsGridConsistent();
        }
        private bool IsGridConsistent()
        {
            //check consistency of whole grid: no redundant entries
            ArrayList sqrExclude = new ArrayList();
            ArrayList colExclude = new ArrayList();
            ArrayList rowExclude = new ArrayList();
            inconsistentCells.Clear();

            //check consistency of top left 3x3 box
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    string myValue = receivedObject.GetValueAt(row, col);
                    if (sqrExclude.Contains(myValue))
                    {
                        string cell = "(" + (row + 1) + "," + (col + 1) + ")";
                        if (!inconsistentCells.Contains(cell)) inconsistentCells.Add(cell);
                    }
                    else if (!myValue.Equals(string.Empty))
                    {
                        sqrExclude.Add(myValue);
                    }
                }
            }

            sqrExclude.Clear();
            //check consistency of top middle 3x3 box
            for (int row = 0; row < 3; row++)
            {
                for (int col = 3; col < 6; col++)
                {
                    string myValue = receivedObject.GetValueAt(row, col);
                    if (sqrExclude.Contains(myValue))
                    {
                        string cell = "(" + (row + 1) + "," + (col + 1) + ")";
                        if (!inconsistentCells.Contains(cell)) inconsistentCells.Add(cell);
                    }
                    else if (!myValue.Equals(string.Empty))
                    {
                        sqrExclude.Add(myValue);
                    }
                }
            }

            sqrExclude.Clear();
            //check consistency of top right 3x3 box
            for (int row = 0; row < 3; row++)
            {
                for (int col = 6; col < 9; col++)
                {
                    string myValue = receivedObject.GetValueAt(row, col);
                    if (sqrExclude.Contains(myValue))
                    {
                        string cell = "(" + (row + 1) + "," + (col + 1) + ")";
                        if (!inconsistentCells.Contains(cell)) inconsistentCells.Add(cell);
                    }
                    else if (!myValue.Equals(string.Empty))
                    {
                        sqrExclude.Add(myValue);
                    }
                }
            }

            sqrExclude.Clear();
            //check consistency of middle left 3x3 box
            for (int row = 3; row < 6; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    string myValue = receivedObject.GetValueAt(row, col);
                    if (sqrExclude.Contains(myValue))
                    {
                        string cell = "(" + (row + 1) + "," + (col + 1) + ")";
                        if (!inconsistentCells.Contains(cell)) inconsistentCells.Add(cell);
                    }
                    else if (!myValue.Equals(string.Empty))
                    {
                        sqrExclude.Add(myValue);
                    }
                }
            }

            sqrExclude.Clear();
            //check consistency of middle 3x3 box
            for (int row = 3; row < 6; row++)
            {
                for (int col = 3; col < 6; col++)
                {
                    string myValue = receivedObject.GetValueAt(row, col);
                    if (sqrExclude.Contains(myValue))
                    {
                        string cell = "(" + (row + 1) + "," + (col + 1) + ")";
                        if (!inconsistentCells.Contains(cell)) inconsistentCells.Add(cell);
                    }
                    else if (!myValue.Equals(string.Empty))
                    {
                        sqrExclude.Add(myValue);
                    }
                }
            }

            sqrExclude.Clear();
            //check consistency of middle right 3x3 box
            for (int row = 3; row < 6; row++)
            {
                for (int col = 6; col < 9; col++)
                {
                    string myValue = receivedObject.GetValueAt(row, col);
                    if (sqrExclude.Contains(myValue))
                    {
                        string cell = "(" + (row + 1) + "," + (col + 1) + ")";
                        if (!inconsistentCells.Contains(cell)) inconsistentCells.Add(cell);
                    }
                    else if (!myValue.Equals(string.Empty))
                    {
                        sqrExclude.Add(myValue);
                    }
                }
            }

            sqrExclude.Clear();
            //check consistency of bottom left 3x3 box
            for (int row = 6; row < 9; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    string myValue = receivedObject.GetValueAt(row, col);
                    if (sqrExclude.Contains(myValue))
                    {
                        string cell = "(" + (row + 1) + "," + (col + 1) + ")";
                        if (!inconsistentCells.Contains(cell)) inconsistentCells.Add(cell);
                    }
                    else if (!myValue.Equals(string.Empty))
                    {
                        sqrExclude.Add(myValue);
                    }
                }
            }

            sqrExclude.Clear();
            //check consistency of bottom middle 3x3 box
            for (int row = 6; row < 9; row++)
            {
                for (int col = 3; col < 6; col++)
                {
                    string myValue = receivedObject.GetValueAt(row, col);
                    if (sqrExclude.Contains(myValue))
                    {
                        string cell = "(" + (row + 1) + "," + (col + 1) + ")";
                        if (!inconsistentCells.Contains(cell)) inconsistentCells.Add(cell);
                    }
                    else if (!myValue.Equals(string.Empty))
                    {
                        sqrExclude.Add(myValue);
                    }
                }
            }

            sqrExclude.Clear();
            //check consistency of bottom right 3x3 box
            for (int row = 6; row < 9; row++)
            {
                for (int col = 6; col < 9; col++)
                {
                    string myValue = receivedObject.GetValueAt(row, col);
                    if (sqrExclude.Contains(myValue))
                    {
                        string cell = "(" + (row + 1) + "," + (col + 1) + ")";
                        if (!inconsistentCells.Contains(cell)) inconsistentCells.Add(cell);
                    }
                    else if (!myValue.Equals(string.Empty))
                    {
                        sqrExclude.Add(myValue);
                    }
                }
            }

            //check consistency of rows
            for (int row = 0; row < 9; row++)
            {
                rowExclude.Clear();
                for (int col = 0; col < 9; col++)
                {
                    string myValue = receivedObject.GetValueAt(row, col);
                    if (rowExclude.Contains(myValue))
                    {
                        string cell = "(" + (row + 1) + "," + (col + 1) + ")";
                        if (!inconsistentCells.Contains(cell)) inconsistentCells.Add(cell);
                    }
                    else if (!myValue.Equals(string.Empty))
                    {
                        rowExclude.Add(myValue);
                    }
                }
            }

            //check consistency of columns
            for (int col = 0; col < 9; col++)
            {
                colExclude.Clear();
                for (int row = 0; row < 9; row++)
                {
                    string myValue = receivedObject.GetValueAt(row, col);
                    if (colExclude.Contains(myValue))
                    {
                        string cell = "(" + (row + 1) + "," + (col + 1) + ")";
                        if (!inconsistentCells.Contains(cell)) inconsistentCells.Add(cell);
                    }
                    else if (!myValue.Equals(string.Empty))
                    {
                        colExclude.Add(myValue);
                    }
                }
            }
            return (inconsistentCells.Count == 0);
        }

        private bool resolveByTrying()
        {
            if (alternatives.Count == 0)
            {
                backupGrid();
                altRow = 0;
                altCol = 0;
                altIndex = 0;
                for (int row = 0; row < 9; row++)
                {
                    for (int col = 0; col < 9; col++)
                    {
                        if (receivedObject.GetValueAt(row, col).Equals(string.Empty))
                        {
                            altRow = row;
                            altCol = col;
                            alternatives = findCandidates(row, col);
                            row = col = 8;
                        }
                    }
                }
            }
            else
            {
                altIndex++;
                if (altIndex <= alternatives.Count - 1)
                {
                    revertGrid();
                    SetCellCand(altRow, altCol, alternatives[altIndex].ToString());
                }
                else
                {
                    MessageBox.Show("Unsolvable board!");
                    receivedObject.SetValueAt(altRow, altIndex, string.Empty);
                    reset();
                    return false;
                }
            }
            return true;
        }

        private void SetCellCand(int myRow, int myCol, string value)
        {
            receivedObject.SetValueAt(myRow, myCol, value);
        }
        private void backupGrid()
        {
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    snapshotValues[row, col] = receivedObject.GetValueAt(row, col);
                }
            }
        }
        private void revertGrid()
        {
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    string value = snapshotValues[row, col];
                    receivedObject.SetValueAt(row, col, value);
                }
            }
        }
        private void reset()
        {
            alternatives.Clear();
        }

        private void printmecands(ArrayList l, int myRow, int myCol)
        {
            l.TrimToSize();
            Console.WriteLine("incommen array är " + l.Capacity);
            for (int i = 0; i < l.Capacity; i++)
            {
                Console.WriteLine("finalcand är " + l[i] + " för " + myRow + "," + myCol);
            }
        }

    }

}
