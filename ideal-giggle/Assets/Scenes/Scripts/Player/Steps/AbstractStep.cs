using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractStep : MonoBehaviour
{
    public Vector3 Movement { get; set; }

   

    abstract public void MoveStep();
}
