using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Newtonsoft.Json;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

[System.Serializable]
public class GameData
{
    public int playerLife;
    public int playerCoin;
    public int waveNumber;
    public List<Tuple<String, float[]>> towers;
    public string saveDate;
}

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public bool isReady = true;
    public bool finishedSpawningWave = true;
    public Waves wavesContainer;
    public GameObject[] towers;
    public GameObject towerContainer;
    public GameObject enemyContainer;
    public GameObject[] levelWaypoints;
    public List<GameObject> enemiesAlive;
    public float boxChildrenSpawnDelay = 0.1f;
    private int currentWaveIndex = 0;

    public InGameUI inGameUI;

    public void ToggleReady()
    {
        isReady = !isReady;
    }


    private void Awake()
    {
        instance = this;
        inGameUI = FindObjectOfType<InGameUI>();
    }

    // Existing code...

    private void Start()
    {
        StartCoroutine(IncreaseCoinOverTime());
    }

    private IEnumerator IncreaseCoinOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            inGameUI.IncrementCoin(10);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isReady && finishedSpawningWave && enemiesAlive.Count == 0)
        {
            startWave(wavesContainer.waves[currentWaveIndex]);
        }
        
    }

    private void startWave(Wave wave)
    {
        finishedSpawningWave = false;
        
        StartCoroutine("waveCoroutine", wave);
    }

    private IEnumerator waveCoroutine(Wave wave)
    {
        int currentSectionIndex = 0;
        int nSections = wave.waveSection.Count;

        while (currentSectionIndex < nSections)
        {
            Vector3 currentSection = wave.waveSection[currentSectionIndex];

            int enemiesSpawned = 0;

            var enemy = wave.enemies[(int)currentSection.x];
            var nEnemy = currentSection.y;
            var delay = currentSection.z/1000;
            
            while (enemiesSpawned < nEnemy)
            {
                yield return new WaitForSeconds(delay);
                GameObject spawnedEnemy = Instantiate(enemy, levelWaypoints[0].transform.position, Quaternion.identity, enemyContainer.transform);

                MainEnemyBehaviour spawnedEnemyBehaviour = spawnedEnemy.GetComponent<MainEnemyBehaviour>();
                spawnedEnemyBehaviour.SetWaypoints(levelWaypoints);
                spawnedEnemyBehaviour.EnemyDied.AddListener(OnEnemyDied);
                spawnedEnemy.tag = "Enemy";
                enemiesAlive.Add(spawnedEnemy);
                enemiesSpawned++;
            }


            currentSectionIndex++;
            yield return null;
        }

        yield return new WaitForSeconds(5);
        currentWaveIndex++;
        finishedSpawningWave = true;
    }

    public void StartNextLayerCoroutine(GameObject[] children, Vector3 position, int targetWaypointIndex, GameObject targetWaypoint)
    {
        var nextLayerSpawnData = new NextLayerSpawnData(children, position, targetWaypointIndex, targetWaypoint);
        StartCoroutine("SpawnNextBoxLayerCoroutine", nextLayerSpawnData);
    }

    private IEnumerator SpawnNextBoxLayerCoroutine(NextLayerSpawnData nextLayerSpawnData)
    {
        foreach (var child in nextLayerSpawnData.children)
        {
            var newEnemy = Instantiate(child, nextLayerSpawnData.position, Quaternion.identity, enemyContainer.transform);
            enemiesAlive.Add(newEnemy);

            var newEnemyBehaviour = newEnemy.GetComponent<MainEnemyBehaviour>();
            newEnemyBehaviour.SetWaypoints(levelWaypoints);
            newEnemyBehaviour.EnemyDied.AddListener(OnEnemyDied);

            var newMovementBehaviour = newEnemy.GetComponent<EnemyMovement>();

            newMovementBehaviour.targetWaypointIndex = nextLayerSpawnData.targetWaypointIndex;
            newMovementBehaviour.targetWaypoint = nextLayerSpawnData.targetWaypoint;
            yield return new WaitForSeconds(boxChildrenSpawnDelay);
        }
        
        yield return null;
    }

    private void OnEnemyDied(GameObject gameObject)
    {
        enemiesAlive.Remove(gameObject);
    }

    private class NextLayerSpawnData
    {
        public GameObject[] children { get; set; }
        public Vector3 position { get; set; }
        public int targetWaypointIndex { get; set; }
        public GameObject targetWaypoint { get; set; }

        public NextLayerSpawnData(GameObject[] children, Vector3 position, int targetWaypointIndex, GameObject targetWaypoint) {
            this.children = children;
            this.position = position;
            this.targetWaypointIndex = targetWaypointIndex;
            this.targetWaypoint = targetWaypoint;
        }
    }

    public static void SaveGame(int playerLife, int playerCoin, int waveNumber, List<Tuple<String, float[]>> towers)
    {
        GameData data = new GameData();
        data.playerLife = playerLife;
        data.playerCoin = playerCoin;
        data.waveNumber = waveNumber;
        data.towers = towers;
        data.saveDate = System.DateTime.Now.ToString();

        string jsonData = JsonConvert.SerializeObject(data);
        PlayerPrefs.SetString("GameData", jsonData);
        PlayerPrefs.Save();
    }

    public static GameData LoadGame()
    {
        if (PlayerPrefs.HasKey("GameData"))
        {
            string jsonData = PlayerPrefs.GetString("GameData");
            GameData data = JsonConvert.DeserializeObject<GameData>(jsonData);
            instance.PlaceTowersFromSave(data.towers);
            return data;
        }
        else
        {
            Debug.LogWarning("No Save Found");
            return null;
        }
    }

    public static void LoadSavedGame()
    {
        GameData loadedData = LoadGame();

        if (loadedData != null)
        {
            Debug.Log("Nombre de vie: " + loadedData.playerLife);
            Debug.Log("Nombre de coins: " + loadedData.playerCoin);
            Debug.Log("Nombre de vague: " + loadedData.waveNumber);
            Debug.Log("Tours: " + loadedData.towers);
            Debug.Log("Date de sauvegarde: " + loadedData.saveDate);
        }
    }

    private void PlaceTowersFromSave(List<Tuple<String, float[]>> towers)
    {
        foreach (var tower in towers)
        {
            Debug.Log(tower.Item1);
            GameObject newTower = null;
            Vector3 newPos = new Vector3(tower.Item2[0], tower.Item2[1], tower.Item2[2]);

            switch (tower.Item1)
            {
                case "Assault":
                    newTower = Instantiate(this.towers[0], newPos, Quaternion.identity, towerContainer.transform);
                    break;
                case "Sniper":
                    newTower = Instantiate(this.towers[1], newPos, Quaternion.identity, towerContainer.transform);
                    break;
                case "Gatling":
                    newTower = Instantiate(this.towers[2], newPos, Quaternion.identity, towerContainer.transform);
                    break;
            }

            if (newTower != null)
            {
                newTower.GetComponent<MainTower>().isPlaced = true;
            }
        }
    }
}
