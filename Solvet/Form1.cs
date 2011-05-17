/* Copyright (c) 2011 Chris Chen. 
 * All rights reserved. This program and the accompanying materials 
 * are made available under the terms of the Eclipse Public License v1.0 
 * which accompanies this distribution, and is available at 
 * http://www.eclipse.org/legal/epl-v10.html
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Net;
using System.Runtime.InteropServices;

namespace Solvet
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            doCalc();
        }

        private void writeLineTextBox1(string input, Color colour, FontStyle style)
        {
            string line = input;
            
            int oldLength = this.richTextBox1.Text.Length;

            this.richTextBox1.AppendText(line);

            richTextBox1.SelectionStart = oldLength;

            richTextBox1.SelectionLength = line.Length;

            richTextBox1.SelectionColor = colour;
            richTextBox1.SelectionFont = new System.Drawing.Font(richTextBox1.SelectionFont, style);
            richTextBox1.SelectionLength = 0;

        }

        private void deleteFromStartOfLine(int startOfLine)
        {
            this.richTextBox1.SelectionStart = startOfLine;
            this.richTextBox1.SelectionLength = this.richTextBox1.TextLength - this.richTextBox1.SelectionStart;
            this.richTextBox1.SelectedText = "";
            this.richTextBox1.SelectionLength = 0;
        }

        private void richTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13) //Enter
            {
                if (this.richTextBox1.SelectionStart < this.richTextBox1.TextLength) // if cursor not at the end
                {
                    this.richTextBox1.SelectionStart = this.richTextBox1.TextLength; // move cursor to end
                    e.Handled = true;
                }
                //else act as normal enter
            }
        }

        private void doCalc()
        {
            // get the exact location of cursor 
            int cursorPosition = richTextBox1.SelectionStart;
            // get the line in question
            int lineEdited = richTextBox1.GetLineFromCharIndex(richTextBox1.SelectionStart);

            string[] lines = this.richTextBox1.Text.Split('\n');

            string query = "";
            if (richTextBox1.SelectionLength == 0)
            {
                query = lines[lineEdited];
            }
            else
            {
                query = richTextBox1.SelectedText;
            }

            Calculation calculation = new Calculation(query);

            int startOfLine = 0;
            for (int i = 0; i < lineEdited; i++)
            {
                startOfLine += lines[i].Length + 1;
            }


            deleteFromStartOfLine(startOfLine);

            if (!calculation.Error)
            {
                writeLineTextBox1(calculation.Query + "\n", Color.Black, FontStyle.Regular);
                writeLineTextBox1(calculation.Input + " =" + "\n", Color.DarkBlue, FontStyle.Regular);
                writeLineTextBox1(calculation.Output + " ", Color.DarkGreen, FontStyle.Regular);

                this.textBox1.Text = calculation.Output;

                Clipboard.SetText(calculation.Output);

                this.toolStripStatusLabel1.Text = "Answer copied to clipboard.";
            }
            else
            {
                writeLineTextBox1(query, Color.DarkRed, FontStyle.Regular);
                this.textBox1.Text = "";

                //Don't show error if there is no query
                if (calculation.Query.Length == 0)
                {
                    this.toolStripStatusLabel1.Text = "";
                }
                else
                {
                    this.toolStripStatusLabel1.Text = calculation.ErrorText;
                }
            }

            richTextBox1.SelectionStart = cursorPosition; //Return cursor to previous position
            richTextBox1.Focus();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (richTextBox1.SelectionLength > 0)
            {
                richTextBox1.Cut();
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (richTextBox1.SelectionLength > 0)
            {
                richTextBox1.Copy();
            }
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text))
            {
                richTextBox1.Paste();
            }
        }

        private void selectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectAll();
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Options subForm = new Options(this);
            subForm.ShowDialog();
        }

        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            About subForm = new About();
            subForm.ShowDialog();
        }
    }
}
