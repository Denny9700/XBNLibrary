using System;
using System.IO;
using System.Linq;

namespace XBNLibraryExtended.Testing
{
  class Program
  {
    static void Main(string[] args)
    {
      var xbnFolder = Path.Combine(Environment.CurrentDirectory, "xbn");
      var xmlFolder = Path.Combine(Environment.CurrentDirectory, "xml");
      var outFolder = Path.Combine(Environment.CurrentDirectory, "out");
      if (!Directory.Exists(xbnFolder))
        Directory.CreateDirectory(xbnFolder);

      if (!Directory.Exists(xmlFolder))
        Directory.CreateDirectory(xmlFolder);

      if (!Directory.Exists(outFolder))
        Directory.CreateDirectory(outFolder);

      var conversionType = -1;
      Console.Write("Select conversion type: [0] xbn->xml, [1]xml->xbn: ");
      conversionType = int.Parse(Console.ReadLine());

      if (conversionType < 0 || conversionType > 1)
      {
        Console.Write("Wrong conversion type");
        return;
      }

      if (conversionType == 0)
      {
        string[] files = Directory.GetFiles(xbnFolder).Where(x => Path.GetExtension(x).Equals(".xbn")).ToArray();
        if (files.Length <= 0)
          Console.WriteLine("No files found");
        else
        {
          Console.WriteLine("Files found:");
          for (int i = 0; i < files.Length; i++)
            Console.WriteLine($"[{i}] --> {Path.GetFileName(files[i])}");
          Console.WriteLine();

          int selected = -1;
          bool sel = false;

          while (!sel)
          {
            Console.Write("Select: ");

            selected = int.Parse(Console.ReadLine());
            if (selected < 0 || selected >= files.Length)
              Console.WriteLine("Invalid selection");
            else
              sel = true;
          }

          Console.WriteLine($"You have selected -> {Path.GetFileName(files[selected])}");

          XMLConverter converter = new XMLConverter(files[selected], $"{Path.Combine(outFolder, Path.GetFileNameWithoutExtension(files[selected]))}.xml");
          converter.StartConversion();

          Console.WriteLine("XBN to XML conversion completed...");
        }
      }
      else if (conversionType == 1)
      {
        string[] files = Directory.GetFiles(xmlFolder).Where(x => Path.GetExtension(x).Equals(".xml")).ToArray();
        if (files.Length <= 0)
          Console.WriteLine("No files found");
        else
        {
          Console.WriteLine("Files found:");
          for (int i = 0; i < files.Length; i++)
            Console.WriteLine($"[{i}] --> {Path.GetFileName(files[i])}");
          Console.WriteLine();

          int selected = -1;
          bool sel = false;

          while (!sel)
          {
            Console.Write("Select: ");

            selected = int.Parse(Console.ReadLine());
            if (selected < 0 || selected >= files.Length)
              Console.WriteLine("Invalid selection");
            else
              sel = true;
          }

          Console.WriteLine($"You have selected -> {Path.GetFileName(files[selected])}");

          XBNConverter converter = new XBNConverter(files[selected], $"{Path.Combine(outFolder, Path.GetFileNameWithoutExtension(files[selected]))}.xbn");
          converter.StartConversion();

          Console.WriteLine("XML to XBN conversion completed...");
        }
      }
      Console.ReadKey();
    }
  }
}
