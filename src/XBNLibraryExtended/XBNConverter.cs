using System;
using System.IO;
using System.Xml;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using XBNLibrary;
using XBNLibrary.structure;

namespace XBNLibraryExtended
{
  public class XBNConverter
  {
    private string _inFile;
    private string _outFile;

    public string XBNFile
    {
      get => this._inFile;
      internal set => this._inFile = value;
    }

    public string XMLFile
    {
      get => this._outFile;
      internal set => this._outFile = value;
    }

    public XBNConverter()
    { }

    public XBNConverter(string xmlFile)
      : this()
      => this.XMLFile = xmlFile;

    public XBNConverter(string xmlFile, string xbnFile)
      : this(xmlFile)
      => this.XBNFile = xbnFile;

    public void StartConversion()
    {
      XBN xbnElement = new XBN();

      XmlDocument document = new XmlDocument();
      document.Load(this.XMLFile);

      XBN_Header header = generateHeader(document);
      xbnElement.Header = header;

      XBN_RootElement rootElement = this.generateRoot(document, header);
      xbnElement.RootElement = rootElement;

      xbnElement.Save(this.XBNFile);
    }

    private XBN_Header generateHeader(XmlDocument document)
    {
      List<string> elementNames = new List<string>();
      var elements = document.SelectNodes("//*");

      for (int i = 0; i < elements.Count; i++)
        if(!elementNames.Contains(elements[i].Name))
          elementNames.Add(elements[i].Name);


      List<string> attributes = new List<string>();
      var nxList = document.SelectNodes("//*");

      if (nxList != null)
      {
        foreach (XmlNode node in nxList)
          attributes.AddRange(getAttributes(node));
      }
      attributes = attributes.Distinct().ToList();

      XBN_Header header = new XBN_Header()
      {
        Tags = elementNames,
        Properties = attributes
      };

      return header;
    }

    public XBN_RootElement generateRoot(XmlDocument document, XBN_Header header)
    {
      XBN_RootElement rootElement = new XBN_RootElement(header);

      foreach(XmlNode xmlNode in document)
        if(xmlNode.NodeType == XmlNodeType.XmlDeclaration)
          document.RemoveChild(xmlNode);

      var firstChild = document.FirstChild;

      rootElement.Name = firstChild.Name;
      if( firstChild.Attributes.Count > 0)
        for(int i = 0; i < firstChild.Attributes.Count; i++)
          rootElement.Properties.Add(new XBN_Property(header, firstChild.Attributes[i].Name, firstChild.Attributes[i].Value));

      if(firstChild.ChildNodes.Count > 0)
        for(int i = 0; i < firstChild.ChildNodes.Count; i++)
          rootElement.Elements.Add(generateElement(firstChild.ChildNodes[i], header));

      return rootElement;
    }

    public XBN_Element generateElement(XmlNode node, XBN_Header header)
    {
      XBN_Element element = new XBN_Element(header);
      
      element.Name = node.Name;
      if(node.Attributes.Count > 0)
        for(int i = 0; i < node.Attributes.Count; i++)
          element.Properties.Add(new XBN_Property(header, node.Attributes[i].Name, node.Attributes[i].Value));

      if(node.ChildNodes.Count > 0)
        for(int i = 0; i < node.ChildNodes.Count; i++)
          element.Child.Add(generateElement(node.ChildNodes[i], header));

      return element;
    }

    private static List<string> getAttributes(XmlNode node)
    {
      List<string> attributes = new List<string>();
      for (int i = 0; i < node.Attributes.Count; i++)
        attributes.Add(node.Attributes[i].Name);

      for (int i = 0; i < node.ChildNodes.Count; i++)
        getAttributes(node.ChildNodes[i]);

      return attributes;
    }
  }
}
