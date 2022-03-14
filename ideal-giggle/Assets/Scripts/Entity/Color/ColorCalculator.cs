using UnityEngine;

public class ColorCalculator : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField]
    private MeshRenderer _meshRenderer;

    public Material[] xMaterials = new Material[2];
    public Material[] nxMaterials = new Material[2];
    public Material[] zMaterials = new Material[2];
    public Material[] nzMaterials = new Material[2];
    public Material[] yMaterials = new Material[2];
    public Material[] nyMaterials = new Material[2];

    Material[][] _directionMaterialsArray;
    Material[][] _faceMaterialsArray;
    Material[] _groundSensitiveArray;

    public void Awake()
    {

        // Korrekte Reihenfolge der Materialien auf den Faces des Cubes ohne Rotation:
        // 0: nx, 1: ny, 2: nz, 3: z, 4: x, 5: y
        _directionMaterialsArray = new Material[][] { nxMaterials, nyMaterials, nzMaterials, zMaterials, xMaterials, yMaterials };

        // Änderungen über Rotation
         
        _faceMaterialsArray = new Material[6][];
        _faceMaterialsArray[0] = GetMaterialsFromDirection(transform.rotation * Vector3.left);
        _faceMaterialsArray[1] = GetMaterialsFromDirection(transform.rotation * Vector3.down);
        _faceMaterialsArray[2] = GetMaterialsFromDirection(transform.rotation * Vector3.back);
        _faceMaterialsArray[3] = GetMaterialsFromDirection(transform.rotation * Vector3.forward);
        _faceMaterialsArray[4] = GetMaterialsFromDirection(transform.rotation * Vector3.right);
        _faceMaterialsArray[5] = GetMaterialsFromDirection(transform.rotation * Vector3.up);
    }


    public void CalculateColor(float xPlanePos, float zPlanePos)
    {
        _groundSensitiveArray = new Material[6];

        Quaternion rotation = Quaternion.Inverse(transform.rotation);

        if (transform.position.x < xPlanePos)
        {
            _groundSensitiveArray[GetIndexFromDirection(rotation * Vector3.left)] = _faceMaterialsArray[GetIndexFromDirection(rotation * Vector3.left)][0];
            _groundSensitiveArray[GetIndexFromDirection(rotation * Vector3.right)] = _faceMaterialsArray[GetIndexFromDirection(rotation * Vector3.right)][1];
        }
        else
        {
            _groundSensitiveArray[GetIndexFromDirection(rotation * Vector3.left)] = _faceMaterialsArray[GetIndexFromDirection(rotation * Vector3.left)][1];
            _groundSensitiveArray[GetIndexFromDirection(rotation * Vector3.right)] = _faceMaterialsArray[GetIndexFromDirection(rotation * Vector3.right)][0];
        }

        if (transform.position.z < zPlanePos)
        {
            _groundSensitiveArray[GetIndexFromDirection(rotation * Vector3.back)] = _faceMaterialsArray[GetIndexFromDirection(rotation * Vector3.back)][0];
            _groundSensitiveArray[GetIndexFromDirection(rotation * Vector3.forward)] = _faceMaterialsArray[GetIndexFromDirection(rotation * Vector3.forward)][1];
        }
        else
        {
            _groundSensitiveArray[GetIndexFromDirection(rotation * Vector3.back)] = _faceMaterialsArray[GetIndexFromDirection(rotation * Vector3.back)][1];
            _groundSensitiveArray[GetIndexFromDirection(rotation * Vector3.forward)] = _faceMaterialsArray[GetIndexFromDirection(rotation * Vector3.forward)][0];
        }

        _groundSensitiveArray[1] = _faceMaterialsArray[1][0];        
        _groundSensitiveArray[5] = _faceMaterialsArray[5][0];

        _meshRenderer.materials = _groundSensitiveArray;
    }

    private int GetIndexFromDirection(Vector3 direction)
    {
        if (V3Equal(direction, Vector3.forward))
        {
            return 3;
        }
        else if (V3Equal(direction, Vector3.back))
        {
            return 2;
        }
        else if (V3Equal(direction, Vector3.right))
        {
            return 4;
        }
        else if (V3Equal(direction, Vector3.left))
        {
            return 0;
        }
        else if (V3Equal(direction, Vector3.up))
        {
            return 5;
        }
        else if (V3Equal(direction, Vector3.down))
        {
            return 1;
        }
        else
        {
            Debug.Log($"FEHLER bei {direction}");
            return -1;
        }
    }


    private Material[] GetMaterialsFromDirection(Vector3 direction)
    {
        if(V3Equal(direction, Vector3.forward))
        {
            return _directionMaterialsArray[3];
        } else if (V3Equal(direction, Vector3.back))
        {
            return _directionMaterialsArray[2];
        } else if (V3Equal(direction, Vector3.right))
        {
            return _directionMaterialsArray[4];
        } else if (V3Equal(direction, Vector3.left))
        {
            return _directionMaterialsArray[0];
        } else if (V3Equal(direction, Vector3.up))
        {
            return _directionMaterialsArray[5];
        } else if (V3Equal(direction, Vector3.down))
        {
            return _directionMaterialsArray[1];
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
