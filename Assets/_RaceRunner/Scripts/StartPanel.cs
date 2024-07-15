using UnityEngine;

public class StartPanel : MonoBehaviour
{
    public CarController player;
    public void TurnOnPlayer() => player.isControllable = true;
}
