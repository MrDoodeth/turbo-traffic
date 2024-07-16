using UnityEngine;

public class StartPanel : MonoBehaviour
{
    public void TurnOnPlayer() => CarController.Instance.isControllable = true;
}
