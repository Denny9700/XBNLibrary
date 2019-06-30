using System;
using System.IO;
using System.Collections.Generic;

namespace XBNLibrary.structure
{
  public class XBN_RootElement
  {
    private XBN_Header Header { get; set; }

    public string Name { get; set; }

    public short Unk1 { get; set; }
    public short Unk2 { get; set; }

    public List<XBN_RootElement> Childs { get; set; }
    public List<XBN_Property> Properties { get; set; }
    public List<XBN_Element> Elements { get; set; }

    public XBN_RootElement(XBN_Header header)
    {
      this.Header = header;

      this.Elements = new List<XBN_Element>();
      this.Childs = new List<XBN_RootElement>();
      this.Properties = new List<XBN_Property>();
    }

    public static XBN_RootElement Deserialize(BinaryReader reader, XBN_Header header)
    {
      if (reader == null)
        throw new Exception("Reader is null");

      if(header == null)
        throw new Exception("Header is null");

      var mainTag = header.Tags[reader.ReadInt16()];
      var unk1 = reader.ReadInt16();
      var unk2 = reader.ReadInt16();

      var totalElements = reader.ReadInt16();

      List<XBN_Element> elements = new List<XBN_Element>();
      List<XBN_Property> properties = new List<XBN_Property>();
      List<XBN_RootElement> childs = new List<XBN_RootElement>();

      if (unk1 > 0)
      {
        for (int i = 0; i < totalElements; i++)
          properties.Add(XBN_Property.Deserialize(reader, header));

        //Testing
        for(int i = 0; i < unk1; i++)
          childs.Add(Deserialize(reader, header));
      }
      else
      {
        for (int i = 0; i < totalElements; i++)
          elements.Add(XBN_Element.Deserialize(reader, header));
      }

      return new XBN_RootElement(header)
      {
        Name = mainTag,
        Unk1 = unk1,
        Unk2 = unk2,
        Childs = childs,
        Elements = elements,
        Properties = properties
      };
    }

    public void Serialize(BinaryWriter writer)
    {
      if (writer == null)
        throw new Exception("Writer is null");

      writer.Write((short)this.Header.Tags.IndexOf(this.Name));
      writer.Write((short)this.Unk1);
      writer.Write((short)this.Unk2);
      writer.Write((short)this.Elements.Count);

      if (this.Unk1 > 0)
      {
        for (int i = 0; i < this.Properties.Count; i++)
          this.Properties[i].Serialize(writer);

        for (int i = 0; i < this.Childs.Count; i++)
          this.Childs[i].Serialize(writer);
      }
      else
      {
        for (int i = 0; i < this.Elements.Count; i++)
          this.Elements[i].Serialize(writer);
      }
    }
  }
}
