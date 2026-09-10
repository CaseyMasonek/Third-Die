using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameSettings settings;
    [SerializeField] private GameObject privacyScreen;
    [SerializeField] private GameObject gamblingScreen;
    [SerializeField] private GameObject psychicScreen;

    private int _currentPlayer = -1;
    private int _psychic = 4;

    public int[] guesses;
    
    private void EnablePrivacy()
    {
        privacyScreen.SetActive(true);
    }

    public void DisablePrivacy()
    {
        privacyScreen.SetActive(false);
        
        NextPlayer();
    }

    private void NextPlayer()
    {
        _currentPlayer++;

        if (_currentPlayer >= settings.numberOfPlayers)
        {
            _currentPlayer = -1;
            
            Reveal();
        }

        if (_currentPlayer == _psychic) Psychic();
        else Gamble();
    }
    
    private void Gamble()
    {
        gamblingScreen.SetActive(true);
        
        guesses[_currentPlayer] = 1;
    }

    private void Psychic()
    {
        psychicScreen.SetActive(true);
    }

    public void SetGuess(int index)
    {
        guesses[_currentPlayer] = index + 1;
    }

    public void Guess()
    {
        gamblingScreen.SetActive(false);
        psychicScreen.SetActive(false);
        
        EnablePrivacy();
        
        Debug.Log(guesses[_currentPlayer]);
    }

    private void Reveal()
    {
        
    }
    
    private void Start()
    {
        guesses = new int[settings.numberOfPlayers];
        
        EnablePrivacy();
    }
}