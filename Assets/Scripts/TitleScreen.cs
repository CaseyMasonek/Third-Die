using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    [SerializeField] private GameSettings settings;

    private void Start()
    {
        settings.numberOfPlayers = 3;
    }
    
    public void SetNumberOfPlayers(int index)
    {
        settings.numberOfPlayers = index + 3;
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }
}
