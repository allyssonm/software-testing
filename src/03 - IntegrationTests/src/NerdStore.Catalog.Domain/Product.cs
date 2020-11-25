using NerdStore.Core.DomainObjects;
using System;

namespace NerdStore.Catalog.Domain
{
    public class Product : Entity, IAggregateRoot
    {
        public Guid CategoryId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool Active { get; private set; }
        public decimal Price { get; private set; }
        public DateTime RegisterDate { get; private set; }
        public string Image { get; private set; }
        public int StockQuantity { get; private set; }
        public Dimensions Dimensions { get; private set; }
        public Category Category { get; private set; }

        protected Product() { }

        public Product(string name, string description, bool active, decimal price, Guid categoryId, DateTime registerDate, string image, Dimensions dimensions)
        {
            CategoryId = categoryId;
            Name = name;
            Description = description;
            Active = active;
            Price = price;
            RegisterDate = registerDate;
            Image = image;
            Dimensions = dimensions;
        }

        public void Activate() => Active = true;

        public void Deactivate() => Active = false;

        public void ChangeCategory(Category category)
        {
            Category = category;
            CategoryId = category.Id;
        }

        public void ChangeDescription(string description)
        {
            Description = description;
        }

        public void DebitStock(int quantity)
        {
            if (quantity < 0) quantity *= -1;
            if (!HasStock(quantity)) throw new DomainException("Insufficient stock");
            StockQuantity -= quantity;
        }

        public void Restock(int quantity)
        {
            StockQuantity += quantity;
        }

        public bool HasStock(int quantity)
        {
            return StockQuantity >= quantity;
        }
    }
}
