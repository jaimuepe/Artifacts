using System.Collections;
using UnityEngine;

public class RopeHead : MonoBehaviour
{
    Rope rope;
    float maxDistance;

    Vector3 startPosition;

    public LayerMask hookMask;

    void Update()
    {
        float distance = Vector2.Distance(transform.position, startPosition);
        if (distance >= maxDistance)
        {
            Destroy(gameObject);
        }
    }

    public void Init(Rope rope, Vector3 startPosition, Vector3 direction, float maxDistance)
    {
        this.rope = rope;
        this.maxDistance = maxDistance;
        this.startPosition = startPosition;
        transform.position = startPosition;
        GetComponent<Rigidbody2D>().velocity = direction;

        float angle = Mathf.Atan2(direction.y, direction.x);
        transform.eulerAngles = new Vector3(0, 0, Mathf.Rad2Deg * (angle + Mathf.PI / 2));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((1 << collision.gameObject.layer & hookMask) != 0)
        {
            if (rope.currentAnchor != null &&
                collision.gameObject.GetInstanceID() == rope.currentAnchor.GetInstanceID())
            {
                return;
            }
            rope.ConnectRope(collision.gameObject, transform.eulerAngles);
            Destroy(gameObject);
        }

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            return;
        }
        Destroy(gameObject);
    }
}
