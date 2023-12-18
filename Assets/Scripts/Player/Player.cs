using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.UI;

[RequireComponent(typeof(NetworkMecanimAnimator))]
public class Player : NetworkBehaviour
{
    NetworkMecanimAnimator _netAnimator;
    CharacterControllerHandler _charControllerHandler;
    [Header("Clip")]
    [SerializeField] AudioClip _shieldClip;
    [Header("Controller")]
    [SerializeField] GameObject _shield;
    [SerializeField] Transform _attackTransform;
    [SerializeField] Transform _headTransform;
    [SerializeField] float _timeShield;
    [Networked(OnChanged = nameof(OnIsShieldChanged))]
    public bool isShield { get; set; }

    [Header("HitBoxes")]
    [SerializeField] Hitbox _headHitBox;
    [SerializeField] Hitbox _spineHitBox;

    private void Awake()
    {
        _attackTransform.forward = _attackTransform.right;
        _netAnimator = GetComponent<NetworkMecanimAnimator>();
        _charControllerHandler = GetComponent<CharacterControllerHandler>();
    }
    private void Start()
    {
        isShield = false;
    }
    public override void FixedUpdateNetwork()
    {
        /* Runner.LagCompensation.Raycast(origin: transform.position, (_headTransform.up * -1), 1, player: Object.InputAuthority, hit: out var hit);
         if (hit.Hitbox !=null)
         {
             if (Object.HasStateAuthority)
             {

             }
         }*/

        if (GetInput(out NetworkInputData input))
        {
            if (input.isShield)
            {
                if (Object.HasStateAuthority)
                {
                    isShield = true;
                    StartCoroutine(ShieldActivatedTime());
                }

            }

            if (input.isAttack)
            {
                _netAnimator.Animator.SetTrigger("isAttack");                
                Runner.LagCompensation.Raycast(origin: transform.position, _attackTransform.forward, 1, player: Object.InputAuthority, hit: out var hitinfo);

                if (hitinfo.Hitbox != null)
                {
                    if (Object.HasStateAuthority)
                    {
                        hitinfo.Hitbox.transform.root.GetComponent<LifeHandler>().PorDamageAttacking(1, transform.position);
                        /* Debug.Log($"{hitinfo}  Se colisiono con  {hitinfo.GameObject.name}");
                         var player = hitinfo.GameObject.GetComponentInParent<Player>();
                         Debug.Log($"Mi posicion: {transform.position}, Posicion al que le pego: {player.transform.position}");
                         var handlerHit = player.GetComponent<LifeHandler>();
                         handlerHit.PorDamageAttacking(1, transform.position);
                         Debug.Log($"{hitinfo} Se colisiono con el player  {player}");*/

                    }
                }
            }
        }
        IEnumerator ShieldActivatedTime()
        {
            yield return new WaitForSeconds(_timeShield);
            isShield = false;
        }
    }

    static void OnIsShieldChanged(Changed<Player> changed)
    {
        //Debug.Log($"{Time.time} OnPorcentDamage value {changed.Behaviour.isShield}");
        changed.Behaviour.ShieldImage();
    }

    public void ShieldImage()
    {
        if (isShield) {
            //sonido Shield
            _charControllerHandler.GetAudioSource().PlayOneShot(_shieldClip);
            Debug.Log("Soinido shield");
            _shield.SetActive(true);
        }
        else _shield.SetActive(false);
    }
    
}
