public class Text
{
    public int Id { get; set; }
    public string TextValue { get; set; }
    public string Color { get; set; }
    public string FontStyle { get; set; }
    public double RotateDeg { get; set; }
    public double ScaleFactor { get; set; }

    // navigation properties

    [System.Text.Json.Serialization.JsonIgnore]
    public int SvgDataId { get; set; }

    [System.Text.Json.Serialization.JsonIgnore]
    public SvgData SvgData { get; set; }
    public Point CurrentTransformation { get; set; }

}