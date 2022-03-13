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

    Material[] _directionMaterialArray;
    Material[] _faceMaterialArray;

    public void Awake()
    {

        // Korrekte Reihenfolge der Materialien auf den Faces des Cubes ohne Rotation:
        // 0: nx, 1: ny, 2: nz, 3: z, 4: x, 5: y
        _directionMaterialArray = new Material[] { nxMaterial, nyMaterial, nzMaterial, zMaterial, xMaterial, yMaterial };

        // Änderungen über Rotation
         
        _faceMaterialArray = new Material[6];
        _faceMaterialArray[0] = GetMaterialFromDirection(transform.rotation * Vector3.left);
        _faceMaterialArray[1] = GetMaterialFromDirection(transform.rotation * Vector3.down);
        _faceMaterialArray[2] = GetMaterialFromDirection(transform.rotation * Vector3.back);
        _faceMaterialArray[3] = GetMaterialFromDirection(transform.rotation * Vector3.forward);
        _faceMaterialArray[4] = GetMaterialFromDirection(transform.rotation * Vector3.right);
        _faceMaterialArray[5] = GetMaterialFromDirection(transform.rotation * Vector3.up);


    }
    public void CalculateColor()
    {
        meshRenderer.materials = _faceMaterialArray;
    }


    private Material GetMaterialFromDirection(Vector3 direction)
    {
        if(V3Equal(direction, Vector3.forward))
        {
            return _directionMaterialArray[3];
        } else if (V3Equal(direction, Vector3.back))
        {
            return _directionMaterialArray[2];
        } else if (V3Equal(direction, Vector3.right))
        {
            return _directionMaterialArray[4];
        } else if (V3Equal(direction, Vector3.left))
        {
            return _directionMaterialArray[0];
        } else if (V3Equal(direction, Vector3.up))
        {
            return _directionMaterialArray[5];
        } else if (V3Equal(direction, Vector3.down))
        {
            return _directionMaterialArray[1];
        } else
        {
            Debug.Log($"FEHLER bei {direction}");
            return null;
        }
        
    }

    public bool V3Equal(Vector3 a, Vector3 b)
    {
        return Vector3.SqrMagnitude(a - b) < 0.0001;
    }
}
