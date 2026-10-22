using System.Drawing;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Lab3_3_1;

public class RadiusVector3D : IComparable<RadiusVector3D>
{
    public RadiusVector3D() { }
    public required double X { get; set; }
    public required double Y { get; set; }
    public required double Z { get; set; }
    [XmlIgnore]
    [JsonConverter(typeof(ColorJsonConverter))]
    public Color LineColor { get; set; }

    [JsonIgnore]
    public string HtmlLineColor
    {
        get => ColorTranslator.ToHtml(LineColor);
        set => LineColor = ColorTranslator.FromHtml(value);
    }

    public static RadiusVector3D Create(double x, double y, double z, Color lineColor) => 
        new() { X = x, Y = y, Z = z, LineColor = lineColor };

    public double Length() => Math.Sqrt(X * X + Y * Y + Z * Z);

    public void IncreaseBy(double segmentLength)
    {
        if (segmentLength == 0)
            return;

        if (segmentLength < 0)
            throw new ArgumentException("Must be greater than or equal to 0", nameof(segmentLength));

        var currentLength = Length();

        if (currentLength == 0)
        {
            X = Y = Z = Math.Sqrt(segmentLength * segmentLength / 3);
            return;
        }

        double newLength = currentLength + segmentLength;
        double scale = newLength / currentLength;

        X *= scale;
        Y *= scale;
        Z *= scale;
    }

    public string Info() => $"{LineColor.Name} V=({X:0.##}; {Y:0.##}; {Z:0.##}) with a length of {Length():0.##}";

    public int CompareTo(RadiusVector3D? other)
    {
        if (other == null)
            return 1;

        return Length().CompareTo(other.Length());
    }
}
