using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameSettings settings;
    [SerializeField] private GameObject privacyScreen;
    [SerializeField] private GameObject gamblingScreen;
    [SerializeField] private GameObject psychicScreen;
    [SerializeField] private GameObject revealScreen;
    [SerializeField] private GameObject resultsScreen;
    [SerializeField] private GameObject voteScreen;
    [SerializeField] private TMP_Text futureText;
    [SerializeField] private TMP_Text scoresText;
    [SerializeField] private TMP_Text winnerText;
    [SerializeField] private TMP_Text psychicText;
    
    [SerializeField] private TMP_Dropdown guesserDropdown;
    [SerializeField] private TMP_Dropdown psychicDropdown;
    
    private int _currentPlayer;
    private int _round = -1;
    private int _psychic;

    private int _roll;

    public int[] guesses;
    public int[] scores;
    
    private bool _isPsychicEliminated;
    
    private void Start()
    {
        guesses = new int[settings.numberOfPlayers];
        scores = new int[settings.numberOfPlayers];
        
        _psychic = Random.Range(0, settings.numberOfPlayers);
        
        StartRound();
    }

    public void StartRound()
    {
        revealScreen.SetActive(false);
        
        _currentPlayer = -1;
        
        _round++;

        _roll = Random.Range(0, 6) + 1;
        
        if (_round == 4) voteScreen.SetActive(true);
        else EnablePrivacy();
    }
    
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

        else if (_currentPlayer == _psychic) Psychic();
        else Gamble();
    }
    
    private void Gamble()
    {
        guesserDropdown.value = 0;
        
        gamblingScreen.SetActive(true);
        
        guesses[_currentPlayer] = 1;
    }

    private void Psychic()
    {
        psychicDropdown.value = 0;
        
        psychicScreen.SetActive(true);
        
        guesses[_currentPlayer] = 1;
        
        futureText.text = "Roll will be: " + _roll;
    }

    public void SetGuess(int index)
    {
        Debug.Log(index);
        
        guesses[_currentPlayer] = index;
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
        gamblingScreen.SetActive(false);
        psychicScreen.SetActive(false);
        revealScreen.SetActive(true);

        string scoreString = "Roll was: " + _roll + "\n\n";
        int index = 0;
        
        foreach (int i in guesses)
        {
            int score;
            int dist = Mathf.Abs(i - _roll);
            
            if (dist == 0) score = 3;
            else if (dist == 1) score = 1;
            else score = 0;
            
            scores[index] += score;
            
            scoreString += "Player " + (index + 1) + ": " + scores[index] + " (+" + score + ")" + "\n";
            index++;
        }
        
        scoresText.text = scoreString;
    }
    
    public void Eliminate(int index)
    {
        voteScreen.SetActive(false);
        
        _isPsychicEliminated = index == _psychic;
        
        Results();
    }

    private void Results()
    {
        resultsScreen.SetActive(true);
        
        bool isPsychicWinner =  DoesPsychicWin();
        
        Debug.Log("is psychich winner?" + isPsychicWinner);

        if (isPsychicWinner)
        {
            winnerText.text = "Psychic Wins!";
        }
        else
        {
            winnerText.text = "Gamblers Win!";
        }
        
        psychicText.text = "Psychic: Player " + (_psychic+1);
    }
    
    private bool DoesPsychicWin()
    {
        if (_isPsychicEliminated) return false;
        
        int psychicScore = scores[_psychic];

        int worseThanPsychic = 0;

        foreach (int score in scores)
        {
            if (score < psychicScore) worseThanPsychic++;   
        }
        
        Debug.Log(worseThanPsychic + " players did worse than the psychic!");
        Debug.Log((settings.numberOfPlayers / 2) + " is the cutoff");

        return worseThanPsychic >= settings.numberOfPlayers / 2;
    }
}