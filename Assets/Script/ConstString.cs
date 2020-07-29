using UnityEngine;

public class ConstString : MonoBehaviour
{
    //Xml file path
    [SerializeField]
    public const string FILE_XML_PLAYER_INFO_PATH = "/Resources/PlayerList.xml";

    //Resources path
    [SerializeField]
    public const string RESOURCES_ALERT_DIALOG_PATH = "Prefab/AlertDialog";
    [SerializeField]
    public const string RESOURCES_BLOCK_PATH = "Prefab/Block";
    [SerializeField]
    public const string RESOURCES_RANK_VIEW_PATH = "Prefab/RankList";
    [SerializeField]
    public const string RESOURCES_RANK_MEMBER_PATH = "Prefab/RankMemberInfo";
    [SerializeField]
    public const string RESOURCES_BACKGROUND_MUSIC_PATH = "Music/BackgroundMusic";
    [SerializeField]
    public const string RESOURCES_MENU_MUSIC_PATH = "Music/MenuMusic";
}
