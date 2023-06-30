using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fusion;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get { return _instance; } }
    static GameManager _instance;

    [SerializeField] GameObject _warnigText;
    [Networked(OnChanged = nameof(ChangeListPlayers))]
    public bool isSpawnplayer { get; set; }

    /*[Networked(OnChanged = nameof(OnWinText))]
    NetworkString<_16> WinText{ get; set; }*/
    [SerializeField] List<GameObject> _playersList = new List<GameObject>();
    [SerializeField] float _boundWidth; 
    [SerializeField] float _boundHeight ;
    [SerializeField] Color _color;
    [Header("Hookeable Objects")]
    [SerializeField]List<GameObject> _hookeableList= new List<GameObject>();
    private bool outOfBoundsCalled = false;
    public override void Spawned()
    {
        if (Instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else { Destroy(this.gameObject);
        } 

        
    }

    public List<GameObject> GetAllHookeableList() { return _hookeableList; }
    public Vector3 ApplyBounds(Vector3 objectPosition)
    {
        if (objectPosition.x > _boundWidth)
            objectPosition.x = -_boundWidth;
        else if (objectPosition.x < -_boundWidth)
            objectPosition.x = _boundWidth;

        if (objectPosition.y > _boundHeight)
            objectPosition.y = -_boundHeight;
        else if (objectPosition.y < -_boundHeight)
            objectPosition.y = _boundHeight;

        return objectPosition;
    }

    public void CheckCollisionWithBounds(CharacterControllerHandler player)
    {
        var playerPosition = ApplyBounds(player.transform.position);
        if (player.transform.position != playerPosition)
        {
            if (!outOfBoundsCalled)
            {
                player.OutOfTheBounds();
                outOfBoundsCalled = true;
            }
            
            //warnigText.SetActive(true);
        }
        else {
            outOfBoundsCalled = false;
            //_warnigText.SetActive(false); 
        }

        
    }


    static void ChangeListPlayers(Changed<GameManager> changed)
    {
        Debug.Log($"{Time.time} OnPorcentDamage value {changed.Behaviour.isSpawnplayer}");

    }


    public void NewPlayerSpawn(GameObject player)
    {
        _playersList.Add(player);
    }

    public void RemovePlayerSpawn(GameObject player)
    {
        _playersList.Remove(player);
       if (_playersList.Count <= 1)
        {
            foreach (var pj in _playersList)
            {
                if (Object.HasInputAuthority)
                {
                    NetworkPlayer name = pj.GetComponent<NetworkPlayer>();
                    Debug.Log($"El jugador {name.GetNickname()} Gano!!!");
                   // RPC_SetWinMessage($"El jugador {name.GetNickname()} Gano!!!".ToString());
                }
            }
               /*NetworkPlayer name = pj.GetComponent<NetworkPlayer>();
                Text winTex = _warnigText.GetComponent<Text>();
                winTex.text = $"El jugador {name.GetNickname()} Gano!!!";
                _warnigText.SetActive(true);*/

        }
    }
   /* public static void OnWinText(Changed<GameManager> changed)
    {
        changed.Behaviour.UpdateWinText(changed.Behaviour.WinText.ToString());
    }
    void UpdateWinText(string newWinMSG)
    {
        Text winTex = _warnigText.GetComponent<Text>();
        winTex.text = newWinMSG;
        _warnigText.SetActive(true);
    }

    [Rpc(RpcSources.InputAuthority,RpcTargets.StateAuthority)]
    public void RPC_SetWinMessage(string msg)
    {
        WinText = msg;
        /*foreach (var pj in _playersList)
        {
            NetworkPlayer name = pj.GetComponent<NetworkPlayer>();
            WinText = $"El jugador {name.GetNickname()} Gano!!!";
        }
        // Text winTex = _warnigText.GetComponent<Text>();
        // winTex.text = $"El jugador {name.GetNickname()} Gano!!!";
        // _warnigText.SetActive(true);
    }*/

    void OnDrawGizmos()
    {
        Gizmos.color = _color;

        Vector3 topLeft = new Vector3(-_boundWidth, _boundHeight, 0);
        Vector3 topRight = new Vector3(_boundWidth, _boundHeight, 0);
        Vector3 botLeft = new Vector3(-_boundWidth, -_boundHeight, 0);
        Vector3 botRight = new Vector3(_boundWidth, -_boundHeight, 0);

        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, botRight);
        Gizmos.DrawLine(botRight, botLeft);
        Gizmos.DrawLine(botLeft, topLeft);

    }
}
