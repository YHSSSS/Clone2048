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
        string path = Application.persistentDataPath + ConstString.FILE_XML_PLAYER_INFO_PATH;
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


    public void CreateChildInXml(string _playerName, int _score)
    {
        int id = GetID();
        string path = Application.persistentDataPath + ConstString.FILE_XML_PLAYER_INFO_PATH;
        if (File.Exists(path))
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);

            XmlNode root = xmlDoc.SelectSingleNode("players");

            XmlElement element = xmlDoc.CreateElement("player");

            element.SetAttribute("ID", id.ToString());

            XmlElement nameNode = xmlDoc.CreateElement("name");
            nameNode.InnerText = _playerName;
            element.AppendChild(nameNode);

            XmlElement scoreNode = xmlDoc.CreateElement("score");
            scoreNode.InnerText = _score.ToString();
            element.AppendChild(scoreNode);

            XmlElement dateNode = xmlDoc.CreateElement("date");
            dateNode.InnerText = System.DateTime.Now.ToUniversalTime().ToString();
            element.AppendChild(dateNode);

            root.AppendChild(element);
            xmlDoc.AppendChild(root);

            xmlDoc.Save(path);
        }
        else
        {
            Debug.Log("file didn't exit");
        }
    }

    private int GetID()
    {
        int currentID = 0;
        string path = Application.persistentDataPath + ConstString.FILE_XML_PLAYER_INFO_PATH;
        if (File.Exists(path))
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);

            XmlNodeList players = xmlDoc.SelectSingleNode("players").ChildNodes;
            foreach (XmlElement player in players)
            {
                currentID = int.Parse(player.GetAttribute("ID").ToString());
            }
        }

        Debug.Log("ID :" + currentID);
        return currentID + 1;
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
        string path = Application.persistentDataPath + ConstString.FILE_XML_PLAYER_INFO_PATH;

        XmlDocument xml = new XmlDocument();

        xml.Load(path);

        List<players> playerList = new List<players>();

        //get the note in xml file
        XmlNodeList playersList = xml.SelectSingleNode("players").ChildNodes;

        foreach (XmlElement playerNode in playersList)
        {
            players playerModel = new players();
            foreach (XmlElement player in playerNode)
            {
                switch (player.Name)
                {
                    case "name":
                        playerModel.playerName = player.InnerText;
                        break;
                    case "score":
                        playerModel.bestScore = player.InnerText;
                        break;
                    case "date":
                        playerModel.recordDate = player.InnerText;
                        break;
                }
            }
            playerList.Add(playerModel);
        }

        return playerList;
    }

    public int GetCurrentValue(string _playerName)
    {
        string path = Application.persistentDataPath + ConstString.FILE_XML_PLAYER_INFO_PATH;

        XmlDocument xml = new XmlDocument();
        xml.Load(path);

        bool playerIsFound = false;

        XmlNodeList players = xml.SelectSingleNode("players").ChildNodes;
        foreach (XmlElement playerList in players)
        {
            foreach(XmlElement player in playerList)
            {
                if(player.Name.Equals("name") && player.InnerText.Equals(_playerName) && !playerIsFound)
                {
                    playerIsFound = true;
                }

                if(player.Name.Equals("score") && playerIsFound)
                {
                    return int.Parse(player.InnerText.ToString());
                }
            }
        }
        return 0;
    }

    public void UpdateScore(string _playerName, int _score)
    {
        string path = Application.persistentDataPath + ConstString.FILE_XML_PLAYER_INFO_PATH;

        XmlDocument xml = new XmlDocument();
        xml.Load(path);

        bool playerIsFound = false;

        XmlNodeList players = xml.SelectSingleNode("players").ChildNodes;
        foreach (XmlElement playerList in players)
        {
            foreach (XmlElement player in playerList)
            {
                if (player.Name.Equals("name") && player.InnerText.Equals(_playerName) && !playerIsFound)
                {
                    playerIsFound = true;
                }

                if (player.Name.Equals("score") && playerIsFound)
                {
                    player.InnerText = _score.ToString();
                }
            }
        }
    }
}
