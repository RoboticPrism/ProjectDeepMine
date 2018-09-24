using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

public class FloorTile : Tile
{

    public float PrefabLocalOffset = 0.5f;
    public float prefabZOffset = -1f;

    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {

        //This prevents rogue prefab objects from appearing when the Tile palette is present
#if UNITY_EDITOR
        if (go != null)
        {
            if (go.scene.name == "Preview Scene")
            {
                DestroyImmediate(go);
            }
        }
#endif

        if (go != null)
        {
            //Modify position of GO to match middle of Tile sprite
            go.transform.position = new Vector3(position.x + PrefabLocalOffset
                , position.y + PrefabLocalOffset
                , prefabZOffset);

        }

        return true;
    }

#if UNITY_EDITOR
    // The following is a helper that adds a menu item to create a FloorTile Asset
    [MenuItem("Assets/Create/FloorTile")]
    public static void CreateFloorTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Floor Tile", "New Floor Tile", "Asset", "Save Floor Tile", "Assets");
        if (path == "")
            return;
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<FloorTile>(), path);
    }
#endif
}
