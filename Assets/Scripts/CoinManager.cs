using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class CoinManager : MonoBehaviour
{
    // Singleton instance of CoinManager
    public static CoinManager instance;

    // Animator for the scene transition effect
    [SerializeField] private Animator transitionAnim;

    // TextMeshPro component to display the coin count
    [SerializeField] private TextMeshProUGUI coinText;

    // Current number of coins collected
    public int coinCount;

    void Update()
    {
        // Update the displayed coin count
        coinText.text = ": " + coinCount.ToString() + "/5";

        // Check if the player has collected enough coins
        if (coinCount == 5)
        {
            coinCount = 0; // Reset coin count
            CoinManager.instance.NextLevel(); // Load the next level
        }
    }

    private void Awake()
    {
        // Check if the instance is already set
        if (instance == null)
        {
            instance = this; // Set the instance
            DontDestroyOnLoad(gameObject); // Keep this object across scenes
        }
        /*else
        {
            Destroy(gameObject);
        }*/
    }

    // Method to load the next level
    public void NextLevel()
    {
        // Get the index of the next scene
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        // If at the last scene, reset to the first scene
        if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }

        // Load the next scene asynchronously
        SceneManager.LoadSceneAsync(nextSceneIndex);
    }

    // Method to load a specific scene by name
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName); // Load the specified scene
    }

    // Coroutine to handle level loading with transitions
    private IEnumerator LoadLevel()
    {
        // Trigger the transition animation
        transitionAnim.SetTrigger("End");
        yield return new WaitForSeconds(1); // Wait for the animation to finish

        // Load the next scene asynchronously
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);

        // Trigger the start of the transition animation
        transitionAnim.SetTrigger("Start");
    }
}
