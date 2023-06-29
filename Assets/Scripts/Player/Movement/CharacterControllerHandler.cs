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
    /*[Header("Controller")]
    [SerializeField] GameObject _shield;*/
    [Header("View")]
    [SerializeField] float _view;

    /*float _holdTimer = 0f;
    private float _holdTimeThreshold = 0.5f;*/
    private void Awake()
    {
        _characterController = GetComponent<NetworkCharacterControllerCustom>();
        _animator = GetComponent<NetworkMecanimAnimator>();
        
    }

    public override void FixedUpdateNetwork()
    {
        
        if (GetInput(out NetworkInputData input))
        {
            
            Vector3 moveDir = Vector3.right * input.movementInput;
            _characterController.Move(moveDir);
        }

       /* if (input.isShield)
        {
            _holdTimer += Runner.DeltaTime;
            Debug.Log($"HoldTimer {_holdTimer}");
            if (!input.isShield && _holdTimer >= _holdTimeThreshold)
            {
                //_isShield = true;
                _shield.gameObject.SetActive(true);
                Debug.Log($"Activaste Escudo  {_holdTimer}");
            }
            else
            {
                _shield.gameObject.SetActive(false);
                _holdTimer = 0f;
            }
        }*/
       /*if (input.isShield)
        {
            _shield.gameObject.SetActive(true);
        }
        else { _shield.gameObject.SetActive(false); }*/

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
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        Gizmos.DrawWireSphere(transform.position, _view);
    }

}
