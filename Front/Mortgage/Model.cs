using LiveChartsCore.Measure;
using System.Reflection.Metadata;

namespace Front.Mortgage
{
    public class Market
    {
        public List<House> Houses { get; } = new();
        public List<Person> People { get; } = new();

        public House CheapestEmptyHouse
        {
            get
            {
                var cheapestEmptyHouse = Houses
                                        .Where(h => h.Owner == null)
                                        .OrderBy(h => h.Price)
                                        .FirstOrDefault();

                return cheapestEmptyHouse;
            }
        }

        public double RentPrice
        {
            get
            {
                double rentPrice;
                if (CheapestEmptyHouse == null)
                {
                    rentPrice = Houses.Max(h => h.Price) * .05 / 12;
                    return rentPrice;
                }

                rentPrice = CheapestEmptyHouse.Price * .05 / 12;
                return rentPrice;
            }
        }

        public void GenerateHoueses(int count, double minPrice, double maxPrice)
        {
            Houses.Clear();

            for (int i = 0; i < count; i++)
            {
                House house = new House
                {
                    Price = GenerateNumber(minPrice, maxPrice)
                };

                Houses.Add(house);
            }
        }

        public void GeneratePeople(int count, double minBalance, double maxBalance, double minSalary, double maxSalary, bool bankPlays = false)
        {
            People.Clear();

            for (int i = 0; i < count; i++)
            {
                var person = new Person
                {
                    Balance = GenerateNumber(minBalance, maxBalance),
                    SalaryAfterTaxAndFood = GenerateNumber(minSalary, maxSalary)
                };

                People.Add(person);
            }

            if (!bankPlays)
                return;

            var biggestBalance = People.Max(p => p.Balance);
            var biggestSalary = People.Max(p => p.SalaryAfterTaxAndFood);
            var bank = new Person
            {
                Balance = biggestBalance * 100,
                SalaryAfterTaxAndFood = biggestSalary * 100
            };

            People.Add(bank);
        }

        public static double GenerateNumber(double min, double max)
        {
            double gap = max - min;
            return min + Random.Shared.NextDouble() * gap;
        }

        public void PlayIteration()
        {
            foreach (var person in People)
            {
                if (person.IsHomeless)
                    continue;

                if (!person.BuyHouse(this))
                    person.SellHouse(this);

                person.Earn(this);
            }
        }
    }

    public class House
    {
        public Person Owner { get; set; }
        public double Price { get; set; }
    }

    public class Person
    {
        public double Balance { get; set; }
        public List<House> Properties { get; } = new();
        public double SalaryAfterTaxAndFood { get; set; }
        public int MaxPropertiesCount { get; set; }
        public bool MustRent { get { return Properties.Count < 1; } }

        public bool IsHomeless { get { return Balance < 0; } }

        public bool BuyHouse(Market market)
        {
            var cheapestEmptyHouse = market.CheapestEmptyHouse;
            if (cheapestEmptyHouse == null || cheapestEmptyHouse.Price * 1.07 > Balance)
                return false;

            Balance -= cheapestEmptyHouse.Price * 1.07;
            cheapestEmptyHouse.Owner = this;
            Properties.Add(cheapestEmptyHouse);

            if (Properties.Count > MaxPropertiesCount)
                MaxPropertiesCount = Properties.Count;

            return true;
        }

        public bool SellHouse(Market market)
        {
            if (Properties.Count <= 1)
                return false;

            var cheapestProperty = Properties
                                    .OrderBy(p => p.Price)
                                    .First();

            var emptyHousesList = market.Houses.Where(h => h.Owner == null).ToList();
            emptyHousesList.Add(cheapestProperty);
            var marketOldAvarage = emptyHousesList.Average(h => h.Price);

            var cheapestEmptyHouse = market.CheapestEmptyHouse;
            if(cheapestEmptyHouse == null)
            {
                var averageHousePrice = market.Houses.Average(h => h.Price);
                cheapestEmptyHouse = market.Houses.OrderBy(h => Math.Abs(h.Price - averageHousePrice)).First();
            }

            double cheapestMarketPrice = cheapestEmptyHouse.Price;

            cheapestProperty.Price = cheapestMarketPrice - 1;
            cheapestProperty.Owner = null;
            Properties.Remove(cheapestProperty);

            var marketNewAverage = market.Houses.Where(h => h.Owner == null).Average(h => h.Price);

            UpdateHusePrices(marketOldAvarage, marketNewAverage, market);

            return true;
        }

        public void Earn(Market market)
        {
            Balance += SalaryAfterTaxAndFood;

            if (MustRent)
            {
                Balance -= market.RentPrice;
                //если денег не хватает
                if (market.RentPrice / SalaryAfterTaxAndFood >= .8)
                    //просит прибавки (или больше работает)
                    SalaryAfterTaxAndFood *= 1.2;
            }
        }

        private void UpdateHusePrices(double oldAverage, double newAverage, Market market)
        {
            double coefficient = newAverage / oldAverage;
            var emptyHouses = market.Houses.Where(h => h.Owner == null);
            foreach(var house in emptyHouses)
            {
                house.Price *= coefficient;
            }
        }
    }


    public class LogarithmicPoint
    {
        public double X { get; set; }
        public double Y { get; set; }
    }

}
