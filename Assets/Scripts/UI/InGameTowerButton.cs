using UnityEngine;
using UnityEngine.UI;

public class InGameTowerButton : MonoBehaviour
{
    public int towerType; // 0 for Assault Turret, 1 for Sniper Turret, 2 for Gatling Turret
    public InGameUI inGameUI;
    public GameController gameController;

    private void Start()
    {
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(SpawnTower);
        }
    }

    private void SpawnTower()
    {
        if (inGameUI != null)
        {
            if (inGameUI.towerCost[towerType] > inGameUI.getCoin())
            {
                Debug.Log("Pas suffisament de coin pour acheter cette tower");
            } else
            {
                inGameUI.spawnTower(towerType);
                inGameUI.SubtractCoin(inGameUI.towerCost[towerType]);
            }
        }
    }
}
