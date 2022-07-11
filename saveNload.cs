using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Sudoku
{
    public class saveNload
    {
        protected string[,] sudoBoard = new string[9, 9];
        protected Sudoku receivedObject;
        public saveNload(Sudoku obj)
        {
            receivedObject = obj;
        }
        public void saveSudoku(int[,] grid)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Title = "Save a generated board";
            saveFile.InitialDirectory = "e:\\Qlik\\sudoku\\";
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                StreamWriter writer = new StreamWriter(saveFile.OpenFile());
                saveData(writer,grid);
            }
            
        }
        public void saveData(StreamWriter writer,int[,] grid)
        {
            string temp = "";
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if (col == 8) //add newline in the end
                    {
                        if (grid[row, col].ToString().Equals("0"))
                        {
                            temp += "." + "\n";
                        }
                        else
                        {
                            temp += grid[row, col].ToString() + "\n";
                        }
                    }
                    else
                    {
                        if (grid[row, col].ToString().Equals("0"))
                        {
                            temp += "." + " ";
                        }
                        else
                        {
                            temp += grid[row, col].ToString() + " ";
                        }
                    }
                }
            }
            try
            {
                writer.Write(temp);
                writer.Dispose();
                writer.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }
        public void loadSudoku()
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Title = "Open a Sudoku game";
            openFile.InitialDirectory = "c:\\Qlik\\sudoku\\";
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                StreamReader sr = new StreamReader(openFile.FileName);
                String allData = sr.ReadToEnd();
                Console.WriteLine("alldata " + allData);
                loadData(allData);
            }
        }
        public bool loadData(string data)
        {
            bool loaded = false;
            int r = 0, c = 0;
            Regex numbers = new Regex("[1-9]");
            foreach (string row in data.Split('\n'))
            {
                string clean = RemoveWhitespace(row);
                //whitespaces completly gone, commong config now
                char[] singleChars = clean.ToCharArray();
                if (singleChars.Length == 9 || singleChars.Length == 0)
                {
                    c = 0;
                    foreach (char col in singleChars)
                    {
                        if (doTheRegex(col))
                        {
                            receivedObject.SetValueAt(r, c, col.ToString());
                            c++;
                        }
                        else
                        {
                            receivedObject.SetValueAt(r, c, "");
                            c++;
                        }
                    }
                    loaded = true;
                }
                else
                {
                    MessageBox.Show("Too many characters on any given row, aborting load");
                    loaded = false;
                    break;
                }
                r++;
            }
            return loaded;
        }
        public bool doTheRegex(char col)
        {
            Regex numbers = new Regex("[1-9]");
            if (numbers.IsMatch(col.ToString()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public string RemoveWhitespace(string input)
        {
            return new string(input.ToCharArray().Where(c => !Char.IsWhiteSpace(c)).ToArray());
        }
    }
}
