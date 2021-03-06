using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;

public class LocalizationText
{
    //Dictionary that will hold localized text for the current language
    private static IDictionary<string, string> _content = new Dictionary<string, string>();
    private static IDictionary<string, string> Content 
    {
        get
        {
            if (_content == null || _content.Count == 0) CreateContent();
            return _content;
        }
    }


    private static string _language = globalDataPreserver.Instance.getCurrentLanguage();
    private static string Language
    {
        get
        {
            return _language;
        }
        set
        {
            if (_language != value)
            {
                _language = value;
                CreateContent();
            }
        }
    }

    public static string GetLanguage()
    {
        return Language;
    }


    public static void SetLanguage(string language)
    {
        Language = language;
    }



    public static string GetText(string key)
	{       
		string result="";		
		Content.TryGetValue(key,out result);
		
		if(string.IsNullOrEmpty(result)) return key + "[" + Language + "]" + " No Text defined";
			
		return result;
	}


	
    
    //Populates the _content dictionary keys and text for the current language
    private static void AddContent(XmlNode xNode)
    {
        foreach (XmlNode node in xNode.ChildNodes)
        {
            if (node.LocalName == "TextKey")
            {
                string value = node.Attributes.GetNamedItem("name").Value;
                string text = string.Empty;
                foreach (XmlNode langNode in node)
                {
                    if (langNode.LocalName == LocalizationText._language)
                    {
                        text = langNode.InnerText;
                        if (LocalizationText._content.ContainsKey(value))
                        {
                            LocalizationText._content.Remove(value);
                            LocalizationText._content.Add(value, value + " has been found multiple times in the XML allowed only once!");
                        }
                        else
                        {
                            LocalizationText._content.Add(value, (!string.IsNullOrEmpty(text)) ? text : ("No Text for " + value + " found"));
                        }
                        break;
                    }
                }
            }
        }
    }


    //Loads the XML file into memory to make the dictionary of values for game text to tap into
    private static void CreateContent()
    {
		XmlDocument xmlDocument = new XmlDocument ();
		xmlDocument.LoadXml (Resources.Load ("LocalizationText").ToString ());
        if (xmlDocument == null)
        {
            System.Console.WriteLine("Couldnt Load Xml");
            return;
        }
        if (_content != null)
        {
            _content.Clear();
        }
        XmlNode xNode = xmlDocument.ChildNodes.Item(1).ChildNodes.Item(0);
        LocalizationText.AddContent(xNode);
    }
}
