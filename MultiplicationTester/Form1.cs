using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MultiplicationTester
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        List <int[]> sums;
        List<Panel> panels;

        private void Form1_Load(object sender, EventArgs e)
        {
            labelOutput.Text = "";
            GenerateSums(10);
            Display();
        }

        private void button_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            if(b.Text == "Проверить")
            {
                int rightAnswersCount = 0;
                for (int i = 0; i < sums.Count; i++)
                {
                    int answer = int.Parse(panels[i].Controls["numeric"].Text);
                    if (answer == sums[i][0] * sums[i][1])
                    {
                        rightAnswersCount++;
                        panels[i].BackColor = Color.LightGreen;
                        panels[i].Controls["rtb"].BackColor = Color.LightGreen;
                    }
                    else
                    {
                        panels[i].BackColor = Color.IndianRed;
                        panels[i].Controls["rtb"].BackColor = Color.IndianRed;
                    }
                    panels[i].Controls["numeric"].Enabled = false;
                }
                labelOutput.Text = "Правильных ответов " + rightAnswersCount + " из " + sums.Count;
                button.Text = "Начать заново";
            }
            else
            {
                labelOutput.Text = "";
                button.Text = "Проверить";
                GenerateSums(10);
                Display();
            }
        }

        private void GenerateSums(int count)
        {
            sums = new List<int[]>();
            Random random = new Random();
            for (int i = 0; i < count; i++)
            {
                int[] sum = new int[2];
                do
                {
                    int hundreds = random.Next(1, 10);
                    int tens;
                    int units;
                    if (random.NextDouble() <= 0.5)
                    {
                        tens = random.Next(1, 10);
                        units = 0;
                    }
                    else
                    {
                        tens = 0;
                        units = random.Next(1, 10);
                    }
                    sum[0] = hundreds * 100 + tens * 10 + units;
                    sum[1] = random.Next(2, 10);
                }
                while (sums.Contains(sum));
                sums.Add(sum);
            }
        }

        private void SelectAllOnEnter(object sender, EventArgs e)
        {
            (sender as NumericUpDown).Select(0, this.ActiveControl.ToString().Length);            
        }

        private void NextTabOnReturnOrSpace(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return || e.KeyCode == Keys.Space)
            {
                e.SuppressKeyPress = true;
                this.SelectNextControl(this.ActiveControl, true, true, true, true);
            }
        }

        private void Display()
        {
            panels = new List<Panel>();
            mainPanel.Controls.Clear();
            for (int i = 0; i < sums.Count; i++)
            {
                Panel panel = new Panel();
                panel.Location = new Point(5 + 90 * (i % 5), 5 + 97 * (i / 5)); 
                panel.Size = new Size(87, 91);
                panel.Name = "panel" + i;
                panel.BorderStyle = BorderStyle.FixedSingle;
                Font font = new Font("Microsoft Sans Serif", 14);
                RichTextBox rtb = new RichTextBox();
                rtb.Location = new Point(25, 5);
                rtb.Size = new Size(57, 47);
                rtb.ReadOnly = true;
                rtb.BackColor = panel.BackColor;
                rtb.Font = font;
                rtb.Text = sums[i][0].ToString() + "\n         " + sums[i][1].ToString();
                rtb.Name = "rtb";
                rtb.TabStop = false;
                rtb.SelectAll();
                rtb.SelectionAlignment = HorizontalAlignment.Right;
                rtb.DeselectAll();
                rtb.Select(rtb.Text.IndexOf('\n'), rtb.Text.Length - rtb.Text.IndexOf('\n'));
                rtb.SelectionFont = new Font("Microsoft Sans Serif", 14, FontStyle.Underline);
                rtb.DeselectAll();
                rtb.BorderStyle = BorderStyle.None;
                rtb.ScrollBars = RichTextBoxScrollBars.None;
                Label label = new Label();
                label.Text = "x";
                label.Font = font;
                label.AutoSize = true;
                label.Location = new Point(5, 18);
                NumericUpDown numeric = new NumericUpDown();
                numeric.Font = font;
                numeric.TextAlign = HorizontalAlignment.Right;
                numeric.UpDownAlign = LeftRightAlignment.Left;
                numeric.Value = 0;
                numeric.Maximum = 10000;
                numeric.Size = new Size(77, 29);
                numeric.Location = new Point(5, 56);
                numeric.Name = "numeric";
                numeric.Enter += new EventHandler(SelectAllOnEnter);
                numeric.KeyDown += new KeyEventHandler(NextTabOnReturnOrSpace);
                numeric.Click += new EventHandler(SelectAllOnEnter);
                panel.Controls.Add(rtb);
                panel.Controls.Add(label);
                panel.Controls.Add(numeric);
                panels.Add(panel);
            }
            foreach (Panel panel in panels)
            {
                mainPanel.Controls.Add(panel);
            }
        }
    }
}
