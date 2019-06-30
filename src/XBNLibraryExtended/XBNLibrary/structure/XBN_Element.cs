using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace XBNLibrary.structure
{
  public class XBN_Element
  {
    private XBN_Header Header { get; set; }

    public string Name { get; set; }
    public List<XBN_Property> Properties { get; set; }
    public List<XBN_Element> Child { get; set; }

    public short Unk3 { get; set;}

    public XBN_Element(XBN_Header header)
    {
      this.Header = header;
      this.Properties = new List<XBN_Property>();
      this.Child = new List<XBN_Element>();
    }

    public static XBN_Element Deserialize(BinaryReader reader, XBN_Header header)
    {
      if (reader == null)
        throw new Exception("Reader is null");

      if (header == null)
        throw new Exception("Header is null");

      var name = header.Tags[reader.ReadInt16()];
      var propertyCount = reader.ReadInt16();
      var unk3 = reader.ReadInt16();
      var tagCount = reader.ReadInt16();

      List<XBN_Property> properties = new List<XBN_Property>();
      for(int i = 0; i < propertyCount; i++)
        properties.Add(XBN_Property.Deserialize(reader, header));

      List<XBN_Element> elements = new List<XBN_Element>();
      for (int i = 0; i < tagCount; i++)
        elements.Add(XBN_Element.Deserialize(reader, header));

      return new XBN_Element(header)
      {
        Name = name,
        Unk3 = unk3,
        Properties = properties,
        Child = elements
      };
    }

    public void Serialize(BinaryWriter writer)
    {
      if(writer == null)
        throw new Exception("Writer is null");

      writer.Write((short)this.Header.Tags.IndexOf(this.Name));
      writer.Write((short)this.Properties.Count);
      writer.Write((short)this.Unk3);
      writer.Write((short)this.Child.Count);

      for(int i = 0; i < this.Properties.Count; i++)
        this.Properties[i].Serialize(writer);

      for(int i = 0; i < this.Child.Count; i++)
        this.Child[i].Serialize(writer);
    }
  }
}
