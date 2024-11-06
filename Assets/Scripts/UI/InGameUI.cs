using UnityEngine;
using UnityEngine.UI;
// For the image : https://tds.fandom.com/f/p/4400000000000180273
public class InGameUI : MonoBehaviour
{
    public Text _life;
    public Text _Coin;
    public Text[] _ButtonId;
    public int[] towerCost = { 170, 300, 720 };

    public GameObject towerContainer;
    public GameObject[] _towers;

    private int currentLife = 200;
    private int currentCoin = 150;

    private void Start()
    {
        IncrementLife(0);
        IncrementCoin(0);
        for (int i = 0; i < _ButtonId.Length; i++) { UpdateTowerCost(i); }
    }

    public void IncrementLife(int nb) { currentLife += nb; _life.text = "Life : " + currentLife; }
    public void IncrementCoin(int nb) { currentCoin += nb; _Coin.text = "Coin : " + currentCoin; }

    public void SubtractLife(int nb) { currentLife -= nb; _life.text = "Life : " + currentLife; }
    public void SubtractCoin(int nb) { currentCoin -= nb; _Coin.text = "Coin : " + currentCoin; }

    public int getLife() { return currentLife; }
    public int getCoin() { return currentCoin; }
    public void UpdateTowerCost(int index) { _ButtonId[index].text = "$ " + towerCost[index]; }

    public void spawnTower(int typeOfTower)
    {
        var tower = Instantiate(_towers[typeOfTower], transform.position, Quaternion.identity, towerContainer.transform);
    }
}
