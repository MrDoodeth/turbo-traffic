using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [Header("Panels")] 
    [SerializeField] private RawImage pausePanel;
    public void OnPausePress()
    {
        if (!CarController.isControllable)
            return;
        
        Time.timeScale = 0f;
        pausePanel.gameObject.SetActive(true);
    }

    public void OnResumePress()
    {
        pausePanel.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
    public void OnGaragePress()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }
}
