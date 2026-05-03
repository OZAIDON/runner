using UnityEngine;

[ExecuteInEditMode]
public class CurvedWorldController : MonoBehaviour
{
    [Header("Curved World Settings")]
    [Range(-0.01f, 0.01f)]
    public float curveStrength = 0.002f;
    public float curveDistance = 10f;

    void Update()
    {
        Shader.SetGlobalFloat("_CurveStrength", curveStrength);
        Shader.SetGlobalFloat("_CurveDistance", curveDistance);

    }
}
