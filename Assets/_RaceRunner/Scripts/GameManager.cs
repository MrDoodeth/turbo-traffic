using NaughtyAttributes;
using UnityEngine;
using YG;

#if UNITY_EDITOR
public class GameManager : MonoBehaviour
{
    [Button("RESET")]
    public void Reset()
    {
        YandexGame.ResetSaveProgress();
        YandexGame.SaveProgress();
    }
}
#endif