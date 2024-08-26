namespace Front
{
	public class EvGenerator
	{
		private double _chance;
		private Random _random;

		public EvGenerator(double chance)
		{
			_chance = chance;
			_random = new Random();
		}

		public bool HaveWon()
		{
			var nextDouble = _random.NextDouble();
			return nextDouble >= _chance / 100;
		}
	}
}
