using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

public class FloorTile : Tile
{

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
