using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceLoader : Singleton<ResourceLoader>
{
    [HideInInspector] public GameObject colorTile;

    // Use this for initialization
    protected override void Awake()
    {
        base.Awake();

        LoadResources();
    }

    private void LoadResources()
    {
        colorTile = Resources.Load<GameObject>("Prefabs/ColorTile");
    }
}
