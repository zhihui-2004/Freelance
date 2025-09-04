using System.Collections.Generic;
using UnityEngine;

public class DissolveAll : MonoBehaviour
{
    [Header("Manual Material Assignments")]
    [Tooltip("Manually assign materials if they cannot be accessed through children.")]
    [SerializeField] private List<Material> manualMaterials = new List<Material>();
    private List<Material> materials = new List<Material>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var renders = GetComponentsInChildren<Renderer>();
        for (int i = 0; i < renders.Length; i++)
        {
            materials.AddRange(renders[i].materials);
        }

        materials.AddRange(manualMaterials);
    }

    public void SetValue(float value)
    {
        for (int i = 0; i < materials.Count; i++)
        {
            if (materials[i] != null)
            {
                materials[i].SetFloat("_Dissolve", value);
            }
        }
    }
}
