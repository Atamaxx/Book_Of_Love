using System.Collections.Generic;
using UnityEngine;


public class TimeShaders : MonoBehaviour
{
    [SerializeField] private BookOf.Time _time;
    [SerializeField] private bool _isTimeForward = true;
    [SerializeField] private float _timeChanges;

    [SerializeField] private List<Renderer> _renderers = new();
    readonly List<Material> _materials = new();


    private void Start()
    {
        SetUp();
    }

    private void Update()
    {
        if (_isTimeForward)
        {
            _timeChanges = _time.PercentPassed;
        }
        else _timeChanges = 1 - _time.PercentPassed;


        foreach (Material material in _materials)
        {
            material.SetFloat("_TimeChanges", _timeChanges);
        }
    }

    private void SetUp()
    {
        foreach (Renderer renderer in _renderers)
        {
            _materials.Add(renderer.material);
        }
    }
}

