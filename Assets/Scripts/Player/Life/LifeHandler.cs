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
    [Header("Sounds")]
    [SerializeField] AudioClip _explosionClip;
    [SerializeField] AudioClip _damageClip;
    [SerializeField] AudioClip _reviveCLip;
    [Header("Explosion GameObjects")]
    [SerializeField] GameObject explotionGO_IZQ;
    [SerializeField] GameObject explotionGO_DER;
    [SerializeField] GameObject explotionGO_ARRIBA;
    [SerializeField] GameObject explotionGO_ABAJO;
    [Networked(OnChanged = nameof(OnExplosionChanged))]
    public bool isExplosion { get; set; }
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
            //Audio Damage
            _charControllerHandler.GetAudioSource().PlayOneShot(_damageClip);
            Debug.Log("Audio Damage");
            _characterController.KnockBack(currentPorcentDamaged, enemyTransform);
        }

    }

    static void OnExplosionChanged(Changed<LifeHandler> changed)
    {
        changed.Behaviour.OnExplosionChanged();
    }
    static void OnCurrentPorcentDamageChanged(Changed<LifeHandler> changed)
    {
        //Debug.Log($"{Time.time} OnPorcentDamage value {changed.Behaviour.currentPorcentDamaged}");

    }
    static void OnDeadStateChange(Changed<LifeHandler> changed)
    {
       // Debug.Log($"{Time.time} OnPorcentDamage value {changed.Behaviour.isDead}");
        bool isCurrDead = changed.Behaviour.isDead;
        changed.LoadOld();
        bool isDeadOld = changed.Behaviour.isDead;
        if (isCurrDead) changed.Behaviour.OnDeath();
        else if (!isCurrDead && isDeadOld) changed.Behaviour.OnRevive();

    }
    public void OnRespawned()
    {
        isExplosion = false;
        isDead = false;
        currentPorcentDamaged = startingPor;
    }
    public void OnDeath()
    {
        _playerObj.SetActive(false);
        //Debug.Log($"[{Time.time}] Ondeath");
        cantLifes--;
    }

    public void OnRevive()
    {
        _charControllerHandler.GetAudioSource().PlayOneShot(_reviveCLip);
        Debug.Log("Revive Sound");
        //isExplosion = false;
        _playerObj.SetActive(true);
        //Debug.Log($"[{Time.time}] OnRevive");
    }
    void OnExplosionChanged()
    {
        if (isExplosion)
        {
            //AUDIO DE EXPLOSION
            _charControllerHandler.GetAudioSource().PlayOneShot(_explosionClip);
            Debug.Log("Explosion Sound");
            if (_charControllerHandler.transform.position.x<=0&&_charControllerHandler.transform.position.y<=0)
            {
                //EXPLOSION DER y ARRIBA
                explotionGO_DER.SetActive(true);
                explotionGO_ARRIBA.SetActive(true);

            }
            else if (_charControllerHandler.transform.position.x<=0&&_charControllerHandler.transform.position.y>=0)
            {
                //EXPLOSION DER y ABAJO
                explotionGO_DER.SetActive(true);
                explotionGO_ABAJO.SetActive(true);
                
            }
            else if (_charControllerHandler.transform.position.x >= 0 && _charControllerHandler.transform.position.y >= 0)
            {
                //EXPLOSION IZQ y ABAJO
                explotionGO_IZQ.SetActive(true);
                explotionGO_ABAJO.SetActive(true);
                
            }
            else if (_charControllerHandler.transform.position.x >= 0 && _charControllerHandler.transform.position.y <= 0)
            {
                //EXPLOSION IZQ y ARRIBA
                explotionGO_IZQ.SetActive(true);
                explotionGO_ARRIBA.SetActive(true);
            }

            //explotionGO.SetActive(true);
        }
        else
        {
            explotionGO_ABAJO.SetActive(false);
            explotionGO_ARRIBA.SetActive(false);
            explotionGO_DER.SetActive(false);
            explotionGO_IZQ.SetActive(false);
           // explotionGO.SetActive(false);
        }

    }

 
    public IEnumerator SvReviveCo()
    {
        yield return new WaitForSeconds(0.5f);
        _charControllerHandler.ReqRespawn();
    }
}
