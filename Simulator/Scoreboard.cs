using System.Drawing;
using System.Windows.Forms;

namespace Simulator
{
    public partial class Scoreboard : Form
    {
        public Scoreboard(double fygdpg, double qgdpgv, double fybd, double qbdv)
        {
            InitializeComponent();

            EvaluateGdpGrowth(fygdpg, qgdpgv);
            EvaluateBudgetDeficit(fybd, qbdv);
            GradePerformance();
        }

        private void EvaluateGdpGrowth(double fy, double qv)
        {
            if (fy < 0.0)
                labelOverallGDPValue.ForeColor = Color.Red;
            else if (fy < 6.0)
                labelOverallGDPValue.ForeColor = Color.Orange;
            else
                labelOverallGDPValue.ForeColor = Color.Green;
            labelOverallGDPValue.Text = $"{fy:+0.0;-0.0}%";

            if (qv < 1.5)
            {
                labelGDPVariationValue.ForeColor = Color.Green;
                labelGDPVariationValue.Text = @"low";
            }
            else if (qv < 3.0)
            {
                labelGDPVariationValue.ForeColor = Color.Orange;
                labelGDPVariationValue.Text = @"medium";
            }
            else
            {
                labelGDPVariationValue.ForeColor = Color.Red;
                labelGDPVariationValue.Text = @"high";
            }
        }

        private void EvaluateBudgetDeficit(double fy, double qv)
        {
            if (fy < 22.5)
                labelOverallBDValue.ForeColor = Color.Green;
            else if (fy < 45.0)
                labelOverallBDValue.ForeColor = Color.Orange;
            else
                labelOverallBDValue.ForeColor = Color.Red;
            labelOverallBDValue.Text = $"{fy:+$0.0;-$0.0} b";

            if (qv < 2.0)
            {
                labelBDVariationValue.ForeColor = Color.Green;
                labelBDVariationValue.Text = @"low";
            }
            else if (qv < 4.0)
            {
                labelBDVariationValue.ForeColor = Color.Orange;
                labelBDVariationValue.Text = @"medium";
            }
            else
            {
                labelBDVariationValue.ForeColor = Color.Red;
                labelBDVariationValue.Text = @"high";
            }
        }

        private void GradePerformance()
        {
            double score = 0;

            if (labelOverallGDPValue.ForeColor == Color.Green)
                score += 4.0;
            else if (labelOverallGDPValue.ForeColor == Color.Red)
                score -= 4.0;

            if (labelGDPVariationValue.ForeColor == Color.Green)
                score += 1.0;
            else if (labelGDPVariationValue.ForeColor == Color.Red)
                score -= 1.0;

            if (labelOverallBDValue.ForeColor == Color.Green)
                score += 2.0;
            else if (labelOverallBDValue.ForeColor == Color.Red)
                score -= 2.0;

            if (labelBDVariationValue.ForeColor == Color.Green)
                score += 0.5;
            else if (labelBDVariationValue.ForeColor == Color.Red)
                score -= 0.5;

            if (score <= -2.5)
            {
                labelGrade.ForeColor = Color.Red;
                labelGrade.Text = @"Terrible";
            }
            else if (score < 2.5)
            {
                labelGrade.ForeColor = Color.Orange;
                labelGrade.Text = @"Mediocre";
            }
            else
            {
                labelGrade.ForeColor = Color.Green;
                labelGrade.Text = @"Excellent";
            }
        }

        private void Scoreboard_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}