using System;
using System.Windows.Forms;

namespace Simulator
{
    public partial class Form1 : Form
    {
        private Expenditure _expendit = new Expenditure();

        private enum Quarters { Q1 = 1, Q2 = 2, Q3 = 3, Q4 = 4 }

        private Quarters _quarter = Quarters.Q1;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.OldValue != e.NewValue)
            {
                label2.Left += (e.NewValue - e.OldValue) * 6;
                label2.Text = "Tax Rate (T)\n" + e.NewValue.ToString() + "% (of GDP)";

                chart5.Series[0].Points[4].YValues[0] = e.NewValue * _expendit.GetExpenditure() / 100;
                chart5.Refresh();
            }
        }

        private void hScrollBar2_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.OldValue != e.NewValue)
            {
                label3.Left += (e.NewValue - e.OldValue) * 6;
                label3.Text = "Gov't Spending (G)\n$" + e.NewValue.ToString() + " billion";

                chart5.Series[1].Points[4].YValues[0] = e.NewValue;
                chart5.Refresh();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (trackBar1.Value < 15)
                trackBar1.Value++;

            // Actual stuff

        }
    }
}
