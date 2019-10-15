using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Builds dirt tiles on top of carpet.
/// </summary>
public class TileBuilder : MonoBehaviour
{

    public Vector2 center;
    public float rowNum;
    public float colNum;
    public float scale = 0.02f;
    public GameObject tilePrefab;

    // Use this for initialization
    private void Start()
    {
        CreateDirtTiles();

        GameManager.instance.SetTileCount((int)(rowNum * colNum));
        GameManager.instance.UpdateScore();
    }

    private void CreateDirtTiles()
    {
        // Adjust the number of rows/columns based on the scale of the dirt tile.
        rowNum = rowNum / scale;
        colNum = colNum / scale;

        // Scale down the prefab
        tilePrefab.transform.localScale = new Vector3(scale, scale, 1);

        Vector2 size = tilePrefab.GetComponent<SpriteRenderer>().bounds.size;

        float startX = (size.x * colNum * -.5f) + size.x / 2;
        float startY = (size.y * rowNum * .5f) - size.y / 2;
        startX += center.x;
        startY += center.y;
        float currX = startX;
        float currY = startY;

        for (int i = 0; i < rowNum; i++)
        {
            for (int j = 0; j < colNum; j++)
            {
                GameObject bg = ((GameObject)Instantiate(tilePrefab, Vector2.zero, Quaternion.identity));
                bg.transform.position = new Vector2(currX, currY);
                bg.transform.SetParent(this.transform);
                bg.transform.localScale = new Vector3(scale, scale, 1);

                currX += size.x;
            }
            currY -= size.y;
            currX = startX;
        }
    }
}
