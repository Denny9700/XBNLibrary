using System;
using System.IO;
using System.Xml;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using XBNLibrary;
using XBNLibrary.structure;

using XBNLibraryExtended.XML;
using XBNLibraryExtended.XML.Nodes;

namespace XBNLibraryExtended
{
  public class XMLConverter
  {
    private XMLElement xmlElement;

    private string _inFile;
    private string _outFile;

    public XMLElement XMLElement
    {
      get => this.xmlElement;
      internal set => this.xmlElement = value;
    }

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

    public XMLConverter()
      { }

    public XMLConverter(string xbnFile)
      : this()
      => this.XBNFile = xbnFile;

    public XMLConverter(string xbnFile, string xmlFile)
      : this(xbnFile)
      => this.XMLFile = xmlFile;


    public void StartConversion()
    {
      XBN xbn = XBN.Load(this.XBNFile);
      if(xbn == null)
        throw new Exception("XBN File is null");

      this.XMLElement = new XMLElement(new XmlDocument());
      this.convert(this.XMLElement.XmlDocument, xbn.RootElement);

      string path = string.Empty;
      if(string.IsNullOrEmpty(this.XMLFile))
        path = $"{Path.Combine(Path.GetDirectoryName(this.XBNFile), Path.GetFileNameWithoutExtension(this.XBNFile))}.xml";
      else
        path = this.XMLFile;

      this.XMLElement.Save(path);
    }

    private void convert(XmlDocument document, XBN_RootElement rootElem)
    {
      document.CreateXmlDeclaration("1.0", "utf-8", null);

      XMLRootNode rootNode = new XMLRootNode();
      rootNode.ElementName = rootElem.Name;

      if (rootElem.Properties.Count > 0)
        for (int i = 0; i < rootElem.Properties.Count; i++)
          rootNode.Attributes.Add(rootElem.Properties[i].Key, rootElem.Properties[i].Value);

      XmlNode root = this.XMLElement.AddRoot(rootNode);

      if(rootElem.Elements.Count > 0)
        for (int i = 0; i < rootElem.Elements.Count; i++)
          this.AddXBNElement(document, root, rootElem.Elements[i]);

      if (rootElem.Childs.Count > 0)
        for(int i = 0; i < rootElem.Childs.Count; i++)
          this.addXBNRoot(document, root, rootElem.Childs[i]);
    }

    private XmlNode addXBNRoot(XmlDocument document, XmlNode node, XBN_RootElement rootElem)
    {
      XMLRootNode rootNode = new XMLRootNode();
      rootNode.ElementName = rootElem.Name;

      if (rootElem.Properties.Count > 0)
        for (int i = 0; i < rootElem.Properties.Count; i++)
          rootNode.Attributes.Add(rootElem.Properties[i].Key, rootElem.Properties[i].Value);

      XmlNode root = this.XMLElement.AddRoot(node, rootNode);

      if (rootElem.Elements.Count > 0)
        for (int i = 0; i < rootElem.Elements.Count; i++)
          this.AddXBNElement(document, root, rootElem.Elements[i]);

      if (rootElem.Childs.Count > 0)
        for (int i = 0; i < rootElem.Childs.Count; i++)
          this.addXBNRoot(document, root, rootElem.Childs[i]);

      return root;
    }

    private XmlNode AddXBNElement(XmlDocument document, XmlNode rootNode, XBN_Element element)
    {
      XMLElementNode elementNode = new XMLElementNode();
      elementNode.ElementName = element.Name;

      if(element.Properties.Count > 0)
        for(int i = 0; i < element.Properties.Count; i++)
          elementNode.Attributes.Add(element.Properties[i].Key, element.Properties[i].Value);

      XmlNode node = this.XMLElement.AddElement(rootNode, elementNode);

      if(element.Child.Count > 0)
        for(int i = 0; i < element.Child.Count; i++)
          this.AddXBNElement(document, node, element.Child[i]);

      return node;
    }
  }
}
