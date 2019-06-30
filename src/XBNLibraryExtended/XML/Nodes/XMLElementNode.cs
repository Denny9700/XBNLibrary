using System;
using System.Collections.Generic;

namespace XBNLibraryExtended.XML.Nodes
{
  public class XMLElementNode
  {
    private string _elementName;
    private string _innerText;
    private Dictionary<string, string> _attributes;

    public string ElementName
    {
      get => this._elementName;
      internal set => this._elementName = value;
    }

    public string InnerText
    {
      get => this._innerText;
      internal set => this._innerText = value;
    }

    public Dictionary<string, string> Attributes
    {
      get => this._attributes;
      internal set => this._attributes = value;
    }

    public XMLElementNode()
    {
      this.ElementName = string.Empty;
      this.Attributes = new Dictionary<string, string>();
    }

    public XMLElementNode(string name)
      : this()
      => this._elementName = name;
    
    public XMLElementNode AddAttribute(string name, string value)
    {
      if(!this.Attributes.ContainsKey(name))
        this.Attributes.Add(name, value);

      return this;
    }

    public XMLElementNode RemoveAttribute(string name)
    {
      this.Attributes.Remove(name);
      return this;
    }

    public XMLElementNode AddInnerText(string text)
    {
      this.InnerText = text;
      return this;
    }
  }
}
