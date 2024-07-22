using TMPro;
using UnityEngine;

public class GameUIStates : MonoBehaviour
{
    [Header("HighSpeed")]
    [SerializeField] private Transform highSpeedPanel;
    [SerializeField] private TMP_Text highSpeedLabel;
    private float highSpeedSession;
    private Vector3 highSpeedPanelPosition;
    [Space]
    
    [Header("Oncoming")]
    [SerializeField] private Transform oncomingPanel;
    [SerializeField] private TMP_Text oncomingLabel;
    private float oncomingSession;
    private Vector3 oncomingPanelPosition;
    
    private const float PANEl_SPEED = 10f;
    
    private void Start()
    {
        highSpeedPanelPosition = highSpeedPanel.transform.position;
        oncomingPanelPosition = oncomingPanel.transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
        Vector3 targetHighSpeedPanelPosition;
        Vector3 targetOncomingPanelPosition;
        
        if (CarController.currentSpeed >= RaceManager.SCORE_START_SPEED)
        {
            targetHighSpeedPanelPosition = new Vector3(-highSpeedPanelPosition.x, highSpeedPanelPosition.y,
                highSpeedPanelPosition.z);
            
            highSpeedSession += Time.deltaTime;
            highSpeedLabel.text = highSpeedSession.ToString("F");
            
            if (CarController.isOncoming)
            {
                oncomingSession += Time.deltaTime;
                targetOncomingPanelPosition = new Vector3(-oncomingPanelPosition.x, oncomingPanelPosition.y,
                    oncomingPanelPosition.z);
                oncomingLabel.text = oncomingSession.ToString("F");
            }
            else
            {
                oncomingSession = 0;
                targetOncomingPanelPosition = oncomingPanelPosition;
            }
        }
        else
        {
            oncomingSession = 0;
            highSpeedSession = 0;
            targetHighSpeedPanelPosition = highSpeedPanelPosition;
            targetOncomingPanelPosition = oncomingPanelPosition;
        }
        
        highSpeedPanel.transform.position = Vector3.Lerp(highSpeedPanel.transform.position,
            targetHighSpeedPanelPosition, PANEl_SPEED * Time.deltaTime);
        
        oncomingPanel.transform.position = Vector3.Lerp(oncomingPanel.transform.position,
            targetOncomingPanelPosition, PANEl_SPEED * Time.deltaTime);
    }
}