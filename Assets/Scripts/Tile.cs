using UnityEngine;

public enum Difficulty
{
    Easy,
    Medium,
    Hard
}
public class Tile : MonoBehaviour
{
    [Header("Layout Lists")]
    public GameObject[] easyLayouts;
    public GameObject[] mediumLayouts;
    public GameObject[] hardLayouts;

    public void SetLayouts(Difficulty dif)
    {
        CloseAllLayouts();
        GameObject selectedLayout = null;

        if (dif == Difficulty.Easy && easyLayouts.Length > 0)
        {
            selectedLayout = easyLayouts[Random.Range(0, easyLayouts.Length)];
        }
        else if (dif == Difficulty.Medium && mediumLayouts.Length > 0)
        {
            selectedLayout = mediumLayouts[Random.Range(0, mediumLayouts.Length)];
        }
        else if (dif == Difficulty.Hard && hardLayouts.Length > 0)
        {
            selectedLayout = hardLayouts[Random.Range(0, hardLayouts.Length)];
        }
        if (selectedLayout != null)
        {
            selectedLayout.SetActive(true);
        }
    }
    void CloseAllLayouts()
    {
        foreach (var layout in easyLayouts) layout.SetActive(false);
        foreach (var layout in  mediumLayouts) layout.SetActive(false);
        foreach (var layout in  hardLayouts) layout.SetActive(false);
    }
}