using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;
    [SerializeField] GameObject MainMenuPrefab;
    [SerializeField] GameObject OptionsMenuPrefab;
    public AudioMixer audioMixer;

    private void Start()
    {
        HideOptions();
        // Restore current audio mixer volume, implement saving volume locally
        float value;
        bool result = audioMixer.GetFloat("Volume", out value);
        if (result) volumeSlider.value = value;
        else volumeSlider.value = -80f;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void EnterCastle()
    {
        GameManager.Game.LoadScene("Castle");
    }

    public void ShowOptions()
    {
        OptionsMenuPrefab.SetActive(true);
        MainMenuPrefab.SetActive(false);
    }

    public void HideOptions()
    {
        OptionsMenuPrefab.SetActive(false);
        MainMenuPrefab.SetActive(true);
        SaveSystem.SaveGame();
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
        DataManager.Data.audioLevel = volume;
    }
}
    