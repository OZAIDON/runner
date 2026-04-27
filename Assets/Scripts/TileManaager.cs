using UnityEngine;
using System.Collections.Generic;

public class TileManaager : MonoBehaviour
{
    [Header("Ground Settings")]
    public GameObject[] tilePrefab;
    public float tileLenght = 30f;
    public int numberOfTiles = 5;
    private float zSpawn = 0f;

    [Header("Referances")]
    public Transform playerTransform;
    private List<GameObject> activeTiles = new List<GameObject>();

    private int layoutCounter = 0;
    void Start()
    {
        for (int i = 0; i < numberOfTiles; i++)
        {
            SpawnTile(0);
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
    public void SpawnTile(int prefabIndex)
    {
        GameObject go = Instantiate(tilePrefab[prefabIndex], transform.forward * zSpawn, transform.rotation);
        activeTiles.Add(go);
        Tile tileScript = go.GetComponent<Tile>();

        if (tileScript != null)
        {
            tileScript.SetLayouts(SelectTileDifficulty());
        }
        zSpawn += tileLenght;
    }
    private void RecycleTile()
    {
        GameObject oldestTile = activeTiles[0];
        oldestTile.transform.position = Vector3.forward * zSpawn;
        zSpawn += tileLenght;
        activeTiles.RemoveAt(0);
        activeTiles.Add(oldestTile);
        Tile tileScript = oldestTile.GetComponent<Tile>();

        if (tileScript != null)
        {
            tileScript.SetLayouts(SelectTileDifficulty());
        }
    }

    private Difficulty SelectTileDifficulty()
    {
        Difficulty nextDif;

        int loopStep = layoutCounter % 5;

        if (loopStep == 0)
        {
            nextDif = Difficulty.Easy;
        }
        else if (loopStep == 4)
        {
            nextDif = Difficulty.Hard;
        }
        else
        {
            nextDif = Difficulty.Medium; 
        }

        layoutCounter++;
        return nextDif;
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
