using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

[RequireComponent(typeof(NetworkCharacterControllerCustom))]
public class CharacterMovementHandler : NetworkBehaviour
{
    NetworkCharacterControllerCustom _movementController;

    private void Awake()
    {
        _movementController = GetComponent<NetworkCharacterControllerCustom>();
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData input))
        {
            Vector3 moveDir = Vector3.forward * input.movementInput;
            _movementController.Move(moveDir);
        }

        if (input.isJumpPressed)
        {
            _movementController.Jump();
        }
    }

}
