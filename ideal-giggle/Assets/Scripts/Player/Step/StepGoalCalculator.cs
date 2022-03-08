using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EntityHelper;

public class StepGoalCalculator : MonoBehaviour
{
    public AbstractEntityController CalculateStepGoalEntity(AbstractEntityController activeEntity, List<AbstractEntityController> playerMovementPath)
    {
        AbstractEntityController endEntity = null;

        for (int i = 0; i < playerMovementPath.Count; i++)
        {
            if (!playerMovementPath[i].Equals(activeEntity))
            {
                continue;
            }

            if (playerMovementPath[i+1].Equals(null)) {
                Debug.LogWarning($"FEHLER: Das gesuchte Path-Item ist null");
                break;
            }

            endEntity = playerMovementPath[i + 1];
        }


        return endEntity;
    }
}
