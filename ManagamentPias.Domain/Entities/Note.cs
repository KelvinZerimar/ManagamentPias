using Newtonsoft.Json;


namespace ManagamentPias.Domain.Entities;

public class Note
{
    public Note()
    { }

    private Note(string category, string title, string richTextContent, DateTime createDate, DateTime? modifyDate, bool isActive)
    {
        Category = category;
        Title = title;
        RichTextContent = richTextContent;
        CreateDate = createDate;
        ModifyDate = modifyDate;
        IsActive = isActive;
    }

    [JsonProperty(PropertyName = "id")]
    public virtual Guid Id { get; set; } = Guid.NewGuid();

    [JsonProperty(PropertyName = "category")]
    public string Category { get; set; } = null!;

    [JsonProperty(PropertyName = "Title")]
    public string Title { get; set; } = null!;

    [JsonProperty(PropertyName = "Content")]
    public string RichTextContent { get; set; } = null!;

    [JsonProperty(PropertyName = "Create")]
    public DateTime CreateDate { get; init; } = DateTime.Now;

    [JsonProperty(PropertyName = "Modify")]
    public DateTime? ModifyDate { get; set; }

    [JsonProperty(PropertyName = "IsActive")]
    public bool IsActive { get; set; } = true;

    static public Note Create(string category, string title, string richTextContent, DateTime createDate, DateTime? modifyDate, bool isActive)
    {
        return new Note(category, title, richTextContent, createDate, modifyDate, isActive);
    }
}

