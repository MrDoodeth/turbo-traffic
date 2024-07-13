using UnityEngine;
using Random = UnityEngine.Random;

public class Road : MonoBehaviour
{
    [SerializeField] private GameObject[] variants;
    public void RandomVariant()
    {
        foreach (var variant in variants)
            variant.SetActive(false);
        variants[Random.Range(0, variants.Length)].SetActive(true);
    }

    private void OnEnable()
    {
        RandomVariant();
    }
}