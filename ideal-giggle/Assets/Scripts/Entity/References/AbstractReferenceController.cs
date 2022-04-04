using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ReferenceHelper;

public abstract class AbstractReferenceController : MonoBehaviour
{
    protected List<EntityReference> _entityReferences;
    protected List<AbstractEntityController> _entityCache;
    protected Vector3 _position;
    protected bool _transitionIsSet = false;
    protected Vector3 _referenceDirection;


    protected void SetReference(int index, AbstractEntityController referencedEntity, ReferenceBehaviourType referenceBehaviour)
    {
        _entityReferences[index] = new EntityReference(referencedEntity, referenceBehaviour);
        _transitionIsSet = true;
    }

}
