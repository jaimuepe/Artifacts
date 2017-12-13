using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{

    public RopeHead ropeHeadPrefab;
    public float speed;
    public Player player;

    public float impulseForce;

    RopeHead head;
    public GameObject staticRopeHead;

    bool canFireRope = true;
    public float fireRopeDelay = 1f;

    public RopeRenderer ropeRenderer;

    public float powerPushDuration;
    public float powerPushForce;

    public float ropeIncrement;
    public AnimationCurve animationCurve;

    public GameObject currentAnchor;

    Coroutine powerPushCoroutine;


    Target target;

    FMODUnity.StudioEventEmitter emitter;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        ropeRenderer = GetComponentInChildren<RopeRenderer>();
        target = GetComponentInChildren<Target>();
        ropeRenderer.gameObject.SetActive(false);
        emitter = GetComponent<FMODUnity.StudioEventEmitter>();
    }

    public void BreakRope()
    {
        player.maxDistanceJoint.enabled = false;
        ropeRenderer.gameObject.SetActive(false);
        if (head != null)
        {
            Destroy(head.gameObject);
            head = null;
        }

        currentAnchor = null;
        staticRopeHead.gameObject.SetActive(false);
    }

    public void FireRope(Vector3 start)
    {
        if (!canFireRope) { return; }

        if (head != null)
        {
            Destroy(head.gameObject);
            head = null;
        }

        target.Show();
        emitter.Play();

        head = Instantiate(ropeHeadPrefab);
        head.Init(this, target.transform.position, speed * target.currentDirection, player.ropeMaxDistance);

        StartCoroutine(WaitForRopeDelay());
    }

    public void ConnectRope(GameObject anchor, Vector3 eulerAngles)
    {
        BreakRope();
        currentAnchor = anchor;

        Vector3 anchorPosition = anchor.transform.position;

        player.maxDistanceJoint.connectedAnchor = anchorPosition;
        player.maxDistanceJoint.enabled = true;

        Vector2 diff = anchorPosition - player.transform.position;

        float distance = diff.magnitude;

        ropeRenderer.anchorPoint = anchorPosition;
        ropeRenderer.gameObject.SetActive(true);

        player.ropeDistance = distance;

        if (powerPushCoroutine != null)
        {
            StopCoroutine(powerPushCoroutine);
        }
        powerPushCoroutine = StartCoroutine(RopePowerPush());

        Vector2 velocity = player.rb.velocity;
        velocity.y = 0f;
        player.rb.velocity = velocity;

        float forceFactor = animationCurve.Evaluate(distance / player.ropeMaxDistance);
        Vector2 diffNorm = diff.normalized;
        player.rb.AddForce(forceFactor * impulseForce * diffNorm, ForceMode2D.Impulse);

        staticRopeHead.transform.position = new Vector3(anchorPosition.x, anchorPosition.y, staticRopeHead.transform.position.z);
        staticRopeHead.transform.eulerAngles = eulerAngles;
        staticRopeHead.gameObject.SetActive(true);
    }

    IEnumerator WaitForRopeDelay()
    {
        canFireRope = false;
        yield return new WaitForSeconds(fireRopeDelay);
        canFireRope = true;
    }

    IEnumerator RopePowerPush()
    {
        float time = 0f;
        Vector3 anchorPosition = player.maxDistanceJoint.connectedAnchor;
        while (time < powerPushDuration)
        {
            Vector3 playerPosition = player.transform.position;
            player.ropeDistance = Vector2.Distance(playerPosition, anchorPosition);
            time += Time.deltaTime;
            yield return null;
        }
    }
}
