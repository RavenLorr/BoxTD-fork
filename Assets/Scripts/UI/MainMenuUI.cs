using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    public GameObject menuHolder;
    public GameObject loadGameHolder;

    public Text[] saveTextInfo;

    private GameData gameData;

    void Start()
    {
        loadGameHolder.SetActive(false);
        menuHolder.SetActive(true);
    }

    private void showSaveInfo()
    {
        if (gameData != null)
        {
            for (int i = 0; i < 3; i++)
            {
                saveTextInfo[i].text = gameData.saveDate; // Je suis un peux bloqué ici ahahah sans tester je sais pas exactement se que ça vas me sortir et comment le split bref^^
                // Pas oublié qu'il peut avoir 1,2 ou 3 save pas que 0 ou 3
            }
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                saveTextInfo[i].text = "Aucune Sauvegarde";
            }
        }
    }

    public void StartNewGame()
    {
        SceneNavigator.OpenGame("Map1");
    }

    public void LoadGame()
    {
        menuHolder.SetActive(false);
        loadGameHolder.SetActive(true);
        showSaveInfo();
    }

    public void QuitGame()
    {
        //UnityEditor.EditorApplication.isPlaying = false;
        SceneNavigator.ExitApp();
    }

    public void backToMenu()
    {
        loadGameHolder.SetActive(false);
        menuHolder.SetActive(true);
    }
}
