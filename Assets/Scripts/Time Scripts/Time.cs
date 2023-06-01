using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Time : MonoBehaviour
{
    [Header("RESULTS")]
    public float ÑurrentLength;
    public float PercentPassed;
    public Vector3 TimePoint;
    [Header("SERIALIZE")]
    [SerializeField] private List<TimeLine> _timeLines = new();
    [SerializeField] private string sceneName;

    [Header("SETTINGS")]
    public bool CanSetPoints = false;
    public Color ActiveColor;
    public Color DisableColor;
    [SerializeField] private float _scrollCooldown = 0.2f;

    [Header("MUSIC")]



    [SerializeField] private List<SpriteRenderer> _renderers = new();
    private readonly List<Transform> _linePoints = new();
    private int _numberOfLines = 0;
    [SerializeField] private int currIndex = 0; // current line index

    private void Start()
    {
        _numberOfLines = _timeLines.Count;

        for (int i = 0; i < _numberOfLines; i++)
        {
            _linePoints.Add(_timeLines[i].Tutututu.transform);
            _renderers.Add(_linePoints[i].GetComponent<SpriteRenderer>());
            _renderers[i].color = DisableColor;
        }
        _renderers[currIndex].color = ActiveColor;
    }

    private void Update()
    {
        WinCondition();
        ChangeLine();
        if (CanSetPoints)
        {
            SetPoint();
        }
    }

    private void ChangeLine()
    {
        float scrollValue = ScrollValue();
        if (scrollValue > 0.05f)
        {
            _renderers[currIndex].color = DisableColor;

            if (currIndex >= _numberOfLines - 1)
                currIndex = 0;
            else currIndex++;

            _renderers[currIndex].color = ActiveColor;
        }
        else if (scrollValue < -0.05f)
        {
            _renderers[currIndex].color = DisableColor;
            if (currIndex <= 0)
                currIndex = _numberOfLines - 1;
            else currIndex--;

            _renderers[currIndex].color = ActiveColor;
        }

        PercentPassed = _timeLines[currIndex].PercentPassed;
        ÑurrentLength = _timeLines[currIndex].ÑurrentLength;
        TimePoint = _timeLines[currIndex].TimePoint;
    }


    private float _lastScrollTime;
    private float ScrollValue()
    {
        float scrollValue = Input.mouseScrollDelta.y;
        float currentTime = UnityEngine.Time.time;
        float timeSinceLastScroll = currentTime - _lastScrollTime;

        if (timeSinceLastScroll < _scrollCooldown)
        {
            scrollValue = 0f;
        }
        if (scrollValue > 0.05f)
        {
            _lastScrollTime = currentTime;
        }
        else if (scrollValue < -0.05f)
        {
            _lastScrollTime = currentTime;
        }
        else scrollValue = 0f;

        return scrollValue;

    }

    private void SetPoint()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(_linePoints[currIndex], _linePoints[currIndex].position, Quaternion.identity);
        }
    }

    private void WinCondition()
    {
        foreach (TimeLine timeLine in _timeLines)
        {
            if (timeLine.PercentPassed != 1)
                return;
        }
        if (sceneName.Length > 1)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}