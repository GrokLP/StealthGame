using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnSixFour : MonoBehaviour
{
    [SerializeField] Transform spawnPos;
    [SerializeField] GameObject enemyPrefab;

    [SerializeField] List <GameObject> enemiesOnScreen = new List<GameObject>();

    private void Start()
    {
        enemiesOnScreen.Add(enemyPrefab);
    }


}
