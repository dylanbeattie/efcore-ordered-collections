using Microsoft.EntityFrameworkCore;

namespace EFCoreOrderedCollection;

public class MyDbContext : DbContext {
	public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }
	public virtual DbSet<Album> Albums => Set<Album>();
	public virtual DbSet<Song> Songs => Set<Song>();

	protected override void OnModelCreating(ModelBuilder modelBuilder) {
		modelBuilder.Entity<Album>(entity => {
			entity.HasMany(a => a.Songs)
				.WithOne(song => song.Album)
				.IsRequired();
		});
	}
}
