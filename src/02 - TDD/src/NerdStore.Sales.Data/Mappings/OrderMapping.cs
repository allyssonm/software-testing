using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NerdStore.Sales.Domain;

namespace NerdStore.Sales.Data.Mappings
{
    public class OrderMapping : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Code)
                .HasDefaultValueSql("NEXT VALUE FOR MySequence");

            builder.Property(c => c.Discount)
                .HasColumnType("decimal(5,2)");

            builder.Property(c => c.TotalPrice)
                .HasColumnType("decimal(5,2)");

            // 1 : N => Pedido : PedidoItems
            builder.HasMany(c => c.OrderItems)
                .WithOne(c => c.Order)
                .HasForeignKey(c => c.OrderId);

            builder.ToTable("Orders");
        }
    }
}
