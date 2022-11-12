using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_InputField newRoomInputField;
    [SerializeField] TMP_Text feedbackText;
    [SerializeField] TMP_Text roomNameText;
    [SerializeField] GameObject roomPanel;
    [SerializeField] GameObject roomListObject;
    [SerializeField] RoomItem roomItemPrefab;

    List<RoomItem> roomItemList = new List<RoomItem>();

    private void Start() {
        feedbackText.text = "";
        PhotonNetwork.JoinLobby();
    }

    public void ClickCreateRoom()
    {
        feedbackText.text = "";

        if(newRoomInputField.text.Length < 3){
            feedbackText.text = "Room name min 3 Characters";
            return;
        }

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(newRoomInputField.text);
    }

    public void JoinRoom(string roomName){
        PhotonNetwork.JoinRoom(roomName);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Created room: " + PhotonNetwork.CurrentRoom.Name);
        feedbackText.text = "Created room" + PhotonNetwork.CurrentRoom.Name;
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room: " + PhotonNetwork.CurrentRoom.Name);
        feedbackText.text = "Joined room: " + PhotonNetwork.CurrentRoom.Name;

        roomNameText.text = PhotonNetwork.CurrentRoom.Name;
        roomPanel.SetActive(true);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (var item in roomItemList)
        {
            Destroy(item.gameObject);
        }

        roomItemList.Clear();

        foreach (var roomInfo in roomList)
        {
            RoomItem newRoomItem = Instantiate(roomItemPrefab, roomListObject.transform);
            newRoomItem.Set(this, roomInfo.Name);
            this.roomItemList.Add(newRoomItem);
        }
    }
}
