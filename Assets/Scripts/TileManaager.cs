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
            if (i == 0) SpawnTile(true);
            else SpawnTile(false);
        }
    }
    void Update()
    {
        if (playerTransform.position.z - 35 > zSpawn - (numberOfTiles * tileLenght))
        {
            RecycleTile();
        }

        if (playerTransform.position.z > 2000f)
        {
            ShiftWorld();
        }
    }
    void SpawnTile(bool isFirstTile = false)
    {
        GameObject go = Instantiate(tilePrefab, transform.forward * zSpawn, transform.rotation);
        activeTiles.Add(go);
        zSpawn += tileLenght;
        if (isFirstTile)
        {
            go.GetComponent<Tile>().SetEmptyLayout();
        }
        else
        {
            go.GetComponent <Tile>().RandomizeObstacles();
        }
    }
    void RecycleTile()
    {
        GameObject oldestTile = activeTiles[0];
        oldestTile.transform.position = Vector3.forward * zSpawn;
        zSpawn += tileLenght;
        activeTiles.RemoveAt(0);
        activeTiles.Add(oldestTile);
        oldestTile.GetComponent<Tile>().RandomizeObstacles();
    }

    void ShiftWorld()
    {
        float shiftAmount = 2000f;
        Vector3 shiftVector = new Vector3(0, 0, -shiftAmount);

        Rigidbody playerRb = playerTransform.GetComponent<Rigidbody>();
        if (playerRb != null)
        {
            playerRb.position += shiftVector;
        }

        Camera.main.transform.position += shiftVector;
        foreach (GameObject tile in activeTiles)
        {
            tile.transform.position += shiftVector;
        }

        zSpawn -= shiftAmount;
        Debug.Log("Dünya sýfýrlandý.");
    }
}
