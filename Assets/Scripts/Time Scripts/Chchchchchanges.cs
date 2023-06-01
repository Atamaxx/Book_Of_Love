using System.Collections.Generic;
using UnityEngine;

public class Chchchchchanges : MonoBehaviour
{
    GameObject[] objectsOnLayer;
    public string TargetLayerName;
    public float TimeChanges;
    public Material MainMaterial;
    [SerializeField] private Time _timeLine;

    readonly List<Renderer> _renderers = new();
    readonly List<Material> _materials = new();
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
        TimeChanges = 1 - _timeLine.PercentPassed;

        foreach (Material material in _materials)
        {
            material.SetFloat("_TimeChanges", TimeChanges);
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
