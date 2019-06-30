using System;
using System.Xml;
using System.Collections.Generic;

using XBNLibraryExtended.XML.Nodes;

namespace XBNLibraryExtended.XML
{
  public class XMLElement
  {
    private XmlDocument document;

    public XmlDocument XmlDocument
    {
      get => this.document;
      internal set => this.document = value;
    }

    public XMLElement(XmlDocument document)
      => this.XmlDocument = document;

    public XmlNode addRoot(string rootName)
    {
      XmlNode rootNode = document.CreateElement(rootName);

      document.AppendChild(rootNode);
      return rootNode;
    }

    public XmlNode AddRoot(XMLRootNode root)
    {
      if (root == null)
        return null;

      XmlNode rootNode = document.CreateElement(root.ElementName);
      foreach (KeyValuePair<string, string> kv in root.Attributes)
      {
        XmlAttribute attribute = document.CreateAttribute(kv.Key);
        attribute.Value = kv.Value;

        rootNode.Attributes.Append(attribute);
      }

      document.AppendChild(rootNode);
      return rootNode;
    }

    public XmlNode AddRoot(XmlNode node, string rootName)
    {
      XmlNode rootNode = document.CreateElement(rootName);

      node.AppendChild(rootNode);
      return rootNode;
    }

    public XmlNode AddRoot(XmlNode node, XMLRootNode root)
    {
      if (root == null)
        return null;

      XmlNode rootNode = document.CreateElement(root.ElementName);
      foreach (KeyValuePair<string, string> kv in root.Attributes)
      {
        XmlAttribute attribute = document.CreateAttribute(kv.Key);
        attribute.Value = kv.Value;

        rootNode.Attributes.Append(attribute);
      }
      
      node.AppendChild(rootNode);
      return rootNode;
    }

    public XmlNode AddElement(XmlNode rootNode, string elementName, String innerText)
    {
      if (string.IsNullOrEmpty(elementName) || string.IsNullOrEmpty(innerText))
        return null;

      XmlNode node = document.CreateElement(elementName);
      node.InnerText = innerText;

      rootNode.AppendChild(node);
      return node;
    }

    public XmlNode AddElement(XmlNode rootNode, XMLElementNode element)
    {
      if (element == null)
        return null;

      XmlNode node = document.CreateElement(element.ElementName);
      foreach (KeyValuePair<string, string> kv in element.Attributes)
      {
        XmlAttribute attribute = document.CreateAttribute(kv.Key);
        attribute.Value = kv.Value;

        node.Attributes.Append(attribute);
      }

      if (!string.IsNullOrEmpty(element.InnerText))
        node.InnerText = element.InnerText;

      rootNode.AppendChild(node);
      return node;
    }

    public XMLElement Save(string xmlPath)
    {
      if (document != null)
        document.Save(xmlPath);

      return this;
    }
  }
}
