namespace WpfFront.ClassesVsAttributes.Second
{
    interface ICanDrive
    {
        void Drive();
    }

    interface ICanFly
    {
        void Fly();
    }

    interface ICanSail
    {
        void Sail();
    }

    class Car : ICanDrive
    {
        public void Drive() { }
    }

    class Airplane : ICanFly
    {
        public void Fly() { }
    }

    class Ship : ICanSail
    {
        public void Sail() { }
    }

    class Amphiby : ICanDrive, ICanSail
    {
        public void Drive() { }
        public void Sail() { }
    }

    class Seaplane : ICanSail, ICanFly
    {
        public void Fly() { }
        public void Sail() { }
    }
    class JamesBondVehicle : ICanDrive, ICanFly, ICanSail
    {
        public void Drive() { }
        public void Fly() { }
        public void Sail() { }
    }
    class BrokenCar : ICanDrive
    {
        public void Drive()
        {
            throw new NotImplementedException();
        }
    }

    class Model
    {
        public Model()
        {
            var car = new Car();
            car.Drive();

            var plane = new Airplane();
            plane.Fly();

            var ship = new Ship();
            ship.Sail();

            var amphiby = new Amphiby();
            amphiby.Drive();
            amphiby.Sail();

            var seaAirplane = new Seaplane();
            seaAirplane.Fly();
            seaAirplane.Sail();

            var jamesBondCar = new JamesBondVehicle();
            jamesBondCar.Drive();
            jamesBondCar.Fly();
            jamesBondCar.Sail();

            var brokenCar = new BrokenCar();
            brokenCar.Drive();

            object anything = GetAnything();

            var canDrive = anything is ICanDrive;
            var canFly = anything is ICanFly;
            var canSail = anything is ICanSail;

            var isVehicle = canDrive || canFly || canSail;
        }

        private object GetAnything()
        {
            var listOfTypes = new List<Type>
            {
                typeof(Car),
                typeof(Airplane),
                typeof(Ship),
                typeof(Amphiby),
                typeof(Seaplane),
                typeof(JamesBondVehicle),
                typeof(BrokenCar)
            };

            double range = 1d / listOfTypes.Count;
            var randomNumber = Random.Shared.NextDouble();

            Type resultType = null;
            for (int i = 0; i < listOfTypes.Count; i++)
            {
                var currentRangeBegin = i * range;
                if (randomNumber <= currentRangeBegin
                    //максимальный рэнж получается чуть меньше едининицы
                    //потому что это double
                    //и ничего не выберется
                    || i == listOfTypes.Count - 1)
                {
                    resultType = listOfTypes[i];
                    break;
                }
            }

            object resultObject = Activator.CreateInstance(resultType);

            return resultObject;
        }
    }
}
