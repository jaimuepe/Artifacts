using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    public float speed = 100f;
    public Vector2 movementDirection = Vector2.zero;

    public float swingForce;
    public float jumpForce;

    public bool IsAlive { get { return deathHandler.alive; } }
    public bool Swinging { get { return maxDistanceJoint.enabled; } }

    public float maxVerticalSpeed;

    [HideInInspector]
    public Rope rope;

    [HideInInspector]
    public BoxCollider2D boxCollider;

    public DistanceJoint2D maxDistanceJoint;

    [HideInInspector]
    public Rigidbody2D rb;

    SpriteRenderer sr;
    GroundCheck feet;

    public float ropeMaxDistance = 10f;
    public float ropeDistance = 0f;

    public bool disabled = false;

    DeathHandler deathHandler;

    public bool CanDoStuff { get { return !GameManager.instance.gamePaused && !disabled && deathHandler.alive; } }

    Transform myTransform;
    Animator animator;

    public FMODUnity.StudioEventEmitter jumpEmitter;
    public FMODUnity.StudioEventEmitter stepsEmitter;
    public FMODUnity.StudioEventEmitter fallEmitter;
    public FMODUnity.StudioEventEmitter dieEmitter;

    public Vector3 defaultPosition;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        myTransform = transform;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        deathHandler = GetComponent<DeathHandler>();
        maxDistanceJoint.enabled = false;
        defaultPosition = transform.position;
    }

    void Start()
    {
        rope = GetComponentInChildren<Rope>();
        feet = GetComponentInChildren<GroundCheck>();
    }

    private void Update()
    {
        if (!CanDoStuff)
        {
            animator.SetBool("running", false);
        }

        if (Swinging)
        {
            animator.SetBool("swinging", true);
            animator.SetBool("running", false);
        }
        else
        {
            animator.SetBool("swinging", false);
            animator.SetBool("running", movementDirection.x != 0);
        }

        animator.SetBool("grounded", feet.grounded);

        if (rb.velocity.x > 0.01f)
        {
            sr.flipX = false;
        }
        else if (rb.velocity.x < -0.01f)
        {
            sr.flipX = true;
        }
    }

    private void FixedUpdate()
    {

        Vector2 velocity = rb.velocity;
        velocity.x = movementDirection.x * speed;

        if (!CanDoStuff)
        {
            rb.velocity = velocity;
            return;
        }

        if (Swinging)
        {
            if (movementDirection.y != 0)
            {
                ropeDistance -= rope.ropeIncrement * movementDirection.y;
            }
            else if (movementDirection.x != 0)
            {
                rb.AddForce(Vector2.right * movementDirection.x * swingForce / Time.deltaTime, ForceMode2D.Force);
            }
            ropeDistance = Mathf.Clamp(ropeDistance, 0.5f, ropeMaxDistance);
            maxDistanceJoint.distance = ropeDistance;
        }
        else
        {
            // Not swinging
            if (Mathf.Abs(rb.velocity.x) < Mathf.Abs(speed))
            {
                rb.velocity = velocity;
            }
            animator.SetBool("running", movementDirection.x != 0);
        }
    }

    public void Jump()
    {
        if (Swinging || feet.grounded)
        {
            CancelHook();
            jumpEmitter.Play();
            rb.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);
        }
    }

    public void CancelHook()
    {
        rope.BreakRope();
    }

    public void ThrowRope()
    {
        rope.FireRope(myTransform.position);
    }

    public void RestoreCheckpoint()
    {
        animator.SetBool("dead", false);

        if (GameManager.instance.bossFightReady)
        {
            LastArtifact lastArtifact = GameManager.instance.lastArtifact;
            myTransform.position = new Vector3(
                lastArtifact.defaultPosition.x,
                lastArtifact.defaultPosition.y,
                myTransform.position.z);

            deathHandler.alive = true;

            return;
        }

        int checkpoint = GameManager.instance.currentCheckpoint;
        Artifact artifact = GameManager.instance.GetArtifact(checkpoint);
        if (artifact == null)
        {
            myTransform.position = defaultPosition;
        }
        else
        {
            myTransform.position = new Vector3(
                artifact.defaultPosition.x,
                artifact.defaultPosition.y,
                myTransform.position.z);
        }

        deathHandler.alive = true;
    }

    public void DeactivateActions()
    {
        rope.BreakRope();
        disabled = true;
    }

    public void ReactivateActions()
    {
        disabled = false;
    }

    public void PlayFootsteps()
    {
        stepsEmitter.Play();
    }


    public void Kill(bool isFromFall)
    {
        if (!GameManager.instance.godModeEnabled)
        {
            KillIgnoreCheats(isFromFall);
        }
    }

    public void KillIgnoreCheats(bool isFromFall)
    {
        if (isFromFall)
        {
            fallEmitter.Play();
        }
        dieEmitter.Play();

        deathHandler.KillPlayer();
        animator.SetBool("dead", true);
    }

    public void DeactivateActionsForSeconds(float seconds)
    {
        rope.BreakRope();
        StartCoroutine(DisabledActionsCoroutine(seconds));
    }

    IEnumerator DisabledActionsCoroutine(float seconds)
    {
        disabled = true;
        yield return new WaitForSeconds(seconds);
        disabled = false;
    }
}
