using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public struct NetworkInputData : INetworkInput
{
    public float movementInput;
    public NetworkBool isJumpPressed;
    public NetworkBool isCanHook;
    public NetworkBool isShield;
    public NetworkBool isAttack;
}
