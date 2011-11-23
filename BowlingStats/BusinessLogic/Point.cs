namespace BowlingStats.BusinessLogic
{
    public class Point
    {
        public Point(string name, double points)
        {
            Name = name;
            Points = points;
        }

        public string Name { get; private set; }

        public double Points { get; private set; }
    }
}