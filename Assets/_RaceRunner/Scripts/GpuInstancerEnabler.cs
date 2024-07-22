using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class GpuInstancerEnabler : MonoBehaviour
{
    private void Awake()
    {
        var materialPropertyBlock = new MaterialPropertyBlock();
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.SetPropertyBlock(materialPropertyBlock);
    }
}
