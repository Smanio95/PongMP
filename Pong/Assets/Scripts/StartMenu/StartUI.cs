using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NetworkManager))]
public class StartUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] float messageWaitTime = 3;

    private PongNM manager;

    public void LimitedConnect(GameObject txt)
    {
        if (manager.numPlayers >= manager.maxConnections)
        {
            StartCoroutine(MaxPlayersCR(txt));
            return;
        }

        manager.StartClient();
    }

    IEnumerator MaxPlayersCR(GameObject txt)
    {
        txt.SetActive(true);
        yield return new WaitForSeconds(messageWaitTime);
        txt.SetActive(false);
    }

    private void Awake()
    {
        manager = GetComponent<PongNM>();
    }

    public void Host() => manager.StartHost();

    public void OnAddressChanged(string txt) => manager.networkAddress = txt; 
}
