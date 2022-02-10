using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPathCalculator : MonoBehaviour
{

    public int radius = 5;

    private List<AbstractEntityController> _newList;

    public void Start()
    {
        _newList = new List<AbstractEntityController>();
    }

    public List<AbstractEntityController> CalculatePath(AbstractEntityController startEntity, AbstractEntityController endEntity)
    {
        _newList.Clear();

        // Try to get from StartEntity to EndEntity through references
        _newList.Add(startEntity);
        _newList.Add(endEntity);




        return _newList;
    }
}
