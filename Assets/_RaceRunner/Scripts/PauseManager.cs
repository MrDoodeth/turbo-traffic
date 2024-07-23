using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [Header("Panels")] 
    [SerializeField] private RawImage pausePanel;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioSource engineSource;
    public void OnPausePress()
    {
        if (!CarController.isControllable)
            return;
        
        Time.timeScale = 0f;
        audioMixer.SetFloat("musicVolume", -80f);
        engineSource.mute = true;
        pausePanel.gameObject.SetActive(true);
    }

    public void OnResumePress()
    {
        pausePanel.gameObject.SetActive(false);
        audioMixer.SetFloat("musicVolume", 0f);
        engineSource.mute = false;
        Time.timeScale = 1f;
    }
    public void OnGaragePress()
    {
        Time.timeScale = 1f;
        audioMixer.SetFloat("musicVolume", 0f);
        engineSource.mute = false;
        SceneManager.LoadScene(1);
    }
}
