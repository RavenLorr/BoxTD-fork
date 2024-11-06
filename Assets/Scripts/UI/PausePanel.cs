using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PausePanel : MonoBehaviour
{
    public GameObject pausePanel;

    private Boolean isActive = false;

    private void Start()
    {
        ActiveMenu(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isActive = !isActive;
            ActiveMenu(false);
        }
    }

    public void ActiveMenu(Boolean buttonC)
    {
        if (buttonC == true)
        {
            pausePanel.SetActive(false);
            isActive = !isActive;
        } else
        {
            pausePanel.SetActive(isActive);
        }
    }

    public void QuitToMenu()
    {
        GameController.SaveGame(0, 0, 0, GetTowersAndPositions());
        SceneNavigator.OpenMenu();
    }

    public void Exit()
    {
        GameController.SaveGame(0, 0, 0, GetTowersAndPositions());
        //UnityEditor.EditorApplication.isPlaying = false;
        SceneNavigator.ExitApp();
    }

    private List<Tuple<string, float[]>> GetTowersAndPositions()
    {
        var response = new List<Tuple<string, float[]>>();
        var controller = GameController.instance;

        var towers = controller.towerContainer.GetComponentsInChildren<Transform>();

        foreach (var item in towers)
        {
            var mainTower = item.GetComponent<MainTower>();
            float[] coords = new float[3];
            if (mainTower != null)
            {
                coords[0] = mainTower.transform.position.x;
                coords[1] = mainTower.transform.position.y;
                coords[2] = mainTower.transform.position.z;
                response.Add(new Tuple<string, float[]>(mainTower.name, coords));
            }
        }

        return response;
    }
}
