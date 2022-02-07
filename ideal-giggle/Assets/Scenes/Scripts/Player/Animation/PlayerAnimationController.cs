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

    private bool _animationIsDone;

    public void Start()
    {
        _startLocalPosition = new Vector3(0, -0.25f, 0);
        _animator = GetComponent<Animator>();
    }

    public void Update()
    {
        bool isCorrectAnimation = _animator.GetCurrentAnimatorStateInfo(0).IsName("StraightStep");
        bool hasLooped = _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1;

        _animationIsDone = isCorrectAnimation && hasLooped && !_animationPlayed;

        if (_animationIsDone)
        {
            Debug.Log(_animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
            StartCoroutine(StopAnimation());
        }

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

    public IEnumerator StopAnimation()
    {
        _animationPlayed = true;
        _animator.SetTrigger("Still");
        
        // Warten bis Frame vorbei ist, damit Animation resetten kann und Würfel
        // somit nicht nach vorne bugged.
        yield return new WaitForEndOfFrame();
        PlacePlayer();
    }

    public void PlacePlayer()
    {
        _playerMovement.transform.position += _movement;
        transform.localPosition = _startLocalPosition;

        _playerMovement.SetIsMoving(false);
    }
}
