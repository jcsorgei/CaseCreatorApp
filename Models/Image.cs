public class Image
{
    public int Id { get; set; }
    public string Base64 { get; set; }
    public double RotateDeg { get; set; }
    public double ScaleFactor { get; set; }

    //navigation properties
    public Point CurrentTransformation { get; set; }

    [System.Text.Json.Serialization.JsonIgnore]
    public int SvgDataId { get; set; }

    [System.Text.Json.Serialization.JsonIgnore]
    public SvgData SvgData { get; set; }
}