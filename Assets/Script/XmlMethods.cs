using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;

public class XmlMethods : MonoBehaviour
{
    public void CreateXML(string playerName)
    {
        string path = Application.dataPath + "/Resource/PlayerList.xml";
        if (File.Exists(path))
        {
            //创建最上一层的节点。
            XmlDocument xml = new XmlDocument();
            //创建最上一层的节点。
            XmlElement root = xml.CreateElement("players");
            //创建子节点1
            XmlElement element1 = xml.CreateElement("player");

            XmlElement elementChild1 = xml.CreateElement("name");
            elementChild1.InnerText = playerName;
            element1.AppendChild(elementChild1);

            XmlElement elementChild2 = xml.CreateElement("score");
            elementChild2.InnerText = "0";
            element1.AppendChild(elementChild2);

            XmlElement elementChild3 = xml.CreateElement("date");
            elementChild3.InnerText = System.DateTime.Now.ToUniversalTime().ToString();
            element1.AppendChild(elementChild3);
            root.AppendChild(element1);
            xml.AppendChild(root);
            //最后保存文件
            xml.Save(path);
        }
        else
        {
            Debug.Log("file didn't exit");
        }
    }

    public void updateScoreValue(string playerName, int score)
    {
        string path = Application.dataPath + "/Resource/PlayerList.xml";
        if (File.Exists(path))
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(path);
            XmlNodeList xmlNodeList = xml.SelectSingleNode("players").ChildNodes;
            foreach (XmlElement xl in xmlNodeList)
            {
                if (xl.Name.Equals(playerName))
                {
                   foreach(XmlElement x2 in xl.ChildNodes)
                    {
                        if (x2.Name.Equals("score"))
                        {
                            x2.InnerText = score.ToString();
                        }
                    }
                }
            }
            xml.Save(path);
        }
        else
        {
            Debug.Log("file didn't exit");
        }
    }

}
