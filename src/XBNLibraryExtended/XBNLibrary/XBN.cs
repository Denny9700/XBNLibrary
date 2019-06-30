using System;
using System.IO;
using System.Linq;

using XBNLibrary.structure;

namespace XBNLibrary
{
  public class XBN
  {
    public XBN_Header Header { get; set; }
    public XBN_RootElement RootElement { get; set; }

    public XBN()
    { }

    public static XBN Load(string path)
    {
      using(FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
      using(BinaryReader br = new BinaryReader(fs))
      {
        var header = XBN_Header.Deserialize(br);
        var element = XBN_RootElement.Deserialize(br, header);

        return new XBN
        {
          Header = header,
          RootElement = element
        };
      }
    }

    public void Save(string path)
    {
      using(FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
      using(BinaryWriter bw = new BinaryWriter(fs))
      {
        if(this.Header == null)
          throw new Exception("Header is null");

        if(this.RootElement == null)
          throw new Exception("Element is null");

        this.Header.Serialize(bw);
        this.RootElement.Serialize(bw);
      }
    }
  }
}
