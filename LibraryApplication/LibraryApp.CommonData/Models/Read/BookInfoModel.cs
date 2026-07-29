using CommonData.Enums;

namespace CommonData.Models.Read;

public class BookInfoModel
{
    public int Id { get; set; }
    
    public string BookName { get; set; }
    
    public string Author { get; set; }
    
    public int YearPublished { get; set; }
    
    public BookStatus Status { get; set; }
}