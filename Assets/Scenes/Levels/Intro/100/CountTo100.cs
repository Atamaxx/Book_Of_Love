using TMPro;
using UnityEngine;

public class CountTo100 : MonoBehaviour
{
    [SerializeField] private bool _goFirst = true;
    [SerializeField] private int _divideBy;
    [SerializeField] private BookOf.Time _time;
    [SerializeField] private TextMeshPro _calculationText;
    [SerializeField] private TextMeshPro _sumText;


    
    private int _currentNum;
    private string _text;
    private int _sum = 0;
    private int opponentNum;
    private bool _firstTurn = true;
    private bool _end;

    private void Update()
    {
        if (_end) return;

        _currentNum = Mathf.Clamp(Mathf.CeilToInt(_time.ÑurrentLength / _divideBy), 1, 10);

        if (_goFirst)
            Gameplay();
        else
        {
            GoSecond();
        }
        if (_end) return;
        _calculationText.text = _text + _currentNum.ToString();        
        _sumText.text = _sum.ToString();
    }

    private void GoSecond()
    {
        if (_firstTurn)
        {
            _firstTurn = false;
            opponentNum = 1;
            _text = 1 + " + ";
            _sum += 1;
        }
        Gameplay();
    }
    
    private void Gameplay()
    {
        if (!InputManager.LeftMouseButtonDown) return;

        _sum += _currentNum;

        if (Win()) return;

        opponentNum = FindCorrectNum();
        _sum += opponentNum;
        _text += _currentNum.ToString();
        
        if (Lose()) return;
        
        _text += " + " + opponentNum + " + ";
        _calculationText.text = _text;
    }

    private bool Win()
    {
        if (_sum == 100)
        {
            _text += " + " + opponentNum;
            _calculationText.text = _text;
            _sumText.text = "100 - looks like you win";
            _end = true;
            return true;
        }
        if (_sum > 100)
        {
            _text += " + " + opponentNum;
            _calculationText.text = _text;
            _sumText.text = _sum+ "? How could you mess this up? Fine, I'll count this as a win";
            _end = true;
            return true;
        }
        return false;
    }
    private bool Lose()
    {
        if (_sum == 100)
        {
            _text += " + " + opponentNum;
            _calculationText.text = _text;
            _sumText.text = "100 - You failed";
            _end = true;
            return true;
        }
        return false;
    }
    private int FindCorrectNum()
    {
        for (int i = 1; i <= 10; i++)
        {
            int sum = _sum + i;
            if (sum == 100)
            {
                return i;
            }
        }

        for (int i = 1; i <= 10; i++)
        {
            int sum = _sum + i;
            if (sum != 1 && (sum - 1) % 11 == 0)
            {
                return i;
            }
        }

        System.Random random = new();
        int randInt = random.Next(1, 6);

        return randInt;
    }
}
