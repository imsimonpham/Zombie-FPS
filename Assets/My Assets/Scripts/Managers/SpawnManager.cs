using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _zombiePrefabs;
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private float _timeBetweenWaves = 10f;
    [SerializeField] private float _waveTimer;
    private int _waveNumber = 1;
    [SerializeField] private int _maxWaveNumber;
    [SerializeField] private int _zombiesPerWave;
    [SerializeField] private Transform _zombiesContainer;

    private void Update()
    {
        if(_waveNumber == _maxWaveNumber) { return; }
        _waveTimer += Time.deltaTime;
        if (Mathf.RoundToInt(_waveTimer) >= _timeBetweenWaves)
            StartNewWave();
    }

    private void StartNewWave()
    {
        _waveTimer = 0f;
        _zombiesPerWave += 2;
        float minSpawnDistance = 4f;
        for(int i = 0; i < _zombiesPerWave; i++)
        {
            //get a random spawn point from the list
            int spawnIndex = Random.Range(0, _spawnPoints.Length);
            Transform spawnPoint = _spawnPoints[spawnIndex];  
            
            //get a spawn position at the spawn point + add a minimum distance between the spawned zombies
            Vector3 spawnPos = spawnPoint.position + Random.insideUnitSphere * minSpawnDistance;

            //set Y pos = spawn point's Y pos
            spawnPos.y = spawnPoint.position.y;

            //spawn zombies and add to container
            int zombiePrefabIndex = Random.Range(0, _zombiePrefabs.Length);
            GameObject zombiePrefab = _zombiePrefabs[zombiePrefabIndex];
            Instantiate(zombiePrefab, spawnPos, spawnPoint.rotation, _zombiesContainer);
        }
        _waveNumber++;
        UIManager.Instance.UpdateWaveNumberText(_waveNumber);
    }
}
