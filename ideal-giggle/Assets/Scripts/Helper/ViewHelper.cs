using UnityEngine;

public class ViewHelper : MonoBehaviour
{
    public enum Dimension { TWO_X, TWO_NX, TWO_Z, TWO_NZ, THREE }

    public enum VerticalDirection { UPPER, LOWER };

    public enum HorizontalDirection { RIGHT, LEFT };

    public enum ViewState { X, X_Z, Z, Z_NX, NX, NX_NZ, NZ, NZ_X}

    public static ViewState ActiveViewState;



    public static bool ActiveViewStateIsThreeD()
    {
        if ((int)ActiveViewState % 2 != 0)
        {
            return true;
        }
        return false;
    }

    public static Vector3 GetViewDirection()
    {
        Vector3 direction = Vector3.zero;
        switch (ActiveViewState)
        {
            // 2D
            case ViewState.X:
                direction = Vector3.left;
                break;
            case ViewState.NX:
                direction = Vector3.right;
                break;
            case ViewState.Z:
                direction = Vector3.back;
                break;
            case ViewState.NZ:
                direction = Vector3.forward;
                break;

            // 3D
            case ViewState.X_Z:
                direction = (Quaternion.Euler(0,45,0) * (Vector3.left + Vector3.up)).normalized;
                break;
            case ViewState.Z_NX:
                direction = (Quaternion.Euler(0, 45, 0) * (Vector3.back + Vector3.up)).normalized;
                break;
            case ViewState.NZ_X:
                direction = (Quaternion.Euler(0, 45, 0) * (Vector3.forward + Vector3.up)).normalized;
                break;
            case ViewState.NX_NZ:
                direction = (Quaternion.Euler(0, 45, 0) * (Vector3.right + Vector3.up)).normalized;
                break;
            default:
                Debug.Log("FEHLER");
                break;
        }

        return direction;
    }

    public static Vector3 RotateDirection(Vector3 point, Vector3 pivot, Vector3 angle)
    {
        Vector3 dir = point - pivot;
        dir = Quaternion.Euler(angle) * dir;
        point = dir + pivot;
        return point;
    }




}
