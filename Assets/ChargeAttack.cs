using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeAttack : MonoBehaviour
{

    public float chargeDistance;

    Player player;

    public float cooldown;
    float nextChargeTimer;

    public float timeBeforeCharge;
    public float chargeTime;

    Transform myTransform;
    Transform playerTransform;

    SpriteRenderer sr;

    ChasePlayer chasePlayer;
    LookTowardsPlayer lookTowardsPlayer;

    Animator animator;

    public FMODUnity.StudioEventEmitter emitter;

    private void Awake()
    {
        myTransform = transform;
        sr = GetComponent<SpriteRenderer>();
        chasePlayer = GetComponent<ChasePlayer>();
        lookTowardsPlayer = GetComponent<LookTowardsPlayer>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        player = FindObjectOfType<Player>();
        playerTransform = player.transform;
    }

    public void Restore()
    {
        StopAllCoroutines();
        chasePlayer.enabled = true;
        lookTowardsPlayer.enabled = true;

        // nextChargeTimer = cooldown;
        sr.color = Color.white;
        animator.SetBool("enraged", false);
    }

    private void Update()
    {
        if (!player.CanDoStuff)
        {
            return;
        }

        if (nextChargeTimer >= 0)
        {
            // Reloading
            nextChargeTimer -= Time.deltaTime;
        }
        else
        {
            // ready
            float distance = Vector2.Distance(myTransform.position, playerTransform.position);
            if (distance <= chargeDistance)
            {
                Charge();
                nextChargeTimer = cooldown;
            }
        }
    }

    public float chargeSpeed;

    void Charge()
    {
        Vector3 target = playerTransform.position;
        StartCoroutine(ChargeCoroutine(target));
    }

    IEnumerator ChargeCoroutine(Vector3 target)
    {
        chasePlayer.enabled = false;
        lookTowardsPlayer.enabled = false;
        animator.SetBool("enraged", true);

        // play animation

        sr.color = Color.red;
        yield return new WaitForSeconds(timeBeforeCharge);

        emitter.Play();
        Vector3 direction = (target - myTransform.position).normalized;

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / chargeTime)
        {
            if (!GameManager.instance.gamePaused)
            {
                myTransform.position += chargeSpeed * new Vector3(direction.x, direction.y, 0);
            }
            yield return null;
        }

        animator.SetBool("enraged", false);
        chasePlayer.enabled = true;
        lookTowardsPlayer.enabled = true;
        sr.color = Color.white;
    }
}
