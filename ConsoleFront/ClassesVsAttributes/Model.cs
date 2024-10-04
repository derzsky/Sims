namespace WpfFront.ClassesVsAttributes.First
{
    class Car
    {
        public void Drive() { }
    }
    class Airplane
    {
        public void Fly() { }
    }
    class Ship
    {
        public void Sail() { }
    }

    public class Model
    {
        public Model()
        {
            var car = new Car();
            car.Drive();

            var plane = new Airplane();
            plane.Fly();

            var ship = new Ship();
            ship.Sail();

            var amphiby = (new Car(), new Ship());
            //amphiby.Drive();
            //amphiby.Sail();
        }
    }
}
