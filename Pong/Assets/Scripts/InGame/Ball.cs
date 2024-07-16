using Mirror;
using Mirror.Examples.Pong;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : NetworkBehaviour
{
    [SerializeField] float speed = 3f;
    [SerializeField] float acceleration = 1.3f;
    [SerializeField] float initialMaxSlope = 0.5f;

    Rigidbody2D rb;

    public delegate void Score(bool isLeft);
    public static Score OnScore;

    private Vector2 currentVelocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public override void OnStartServer()
    {
        base.OnStartServer();

        rb.simulated = true;

        Move();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
    }

    public override void OnStopClient()
    {
        base.OnStopClient();
    }

    [Server]
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(Tags.SCORE))
        {
            rb.velocity = Vector2.zero;
            transform.position = Vector2.zero;

            OnScore?.Invoke(Vector2.Dot(currentVelocity.normalized, Vector2.right) > 0);
            return;
        }

        Vector2 reflectVelocity = Vector2.Reflect(currentVelocity, collision.GetContact(0).normal);

        if (collision.collider.CompareTag(Tags.RACKET))
        {
            reflectVelocity *= acceleration;
        }

        currentVelocity = rb.velocity = reflectVelocity;

    }

    [Server]
    public void ResetBall()
    {
        rb.velocity = Vector2.zero;
        transform.position = Vector2.zero;

        Move();
    }

    [Server]
    public void Move() => currentVelocity = rb.velocity = Utils.CustomRandomInsideUnitCircle(initialMaxSlope) * speed;

}
