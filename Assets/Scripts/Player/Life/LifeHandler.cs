using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System;
[RequireComponent(typeof(NetworkCharacterControllerCustom))]
[RequireComponent(typeof(Player))]
public class LifeHandler : NetworkBehaviour
{
    
    NetworkCharacterControllerCustom _characterController;
    Player _player;
    [Networked(OnChanged = nameof(OnCurrentPorcentDamageChanged))]
    byte currentPorcentDamaged { get; set; }

    const byte startingPor = 0;

    //[Networked(OnChanged =nameof(OnDeadStateChange))]
    public bool isDead { get; set; }

    
    private void Awake()
    {
        _characterController = GetComponent<NetworkCharacterControllerCustom>();
        _player = GetComponent<Player>();
    }
    void Start()
    {
        currentPorcentDamaged = startingPor;
        isDead = false;
    }

    public void PorDamageAttacking(byte dmg,Vector3 enemyTransform)
    {
        if (isDead) return;
        if (!_player.isShield)
        {
            currentPorcentDamaged += dmg;
            _characterController.KnockBack(currentPorcentDamaged, enemyTransform);
        }

    }
    

    static void OnCurrentPorcentDamageChanged(Changed<LifeHandler> changed)
    {
        Debug.Log($"{Time.time} OnPorcentDamage value {changed.Behaviour.currentPorcentDamaged}");

    }
   /* public void OnDeadStateChange(Changed<LifeHandler> changed)
    {
        Debug.Log($"{Time.time} OnPorcentDamage value {changed.Behaviour._isDead}");
    }*/
    /*private void OnPorDmgUp()
    {

    }*/

}
