using UnityEngine;
using System.IO;
using System.Xml;

public class XmlMethods : MonoBehaviour
{
    private string path;

    private void Start()
    {
        path = Application.dataPath + ConstString.RESOURCES_XML_PLAYER_INFO_PATH;
    }

    public void CreateXml()
    {
        if (!File.Exists(path))
        {
            //create a xml document
            XmlDocument xmlDoc = new XmlDocument();

            XmlDeclaration xmldecl = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", "");
            xmlDoc.AppendChild(xmldecl);

            //create the first element
            XmlElement root = xmlDoc.CreateElement("players");

            xmlDoc.AppendChild(root);

            //save the file 
            xmlDoc.Save(path);
        }
        else
        {
            Debug.Log("file exit");
        }
    }

    public void CreateChildInXml(string playerName, int score)
    {
        string path = Application.dataPath + ConstString.RESOURCES_XML_PLAYER_INFO_PATH;
        if (File.Exists(path))
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(path);

            XmlElement root = xml.GetElementById("players");

            //create the second element
            XmlElement element1 = xml.CreateElement("player");

            //create child elements and set the value text
            XmlElement elementChild1 = xml.CreateElement("name");
            elementChild1.InnerText = playerName;
            element1.AppendChild(elementChild1);

            XmlElement elementChild2 = xml.CreateElement("score");
            elementChild2.InnerText = score.ToString();
            element1.AppendChild(elementChild2);

            XmlElement elementChild3 = xml.CreateElement("date");
            elementChild3.InnerText = System.DateTime.Now.ToUniversalTime().ToString();
            element1.AppendChild(elementChild3);

            root.AppendChild(element1);

            xml.Save(path);
        }
        else
        {
            Debug.Log("file didn't exit");
        }
    }

    public void LoadXml()
    {
        string path = Application.dataPath + ConstString.RESOURCES_XML_PLAYER_INFO_PATH;

        XmlDocument xml = new XmlDocument();

        xml.Load(path);

        //get the note in xml file
        XmlNodeList playerList = xml.SelectSingleNode("players").ChildNodes;

        //遍历所有子节点
        foreach (XmlElement player in playerList)
        {
            foreach (XmlElement elements in player.ChildNodes)
            {
                //放到一个textlist文本里
                //textList.Add(xl2.GetAttribute("name") + ": " + xl2.InnerText);
                //得到name为a的节点里的内容。放到TextList里
                /*
                if (xl2.GetAttribute("name") == "a")
                {
                    Adialogue.Add(xl2.GetAttribute("name") + ": " + xl2.InnerText);
                    print("******************" + xl2.GetAttribute("name") + ": " + xl2.InnerText);
                }
                //得到name为b的节点里的内容。放到TextList里
                else if (xl2.GetAttribute("map") == "abc")
                {
                    Bdialogue.Add(xl2.GetAttribute("name") + ": " + xl2.InnerText);
                    print("******************" + xl2.GetAttribute("name") + ": " + xl2.InnerText);
                }
                */
                print(elements.Name + ":" + elements.InnerText);
            }
        }
    }

    public int GetCurrentValue(string playerName)
    {
        XmlDocument xml = new XmlDocument();
        xml.Load(path);

        XmlNodeList xmlNodeList = xml.SelectSingleNode("players").ChildNodes;
        foreach (XmlElement xl in xmlNodeList)
        {
            if (xl.Name.Equals(playerName))
            {
                foreach (XmlElement x2 in xl.ChildNodes)
                {
                    if (x2.Name.Equals("score"))
                    {
                        return int.Parse(x2.InnerText.ToString());
                    }
                }
            }
        }
        return 0;
    }
}
