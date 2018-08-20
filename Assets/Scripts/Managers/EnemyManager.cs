using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    public EnemyManager instance;

    List<EnemySpawner> enemySpawners = new List<EnemySpawner>();
    public List<EnemyBase> spawnableEnemies = new List<EnemyBase>();

	// Use this for initialization
	void Start () {
        instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
