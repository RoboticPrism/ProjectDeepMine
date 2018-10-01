﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class Miner : MoveableBase {
    public float mineSpeed = 1f;
    public float buildSpeed = 1f;

    public MinerTask currentTask;

    // Use this for initialization
    void Start () {
        MinerManager.instance.AddMiner(this);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        DoNextTask();
    }

    private void DoNextTask()
    {
        if(currentTask == null)
        {
            StartTask(MinerManager.instance.GrabNextTask());
        }
    }

    public void StartTask(MinerTask newTask)
    {
        if (newTask != null && newTask.target)
        {
            newTask.StartTaskCoroutine(this);
            currentTask = newTask;
            currentTask.owner = this;
            EventManager.TriggerEvent("TaskStarted", currentTask.target);
        }
    }

    public void SwapTask(MinerTask task)
    {
        UnqueueTask();
        StartTask(task);
    }

    // Prematurely stop work on a task
    public void UnqueueTask()
    {
        currentTask = null;
        StopAllCoroutines();
    }

    public void CompleteTask()
    {
        MinerManager.instance.CompleteTask(currentTask);
        currentTask.DestroySelf();
        currentTask = null;
        StopAllCoroutines();
    }

    ///////////////////
    // TASK HANDLING //
    ///////////////////

    public IEnumerator MineTask(MineableWall mineableWall)
    {
        Vector3Int gridPos = GridUtilities.WorldToCell(TilemapManager.instance.wallTilemap, mineableWall.transform.position);
        yield return StartCoroutine(MoveTo(gridPos));
        yield return StartCoroutine(RotateTowards(mineableWall.transform.position));
        yield return StartCoroutine(MineWall(mineableWall));
        CompleteTask();
    }

    public IEnumerator BuildTask(BuildingBase building)
    {
        Vector3Int gridPos = GridUtilities.WorldToCell(TilemapManager.instance.wallTilemap, building.transform.position);
        yield return StartCoroutine(MoveTo(gridPos));
        yield return StartCoroutine(RotateTowards(building.transform.position));
        yield return StartCoroutine(BuildBuilding(building));
        CompleteTask();
    }

    public IEnumerator RepairTask(BuildingBase building)
    {
        Vector3Int gridPos = GridUtilities.WorldToCell(TilemapManager.instance.wallTilemap, building.transform.position);
        yield return StartCoroutine(MoveTo(gridPos));
        yield return StartCoroutine(RotateTowards(building.transform.position));
        yield return StartCoroutine(RepairBuilding(building));
        CompleteTask();
    }

    public IEnumerator DeconstructTask(BuildingBase building)
    {
        Vector3Int gridPos = GridUtilities.WorldToCell(TilemapManager.instance.wallTilemap, building.transform.position);
        yield return StartCoroutine(MoveTo(gridPos));
        yield return StartCoroutine(RotateTowards(building.transform.position));
        yield return StartCoroutine(DeconstructBuilding(building));
        CompleteTask();
    }

    public IEnumerator MineWall(MineableWall mineableWall)
    {
        while (mineableWall.life > 0)
        {
            mineableWall.MineWall(mineSpeed);
            yield return null;
        }
    }

    public IEnumerator BuildBuilding(BuildingBase building)
    {
        while (building.buildAmount < building.buildMax)
        {
            building.AddConstruction(buildSpeed);
            yield return null;
        }
    }

    public IEnumerator RepairBuilding(BuildingBase building)
    {
        while (building.life < building.lifeMax)
        {
            building.AddLife(buildSpeed);
            yield return null;
        }
    }

    public IEnumerator DeconstructBuilding(BuildingBase building)
    {
        while (building.buildAmount > 0)
        {
            building.AddConstruction(-buildSpeed);
            yield return null;
        }
    }
}
