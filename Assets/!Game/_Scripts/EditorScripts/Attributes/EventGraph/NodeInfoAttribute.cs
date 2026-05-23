using System;

public class NodeInfoAttribute : Attribute
{
    private string _nodeTitle;
    private string _menuItem;
    public string Title => _nodeTitle;
    public string MenuItem => _menuItem;

    public NodeInfoAttribute(string nodeTitle, string menuItem = "")
    {
        _nodeTitle = nodeTitle;
        _menuItem = menuItem;
    }
}
