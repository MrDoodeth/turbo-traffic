using NaughtyAttributes;
using UnityEngine;
using YG;

public class ResetTool : MonoBehaviour
{
    #if UNITY_EDITOR
    [Button("RESET")]
    #endif
    public void Reset()
    {
        YG2.SetDefaultSaves();
        YG2.SaveProgress();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.R) && Input.GetKey(KeyCode.E) && Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.T))
        {
            Reset();
        }
    }
}