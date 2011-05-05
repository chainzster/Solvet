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

namespace Solvet
{
    public partial class Options : Form
    {
        Form1 mainForm;

        public Options(Form1 mainForm)
        {
            this.mainForm = mainForm;
            InitializeComponent();

            string result = "";

            string url = "http://www.google.com/supported_domains";

            try
            {
                WebClient client = new WebClient();
                result = client.DownloadString(url);

            }
            catch (Exception ex)
            {
                // handle error
                Console.WriteLine(ex.Message);
            }

            string[] lines = result.Split('\n');

            this.listBox1.Items.AddRange(lines);
            this.listBox1.Items.RemoveAt(this.listBox1.Items.Count - 1); //Removes the last entry which is blank and causes several errors if selected
            this.listBox1.Items.RemoveAt(this.listBox1.Items.Count - 1); //Removes .cat
            this.listBox1.Items.RemoveAt(0); //Removes .com

            this.listBox1.SelectedItem = Properties.Settings.Default.TLD;
            
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string current = (string)this.listBox1.SelectedItem;

            string result = "";
            string url = "http://www.iana.org/domains/root/db/" + current.Remove(0, current.Length - 2) + ".html";

            try
            {
                WebClient client = new WebClient();
                result = client.DownloadString(url);

            }
            catch (Exception ex)
            {
                // handle error
                Console.WriteLine(ex.Message);
            }

            string country = Utils.ExtractString("designed for <b>", "</b>)</p>", result);

            this.label1.Text = Utils.CleanHTML(country);
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.TLD = (string)this.listBox1.SelectedItem;
            Properties.Settings.Default.Save();
            this.Close();
        }

       
    }
}
