using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NetworkManager))]
public class StartUI : MonoBehaviour
{
    [SerializeField] string defaultAddress = "localhost";

    public void Connect() => NetworkManager.singleton.StartClient();

    public void Host() => NetworkManager.singleton.StartHost();

    public void OnAddressChanged(string txt) => NetworkManager.singleton.networkAddress = string.IsNullOrEmpty(txt) ? defaultAddress : txt;
}
