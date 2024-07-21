using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [Header("Buttons")] 
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private Button garageButton;
    [Space]
    
    [Header("Panels")] 
    [SerializeField] private RawImage pausePanel;
    
    private void OnEnable()
    {
        pauseButton.onClick.AddListener(OnPausePress);
        resumeButton.onClick.AddListener(OnResumePress);
        garageButton.onClick.AddListener(OnGaragePress);
        RaceManager.OnPlayerFinished += () => pauseButton.interactable = false;
    }
    private void OnDisable()
    {
        pauseButton.onClick.RemoveListener(OnPausePress);
        resumeButton.onClick.RemoveListener(OnResumePress);
        garageButton.onClick.RemoveListener(OnGaragePress);
        RaceManager.OnPlayerFinished -= () => pauseButton.interactable = false;
    }
    private void OnPausePress()
    {
        Time.timeScale = 0f;
        pausePanel.gameObject.SetActive(true);
    }

    private void OnResumePress()
    {
        pausePanel.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
    private void OnGaragePress()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }
}
