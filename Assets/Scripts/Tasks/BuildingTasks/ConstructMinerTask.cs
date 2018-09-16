using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructMinerTask : BuildingTask {

    public Miner minerPrefab;
    public int oreCost;

    public override bool TaskAvailable()
    {
        return base.TaskAvailable() && ResourceManager.instance.oreCount >= oreCost;
    }

    protected override void StartTask()
    {
        base.StartTask();
        ResourceManager.instance.AddOre(-oreCost, this.transform.position);
    }

    public override void Complete()
    {
        GridUtilities gridUtil = new GridUtilities(TilemapManager.instance.wallTilemap);
        List<Vector3Int> openPositions = gridUtil.GetEmptyNeighbors(owner.gameObject);
        if (openPositions.Count > 0)
        {
            Miner newMiner = Instantiate(minerPrefab, openPositions[0] + new Vector3(0.5f, 0.5f, 0), Quaternion.Euler(Vector3.zero));
            MinerManager.instance.AddMiner(newMiner);
            base.Complete();
        }
    }
}
