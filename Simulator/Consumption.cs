namespace Simulator
{
    internal class Consumption
    {
        public Consumption()
        {
            AutonomousConsumption = 8.0;
            MarginalPropensity = 0.58;
        }

        private double AutonomousConsumption { get; }
        public double MarginalPropensity { get; set; }

        public double GetConsumption(double income, double taxes)
        {
            return AutonomousConsumption + MarginalPropensity*((1 - taxes)*income);
        }
    }
}