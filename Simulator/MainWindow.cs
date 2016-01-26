using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Simulator
{
    public partial class MainWindow : Form
    {
        private readonly List<double> _deficits = new List<double>(15);
        private readonly Expenditure _expenditure = new Expenditure();
        private readonly double _govSpendingMultiplier = 2.07;

        private readonly List<double> _growth = new List<double>(15);

        private readonly double _incomeTaxMultiplier = -1.62;

        private readonly Introduction _introduction = new Introduction();
        private readonly Random _rng = new Random();

        private readonly List<int> _shocks = new List<int>(3);

        private Quarters _quarter = Quarters.Q1;
        private Scoreboard _scoreboard;
        private int _shockN;
        private Quarters _shockQ = Quarters.Q1;

        public MainWindow()
        {
            InitializeComponent();
            _introduction.ShowDialog();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            mainTabControl.SelectedIndex = 1;
            InitialGrowth();
            InitialDeficit();
            InitialShocks();
        }

        private void InitialGrowth()
        {
            var newGrowth = 0.0;
            for (var i = 3; i < 5; i++)
            {
                newGrowth = (chartOutput.Series[0].Points[i].YValues[0]/chartOutput.Series[0].Points[i - 1].YValues[0] -
                             1)*100;
                _growth.Add(newGrowth);
            }
            labelGrowth.ForeColor = Color.Green;
            labelGrowth.Text = $"Growth: {newGrowth:+0.0;-0.0}%";
            UpdateFreakonomist();
        }

        private void InitialDeficit()
        {
            var newDeficit = 0.0;
            for (var i = 2; i < 4; i++)
            {
                newDeficit = chartBudget.Series[1].Points[i].YValues[0] - chartBudget.Series[0].Points[i].YValues[0];
                _deficits.Add(newDeficit);
            }
            labelDeficit.ForeColor = Color.Red;
            labelDeficit.Text = $"Deficit: {newDeficit:+$0.0;-$0.0} b";
            UpdateFiscalTimes();
        }

        private void InitialShocks()
        {
            var r = _rng.Next(0, 2);
            if (r == 0)
                _shockQ = Quarters.Q4;
            for (var i = 1; i < 4; i++)
                _shocks.Add(i);
        }

        private void hScrollBarTaxRate_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.OldValue != e.NewValue)
            {
                labelTaxRate.Text = $"Effective Tax Rate (T)\n{e.NewValue/5.0:0.0}% of GDP";

                chartBudget.Series[0].Points[chartBudget.Series[0].Points.Count - 1].YValues[0] = e.NewValue*
                                                                                                  chartOutput.Series[0]
                                                                                                      .Points[
                                                                                                          chartOutput
                                                                                                              .Series[0]
                                                                                                              .Points
                                                                                                              .Count - 1
                                                                                                      ].YValues[0]/500.0;
                chartBudget.Refresh();
            }
        }

        private void hScrollBarSpending_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.OldValue != e.NewValue)
            {
                labelSpending.Text = $"Gov't Spending (G)\n${e.NewValue/5.0:0.0} billion";

                chartBudget.Series[1].Points[chartBudget.Series[1].Points.Count - 1].YValues[0] = e.NewValue/5.0;
                chartBudget.Refresh();
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (trackBarTime.Value < 15)
            {
                trackBarTime.Value++;
                CheckForShock();
                UpdateConsumption();
                UpdateInvestment();
                UpdateNetExports();
                UpdateExpenditure();
                UpdateTaxes();
                UpdateGovSpending();
                UpdateGrowth();
                UpdateDeficit();
                UpdateFreakonomist();
                UpdateFiscalTimes();

                if (_quarter != Quarters.Q4)
                    _quarter++;
                else
                    _quarter = Quarters.Q1;
            }
            if (trackBarTime.Value == 15)
                DisplayScoreboard();
        }

        private void CheckForShock()
        {
            _shockN = 0;
            if ((_quarter == Quarters.Q3 && _shockQ == Quarters.Q4) ||
                (_quarter == Quarters.Q4 && _shockQ == Quarters.Q1))
            {
                if (_shocks.Count > 0)
                {
                    _shockN = _shocks[_rng.Next(0, _shocks.Count)];
                    _shocks.Remove(_shockN);

                    switch (_shockN)
                    {
                        case 1:
                            _expenditure.Consumpt.MarginalPropensity -= 0.03;
                            break;

                        case 2:
                            _expenditure.Investment *= 0.89;
                            break;

                        case 3:
                            _expenditure.Exports *= 0.97;
                            break;
                    }
                }
            }
        }

        private void UpdateConsumption()
        {
            var newConsumption =
                _expenditure.Consumpt.GetConsumption(
                    chartOutput.Series[0].Points[chartOutput.Series[0].Points.Count - 1].YValues[0],
                    hScrollBarTaxRate.Value/500.0);
            chartConsumption.Series[0].Points.AddXY(chartConsumption.ChartAreas[0].Axes[0].Maximum + 1, newConsumption);
            chartConsumption.ChartAreas[0].Axes[0].Minimum += 2;
            chartConsumption.ChartAreas[0].Axes[0].Maximum += 2;
            var nextQuarter = new CustomLabel(chartConsumption.ChartAreas[0].Axes[0].Maximum - 2,
                chartConsumption.ChartAreas[0].Axes[0].Maximum, _quarter.ToString(), 0, LabelMarkStyle.None);
            chartConsumption.ChartAreas[0].Axes[0].CustomLabels.Insert(
                chartConsumption.ChartAreas[0].Axes[0].CustomLabels.Count, nextQuarter);
        }

        private void UpdateInvestment()
        {
            if (_rng.Next(2) == 0)
                _expenditure.Investment *= _rng.Next(97, 100)/100.0;
            else
                _expenditure.Investment *= _rng.Next(101, 104)/100.0;


            chartInvestment.Series[0].Points.AddXY(chartInvestment.ChartAreas[0].Axes[0].Maximum + 1,
                _expenditure.Investment);
            chartInvestment.ChartAreas[0].Axes[0].Minimum += 2;
            chartInvestment.ChartAreas[0].Axes[0].Maximum += 2;
            var nextQuarter = new CustomLabel(chartInvestment.ChartAreas[0].Axes[0].Maximum - 2,
                chartInvestment.ChartAreas[0].Axes[0].Maximum, _quarter.ToString(), 0, LabelMarkStyle.None);
            chartInvestment.ChartAreas[0].Axes[0].CustomLabels.Insert(
                chartInvestment.ChartAreas[0].Axes[0].CustomLabels.Count, nextQuarter);
        }

        private void UpdateNetExports()
        {
            if (_rng.Next(2) == 0)
                _expenditure.Exports *= _rng.Next(990, 1000)/1000.0;
            else
                _expenditure.Exports *= _rng.Next(1001, 1011)/1000.0;

            chartNetExports.Series[0].Points.AddXY(chartNetExports.ChartAreas[0].Axes[0].Maximum + 1,
                _expenditure.NetExports());
            chartNetExports.ChartAreas[0].Axes[0].Minimum += 2;
            chartNetExports.ChartAreas[0].Axes[0].Maximum += 2;
            var nextQuarter = new CustomLabel(chartNetExports.ChartAreas[0].Axes[0].Maximum - 2,
                chartNetExports.ChartAreas[0].Axes[0].Maximum, _quarter.ToString(), 0, LabelMarkStyle.None);
            chartNetExports.ChartAreas[0].Axes[0].CustomLabels.Insert(
                chartNetExports.ChartAreas[0].Axes[0].CustomLabels.Count, nextQuarter);
        }

        private void UpdateExpenditure()
        {
            var newConsumption =
                chartConsumption.Series[0].Points[chartConsumption.Series[0].Points.Count - 1].YValues[0];

            var multGovSpending = (chartBudget.Series[1].Points[chartBudget.Series[1].Points.Count - 1].YValues[0] -
                                   chartBudget.Series[1].Points[chartBudget.Series[1].Points.Count - 2].YValues[0])*
                                  (_govSpendingMultiplier - 1.0);

            var multIncomeTax = (chartBudget.Series[0].Points[chartBudget.Series[0].Points.Count - 1].YValues[0] -
                                 chartBudget.Series[0].Points[chartBudget.Series[0].Points.Count - 2].YValues[0])*
                                (_incomeTaxMultiplier + 1.0);

            var newExpenditure = _expenditure.GetExpenditure(newConsumption,
                chartBudget.Series[1].Points[chartBudget.Series[1].Points.Count - 1].YValues[0]);
            newExpenditure += multGovSpending;
            newExpenditure += multIncomeTax;

            chartOutput.Series[0].Points.AddXY(chartOutput.ChartAreas[0].Axes[0].Maximum + 1, newExpenditure);
            chartOutput.ChartAreas[0].Axes[0].Minimum += 2;
            chartOutput.ChartAreas[0].Axes[0].Maximum += 2;
            var nextQuarter = new CustomLabel(chartOutput.ChartAreas[0].Axes[0].Maximum - 2,
                chartOutput.ChartAreas[0].Axes[0].Maximum, _quarter.ToString(), 0, LabelMarkStyle.None);
            chartOutput.ChartAreas[0].Axes[0].CustomLabels.Insert(chartOutput.ChartAreas[0].Axes[0].CustomLabels.Count,
                nextQuarter);
        }

        private void UpdateTaxes()
        {
            var newTaxes = hScrollBarTaxRate.Value/500.0*
                           chartOutput.Series[0].Points[chartOutput.Series[0].Points.Count - 1].YValues[0];
            chartBudget.Series[0].Points.AddXY(chartBudget.ChartAreas[0].Axes[0].Maximum + 1, newTaxes);
            chartBudget.Series[0].Points[chartBudget.Series[0].Points.Count - 2].BorderDashStyle = ChartDashStyle.Solid;
            chartBudget.Series[0].Points[chartBudget.Series[0].Points.Count - 1].BorderDashStyle = ChartDashStyle.Dot;
            chartBudget.ChartAreas[0].Axes[0].Minimum += 2;
            chartBudget.ChartAreas[0].Axes[0].Maximum += 2;
            chartBudget.ChartAreas[0].Axes[0].CustomLabels[chartBudget.ChartAreas[0].Axes[0].CustomLabels.Count - 1]
                .Text = _quarter.ToString();
            var future = new CustomLabel(chartBudget.ChartAreas[0].Axes[0].Maximum - 2,
                chartBudget.ChartAreas[0].Axes[0].Maximum, "Future", 0, LabelMarkStyle.None);
            chartBudget.ChartAreas[0].Axes[0].CustomLabels.Insert(chartBudget.ChartAreas[0].Axes[0].CustomLabels.Count,
                future);
        }

        private void UpdateGovSpending()
        {
            chartBudget.Series[1].Points.AddXY(chartBudget.ChartAreas[0].Axes[0].Maximum - 1,
                hScrollBarSpending.Value/5.0);
            chartBudget.Series[1].Points[chartBudget.Series[1].Points.Count - 2].BorderDashStyle = ChartDashStyle.Solid;
            chartBudget.Series[1].Points[chartBudget.Series[1].Points.Count - 1].BorderDashStyle = ChartDashStyle.Dot;
        }

        private void UpdateGrowth()
        {
            var newGrowth = (chartOutput.Series[0].Points[chartOutput.Series[0].Points.Count - 1].YValues[0]/
                             chartOutput.Series[0].Points[chartOutput.Series[0].Points.Count - 2].YValues[0] - 1)*100;
            _growth.Add(newGrowth);
            labelGrowth.ForeColor = newGrowth > 0.0 ? Color.Green : Color.Red;
            labelGrowth.Text = $"Growth: {newGrowth:+0.0;-0.0}%";
        }

        private void UpdateDeficit()
        {
            var newDeficit = chartBudget.Series[1].Points[chartBudget.Series[1].Points.Count - 2].YValues[0] -
                             chartBudget.Series[0].Points[chartBudget.Series[0].Points.Count - 2].YValues[0];
            _deficits.Add(newDeficit);
            labelDeficit.ForeColor = newDeficit > 0.0 ? Color.Red : Color.Green;
            labelDeficit.Text = $"Deficit: {newDeficit:+$0.0;-$0.0} b";
        }

        private void UpdateFreakonomist()
        {
            if (_shockN == 0)
            {
                var secondLastGrowth = _growth[_growth.Count - 2];
                var lastGrowth = _growth[_growth.Count - 1];

                if (secondLastGrowth <= 0)
                {
                    if (lastGrowth <= 0)
                    {
                        labelFreakonomist.Text = @"GDP growth still negative";
                        textBoxFreakonomist.Text = @"Recession of the economy persists.";
                    }
                    else
                    {
                        labelFreakonomist.Text = @"Gross Domestic Product rising";
                        textBoxFreakonomist.Text = @"Economy begins to expand.";
                    }
                }

                else
                {
                    if (lastGrowth <= 0)
                    {
                        labelFreakonomist.Text = @"Gross Domestic Product falling";
                        textBoxFreakonomist.Text = @"Economy starts contracting.";
                    }
                    else
                    {
                        labelFreakonomist.Text = @"GDP growth stays positive";
                        textBoxFreakonomist.Text = @"Expansion of the economy continues.";
                    }
                }
            }
            else
            {
                switch (_shockN)
                {
                    case 1:
                        labelFreakonomist.Text = @"EXTRA: Natural disasters strike";
                        textBoxFreakonomist.Text = @"Consumer purchasing power decreases.";
                        break;

                    case 2:
                        labelFreakonomist.Text = @"EXTRA: Political tensions rise";
                        textBoxFreakonomist.Text = @"Investors lose confidence and flee.";
                        break;

                    case 3:
                        labelFreakonomist.Text = @"EXTRA: Country faces embargo";
                        textBoxFreakonomist.Text = @"Main export destination ceases trade.";
                        break;
                }
            }
        }

        private void UpdateFiscalTimes()
        {
            var secondLastDeficit = _deficits[_deficits.Count - 2];
            var lastDeficit = _deficits[_deficits.Count - 1];

            if (secondLastDeficit <= 0)
            {
                if (lastDeficit <= 0)
                {
                    labelFiscalTimes.Text = @"National debt is shrinking";
                    textBoxFiscalTimes.Text = @"Government continues to run budget surplus.";
                }
                else
                {
                    labelFiscalTimes.Text = @"Budget deficit positive";
                    textBoxFiscalTimes.Text = @"Public spending exceeds tax revenue.";
                }
            }

            else
            {
                if (lastDeficit <= 0)
                {
                    labelFiscalTimes.Text = @"Budget deficit negative";
                    textBoxFiscalTimes.Text = @"Tax revenue higher than government spending.";
                }
                else
                {
                    labelFiscalTimes.Text = @"National debt accumulating";
                    textBoxFiscalTimes.Text = @"Government keeps running deficits.";
                }
            }
        }

        private void listBoxTerms_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (listBoxTerms.SelectedIndex)
            {
                case 0:
                    textBoxDefinitions.Text = @"Budget Deficit" + "\r\n\r\n" +
                                              @"A status of financial health in which expenditures exceed revenue. The term budget deficit is most commonly used to refer to government spending rather than business or individual spending. When referring to accrued federal government deficits, the term national debt is used. The opposite of a budget deficit is a budget surplus, and when inflows equal outflows, the budget is said to be balanced. Countries can counter budget deficits by promoting economic growth, reducing government spending and increasing taxes.";
                    break;

                case 1:
                    textBoxDefinitions.Text = @"c (autonomous)" + "\r\n\r\n" +
                                              @"The amount of consumption that would take place in an economy if consumers had no income. Types of goods or services that fall under autonomous consumption include items often dubbed needs: food, housing, electricity. If consumers have no discretionary income they still will need some place to live and something to eat. When combined with discretionary income, autonomous consumption represents real wages.";
                    break;

                case 2:
                    textBoxDefinitions.Text = @"Consumption" + "\r\n\r\n" +
                                              @"Consumption, in economics, is the use of goods and services by households. Consumption is distinct from consumption expenditure, which is the purchase of goods and services for use by households. Consumption differs from consumption expenditure primarily because durable goods, such as automobiles, generate an expenditure mainly in the period when they are purchased, but they generate consumption services (for example, an automobile provides transportation services) until they are replaced or scrapped.";
                    break;

                case 3:
                    textBoxDefinitions.Text = @"Effective Tax Rate" + "\r\n\r\n" +
                                              @"The average rate at which an individual or corporation is taxed. The effective tax rate for individuals is the average rate at which their earned income is taxed. For a corporation, it is the average rate at which its pre-tax profits are taxed. An individual's effective tax rate is calculated by dividing total tax expense by taxable income. For corporations, the effective tax rate is computed by dividing total tax expenses by the firm's earnings before taxes. The effective tax rate is the net rate a taxpayer pays if all forms of taxes are included and divided by taxable income.";
                    break;

                case 4:
                    textBoxDefinitions.Text = @"GDP Growth" + "\r\n\r\n" +
                                              @"Increase in a country's productive capacity, as measured by comparing gross domestic product (GDP) in a year with the GDP in the previous year. Increase in the capital stock, advances in technology, and improvement in the quality and level of literacy are considered to be the principal causes of economic growth. In recent years, the idea of sustainable development has brought in additional factors such as environmentally sound processes that must be taken into account in growing an economy.";
                    break;

                case 5:
                    textBoxDefinitions.Text = @"Government Budget" + "\r\n\r\n" +
                                              @"A government budget is the budget of a country, which is a written estimate of anticipated revenue and expenditures during a specific period of time. Budgeting is the process of estimating the revenue the government anticipates that it will generate and the expenses it will incur. In governments, revenue is generated from taxes and fees it imposes. Government expenditures range from national defense, infrastructure, grants for research, education, and the arts, and social programs such as Social Security.";
                    break;

                case 6:
                    textBoxDefinitions.Text = @"Government Spending" + "\r\n\r\n" +
                                              @"Government spending or expenditure includes all government consumption, investment, and transfer payments. The acquisition by governments, of goods and services for current use, to directly satisfy the individual or collective needs of the community, is classed as government consumption. Government acquisition of goods and services intended to create future benefits, such as infrastructure investment or research spending, is classed as government investment. Together, these two constitute one of the major components of gross domestic product.";
                    break;

                case 7:
                    textBoxDefinitions.Text = @"Gross Domestic Product" + "\r\n\r\n" +
                                              @"The monetary value of all the finished goods and services produced within a country's borders in a specific time period, though GDP is usually calculated on an annual basis. It includes all of private and public consumption, government outlays, investments and exports less imports that occur within a defined territory. The gross domestic product is one of the primary indicators used to gauge the health of a country's economy.";
                    break;

                case 8:
                    textBoxDefinitions.Text = @"Investment" + "\r\n\r\n" +
                                              @"Assets or items that are purchased with the hope that they will generate income or appreciate in the future. In an economic sense, investment is the purchase of goods that are not consumed today but are used in the future to create wealth — the accumulation of newly produced physical entities, such as factories, machinery, houses, and goods inventories. In finance, an investment is a monetary asset purchased with the idea that the asset will provide income in the future or appreciate and be sold at a higher price.";
                    break;

                case 9:
                    textBoxDefinitions.Text = @"Keynesian Economics" + "\r\n\r\n" +
                                              @"An economic theory of total spending in the economy and its effects on output and inflation. Keynesian economics was developed by the British economist John Maynard Keynes during the 1930s in an attempt to understand the Great Depression. Keynes advocated increased government expenditures and lower taxes to stimulate demand and pull the global economy out of the Depression. Keynesian economics is considered to be a “demand-side” theory that focuses on changes in the economy over the short run.";
                    break;

                case 10:
                    textBoxDefinitions.Text = @"MPC" + "\r\n\r\n" +
                                              @"The proportion of an aggregate raise in pay that a consumer spends on the consumption of goods and services, as opposed to saving it. Marginal propensity to consume is a component of Keynesian macroeconomic theory and is calculated as the change in consumption divided by the change in income. According to Keynesian theory, an increase in production increases consumers’ income, and they will then spend more. This additional spending will generate additional production, creating a continuous cycle.";
                    break;

                case 11:
                    textBoxDefinitions.Text = @"Net Exports" + "\r\n\r\n" +
                                              @"Net exports are the difference between a country's total value of exports and total value of imports. Depending on whether a country imports more goods or exports more goods, net exports can have a positive or negative value. When the value of goods exported is higher than the value of goods imported, the country is said to have a positive balance of trade for the period. Net exports is an important variable used in the calculation of a country's GDP.";
                    break;

                case 12:
                    textBoxDefinitions.Text = @"Tax Revenue" + "\r\n\r\n" +
                                              @"Tax revenue is defined as the revenues collected from taxes on income and profits, social security contributions, taxes levied on goods and services, payroll taxes, taxes on the ownership and transfer of property, and other taxes. Total tax revenue as a percentage of GDP indicates the share of a country's output that is collected by the government through taxes. It can be regarded as one measure of the degree to which the government controls the economy's resources. ";
                    break;
            }
        }

        private void DisplayScoreboard()
        {
            var gdpOverall = 100.0*chartOutput.Series[0].Points[chartOutput.Series[0].Points.Count - 1].YValues[0]/
                             chartOutput.Series[0].Points[4].YValues[0] - 100.0;

            var gdpAverage = _growth.Average();

            var gdpVariance = Math.Abs((_growth.Max() + _growth.Min())/2.0 - gdpAverage);

            var bdOverall = _deficits.Sum();

            var bdAverage = _deficits.Average();

            var bdVariance = Math.Abs((_deficits.Max() + _deficits.Min())/2.0 - bdAverage);

            _scoreboard = new Scoreboard(gdpOverall, gdpVariance, bdOverall, bdVariance);
            _scoreboard.ShowDialog();
        }

        private enum Quarters
        {
            Q1 = 1,
            Q2 = 2,
            Q3 = 3,
            Q4 = 4
        }
    }
}