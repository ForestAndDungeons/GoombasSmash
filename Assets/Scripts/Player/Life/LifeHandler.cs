using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System;
[RequireComponent(typeof(NetworkCharacterControllerCustom))]
[RequireComponent(typeof(Player))]
[RequireComponent(typeof(CharacterControllerHandler))]
public class LifeHandler : NetworkBehaviour
{
    
    NetworkCharacterControllerCustom _characterController;
    Player _player;
    CharacterControllerHandler _charControllerHandler;
    GameObject _playerObj;
    [Networked(OnChanged = nameof(OnCurrentPorcentDamageChanged))]
    byte currentPorcentDamaged { get; set; }

    const byte startingPor = 0;

    [Networked(OnChanged =nameof(OnDeadStateChange))]
    public bool isDead { get; set; }

    public byte cantLifes = 3;
    private void Awake()
    {
        _characterController = GetComponent<NetworkCharacterControllerCustom>();
        _player = GetComponent<Player>();
        _charControllerHandler = GetComponent<CharacterControllerHandler>();
        _playerObj = GetComponentInChildren<Animator>().gameObject;
        Debug.Log($"_playerObj {_playerObj}");
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
    static void OnDeadStateChange(Changed<LifeHandler> changed)
    {
        Debug.Log($"{Time.time} OnPorcentDamage value {changed.Behaviour.isDead}");
        bool isCurrDead = changed.Behaviour.isDead;
        changed.LoadOld();
        bool isDeadOld = changed.Behaviour.isDead;
        if (isCurrDead) changed.Behaviour.OnDeath();
        else if (!isCurrDead && isDeadOld) changed.Behaviour.OnRevive();

    }
    public void OnRespawned()
    {
        isDead = false;
        currentPorcentDamaged = startingPor;
    }
    public void OnDeath()
    {
        _playerObj.SetActive(false);
        Debug.Log($"[{Time.time}] Ondeath");
        cantLifes--;
    }

    public void OnRevive()
    {
        _playerObj.SetActive(true);
        Debug.Log($"[{Time.time}] OnRevive");
    }
    public IEnumerator SvReviveCo()
    {
        yield return new WaitForSeconds(0.5f);
        _charControllerHandler.ReqRespawn();
    }
}
