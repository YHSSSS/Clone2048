using UnityEngine;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Linq;

public struct players
{
    public string playerName;
    public string bestScore;
    public string recordDate;
}


public class XmlMethods : MonoBehaviour
{
    public void CreateXml()
    {
        string path = Application.dataPath + ConstString.FILE_XML_PLAYER_INFO_PATH;
        if (!File.Exists(path))
        {
            //create a xml document
            XmlDocument xmlDoc = new XmlDocument();

            XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", "");

            //create the first element
            XmlElement root = xmlDoc.CreateElement("players");

            //create the second element
            XmlElement element1 = xmlDoc.CreateElement("player");

            //create child elements and set the value text
            XmlElement elementChild1 = xmlDoc.CreateElement("name");
            elementChild1.InnerText = "Initial player";
            element1.AppendChild(elementChild1);

            XmlElement elementChild2 = xmlDoc.CreateElement("score");
            elementChild2.InnerText = "0";
            element1.AppendChild(elementChild2);

            XmlElement elementChild3 = xmlDoc.CreateElement("date");
            elementChild3.InnerText = System.DateTime.Now.ToUniversalTime().ToString();
            element1.AppendChild(elementChild3);

            root.AppendChild(element1);

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
        string path = Application.dataPath + ConstString.FILE_XML_PLAYER_INFO_PATH;
        if (File.Exists(path))
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);

            XmlElement root = xmlDoc.GetElementById("players");

            //create the second element
            XmlElement element1 = xmlDoc.CreateElement("player");

            //create child elements and set the value text
            XmlElement elementChild1 = xmlDoc.CreateElement("name");
            elementChild1.InnerText = playerName;
            element1.AppendChild(elementChild1);

            XmlElement elementChild2 = xmlDoc.CreateElement("score");
            elementChild2.InnerText = score.ToString();
            element1.AppendChild(elementChild2);

            XmlElement elementChild3 = xmlDoc.CreateElement("date");
            elementChild3.InnerText = System.DateTime.Now.ToUniversalTime().ToString();
            element1.AppendChild(elementChild3);

            root.AppendChild(element1);

            xmlDoc.Save(path);
        }
        else
        {
            Debug.Log("file didn't exit");
        }
    }

    public List<players> GetSortedPlayerList()
    {
        //load the player score record in the xml file to the list
        List<players> playerList = LoadXml();

        //using the best score to sort the list 
        //playerList = playerList.OrderByDescending(player => player.bestScore).ToList();
        playerList = playerList.OrderBy(player => player.bestScore).ToList();

        return playerList;
    }

    private List<players> LoadXml()
    {
        string path = Application.dataPath + ConstString.FILE_XML_PLAYER_INFO_PATH;

        XmlDocument xml = new XmlDocument();

        xml.Load(path);

        List<players> playerList = new List<players>();

        //get the note in xml file
        XmlNodeList players = xml.SelectSingleNode("players").ChildNodes;

        //遍历所有子节点
        foreach (XmlElement player in players)
        {
            players playerModel = new players();

            playerModel.playerName = player.GetAttribute("name").ToString();
            playerModel.bestScore = player.GetAttribute("score").ToString();
            playerModel.recordDate = player.GetAttribute("date").ToString();

            playerList.Add(playerModel);
        }

        return playerList;
    }

    public int GetCurrentValue(string playerName)
    {
        string path = Application.dataPath + ConstString.FILE_XML_PLAYER_INFO_PATH;

        XmlDocument xml = new XmlDocument();
        xml.Load(path);

        XmlNodeList xmlNodeList = xml.SelectSingleNode("players").ChildNodes;
        foreach (XmlElement player in xmlNodeList)
        {
            if (player.Name.Equals(playerName))
            {
                return int.Parse(player.GetAttribute("score").ToString());
            }
        }
        return 0;
    }
}
