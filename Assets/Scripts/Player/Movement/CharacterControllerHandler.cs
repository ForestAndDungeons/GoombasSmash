using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

[RequireComponent(typeof(NetworkCharacterControllerCustom))]
[RequireComponent(typeof(NetworkMecanimAnimator))]
[RequireComponent(typeof(LifeHandler))]
public class CharacterControllerHandler : NetworkBehaviour
{
    NetworkCharacterControllerCustom _characterController;
    NetworkMecanimAnimator _animator;
    LifeHandler _lifeHandler;
    bool canDoubleJump = false;
    [Header("Aduio")]
    [SerializeField] AudioSource _audioSourse;
    [Header("View")]
    [SerializeField] float _view;
    bool isRespawnReq = false;
    public bool playerIsReady;

    private void Awake()
    {
        _characterController = GetComponent<NetworkCharacterControllerCustom>();
        _animator = GetComponent<NetworkMecanimAnimator>();
        _lifeHandler = GetComponent<LifeHandler>();
        transform.forward = transform.right;
    }

    public override void FixedUpdateNetwork()
    {
        if (!GameManager.Instance.isPlayerWin)
        {
            if (Object.HasStateAuthority)
            {
                if (_lifeHandler.cantLifes <= 0)
                {
                    if (!Object.HasInputAuthority)
                    {
                        GameManager.Instance.isHostDead = false;
                        Runner.Despawn(Object);
                        StartCoroutine(WaitToDisconnectPlayer());
                       // Runner.Disconnect(Object.InputAuthority);
                        //Runner.Despawn(Object);
                    }
                    else
                    {
                        GameManager.Instance.isHostDead = true;
                        GameManager.Instance.imTheHost = true;
                        Runner.Despawn(Object);
                        StartCoroutine(WaitToBackToMenuHost());
                       // Runner.Shutdown(true, ShutdownReason.Ok, false);
                    }
                    //Runner.Despawn(Object);
                    GameManager.Instance.RemovePlayerSpawn(this.gameObject);

                    return;
                }
                if (isRespawnReq)
                {
                    Respawn();
                    return;
                }
                if (_lifeHandler.isDead) return;
                GameManager.Instance.CheckCollisionWithBounds(this);
            }
        }
        else
        {
            StartCoroutine(GoBackToMenu());
            
        }

        if (!GameManager.Instance.isWaitingPlayers)
        {
            if (GetInput(out NetworkInputData input))
            {

                Vector3 moveDir = Vector3.right * input.movementInput;

                _characterController.Move(moveDir);
            }

            if (input.isJumpPressed)
            {
                if (_characterController.IsGrounded)
                {
                    _animator.Animator.SetTrigger("isJump");

                    _characterController.Jump();
                    canDoubleJump = true;
                }
                else if (canDoubleJump)
                {
                    _characterController.DoubleJump();
                    _animator.Animator.SetTrigger("isJump");
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
        _animator.Animator.SetFloat("Horizontal", Mathf.Abs(_characterController.Velocity.x));
    }

    public AudioSource GetAudioSource() { return _audioSourse; }

    IEnumerator WaitToDisconnectPlayer()
    {
        yield return new WaitForSeconds(3f);
        Runner.Disconnect(Object.InputAuthority);
    }
    IEnumerator GoBackToMenu()
    {
        
        yield return new WaitForSeconds(5f);
        Runner.Shutdown(true, ShutdownReason.Ok, false);
        //Runner.Disconnect(Object.InputAuthority);
    }
    IEnumerator WaitToBackToMenuHost()
    {
        yield return new WaitForSeconds(5f);
        Runner.Shutdown(true, ShutdownReason.Ok, false);
    }

    public void ReqRespawn()
    {
        isRespawnReq = true;
    }
    void Respawn()
    {
        _lifeHandler.OnRespawned();
        _characterController.TeleportToPosition(Utils.GetRandomSpawn());
        isRespawnReq = false;
    }
    public void OutOfTheBounds()
    {
        var playerNet = gameObject.GetComponent<NetworkPlayer>();
        StartCoroutine(_lifeHandler.SvReviveCo());
        _lifeHandler.isExplosion = true;
        _lifeHandler.isDead = true;
        Debug.Log($"[Damage MSG] {playerNet.GetNickname()} recibio daño");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        Gizmos.DrawWireSphere(transform.position, _view);
    }

}
