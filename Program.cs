using System.Drawing;
using System.Text.Json;
using System.Xml.Serialization;

namespace Lab3_3_1;

public class Program
{
    private static readonly RadiusVector3D[] _sourceVectors = [
        RadiusVector3D.Create(1, 2, 3, Color.Red),
        RadiusVector3D.Create(2, -4, 6, Color.Green),
        RadiusVector3D.Create(-3, -5, -7, Color.Blue)
    ];

    public static void Main(string[] args)
    {
        Console.WriteLine("Source vectors:");
        PrintVectors(_sourceVectors);
        Console.WriteLine();

        JsonSerialization("VectorsArray", _sourceVectors);
        JsonSerialization("VectorsList", _sourceVectors.ToList());
        XmlSerialization("VectorsArray", _sourceVectors);
        XmlSerialization("VectorsList", _sourceVectors.ToList());
        BinarySerialization("VectorsArray", _sourceVectors);
        BinarySerialization("VectorsList", _sourceVectors.ToList());

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }

    private static void PrintVectors(IList<RadiusVector3D> vectors)
    {
        foreach (var vector in vectors)
            Console.WriteLine($"\t-{vector.Info()}");
    }

    private static void JsonSerialization<T>(string fileName, T vectors) where T : IList<RadiusVector3D>
    {
        fileName = $"{fileName}.json";

        Console.Write("JSON serialization. ");
        using FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite);
        JsonSerializer.Serialize(fs, vectors);
        Console.Write($"Data has been saved to {fileName}\n");

        fs.Position = 0;
        var deserializedVectors = JsonSerializer.Deserialize<T>(fs);

        if (deserializedVectors == null || deserializedVectors.Count == 0)
        {
            Console.WriteLine("Something went wrong. Deserialized vectors array is empty");
            return;
        }

        Console.WriteLine("Deserialized vectors:");
        PrintVectors(deserializedVectors);
        Console.WriteLine();
    }

    private static void XmlSerialization<T>(string fileName, T vectors) where T : class, IList<RadiusVector3D>
    {
        fileName = $"{fileName}.xml";

        Console.Write("XML serialization. ");
        using FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite);
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
        xmlSerializer.Serialize(fs, vectors);
        Console.Write($"Data has been saved to {fileName}\n");

        fs.Position = 0;
        var deserializedVectors = xmlSerializer.Deserialize(fs) as T;

        if (deserializedVectors == null || deserializedVectors.Count == 0)
        {
            Console.WriteLine("Something went wrong. Deserialized vectors array is empty");
            return;
        }

        Console.WriteLine("Deserialized vectors:");
        PrintVectors(deserializedVectors);
        Console.WriteLine();
    }

    private static void BinarySerialization<T>(string fileName, T vectors)
    where T : IList<RadiusVector3D>
    {
        fileName = $"{fileName}.bin";

        Console.Write("Binary serialization. ");

        // Серіалізація
        using (FileStream fs = new FileStream(fileName, FileMode.Create))
        using (BinaryWriter writer = new BinaryWriter(fs))
        {
            writer.Write(vectors.Count);

            foreach (var vector in vectors)
            {
                writer.Write(vector.X);
                writer.Write(vector.Y);
                writer.Write(vector.Z);
                writer.Write(vector.LineColor.ToArgb());
            }
        }

        Console.WriteLine($"Data has been saved to {fileName}");

        // Десеріалізація
        List<RadiusVector3D> deserializedVectors = new();

        using (FileStream fs = new FileStream(fileName, FileMode.Open))
        using (BinaryReader reader = new BinaryReader(fs))
        {
            int count = reader.ReadInt32();

            for (int i = 0; i < count; i++)
            {
                var x = reader.ReadDouble();
                var y = reader.ReadDouble();
                var z = reader.ReadDouble();
                var color = Color.FromArgb(reader.ReadInt32());

                deserializedVectors.Add(
                    RadiusVector3D.Create(x, y, z, color)
                );
            }
        }

        Console.WriteLine("Deserialized vectors:");
        PrintVectors(deserializedVectors);
        Console.WriteLine();
    }
}
