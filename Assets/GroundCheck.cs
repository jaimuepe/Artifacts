using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public float maxSafeVerticalSpeed;

    bool wasGrounded;
    public bool grounded;

    public LayerMask groundLayer;

    public float distance;

    public float skinWidth = 0.02f;
    public float verticalRays = 3;

    float distanceBetweenRays;

    Player player;
    Transform playerTransform;

    Vector2 lowerLeftCorner;

    Vector2 Position2D
    {
        get { return new Vector2(playerTransform.position.x, playerTransform.position.y); }
    }

    public FMODUnity.StudioEventEmitter emitter;

    private void Start()
    {
        player = GetComponentInParent<Player>();
        playerTransform = player.transform;

        BoxCollider2D playerCollider = player.GetComponent<BoxCollider2D>();
        Vector2 boxCenter = playerCollider.offset;
        Vector2 extents = new Vector2(playerCollider.bounds.extents.x, playerCollider.bounds.extents.y);

        lowerLeftCorner = boxCenter - extents;
        Vector2 lowerRightCorner = boxCenter + new Vector2(extents.x, -extents.y);

        distanceBetweenRays = (lowerRightCorner.x - lowerLeftCorner.x - 2 * skinWidth) / (verticalRays - 1);

        emitter = GetComponent<FMODUnity.StudioEventEmitter>();

    }

    private void Update()
    {
        Vector2 rayPosition = Position2D + lowerLeftCorner + skinWidth * Vector2.right;

        grounded = false;

        for (int i = 0; i < verticalRays; i++)
        {
            Vector2 position = rayPosition + i * distanceBetweenRays * Vector2.right;
            Debug.DrawRay(position, Vector2.down * distance, Color.red);

            RaycastHit2D hit = Physics2D.Raycast(position, Vector2.down, distance, groundLayer);
            if (hit)
            {
                grounded = true;

                if (GameManager.instance.fallDamageEnabled &&
                  player.IsAlive && !wasGrounded && Mathf.Abs(player.rb.velocity.y) > maxSafeVerticalSpeed)
                {
                    player.Kill(true);
                }
                break;
            }
        }

        wasGrounded = grounded;
    }
}
