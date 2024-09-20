using Front.Healthcare;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelUnitTests
{
    [TestClass]
    public class PersonTest
    {
        [TestMethod]
        public void GetNewSicknessFalse()
        {
            var doubleQueue = new Queue<double>();
            doubleQueue.Enqueue(.01);

            var market = GenerateMarket(doubleQueue);

            var person = new Person(market);
            Assert.IsFalse(person.Sicknesses.Any());

            person.GetNewSickness();
            Assert.IsTrue(!person.Sicknesses.Any());
        }

        [TestMethod]
        public void GetNewSicknessTrue()
        {
            var doubleQueue = new Queue<double>();
            doubleQueue.Enqueue(.01);
            doubleQueue.Enqueue(.1);
            doubleQueue.Enqueue(.1);
            doubleQueue.Enqueue(.1);

            var market = GenerateMarket(doubleQueue);

            var person = new Person(market);

            person.GetNewSickness();
            Assert.IsFalse(person.Sicknesses.Any());

            person.GetNewSickness();
            Assert.IsTrue(person.Sicknesses.Any());
        }

        [TestMethod]
        public void SicknessIsUrgent()
        {
            var doubleQueue = new Queue<double>();
            doubleQueue.Enqueue(.1);
            doubleQueue.Enqueue(.9);
            doubleQueue.Enqueue(.3);

            var market = GenerateMarket(doubleQueue);

            var person = new Person(market);
            person.GetNewSickness();
            Assert.AreEqual(.9, person.Sicknesses.First().Strength);
            Assert.IsTrue(person.Sicknesses.First().IsUrgent);
        }

        [TestMethod]
        public void SicknessIsNotUrgent()
        {
            var doubleQueue = new Queue<double>();
            doubleQueue.Enqueue(.1);
            doubleQueue.Enqueue(.7);
            doubleQueue.Enqueue(.3);

            var market = GenerateMarket(doubleQueue);

            var person = new Person(market);
            person.GetNewSickness();
            Assert.AreEqual(.7, person.Sicknesses.First().Strength);
            Assert.IsFalse(person.Sicknesses.First().IsUrgent);
        }

        [TestMethod]
        public void SicknessRegistersAsUrgent()
        {
            var doubleQueue = new Queue<double>();
            doubleQueue.Enqueue(.1);
            doubleQueue.Enqueue(.9);
            doubleQueue.Enqueue(.3);

            var market = GenerateMarket(doubleQueue);

            var person = new Person(market);
            person.Update();

            Assert.AreEqual(person.Sicknesses.First(), market.Line.First(s => s.IsUrgent));
            Assert.IsNull(market.Line.FirstOrDefault(s => !s.IsUrgent));
        }

        [TestMethod]
        public void SicknessRegistersAsNotUrgent()
        {
            var doubleQueue = new Queue<double>();
            doubleQueue.Enqueue(.1);
            doubleQueue.Enqueue(.5);
            doubleQueue.Enqueue(.3);
            doubleQueue.Enqueue(.6);

            var market = GenerateMarket(doubleQueue);

            var person = new Person(market);
            person.Update();

            Assert.AreEqual(person.Sicknesses.First(), market.Line.First(s => !s.IsUrgent));
            Assert.IsNull(market.Line.FirstOrDefault(s=>s.IsUrgent));
        }

        [TestMethod]
        public void SicknessProgressesToUrgent()
        {
            var doubleQueue = new Queue<double>();
            doubleQueue.Enqueue(.1);
            doubleQueue.Enqueue(.4);
            doubleQueue.Enqueue(.3);

            var market = GenerateMarket(doubleQueue);

            var person = new Person(market);

            doubleQueue.Enqueue(1);
            person.Update();
            Assert.AreEqual(.5, person.Sicknesses.First().Strength, .0001);

            doubleQueue.Enqueue(0);
            doubleQueue.Enqueue(1);
            person.Update();
            Assert.IsFalse(person.Sicknesses.First().IsUrgent);
            Assert.AreEqual(.6, person.Sicknesses.First().Strength, .0001);

            doubleQueue.Enqueue(0);
            doubleQueue.Enqueue(1);
            person.Update();
            Assert.IsFalse(person.Sicknesses.First().IsUrgent);
            Assert.AreEqual(.7, person.Sicknesses.First().Strength, .0001);

            doubleQueue.Enqueue(0);
            doubleQueue.Enqueue(1);
            person.Update();
            Assert.IsFalse(person.Sicknesses.First().IsUrgent);
            Assert.AreEqual(.8, person.Sicknesses.First().Strength, .0001);

            doubleQueue.Enqueue(0);
            doubleQueue.Enqueue(1);
            person.Update();
            Assert.IsTrue(person.Sicknesses.First().IsUrgent);
            Assert.AreEqual(.9, person.Sicknesses.First().Strength, .0001);

            doubleQueue.Enqueue(0);
            doubleQueue.Enqueue(1);
            person.Update();
            Assert.IsTrue(person.Sicknesses.First().IsUrgent);
            Assert.AreEqual(.9, person.Sicknesses.First().Strength, .0001);
        }

        [TestMethod]
        public void SicknessCanBeHealed()
        {
            var doubleQueue = new Queue<double>();
            doubleQueue.Enqueue(.1);
            doubleQueue.Enqueue(.8);
            doubleQueue.Enqueue(.3);
            doubleQueue.Enqueue(.0);

            var market = GenerateMarket(doubleQueue);

            var person = new Person(market);
            person.Update();
            Assert.AreEqual(.7, person.Sicknesses.First().Strength, .001);

            doubleQueue.Enqueue(0);
            doubleQueue.Enqueue(0);
            person.Update();
            Assert.AreEqual(.6, person.Sicknesses.First().Strength, .001);

            doubleQueue.Enqueue(0);
            doubleQueue.Enqueue(0);
            person.Update();
            Assert.AreEqual(.5, person.Sicknesses.First().Strength, .001);

            doubleQueue.Enqueue(0);
            doubleQueue.Enqueue(0);
            person.Update();
            Assert.AreEqual(.4, person.Sicknesses.First().Strength, .001);

            doubleQueue.Enqueue(0);
            doubleQueue.Enqueue(0);
            person.Update();
            Assert.AreEqual(.3, person.Sicknesses.First().Strength, .001);

            doubleQueue.Enqueue(0);
            doubleQueue.Enqueue(0);
            person.Update();
            Assert.AreEqual(.2, person.Sicknesses.First().Strength, .001);

            doubleQueue.Enqueue(0);
            doubleQueue.Enqueue(0);
            person.Update();
            Assert.AreEqual(.1, person.Sicknesses.First().Strength, .001);

            doubleQueue.Enqueue(0);
            doubleQueue.Enqueue(0);
            person.Update();
            Assert.AreEqual(.0, person.Sicknesses.First().Strength, .001);

            doubleQueue.Enqueue(0);
            doubleQueue.Enqueue(0);
            person.Update();
            Assert.AreEqual(0, person.Sicknesses.First().Strength, .001);
        }

        [TestMethod]
        public void SicknessUnregistersUrgent()
        {
            var doubleQueue = new Queue<double>();
            doubleQueue.Enqueue(.1);
            doubleQueue.Enqueue(.9);
            doubleQueue.Enqueue(.3);

            var market = GenerateMarket(doubleQueue);
            market.SelfPaidCare = true;

            var person = new Person(market);
            person.Update();

            Assert.AreEqual(person.Sicknesses.First(), market.Line.First());

            person.Sicknesses.First().Heal(.89);
            Assert.AreEqual(.01, person.Sicknesses.First().Strength, .001);

            doubleQueue.Enqueue(0);
            doubleQueue.Enqueue(0);
            doubleQueue.Enqueue(1);
            person.Update();
            Assert.IsNull(person.Sicknesses.FirstOrDefault(s=>!s.IsCheckup));
            Assert.IsNull(market.Line.FirstOrDefault(s=>!s.IsCheckup));
            Assert.IsNotNull(market.TotalSicknesses.FirstOrDefault());
        }


        private Market GenerateMarket(Queue<double> doubles)
        {
            var generator = new NumberGenerator(doubles);

            var market = new Market
            {
                Generator = generator,
                RiskToGetSickness = .05,
                SicknessIsAcceptedByMarketThreshold = .2,
                SicknessIsHealedThreshold = .05,
                SicknessProgressionProgability = .5,
                SicknessProgressionStep = .1,
                TolereableSicknessThreshold = .6,
                UrgencyThreshold = .8
            };

            return market;
        }
    }
}
