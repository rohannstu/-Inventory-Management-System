using InventoryManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagementSystem.Infrastructure.Persistence.Configurations;

public class StockMovementConfiguration : IEntityTypeConfiguration<StockMovement>
{
    public void Configure(EntityTypeBuilder<StockMovement> builder)
    {
        builder.ToTable("StockMovements");

        builder.HasKey(sm => sm.Id);

        // Store the enum as its string name ("StockIn", "Adjustment", etc.)
        // instead of the default int — see Phase 2's trade-off discussion.
        // Resilient to future reordering; readable directly in DBeaver with no lookup.
        builder.Property(sm => sm.Type)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(sm => sm.Quantity).IsRequired();
        builder.Property(sm => sm.Notes).HasMaxLength(1000);
        builder.Property(sm => sm.OccurredAtUtc).IsRequired();

        builder.Property(sm => sm.PerformedByUserId); // nullable Guid, no FK constraint yet — no User table exists

        builder.HasOne(sm => sm.Product)
            .WithMany()
            .HasForeignKey(sm => sm.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(sm => sm.Warehouse)
            .WithMany(w => w.StockMovements)
            .HasForeignKey(sm => sm.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);

        // Query performance: we'll constantly filter/sort stock movements
        // by product and by date range (Phase 6 pagination/filtering, Phase 11 reporting).
        builder.HasIndex(sm => sm.ProductId);
        builder.HasIndex(sm => sm.OccurredAtUtc);
    }
}