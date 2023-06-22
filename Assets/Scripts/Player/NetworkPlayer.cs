using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System;

[RequireComponent(typeof(CharacterInputsHandler))]
public class NetworkPlayer : NetworkBehaviour
{
    public static NetworkPlayer local { get; private set; }
    NicknameText _myNickName;
    [Networked(OnChanged = nameof(OnNicknameChanged))]
    NetworkString<_16> Nickname { get; set; }
    public event Action OnLeft = delegate { };
    public override void Spawned()
    {
        _myNickName = NicknamesHandler.Instance.AddNickname(this);

        Color skinColor = Color.white;
        if (Object.HasInputAuthority)
        {
            local = this;

            skinColor = Color.blue;

            RPC_SetNickname("Player 1");
        }
        else
        {
            skinColor = Color.red;
        }
        GetComponentInChildren<SpriteRenderer>().color = skinColor;
    }
    
    [Rpc(RpcSources.InputAuthority,RpcTargets.StateAuthority)]
    void RPC_SetNickname(string newNickName)
    {
        Nickname = newNickName;
    }
    public static void OnNicknameChanged(Changed<NetworkPlayer> changed)
    {
        changed.Behaviour.UpdateNickname(changed.Behaviour.Nickname.ToString());
    }
    void UpdateNickname(string newNick)
    {
        _myNickName.UpdateName(newNick);
    }
    public CharacterInputsHandler GetInputHandler()
    {
        return GetComponent<CharacterInputsHandler>();
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        OnLeft();
    }
}
