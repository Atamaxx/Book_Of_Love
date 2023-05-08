using System.Collections.Generic;
using UnityEngine;

public class Chchchchchanges : MonoBehaviour
{
    GameObject[] objectsOnLayer;
    public string TargetLayerName;
    public float transparencyValue;
    public Material MainMaterial;

    List<Renderer> _renderers = new();
    List<Material> _materials = new();
    private void Awake()
    {
        ObjectFinder();       
    }
    private void Start()
    {
        SetUp();
    }
    private void Update()
    {
        transparencyValue = 1 - TimeLine.PercentPassed;

        foreach (Material material in _materials)
        {
            material.SetFloat("_Fade", transparencyValue);
        }
    }
    private void ObjectFinder()
    {
        objectsOnLayer = GameObject.FindGameObjectsWithTag(TargetLayerName);   
    }

    private void SetUp()
    {
        int index = 0;
        foreach (GameObject obj in objectsOnLayer)
        {
            _renderers.Add(obj.GetComponent<Renderer>());
            _materials.Add(_renderers[index].material);
            index++;
        }
    }
}
