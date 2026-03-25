using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

// AppDbContext veritabanina acilan ana kapidir.
// Tum entity setleri ve EF Core mapping konfigurations bu sinif uzerinden yonetilir.
public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    // Fluent API konfigurations ayri siniflarda tutulur.
    // Bu sayede entity'ler temiz kalir, mapping detaylari Infrastructure icinde yasar.
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    // Buradaki DbSet alanlari EF Core'a hangi tablolarla calisacagini soyler.
    // Her yeni modulde ilgili aggregate/root icin bir DbSet eklenir.
    // Ornek:
    // Users tablosu kullanicilari,
    // JobPostings tablosu is ilanlarini,
    // CvDocuments tablosu CV kayitlarini temsil eder.
    // public DbSet<User> Users => Set<User>();
    // public DbSet<JobPosting> JobPostings => Set<JobPosting>();
    // public DbSet<CvDocument> CvDocuments => Set<CvDocument>();
}
