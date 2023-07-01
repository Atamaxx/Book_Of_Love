using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BookOf
{
    public class Time : MonoBehaviour
    {
        [Header("SETTINGS")]
        public bool CanWin = false;
        public bool CanFreezeTime = false;

        public Color ActiveColor;
        public Color DisableColor;
        [SerializeField] private float _scrollCooldown = 0.2f;

        [Header("- setting points")]
        public bool CanSetPoints = false;
        public int MaxNumberOfPoints = 3;
        public List<Vector3> SetPoints = new();
        public int ActivePointNum = 0;
        public Transform PointPrefab;

[Header("RESULTS")]
        public float ÑurrentLength;
        public float PercentPassed;
        public Vector3 TimePoint;
        public Vector3 LastPoint;
        [Header("SERIALIZE")]
        [SerializeField] private List<TimeLine> _timeLines = new();
        [SerializeField] private string sceneName;

        [Header("MUSIC")]



        [SerializeField] private List<SpriteRenderer> _renderers = new();
        private readonly List<Transform> _linePoints = new();
        private int _numberOfLines = 0;
        [SerializeField] private int currIndex = 0; // current line index

        private void Awake()
        {
            _numberOfLines = _timeLines.Count;

            for (int i = 0; i < _numberOfLines; i++)
            {
                _linePoints.Add(_timeLines[i].Tutututu.transform);
                _renderers.Add(_linePoints[i].GetComponent<SpriteRenderer>());
                _renderers[i].color = DisableColor;
            }
            _renderers[currIndex].color = ActiveColor;

            //StartingPoints();
        }

        private GameObject _parentObject;
        private void Start()
        {
            if (CanSetPoints)
                _parentObject = new("Setted Points");
        }

        private void Update()
        {
            WinCondition();
            ChangeLine();
            SetPoint();
            TimeFrozen();


        }


        #region CHANGING LINES
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
        #endregion

        #region SETTING POINTS

        private void SetPoint()
        {
            if (InputManager.LeftMouseButtonDown && CanSetPoints)
            {
                int numberOfPoints = SetPoints.Count;
                if (numberOfPoints >= MaxNumberOfPoints)
                {
                    SetPoints.RemoveAt(0);
                }
                SetPoints.Add(TimePoint);
                ActivePointNum = numberOfPoints - 1;

                OnPointSet();
            }
        }

        private void OnPointSet()
        {
            foreach (Transform child in _parentObject.transform)
            {
                Destroy(child.gameObject);
            }

            foreach (Vector3 point in SetPoints)
            {
                Transform currPoint = Instantiate(PointPrefab, point, Quaternion.identity);
                currPoint.SetParent(_parentObject.transform);
                currPoint.name = "point";
            }


            if (CanFreezeTime)
                FreezePast();
        }


        #endregion


        private void WinCondition()
        {
            if (!CanWin) return;
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


        private float _freezeDistance;
        private float _freezePercent;
        private Vector3 _freezePoint;
        private bool _isTimeFrozen;

        private void FreezePast()
        {
            _isTimeFrozen = true;
            _freezeDistance = ÑurrentLength;
            _freezePoint = TimePoint;
            _freezePercent = PercentPassed;
        }

        private void TimeFrozen()
        {
            if (!_isTimeFrozen || _freezeDistance < ÑurrentLength) return;

            ChangeAllResults(_freezeDistance, _freezePoint, _freezePercent);
        }

        private void ChangeAllResults(float newDistance, Vector3 newTimePoint, float newPercent)
        {
            ÑurrentLength = newDistance;
            TimePoint = newTimePoint;
            PercentPassed = newPercent;

            //_timeLines[currIndex].ÑurrentLength = newDistance;
            //_timeLines[currIndex].TimePoint = newTimePoint;
            //_timeLines[currIndex].PercentPassed = newPercent;
        }
    }
}