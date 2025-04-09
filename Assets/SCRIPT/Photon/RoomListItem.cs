using Photon.Realtime;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class RoomListItem : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    public RoomInfo roomInfo;
    public void Setup(RoomInfo _roomInfo)
    {
        roomInfo = _roomInfo;
        text.text = _roomInfo.Name;
    }

    public void OnClick()
    {
        Launcher.Instance.JoinRoom(roomInfo); 
    }
}
