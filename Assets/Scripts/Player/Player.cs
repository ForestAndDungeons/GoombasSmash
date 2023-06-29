using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.UI;

public class Player : NetworkBehaviour
{
    [Header("Controller")]
    [SerializeField] GameObject _shield;
    [SerializeField] Transform _attackTransform;
    float _holdTimer = 0f;
    private float _holdTimeThreshold = 0.5f;

    private void Awake()
    {
        _attackTransform.forward = _attackTransform.right;
    }
    public override void FixedUpdateNetwork()
    {
        Runner.LagCompensation.Raycast(origin: transform.position, transform.up * -1, 1, player: Object.InputAuthority, hit: out var hit);
        if (hit.Hitbox !=null)
        {
            if (Object.HasStateAuthority)
            {
                Debug.Log($"{hit}  Se colisiono con  {hit.GameObject.name}");
            }
        }

        if (GetInput(out NetworkInputData input))
        {
            if (input.isShield)
            {
                _holdTimer += Time.time;
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
            }
            
            if (input.isAttack)
            {
                Runner.LagCompensation.Raycast(origin: transform.position, _attackTransform.forward, 100, player: Object.InputAuthority, hit: out var hitinfo);

                if (hitinfo.Hitbox !=null)
                {
                    if (Object.HasStateAuthority)
                    {
                        //var handlerHit = hitinfo.GameObject.GetComponent<LifeHandler>();
                        //handlerHit.PorDamageAttacking(1);
                        //Debug.Log($"Handlerhit:  {handlerHit},  handlerHit Gameobject name:  {handlerHit.gameObject.name}");
                        var player = hitinfo.GameObject.GetComponentInParent<Player>();
                        Debug.Log($"Mi posicion: {transform.position}, Posicion al que le pego: {player.transform.position}");
                        var handlerHit = player.GetComponent<LifeHandler>();
                        handlerHit.PorDamageAttacking(1,transform.position);
                        Debug.Log($"{hitinfo} Se colisiono con  {player}");
                    }
                }
            }
        }
    }

   
}
