using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

public class WallTile : Tile {

    public float PrefabLocalOffset = 0.5f;
    public float prefabZOffset = -1f;

    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {

        

        if (go != null)
        {
            //Modify position of GO to match middle of Tile sprite
            go.transform.position = new Vector3(position.x + PrefabLocalOffset
                , position.y + PrefabLocalOffset
                , prefabZOffset);

        }

        //This prevents rogue prefab objects from appearing when the Tile palette is present
#if UNITY_EDITOR
        if (go != null)
        {
            if (go.scene.name == null)
            {
                DestroyImmediate(go);
            }
        }
#endif

        return true;
    }

#if UNITY_EDITOR
    // The following is a helper that adds a menu item to create a WallTile Asset
    [MenuItem("Assets/Create/WallTile")]
    public static void CreateWallTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Wall Tile", "New Wall Tile", "Asset", "Save Wall Tile", "Assets");
        if (path == "")
            return;
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<WallTile>(), path);
    }
#endif
}
