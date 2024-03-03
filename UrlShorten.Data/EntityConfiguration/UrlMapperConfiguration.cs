

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UrlShorten.Data.Entities;

namespace UrlShorten.Data.EntityConfiguration
{
    public class UrlMapperConfiguration : IEntityTypeConfiguration<UrlMapper>
    {
        public void Configure(EntityTypeBuilder<UrlMapper> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.InputUrl).IsRequired();
            builder.Property(e => e.ShortenUrl).IsRequired();
        }
    }
}
