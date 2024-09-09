namespace Front.PositiveEV
{
    public class EvGenerator
    {
        private Random _random;

        public EvGenerator()
        {
            _random = new Random();
        }

        public bool HasWon(double chance)
        {
            var nextDouble = _random.NextDouble();
            return nextDouble >= 1 - chance / 100;
        }
    }
}
