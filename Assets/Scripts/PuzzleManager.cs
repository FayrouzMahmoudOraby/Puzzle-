using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance;

    public GameObject winText;
    public GameObject restartButton;

    private int correctPlacements = 0;
    private int totalCubes = 3;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("PuzzleManager instance assigned.");
        }
        else
        {
            Debug.LogWarning("Multiple PuzzleManager instances found! Destroying duplicate.");
            Destroy(gameObject);
        }

        if (winText != null)
        {
            winText.SetActive(false);
            Debug.Log("Win Text initialized and hidden.");
        }
        else
        {
            Debug.LogWarning("winText is not assigned in the Inspector!");
        }

        if (restartButton != null)
        {
            restartButton.SetActive(false);
            Debug.Log("Restart Button initialized and hidden.");
        }
        else
        {
            Debug.LogWarning("restartButton is not assigned in the Inspector!");
        }
    }

    public void NotifyCubePlaced()
    {
        correctPlacements++;
        Debug.Log($"Correct cube placed! Total correct: {correctPlacements}/{totalCubes}");

        if (correctPlacements >= totalCubes)
        {
            Debug.Log("All cubes placed. Triggering win condition.");
            ShowWin();
        }
    }

    private void ShowWin()
    {
        if (winText != null)
        {
            winText.SetActive(true);
            Debug.Log("Win Text displayed.");
        }
        else
        {
            Debug.LogError("Win Text is missing in ShowWin().");
        }

        if (restartButton != null)
        {
            restartButton.SetActive(true);
            Debug.Log("Restart Button displayed.");
        }
        else
        {
            Debug.LogError("Restart Button is missing in ShowWin().");
        }

        Debug.Log("Puzzle Completed!");
    }

    public void RestartPuzzle()
    {
        Debug.Log("Restart button clicked. Reloading scene.");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
