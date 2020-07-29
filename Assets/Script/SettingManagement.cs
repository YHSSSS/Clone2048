using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingManagement : MonoBehaviour
{
    [HideInInspector]
    private GameObject backgroundMusicButton;
    [HideInInspector]
    private GameObject backgroundMusicSlienceButton;
    [HideInInspector]
    private AudioSource menuMusic;

    [HideInInspector]
    private GameObject grid;

    private void Start()
    {
        //Find game objects
        backgroundMusicButton = GameObject.Find("MusicButton");
        if (!backgroundMusicButton)
            Debug.LogError("Failed to find background music button");

        backgroundMusicSlienceButton = GameObject.Find("SilenceButton");
        if (!backgroundMusicSlienceButton)
            Debug.LogError("Failed to find background music silence button");

        grid = GameObject.Find("GridPart");
        if (!grid)
            Debug.LogError("Failed to find grid part object");

        //Get component
        menuMusic = Camera.main.GetComponent<AudioSource>();
        if (!menuMusic)
            Debug.LogError("Failed to get audio source component");

        backgroundMusicButton.SetActive(true);
        backgroundMusicSlienceButton.SetActive(false);
    }
    public void StartNewGame()
    {
        //Set up the grid and blocks in the grid then initialize the variables and scripts to move the blocks.
        grid.GetComponent<SettingUpGrid>().SetUp();
        grid.GetComponent<BlocksMovement>().InitializeMovement();
    }

    public void ShowMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void PauseBackgroundMusic()
    {
        menuMusic.Pause();

        backgroundMusicButton.SetActive(false);
        backgroundMusicSlienceButton.SetActive(true);
    }

    public void PlayBackgroundMusic()
    {
        menuMusic.Play();

        backgroundMusicButton.SetActive(true);
        backgroundMusicSlienceButton.SetActive(false);
    }
}
