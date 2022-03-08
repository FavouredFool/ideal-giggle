using UnityEngine;

public class ColorCalculator : MonoBehaviour
{
    public MeshRenderer meshRenderer;

    public Material xMaterial;
    public Material nxMaterial;
    public Material zMaterial;
    public Material nzMaterial;
    public Material yMaterial;
    public Material nyMaterial;

    Material[] _materialArray;

    public void Awake()
    {

        // Korrekte Reihenfolge der Materialien auf den Faces des Cubes:
        // 0: nx, 1: ny, 2: nz, 3: z, 4: x, 5: y

        _materialArray = new Material[6];
        _materialArray[0] = nxMaterial;
        _materialArray[1] = nyMaterial;
        _materialArray[2] = nzMaterial;
        _materialArray[3] = zMaterial;
        _materialArray[4] = xMaterial;
        _materialArray[5] = yMaterial;
    }
    public void CalculateColor()
    {
        meshRenderer.materials = _materialArray;
    }
}
