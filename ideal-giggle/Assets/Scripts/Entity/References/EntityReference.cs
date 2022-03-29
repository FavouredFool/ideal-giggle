using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ReferenceHelper;

public class EntityReference
{

    AbstractEntityController _referenceEntity;
    ReferenceBehaviourType _referenceBehaviour;

    public EntityReference(AbstractEntityController referenceEntity, ReferenceBehaviourType referenceBehaviour)
    {
        _referenceEntity = referenceEntity;
        _referenceBehaviour = referenceBehaviour;
    }

    public AbstractEntityController GetReferenceEntity()
    {
        return _referenceEntity;
    }

    public ReferenceBehaviourType GetReferenceBehaviorType()
    {
        return _referenceBehaviour;
    }
}
