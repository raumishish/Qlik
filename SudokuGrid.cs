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
    public partial class Sudoku : Form
    {
        protected string[,] sudoBoard;
        public Sudoku()
        {
            InitializeComponent();
            sudokuGrid = new RichTextBox[9, 9];
            // 1st box
            sudokuGrid[0, 0] = richTextBox1;
            sudokuGrid[0, 1] = richTextBox2;
            sudokuGrid[0, 2] = richTextBox3;
            sudokuGrid[1, 0] = richTextBox4;
            sudokuGrid[1, 1] = richTextBox5;
            sudokuGrid[1, 2] = richTextBox6;
            sudokuGrid[2, 0] = richTextBox7;
            sudokuGrid[2, 1] = richTextBox8;
            sudokuGrid[2, 2] = richTextBox9;
            // 2nd box
            sudokuGrid[0, 3] = richTextBox10;
            sudokuGrid[0, 4] = richTextBox11;
            sudokuGrid[0, 5] = richTextBox12;
            sudokuGrid[1, 3] = richTextBox13;
            sudokuGrid[1, 4] = richTextBox14;
            sudokuGrid[1, 5] = richTextBox15;
            sudokuGrid[2, 3] = richTextBox16;
            sudokuGrid[2, 4] = richTextBox17;
            sudokuGrid[2, 5] = richTextBox18;
            // 3rd box
            sudokuGrid[0, 6] = richTextBox19;
            sudokuGrid[0, 7] = richTextBox20;
            sudokuGrid[0, 8] = richTextBox21;
            sudokuGrid[1, 6] = richTextBox22;
            sudokuGrid[1, 7] = richTextBox23;
            sudokuGrid[1, 8] = richTextBox24;
            sudokuGrid[2, 6] = richTextBox25;
            sudokuGrid[2, 7] = richTextBox26;
            sudokuGrid[2, 8] = richTextBox27;

            // 4th box
            sudokuGrid[3, 0] = richTextBox28;
            sudokuGrid[3, 1] = richTextBox29;
            sudokuGrid[3, 2] = richTextBox30;
            sudokuGrid[4, 0] = richTextBox31;
            sudokuGrid[4, 1] = richTextBox32;
            sudokuGrid[4, 2] = richTextBox33;
            sudokuGrid[5, 0] = richTextBox34;
            sudokuGrid[5, 1] = richTextBox35;
            sudokuGrid[5, 2] = richTextBox36;
            // 5th box
            sudokuGrid[3, 3] = richTextBox37;
            sudokuGrid[3, 4] = richTextBox38;
            sudokuGrid[3, 5] = richTextBox39;
            sudokuGrid[4, 3] = richTextBox40;
            sudokuGrid[4, 4] = richTextBox41;
            sudokuGrid[4, 5] = richTextBox42;
            sudokuGrid[5, 3] = richTextBox43;
            sudokuGrid[5, 4] = richTextBox44;
            sudokuGrid[5, 5] = richTextBox45;
            // 6th box
            sudokuGrid[3, 6] = richTextBox46;
            sudokuGrid[3, 7] = richTextBox47;
            sudokuGrid[3, 8] = richTextBox48;
            sudokuGrid[4, 6] = richTextBox49;
            sudokuGrid[4, 7] = richTextBox50;
            sudokuGrid[4, 8] = richTextBox51;
            sudokuGrid[5, 6] = richTextBox52;
            sudokuGrid[5, 7] = richTextBox53;
            sudokuGrid[5, 8] = richTextBox54;
            // 7th box
            sudokuGrid[6, 0] = richTextBox55;
            sudokuGrid[6, 1] = richTextBox56;
            sudokuGrid[6, 2] = richTextBox57;
            sudokuGrid[7, 0] = richTextBox58;
            sudokuGrid[7, 1] = richTextBox59;
            sudokuGrid[7, 2] = richTextBox60;
            sudokuGrid[8, 0] = richTextBox61;
            sudokuGrid[8, 1] = richTextBox62;
            sudokuGrid[8, 2] = richTextBox63;
            // 8th box
            sudokuGrid[6, 3] = richTextBox64;
            sudokuGrid[6, 4] = richTextBox65;
            sudokuGrid[6, 5] = richTextBox66;
            sudokuGrid[7, 3] = richTextBox67;
            sudokuGrid[7, 4] = richTextBox68;
            sudokuGrid[7, 5] = richTextBox69;
            sudokuGrid[8, 3] = richTextBox70;
            sudokuGrid[8, 4] = richTextBox71;
            sudokuGrid[8, 5] = richTextBox72;
            // 9th box
            sudokuGrid[6, 6] = richTextBox73;
            sudokuGrid[6, 7] = richTextBox74;
            sudokuGrid[6, 8] = richTextBox75;
            sudokuGrid[7, 6] = richTextBox76;
            sudokuGrid[7, 7] = richTextBox77;
            sudokuGrid[7, 8] = richTextBox78;
            sudokuGrid[8, 6] = richTextBox79;
            sudokuGrid[8, 7] = richTextBox80;
            sudokuGrid[8, 8] = richTextBox81;
        }

        private void load_button_Click(object sender, EventArgs e)
        {
            s = new saveNload(this);
            s.loadSudoku();
            solveButton.Enabled = true;
        }
        private void solveButton_Click(object sender, EventArgs e)
        {
            solver s = new solver(this);
            s.solve();
        }
        private void generate_button_Click(object sender, EventArgs e)
        {
            generator g = new generator(this);
            g.generatorStart();
            solveButton.Enabled = true;
        }

        public string GetValueAt(int row, int column)
        {
            return sudokuGrid[row, column].Text;
        }
        public void SetValueAt(int row, int column, string val)
        {
            sudokuGrid[row, column].Text = val;
        }
    }
}
