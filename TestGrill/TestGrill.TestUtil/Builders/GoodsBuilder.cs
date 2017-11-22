using TestGrill.Entities;

namespace TestGrill.TestUtil.Builders
{
    public class GoodsBuilder
    {
        private readonly Goods innerObject = new Goods();

        public static implicit operator Goods(GoodsBuilder instance)
        {
            return instance.Build();
        }

        public GoodsBuilder Quantity(int quantity)
        {
            this.innerObject.Quantity = quantity;
            return this;
        }

        public GoodsBuilder Name(string name)
        {
            this.innerObject.Name = name;
            return this;
        }

        public GoodsBuilder Width(int width)
        {
            this.innerObject.Width = width;
            return this;
        }

        public GoodsBuilder Length(int length)
        {
            this.innerObject.Length = length;
            return this;
        }

        private Goods Build()
        {
            return this.innerObject;
        }
    }
}