using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System;
[RequireComponent(typeof(NetworkCharacterControllerCustom))]
public class LifeHandler : NetworkBehaviour
{
    [SerializeField] float _maxLife;
    NetworkCharacterControllerCustom _characterController;
    float _currentPorcentDamaged { get; set; }
    //[Networked(OnChanged =nameof(OnDeadStateChange))]
    bool _isDead { get; set; }

    public event Action<bool> OnDeadStateChange = delegate { };
    public event Action onRespawn = delegate { };
    private void Awake()
    {
        _characterController = GetComponent<NetworkCharacterControllerCustom>();
    }
    void Start()
    {
        _isDead = false;
    }

    public void PorDamageAttacking(float dmg,Vector3 enemyTransform)
    {
        if (_isDead) return;
        _currentPorcentDamaged += dmg;
        _characterController.KnockBack(_currentPorcentDamaged, enemyTransform);

    }
    public bool GetIsDead() { return _isDead; }



}
