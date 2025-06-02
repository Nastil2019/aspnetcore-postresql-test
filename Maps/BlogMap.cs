using dockerapi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace dockerapi.Maps
{
#pragma warning disable CS1591
    public class BlogMap : IEntityTypeConfiguration<Blog>
    {
        public void Configure(EntityTypeBuilder<Blog> builder)
        {
            // Настройка таблицы и первичного ключа
            builder.ToTable("blog");
            builder.HasKey(x => x.Id);

            // Настройка свойств
            builder.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("title");

            builder.Property(x => x.Description)
                .HasColumnName("description");
        }
    }
#pragma warning restore CS1591
}