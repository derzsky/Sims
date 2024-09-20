using Front.Healthcare;

namespace ModelUnitTests
{
    [TestClass]
    public class SicknessTest
    {
        [TestMethod]
        public void Constructor()
        {
            var market = GenerateMarket(.5, .8);

            var sickness1 = new Sickness(.9, market, .2);
            Assert.AreEqual(sickness1.Strength, .9);
            Assert.AreEqual(sickness1.IsUrgent, true);

            var sickness2 = new Sickness(.7, market, .2);
            Assert.AreEqual(sickness2.Strength, .7);
            Assert.AreEqual(sickness2.IsUrgent, false);
        }

        [TestMethod]
        public void UpdateUrgent()
        {
            var market = GenerateMarket(.5, .8);

            var sickness = new Sickness(.9, market, .2);
            sickness.Update();
            Assert.AreEqual(sickness.Strength, .9);
            Assert.IsTrue(sickness.IsUrgent);
        }

        [TestMethod]
        public void UpdateNotUrgent()
        {
            var market = GenerateMarket(.5, .8);

            var sickness = new Sickness(.3, market, .2);
            Assert.AreEqual(sickness.Strength, .3, .001);

            sickness.Update();
            Assert.AreEqual(sickness.Strength, .4, .001);

            sickness.Update();
            Assert.AreEqual(sickness.Strength, .3, .001);

            sickness.Update();
            Assert.AreEqual(.4, sickness.Strength, .001);

            sickness.Update();
            Assert.AreEqual(.5, sickness.Strength, .001);
        }

        Market GenerateMarket(double sicknessProgressionProgability, double UrgencyThreshold)
        {
            var doubleQueue = new Queue<double>();
            doubleQueue.Enqueue(.5);
            doubleQueue.Enqueue(.3);
            doubleQueue.Enqueue(.6);
            doubleQueue.Enqueue(.9);

            var generator = new NumberGenerator(doubleQueue);

            var market = new Market
            {
                SicknessProgressionStep = .1,
                SicknessProgressionProgability = sicknessProgressionProgability,
                UrgencyThreshold = UrgencyThreshold,
                Generator = generator
            };

            return market;
        }
    }
}