using Mirror;
using Mirror.Examples.Pong;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PongNM : NetworkManager
{
    NMHelper helper;

    public override Transform GetStartPosition()
    {
        if (helper == null) return null;

        return numPlayers == 0 ? helper.LeftPos : helper.RightPos;
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        if (sceneName != onlineScene) return;

        helper = FindObjectOfType<NMHelper>();
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);

        if (numPlayers != maxConnections) return;

        GameObject ballSpawn = Instantiate(spawnPrefabs[0]);

        helper.Ball = ballSpawn.GetComponent<Ball>();

        NetworkServer.Spawn(ballSpawn, conn);
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        base.OnServerDisconnect(conn);

        StartCoroutine(StopDelayed());
    }

    IEnumerator StopDelayed()
    {
        yield return new WaitForSeconds(1);
        Disconnect();
    }

    [Server] // declaring [Server] just for safety as "OnServerDisconnect" is only called on server side...
    private void Disconnect() => StopHost();

    public delegate void Disconnected();
    public static Disconnected OnDisconnected;

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();

        OnDisconnected?.Invoke();
    }

}
