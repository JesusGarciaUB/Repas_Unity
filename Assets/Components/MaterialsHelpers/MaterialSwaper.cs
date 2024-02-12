using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]

public class MaterialSwaper : MonoBehaviour
{
    private MeshRenderer _renderer;

    [SerializeField] private Material _materialToSwap;
    private Material _initialMaterial;

    private void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
        _initialMaterial = _renderer.material;
    }

    public void SetSecondaryMaterial()
    {
        if (_materialToSwap != null)
        {
            _renderer.material = _materialToSwap;
        }
    }

    public void SetPrimaryMaterial()
    {
        if (_initialMaterial != null)
        {
            _renderer.material = _initialMaterial;
        }
    }
}
