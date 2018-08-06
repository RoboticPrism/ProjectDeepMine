using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;


// A wall in a room, lights itself accordingly
public class MineableWall : WallBase {

    public float destroyTime = 100f;
    public MineTask task;

    public enum wallType { dirt, stone };
    public wallType type;
    
	// Use this for initialization
	protected override void Start () {
        base.Start();
    }
	
	// Update is called once per frame
	protected override void Update () {
        base.Update();
	}

    // FixedUpdate is called once per tick
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public void MineWall(float mineSpeed)
    {
        this.destroyTime -= mineSpeed;
    }
}
