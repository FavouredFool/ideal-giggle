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

    private bool _animationPlayed;

    public void Start()
    {
        _startLocalPosition = new Vector3(0, -0.25f, 0);
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
        _animationPlayed = false;
        _animator.SetTrigger("StraightStep");
    }

    public void AnimationEnd_PlacePlayer()
    {
        // Animations in an Animator will be played twice while transitioning.
        // Therefore this guard is necessary.
        if (_animationPlayed)
        {
            return;
        }

        _animator.SetTrigger("Still");

        _animationPlayed = true;

        _playerMovement.transform.position += _movement;

        transform.localPosition = _startLocalPosition;
        Debug.Log($"VisualPosition: {transform.position}");
        Debug.Log($"VisualLocalPosition: {transform.localPosition}");

        Debug.Log($"Position: {_playerMovement.transform.position}");

        // Bug happens before setisMoving is turned back to false;
        // -> No Doubling

        _playerMovement.SetIsMoving(false);

    }
}
