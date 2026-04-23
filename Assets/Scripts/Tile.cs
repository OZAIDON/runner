using UnityEngine;

public class Tile : MonoBehaviour
{
    [Header("Obstacle Layouts")]
    public GameObject[] layouts;

    public void RandomizeObstacles()
    {
        foreach (GameObject layout in layouts)
        {
            layout.SetActive(false);
        }
        int randomIndex = Random.Range(0, layouts.Length);
        layouts[randomIndex].SetActive(true);
    }
    public void SetEmptyLayout()
    {
        foreach (GameObject layout in layouts) layout.SetActive(false);
        layouts[0].SetActive(true);
    }
}
