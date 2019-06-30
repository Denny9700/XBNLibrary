using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace XBNLibrary.structure
{
  public class XBN_Header
  {
    public byte[] Head { get; internal set; }

    public List<string> Tags { get; set; }
    public List<string> Properties { get; set; }

    public XBN_Header()
    {
      this.Head = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xCD, 0xCC, 0xCC, 0x3D };

      this.Tags = new List<string>();
      this.Properties = new List<string>();
    }

    public static XBN_Header Deserialize(BinaryReader reader)
    {
      if(reader == null)
        throw new Exception("Reader is null");

      var head = reader.ReadBytes(8);
      var tagCount = reader.ReadInt16();

      List<string> tags = new List<string>();
      for(int i = 0; i < tagCount; i++)
        tags.Add(reader.ReadXBNStr());

      var propertyCount = reader.ReadInt16();

      List<string> properties = new List<string>();
      for(int i = 0; i < propertyCount; i++)
        properties.Add(reader.ReadXBNStr());

      return new XBN_Header
      {
        Head = head,
        Tags = tags,
        Properties = properties
      };
    }

    public void Serialize(BinaryWriter writer)
    {
      if(writer == null)
        throw new Exception("Writer is null");

      writer.Write(this.Head);
      writer.Write((short)this.Tags.Count);

      for(int i = 0; i < this.Tags.Count; i++)
        writer.WriteXBNStr(this.Tags[i]);

      writer.Write((short)this.Properties.Count);

      for(int i = 0; i < this.Properties.Count; i++)
        writer.WriteXBNStr(this.Properties[i]);
    }
  }
}
