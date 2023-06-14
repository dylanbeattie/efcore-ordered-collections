namespace EFCoreOrderedCollection;

public class Album {
	public Guid Id { get; set; } = Guid.NewGuid();
	public string Name { get; set; } = String.Empty;
	public List<Song> Songs { get; set; } = new();
}
