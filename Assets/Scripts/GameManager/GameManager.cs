using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get { return _instance; } }
    static GameManager _instance;

    [SerializeField] GameObject _warnigText;

    [SerializeField] float _boundWidth; 
    [SerializeField] float _boundHeight ;
    [SerializeField] Color _color;
    [Header("Hookeable Objects")]
    [SerializeField]List<GameObject> _hookeableList= new List<GameObject>();
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

    public void CheckCollisionWithBounds(NetworkPlayer player)
    {
        var playerPosition = ApplyBounds(player.transform.position);
        if (player.transform.position != playerPosition)
        {
            _warnigText.SetActive(true);
            player.ReciveDamage();
        }
        else { _warnigText.SetActive(false); }

        
    }
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
