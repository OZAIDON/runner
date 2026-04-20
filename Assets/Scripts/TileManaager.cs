using UnityEngine;
using System.Collections.Generic;

public class TileManaager : MonoBehaviour
{
    [Header("Ground Settings")]
    public GameObject tilePrefab;
    public float tileLenght = 30f;
    public int numberOfTiles = 5;

    [Header("Referances")]
    public Transform playerTransform;

    private List<GameObject> activeTiles = new List<GameObject>();
    private float zSpawn = 0f;
    void Start()
    {
        for (int i = 0; i < numberOfTiles; i++)
        {
            SpawnTile();
        }
    }
    void Update()
    {
        if (playerTransform.position.z - 35 > zSpawn - (numberOfTiles * tileLenght))
        {
            RecycleTile();
        }
    }
    void SpawnTile()
    {
        GameObject go = Instantiate(tilePrefab, transform.forward * zSpawn, transform.rotation);
        activeTiles.Add(go);
        zSpawn += tileLenght;
    }
    void RecycleTile()
    {
        GameObject oldestTile = activeTiles[0];
        oldestTile.transform.position = Vector3.forward * zSpawn;
        zSpawn += tileLenght;
        activeTiles.RemoveAt(0);
        activeTiles.Add(oldestTile);
    }
}
