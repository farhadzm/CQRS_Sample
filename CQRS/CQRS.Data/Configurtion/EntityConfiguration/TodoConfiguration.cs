using CQRS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CQRS.Data.Configurtion
{
    public class TodoConfiguration : IEntityTypeConfiguration<Todo>
    {
        public void Configure(EntityTypeBuilder<Todo> builder)
        {
            builder.Property(a => a.Title).IsRequired();

            builder.HasOne(a => a.UserCreator)
                .WithMany(a => a.TodoCreators)
                .HasForeignKey(a => a.UserCreatorId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.UserEditor)
                .WithMany(a => a.TodoEditors)
                .HasForeignKey(a => a.UserEditorId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);
        }
    }
}
