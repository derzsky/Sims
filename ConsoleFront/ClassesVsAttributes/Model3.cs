namespace WpfFront.ClassesVsAttributes.Third
{
    interface ICanDrive
    {
        bool CheckIfCanDrive();
        void Drive();
    }
    interface ICanFly
    {
        bool CheckIfCanFly();
        void Fly();
    }
    interface ICanSail
    {
        bool CheckIfCanSail();
        void Sail();
    }
    class Car : ICanDrive
    {
        public bool CheckIfCanDrive() { return true; }
        public void Drive() { }
    }

    class Airplane : ICanFly
    {
        public bool CheckIfCanFly() { return true; }
        public void Fly() { }
    }

    class Ship : ICanSail
    {
        public bool CheckIfCanSail() { return false; }
        public void Sail() { }
    }

    class Amphiby : ICanDrive, ICanSail
    {
        public bool CheckIfCanDrive() { return true; }
        public bool CheckIfCanSail() { return true; }
        public void Drive() { }
        public void Sail() { }
    }

    class Seaplane : ICanSail, ICanFly
    {
        public bool CheckIfCanFly() { return true; }
        public bool CheckIfCanSail() { return true; }
        public void Fly() { }
        public void Sail() { }
    }
    class JamesBondVehicle : ICanDrive, ICanFly, ICanSail
    {
        public bool CheckIfCanDrive() { return true; }
        public bool CheckIfCanFly() { return true; }
        public bool CheckIfCanSail() { return true; }
        public void Drive() { }
        public void Fly() { }
        public void Sail() { }
    }
    class BrokenCar : ICanDrive
    {
        public bool CheckIfCanDrive() { return false; }
        public void Drive()
        {
            throw new NotImplementedException();
        }
    }
    class Model
    {
        public Model()
        {
            var anything = GetAnything();
            var anything2 = new BrokenCar();

            var isVehicle = IsVehicle(anything2);
        }

        private bool IsVehicle(object obj)
        {
            var result = false;

            var canDrive = obj as ICanDrive;
            var canFly = obj as ICanFly;
            var canSail = obj as ICanSail;

            if (canDrive is not null)
                result = canDrive.CheckIfCanDrive();

            if (canFly is not null)
                result = result || canFly.CheckIfCanFly();

            if (canSail is not null)
                result = result || canSail.CheckIfCanSail();

            return result;
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
