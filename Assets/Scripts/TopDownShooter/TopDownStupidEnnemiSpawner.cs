using System;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class TopDownStupidEnnemiSpawner : MonoBehaviour
{
    [SerializeField] private Vector3[] _spawnPoints;
    [SerializeField] private float _spawningRange = 2;
    [SerializeField] private float _spawningDelay = 5;
    [SerializeField] private TopDownStupidMonsters _prefabsStupidMonster;
    [SerializeField] private TopDownShooterControler _player;
 

    private float _spawningTimer;
    private bool _canSpawn { get => _spawningTimer > _spawningDelay; }

    void Update()
    {
        if (_canSpawn) {
            Vector3 pos = _spawnPoints[Random.Range(0, _spawnPoints.Length)];
            pos += new Vector3(Random.Range(-_spawningRange, _spawningRange)
                , 0, Random.Range(-_spawningRange, _spawningRange));
            TopDownStupidMonsters monster =Instantiate(_prefabsStupidMonster, pos, Quaternion.identity);
            monster.SetPlayer(_player);
            _spawningTimer = 0;
        }
        else {
            _spawningTimer += Time.deltaTime;
        }
        
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        foreach (var pos in _spawnPoints) {
            Gizmos.DrawWireSphere(pos, _spawningRange);
        }
    }
}
