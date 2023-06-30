using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

[RequireComponent(typeof(NetworkCharacterControllerCustom))]
[RequireComponent(typeof(NetworkMecanimAnimator))]
public class CharacterControllerHandler : NetworkBehaviour
{
    NetworkCharacterControllerCustom _characterController;
    NetworkMecanimAnimator _animator;
    bool canDoubleJump = false;
    [Header("View")]
    [SerializeField] float _view;
    

    private void Awake()
    {
        _characterController = GetComponent<NetworkCharacterControllerCustom>();
        _animator = GetComponent<NetworkMecanimAnimator>();
        Debug.Log($"my animator {_animator}");
        transform.forward = transform.right;
    }

    public override void FixedUpdateNetwork()
    {
        
        if (GetInput(out NetworkInputData input))
        {
            
            Vector3 moveDir = Vector3.right * input.movementInput;

            _characterController.Move(moveDir);
        }

        if (input.isJumpPressed)
        {
            if (_characterController.IsGrounded)
            {
                _characterController.Jump();
                canDoubleJump = true;
            }
            else if (canDoubleJump)
            {
                _characterController.DoubleJump();
                canDoubleJump = false;
            }

        }
        foreach (var tpItem in GameManager.Instance.GetAllHookeableList())
        {
            if (Vector3.Distance(transform.position, tpItem.transform.position) <= _view)
            {
                if (!_characterController.IsGrounded && input.isCanHook)
                {
                    var childTransf = tpItem.GetComponentInChildren<Transform>();
                    
                    _characterController.TP(tpItem.transform);
                    input.isCanHook = false;
                    
                }
            }

        }
        _animator.Animator.SetFloat("Horizontal", Mathf.Abs(_characterController.Velocity.x));
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        Gizmos.DrawWireSphere(transform.position, _view);
    }

}
