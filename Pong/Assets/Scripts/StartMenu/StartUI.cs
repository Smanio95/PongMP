using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(NetworkManager))]
public class StartUI : MonoBehaviour
{
    [SerializeField] string defaultAddress = "localhost";
    [Header("Exceptions")]
    [SerializeField] TMP_Text commTxt;
    [SerializeField] string cannotConnectString = "Could not connect to the server. This may be caused by max N of players reached or there is not an active server.";
    [SerializeField] string hostExcString = "Host is already present at this address!";
    [SerializeField] float waitTime = 5;

    private bool hasTriedToConnect = false;

    private void OnEnable()
    {
        PongNM.OnDisconnected += CannotConnect;
    }

    private void OnDisable()
    {
        PongNM.OnDisconnected -= CannotConnect;
    }

    public void Connect()
    {
        hasTriedToConnect = true;
        NetworkManager.singleton.StartClient();
    }

    public void Host()
    {
        try
        {
            NetworkManager.singleton.StartHost();
        }
        catch (SocketException)
        {
            Debug.Log("host already present");
            StopAllCoroutines();
            if (commTxt) StartCoroutine(Communication(hostExcString));
        }
    }

    public void OnAddressChanged(string txt) => NetworkManager.singleton.networkAddress = string.IsNullOrEmpty(txt) ? defaultAddress : txt;

    IEnumerator Communication(string msg)
    {
        commTxt.gameObject.SetActive(true);

        commTxt.text = msg;

        yield return new WaitForSeconds(waitTime);

        commTxt.gameObject.SetActive(false);

    }

    private void CannotConnect()
    {
        if (!hasTriedToConnect) return; 

        StopAllCoroutines();

        if(commTxt) StartCoroutine(Communication(cannotConnectString));

        hasTriedToConnect = false;
    }
}
