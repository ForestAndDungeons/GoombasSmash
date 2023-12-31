using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIReadyHandler : NetworkBehaviour
{
    [Header("Button")]
    public Button readyButton;
    [Header("Text")]
    public Text countDownText;
    [Header("Object")]
    public GameObject readyGameobject;
    public GameObject levelGameObject;
    public GameObject readyObjects;
    public GameObject cloudsLevel;
    [Header("GetsReady")]
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _getsReady;
    

    TickTimer countDownTickTimer = TickTimer.None;
    
    [Networked(OnChanged = nameof(OnCountdownChanged))]
    byte countDown { get; set; }

    [Networked(OnChanged = nameof(OnStartGameChanged))]
    public bool startGame { get; set; }

    [Networked(OnChanged = nameof(OnIsReadyChanged))]
    bool isReady { get; set; }

    void Start()
    {
        countDownText.text = "";
        //isReady = false;
       //startGame = false;
    }

    
    void Update()
    {
        if (countDownTickTimer.Expired(Runner))
        {
            startGame = true;
            countDownTickTimer = TickTimer.None;
        }
        else if (countDownTickTimer.IsRunning)
        {
            countDown = (byte)countDownTickTimer.RemainingTime(Runner);
        }
        StartCoroutine(WaitASecondToCheckPlayerList());
        if (GameManager.Instance.isPlayerWin)
        {
            _audioSource.Stop();
        }
    }

    IEnumerator WaitASecondToAssignValues()
    {
        yield return new WaitForSeconds(0.2f);
        isReady = false;
        startGame = false;
    }
    IEnumerator WaitASecondToCheckPlayerList()
    {
        yield return new WaitForSeconds(0.2f);
        if (GameManager.Instance.GetPlayerList().Count >= 2)
        {
            if (Object.HasStateAuthority)
            {
                readyButton.gameObject.SetActive(true);
            }
        }
    }

    public void OnStart()
    {
        if (isReady) isReady = false;
        else isReady = true;
        if (Runner.IsServer)
        {
            if (isReady)
            {
                countDownTickTimer = TickTimer.CreateFromSeconds(Runner,5);
            }
            else
            {
                countDownTickTimer = TickTimer.None;
                countDown = 0;
            }
        }
    }

    static void OnCountdownChanged(Changed<UIReadyHandler> changed)
    {
        changed.Behaviour.OnCountdownChanged();
    }

    static void OnIsReadyChanged(Changed<UIReadyHandler> changed)
    {
        changed.Behaviour.OnIsReadyChanged();
    }
    static void OnStartGameChanged(Changed<UIReadyHandler> changed)
    {
        changed.Behaviour.OnStartGameChanged();
    }

    void OnStartGameChanged()
    {
        if (startGame)
        {
            readyObjects.SetActive(false);
            readyGameobject.SetActive(false);
            cloudsLevel.SetActive(true);
            levelGameObject.SetActive(true);
            GameManager.Instance.isWaitingPlayers = false;
            _audioSource.volume = 0.03f;
            _audioSource.Play();
        }
    }
    void OnIsReadyChanged()
    {
        if (isReady)
        {
            _audioSource.volume=0.5f;
            _audioSource.PlayOneShot(_getsReady);
            Debug.Log("GetsReady Sound");      
        }
    }
    private void OnCountdownChanged()
    {
        
        if (countDown == 0)
        {
            countDownText.text = $"";
        }
        else
        {
            countDownText.text = $"La pelea comienza en {countDown}";  
        }
        
    }


}
