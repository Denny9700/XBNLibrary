using System;
using System.Collections.Generic;

namespace XBNLibraryExtended.XML.Nodes
{
  public class XMLRootNode
  {
    private string _elementName;
    private Dictionary<string, string> _attributes;

    public string ElementName
    {
      get => this._elementName;
      internal set => this._elementName = value;
    }

    public Dictionary<string, string> Attributes
    {
      get => this._attributes;
      internal set => this._attributes = value;
    }

    public XMLRootNode()
    {
      this.ElementName = string.Empty;
      this.Attributes = new Dictionary<string, string>();
    }

    public XMLRootNode(string name)
      : this()
      => this.ElementName = name;

    public void AddAttribute(string name, string value)
    {
      if (!this.Attributes.ContainsKey(name))
        this.Attributes.Add(name, value);
    }

    public void RemoveAttribute(string name)
      => this.Attributes.Remove(name);
  }
}
