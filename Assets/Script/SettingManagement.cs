using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingManagement : MonoBehaviour
{
    [SerializeField]
    private GameObject backgroundMusicButton;
    [SerializeField]
    private GameObject backgroundMusicSlienceButton;
    [SerializeField]
    private AudioSource audio;

    private GameObject grid;

    private void Start()
    {
        backgroundMusicButton.SetActive(true);
        backgroundMusicSlienceButton.SetActive(false);

        grid = GameObject.Find("GridPart");
    }
    public void StartNewGame()
    {
        grid.GetComponent<SettingUpGrid>().SetUp();
        grid.GetComponent<BlocksMovement>().InitializeMovement();
    }

    public void ShowMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void BackgroundMusicSlience()
    {
        audio.Pause();
        backgroundMusicButton.SetActive(false);
        backgroundMusicSlienceButton.SetActive(true);
    }

    public void BackgroundMusic()
    {
        audio.Play();
        backgroundMusicButton.SetActive(true);
        backgroundMusicSlienceButton.SetActive(false);
    }
}
