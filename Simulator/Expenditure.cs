namespace Simulator
{
    internal class Expenditure
    {
        internal readonly Consumption Consumpt = new Consumption();

        public Expenditure()
        {
            Investment = 17.7;
            Exports = 55.8;
            Imports = 56.4;
        }

        public double Investment { get; set; }

        public double Exports { get; set; }
        private double Imports { get; }

        public double NetExports()
        {
            return Exports - Imports;
        }

        public double GetExpenditure(double cons, double govSpend)
        {
            return cons + Investment + govSpend + NetExports();
        }
    }
}