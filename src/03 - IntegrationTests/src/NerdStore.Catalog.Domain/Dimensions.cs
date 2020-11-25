namespace NerdStore.Catalog.Domain
{
    public class Dimensions
    {
        public decimal Hight { get; private set; }
        public decimal Width { get; private set; }
        public decimal Depth { get; private set; }

        public Dimensions(decimal hight, decimal width, decimal depth)
        {
            Hight = hight;
            Width = width;
            Depth = depth;
        }

        public string GetDescription()
        {
            return $"LxAxP: {Width} x {Hight} x {Depth}";
        }

        public override string ToString()
        {
            return GetDescription();
        }
    }
}
