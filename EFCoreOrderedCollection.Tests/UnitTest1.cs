using System.Diagnostics;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace EFCoreOrderedCollection.Tests {
	public class UnitTest1 {
		private async Task<MyDbContext> Connect(string? dbName = null) {
			dbName ??= Guid.NewGuid().ToString();
			var connectionString = $"Data Source={dbName};Mode=Memory;Cache=Shared";
			var sqlite = new SqliteConnection(connectionString);
			sqlite.Open();
			var options = new DbContextOptionsBuilder<MyDbContext>().UseSqlite(sqlite).Options;
			var db = new MyDbContext(options);
			await db.Database.EnsureCreatedAsync();
			return db;
		}
		
		[Fact]
		public async Task OrderOfSongsIsPreserved() {
			var dbName = Guid.NewGuid().ToString("N");
			var db1 = await Connect(dbName);
			var songs = new[] { "Telegraph Road", "Private Investigations",
				"Industrial Disease", "Love Over Gold", "It Never Rains"
			};
			var album = new Album { Name = "Love Over Gold" };
			foreach (var song in songs) album.Songs.Add(new() { Name = song });
			db1.Albums.Add(album);
			var names = album.Songs.Select(s => s.Name).ToArray();
			names.ShouldBe(songs);
			await db1.SaveChangesAsync();

			var db2 = await Connect(dbName);
			var album2 = db2.Albums.Include(a => a.Songs).First();
			album2.Songs.Count.ShouldBe(5);
			names = album2.Songs.Select(s => s.Name).ToArray();
			names.ShouldBe(songs);
		}
	}
}
