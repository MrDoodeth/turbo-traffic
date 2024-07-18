using System;
using NaughtyAttributes;
using UnityEngine;
using YG;

public class GameManager : MonoBehaviour
{
    [Button("RESET")]
    public void Reset()
    {
        YandexGame.ResetSaveProgress();
        YandexGame.SaveProgress();
    }
}