using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    bool _isPlayer1Turn;
    int _player1Point;
    int _player2Point;
    int _player1Score;
    int _player2Score;
    int _currentRound;
    int _totalRound;
    int _roundTime;

    public int Player1Point => _player1Point;
    public int Player2Point => _player2Point;
    public int Player1Score => _player1Score;
    public int Player2Score => _player2Score;
    public int CurrentRound => _currentRound;
    public int TotalRound => _totalRound;

    public int RoundTime => _roundTime;

    public event Action<int, int> OnAnyPlayersPointIncreased;
    public event Action<bool> OnPlayerTurnChangedIsPlayer1Turn;    

    void OnEnable()
    {
        CardManager.Instance.OnAllCardsOpened += OnAllCardsOpenedEvent;
    }

    public bool IsPlayer1Turn()
    {
        return _isPlayer1Turn;
    }

    public void NextPlayersTurn()
    {
        _isPlayer1Turn = !_isPlayer1Turn;
        OnPlayerTurnChangedIsPlayer1Turn?.Invoke(_isPlayer1Turn);
    }

    public void IncreasePlayer1Point(int amount)
    {
        _player1Point += amount;
        OnAnyPlayersPointIncreased?.Invoke(_player1Point, _player2Point);
    }

    public void IncreasePlayer2Point(int amount)
    {
        _player2Point += amount;
        OnAnyPlayersPointIncreased?.Invoke(_player1Point, _player2Point);
    }

    void OnAllCardsOpenedEvent()
    {
        FinishTheRound();
    }

    public void FinishTheRound()
    {
        IncreaseWinnerScore();
        ResetPoints();
    }

    public void PrepareForRound()
    {
        IncreaseCurrentRound();
        SetPlayer1sTurn();
    }

    void SetPlayer1sTurn()
    {
        _isPlayer1Turn = true;
        OnPlayerTurnChangedIsPlayer1Turn?.Invoke(true);
    }

    void IncreaseCurrentRound()
    {
        _currentRound++;
    }

    void IncreaseWinnerScore()
    {
        if (_player1Point > _player2Point)
        {
            _player1Score++;
        }
        else if (_player2Point > _player1Point)
        {
            _player2Score++;
        }
    }

    void ResetPoints()
    {
        _player1Point = 0;
        _player2Point = 0;
    }

    public void SetRounds(int roundTime, int roundCount)
    {
        _roundTime = roundTime;
        _totalRound = roundCount;
    }

    public void ResetPlayers()
    {
        _currentRound = 0;
        _player1Point = 0;
        _player2Point = 0;
        _player1Score = 0;
        _player2Score = 0;
    }
}
