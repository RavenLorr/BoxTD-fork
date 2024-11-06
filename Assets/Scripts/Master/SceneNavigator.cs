using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNavigator : MonoBehaviour
{
    private void Start()
    {
        OpenMenu();
    }

    public static void OpenMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public static void OpenGame(string level)
    {
        SceneManager.LoadScene(level);
    }

    public static void ExitApp()
    {
        Application.Quit();
    }
}
