using WpfFront.ClassesVsAttributes.Third;

namespace ConsoleFront
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var v = new Model();

            double result = 4;
            for(double i = 2; i < 50_000_000; i++)
            {
                var znamenatel = i * 2 - 1;
                if (i % 2 == 0)
                    znamenatel = -znamenatel;

                result += 4d / znamenatel;
            }

            Console.WriteLine("Hello, World!");

            Console.ReadKey();
        }
    }
}
