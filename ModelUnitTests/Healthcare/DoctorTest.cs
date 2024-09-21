using Front.Healthcare;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelUnitTests.Healthcare
{
    [TestClass]
    public class DoctorTest
    {
        [TestMethod]
        public void DoctorHelpsIfAccepted()
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

            var doctor = new Doctor(market);

            doctor.Update();
            Assert.AreEqual(.3, market.Line.First().Strength, .0001);

            doctor.Update();
            Assert.AreEqual(.1, market.Line.First().Strength, .0001);

            doctor.Update();
            Assert.AreEqual(.0, market.Line.First().Strength, .0001);
        }

        private Market GenerateMarket(Queue<double> doubles)
        {
            var generator = new NumberGenerator(doubles);

            var market = new Market
            {
                Generator = generator,
                DoctorSkillMultiplyer = 2,
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
