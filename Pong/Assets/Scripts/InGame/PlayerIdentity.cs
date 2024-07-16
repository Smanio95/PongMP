using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerIdentity : NetworkBehaviour
{
    [Header("Custom collision")]
    [SerializeField] Transform topChecker;
    [SerializeField] Transform botChecker;
    [SerializeField] float detectionRadius = 0.2f;

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
        float dir = Input.GetAxis("Vertical");

        if (dir == 0) return;
        if (CustomCheckCollision(dir > 0 ? 1 : -1)) return;

        transform.Translate(dir * speed * Time.deltaTime * Vector2.up);
    }

    private void Awake()
    {
        NMHelper.OnRestart += RestartGame;
    }

    private void OnDestroy()
    {
        NMHelper.OnRestart -= RestartGame;
    }

    private void RestartGame() => transform.position = startPosition;

    private bool CustomCheckCollision(int normalizedDir)
    {
        Vector3 checkerPos = topChecker.position;

        if (normalizedDir < 0) checkerPos = botChecker.position;

        Collider2D[] hits = Physics2D.OverlapCircleAll(checkerPos, detectionRadius)
                                     .Where(x => !x.CompareTag(Tags.RACKET))
                                     .ToArray();

        return hits.Length > 0;
    }

}
