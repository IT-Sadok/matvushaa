namespace CommonData.Models.Read;

public class BookInfoModel
{
    public int Id { get; set; }
    
    public string BookName { get; set; }
    
    public string Author { get; set; }
    
    public int ReleaseYear { get; set; }
    
    public bool Status { get; set; }
}