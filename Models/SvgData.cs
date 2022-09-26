using System.Collections.Generic;

public class SvgData
{
    public int Id { get; set; }
    public string AppUserId { get; set; }
    public string Name { get; set; }
    public string Base64 { get; set; }
    public string Svg { get; set; }
    public int TemplateId { get; set; }
    public string TemplateColor { get; set; }

    //navigation properties
    public List<Image> Images { get; set; } = new();
    public List<Text> Texts { get; set; } = new();


}