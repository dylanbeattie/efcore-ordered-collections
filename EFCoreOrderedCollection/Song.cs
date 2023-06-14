namespace EFCoreOrderedCollection; 

public class Song {
	public Guid Id { get; set; }
	public Album? Album { get; set; }
	public string Name { get; set; }
}
