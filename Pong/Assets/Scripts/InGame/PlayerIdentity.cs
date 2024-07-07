using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdentity : NetworkBehaviour
{
    [SerializeField] float speed = 10;

    Vector2 startPosition;

    public override void OnStartAuthority()
    {
        if (!isLocalPlayer) return;
        
        enabled = true;

        startPosition = transform.position;
    }

    [ClientCallback]
    private void Update()
    {
        transform.Translate(Input.GetAxis("Vertical") * speed * Time.deltaTime * Vector2.up);
    }

    private void Awake()
    {
        NMHelper.OnRestart += RestartGame;
    }

    private void OnDestroy()
    {
        NMHelper.OnRestart -= RestartGame;
    }

    private void RestartGame()
    {
        transform.position = startPosition;
    }
}
