using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NicknamesHandler : MonoBehaviour
{
    public static NicknamesHandler Instance { get; private set; }
    [SerializeField] NicknameText _nicknameTextPref;
    List<NicknameText> _allNicknames;

    private void Awake()
    {
        if (Instance) Destroy(gameObject);
        else Instance = this;

        _allNicknames = new List<NicknameText>();
    }

    public NicknameText AddNickname(NetworkPlayer player)
    {
        NicknameText newNickname = Instantiate(_nicknameTextPref, transform).SetPlayer(player);

        _allNicknames.Add(newNickname);
        player.OnLeft += () => {
            _allNicknames.Remove(newNickname);
            Destroy(newNickname.gameObject);
        };
        return newNickname;
    }
    private void LateUpdate()
    {
        foreach (var nickname in _allNicknames)
        {
            nickname.UpdatePosition();
        }
    }
}
