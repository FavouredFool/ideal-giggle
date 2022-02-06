using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField]
    private PlayerMovementController _playerMovement;

    private Animator _animator;

    private Vector3 _startLocalPosition;

    private Vector3 _movement;

    public void Start()
    {
        _startLocalPosition = transform.localPosition;
        _animator = GetComponent<Animator>();
    }

    public void MoveStep(Vector3 movement)
    {
        _movement = movement;

        // Muss sensitiv gegenüber den jeweiligen Bodenflächen sein.

        RotatePlayerTarget();
        PlayAnimation();
    }

    public void RotatePlayerTarget()
    {
        float angle = Vector3.SignedAngle(Vector3.right, _movement, Vector3.up);

        var rotation = new Vector3(0, angle, 0);
        _playerMovement.transform.eulerAngles = rotation;
    }

    public void PlayAnimation()
    {
        _animator.SetTrigger("StraightStep");

    }

    public void PlacePlayer()
    {
        transform.localPosition = _startLocalPosition;
        _playerMovement.transform.position += _movement;
        
        _playerMovement.SetIsMoving(false);
        
    }
}
