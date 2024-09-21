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

        [TestMethod]
        public void PersonRegistersCheckup()
        {
            var doubleQueue = new Queue<double>();
            doubleQueue.Enqueue(.1); //заболеет ли
            doubleQueue.Enqueue(.6); //как сильно
            doubleQueue.Enqueue(.3); //будет ли рынок лечить человека
            doubleQueue.Enqueue(0); //прогресс болезни

            var market = GenerateMarket(doubleQueue);
            market.SelfPaidCare = true;

            var person = new Person(market);
            Assert.AreEqual(0, person.Sicknesses.Count());

            doubleQueue.Enqueue(0); //сам он её лечить не хочет
            person.Update(); //первый раз 
            Assert.AreEqual(1, person.Sicknesses.Count());
            Assert.AreEqual(.5, person.Sicknesses.First().Strength, .001);

            var firstSickness = person.Sicknesses.First();

            doubleQueue.Enqueue(0); //новой болезни нет
            doubleQueue.Enqueue(0); //первая болезнь регрессирует
            doubleQueue.Enqueue(0); //сам он её лечить не хочет
            person.Update();
            Assert.AreEqual(.4, person.Sicknesses.First().Strength, .001);

            doubleQueue.Enqueue(0); //новой болезни нет
            doubleQueue.Enqueue(0); //первая болезнь регрессирует
            doubleQueue.Enqueue(0); //сам он её лечить не хочет
            person.Update();
            Assert.AreEqual(.3, person.Sicknesses.First().Strength, .001);

            doubleQueue.Enqueue(0); //новой болезни нет
            doubleQueue.Enqueue(0); //первая болезнь регрессирует
            doubleQueue.Enqueue(0); //сам он её лечить не хочет
            person.Update();
            Assert.AreEqual(.2, person.Sicknesses.First().Strength, .001);

            doubleQueue.Enqueue(0); //новой болезни нет
            doubleQueue.Enqueue(0); //первая болезнь регрессирует
            doubleQueue.Enqueue(0); //сам он её лечить не хочет
            person.Update();
            Assert.AreEqual(.1, person.Sicknesses.First().Strength, .001);
            Assert.IsFalse(person.Sicknesses.First().IsCheckup);
            Assert.AreSame(firstSickness, person.Sicknesses.First());

            doubleQueue.Enqueue(0); //новой болезни нет
            doubleQueue.Enqueue(0); //первая болезнь регрессирует
            doubleQueue.Enqueue(1); //но готов заплатить за чекап
            person.Update();
            Assert.AreEqual(0, person.Sicknesses.First().Strength, .001);
            Assert.IsTrue(person.Sicknesses.First().IsCheckup);
            Assert.AreNotSame(firstSickness, person.Sicknesses.First());
            Assert.AreEqual(1, person.Sicknesses.Count());
            Assert.AreEqual(1, market.Line.Count());
            Assert.AreEqual(2, market.TotalSicknesses.Count());
        }

        [TestMethod]
        public void PersonUpdatesCheckup()
        {
            var doubleQueue = new Queue<double>();
            doubleQueue.Enqueue(0); //заболеет ли
            doubleQueue.Enqueue(.6); //готов оплатить чекап

            var market = GenerateMarket(doubleQueue);
            market.SelfPaidCare = true;

            var person = new Person(market);
            Assert.AreEqual(0, person.Sicknesses.Count());

            person.Update();
            Assert.AreEqual(1, person.Sicknesses.Count());
            Assert.IsTrue(person.Sicknesses.First().IsCheckup);
            var checkupSickness = person.Sicknesses.First();

            doubleQueue.Enqueue(1); //заболел
            doubleQueue.Enqueue(.6); //как сильно
            doubleQueue.Enqueue(.3); //будет ли рынок лечить человека
            doubleQueue.Enqueue(0); //болезнь не прогрессирует
            doubleQueue.Enqueue(0); //сам лечить не хочет
            person.Update();
            Assert.AreEqual(1, person.Sicknesses.Count());
            Assert.IsFalse(person.Sicknesses.First().IsCheckup);
            Assert.AreEqual(.5, person.Sicknesses.First().Strength);
            Assert.AreEqual(.3, person.Sicknesses.First().AcceptanceByMarket);
            Assert.AreSame(person.Sicknesses.First(), checkupSickness);
            Assert.AreEqual(1, market.TotalSicknesses.Count);
        }


        private Market GenerateMarket(Queue<double> doubles)
        {
            var generator = new NumberGenerator(doubles);

            var market = new Market
            {
                Generator = generator,
                PeopleWantToCkeckUp = .5,
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
