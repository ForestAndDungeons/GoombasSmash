using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fusion;
using System.Linq;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get { return _instance; } }
    static GameManager _instance;

    [SerializeField] Text _winningText;
    [Networked(OnChanged = nameof(IsPlayerWinChanged))]
    public bool isPlayerWin { get; set; }
    [Networked(OnChanged = nameof(ChangeListPlayers))]
    public bool isSpawnplayer { get; set; }
    public bool isWaitingPlayers = true;
    public GameObject buttonGoToMenu;
    [Networked(OnChanged = nameof(IsHostDead))]
    public bool isHostDead { get; set; }
    public bool imTheHost;
    
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

    static void IsPlayerWinChanged(Changed<GameManager> changed)
    {
        changed.Behaviour.IsPlayerWinChanged();
    }
    static void ChangeListPlayers(Changed<GameManager> changed)
    {
        Debug.Log($"{Time.time} OnPorcentDamage value {changed.Behaviour.isSpawnplayer}");

    }
    static void IsHostDead(Changed<GameManager> changed)
    {
        Debug.Log($"{Time.time} OnPorcentDamage value {changed.Behaviour.isHostDead}");
    }
    private void IsPlayerWinChanged()
    {
        if (isPlayerWin)
        {

            // NetworkPlayer name = _playersList[0].GetComponent<NetworkPlayer>();
            // RPC_SetWinningText(name);
            //_winningText.text = $"{name.GetNickname()} Gano la partida";
            //StartCoroutine(WaitToCheckPlayerList());
            if (isHostDead)
            {
                if (!imTheHost)
                {
                    NetworkPlayer name = _playersList[1].GetComponent<NetworkPlayer>();
                    _winningText.text = $"<color=cyan><b>{name.GetNickname()}</b></color> <i>Gano la partida</i>";
                }
                else
                {
                    NetworkPlayer name = _playersList[0].GetComponent<NetworkPlayer>();
                    _winningText.text = $"<color=cyan><b>{name.GetNickname()}</b></color> <i>Gano la partida</i>";
                }
            }
            else
            {
                NetworkPlayer name = _playersList[0].GetComponent<NetworkPlayer>();
                _winningText.text = $"<color=cyan><b>{name.GetNickname()}</b></color> <i>Gano la partida</i>";
            }
            //_winningText.text = $"GANASTE!";
            //buttonGoToMenu.SetActive(true);
        }
        else {
            _winningText.text = "";
           // buttonGoToMenu.SetActive(false);
        }
    }

    /*IEnumerator WaitToCheckPlayerList()
    {
        yield return new WaitForSeconds(0.5f);

    }*/

    /*[Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    void RPC_SetWinningText(NetworkPlayer name)
    {
        _winningText.text = $"{name.GetNickname()} Gano la partida";
    }*/
    public void NewPlayerSpawn(GameObject player)
    {
        _playersList.Add(player);
    }

    public void RemovePlayerSpawn(GameObject player)
    {
        _playersList.Remove(player);
        if (_playersList.Count <= 1)
        {
            isPlayerWin = true;
            /*foreach (var pj in _playersList)
            {
                if (Object.HasInputAuthority)
                {
                    isPlayerWin = true;
                   NetworkPlayer name = pj.GetComponent<NetworkPlayer>();
                    Debug.Log($"El jugador {name.GetNickname()} Gano!!!");
                }
            }
            */
        }
    }

    public List<GameObject>  GetPlayerList() { return _playersList; }


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
