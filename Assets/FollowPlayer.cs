using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform target;

    public static FollowPlayer instance;

    private float velocityX;
    private float velocityY;

    Transform myTransform;

    public Vector2 offset;
    public Vector2 defaultOffset;

    public float verticalSmoothTime;
    public float horizontalSmoothTime = 0.3F;

    public float defaultVerticalSmoothTime;
    public float defaultHorizontalSmoothTime = 0.3F;

    bool focusingPlayer;

    Player player;
    Transform playerTransform;

    float width;
    float height;

    public float cellSizeX = 19.2f;
    public float cellSizeY = 25f;

    public LayerMask boundsLayer;

    public bool followHorizontally = true;
    public bool followVertically = false;

    private void Awake()
    {
        instance = this;
        myTransform = transform;
        horizontalSmoothTime = defaultHorizontalSmoothTime;
    }

    private void Start()
    {
        player = FindObjectOfType<Player>();
        playerTransform = player.transform;
        FocusOnPlayer();

        height = Camera.main.orthographicSize * 2;
        float aspectRatio = (float)Screen.width / Screen.height;
        width = height * aspectRatio;

        myTransform.position = new Vector3(
            cellSizeX * (int)((playerTransform.position.x + cellSizeX / 2) / cellSizeX),
            cellSizeY * (int)((playerTransform.position.y + cellSizeY / 2) / cellSizeY),
        myTransform.position.z);
    }

    private void Update()
    {
        if (focusingPlayer)
        {
            if (!player.Swinging)
            {
                if (player.rb.velocity.x > 0.05f)
                {
                    offset = new Vector2(Mathf.Abs(offset.x), Mathf.Abs(offset.y));
                    horizontalSmoothTime = Mathf.Clamp(horizontalSmoothTime * 0.85f, defaultHorizontalSmoothTime, 1f);
                    verticalSmoothTime = Mathf.Clamp(verticalSmoothTime * 0.85f, defaultVerticalSmoothTime, 1f);
                }
                else if (player.rb.velocity.x < -0.5f)
                {
                    offset = new Vector2(-Mathf.Abs(offset.x), Mathf.Abs(offset.y));
                    horizontalSmoothTime = Mathf.Clamp(horizontalSmoothTime * 0.85f, defaultHorizontalSmoothTime, 1f);
                    verticalSmoothTime = Mathf.Clamp(verticalSmoothTime * 0.85f, defaultVerticalSmoothTime, 1f);
                }
            }
        }
    }

    public void FocusOnPlayer()
    {
        FocusOnPlayer(defaultOffset, followHorizontally, followVertically);
    }

    public void FocusOnPlayer(Vector2 offset)
    {
        FocusOnPlayer(offset, followHorizontally, followVertically);
    }

    public void FocusOnPlayer(Vector3 newOffset, bool followHorizontally, bool followVertically)
    {
        focusingPlayer = true;
        offset = newOffset;
        target = playerTransform;
        this.followHorizontally = followHorizontally;
        this.followVertically = followVertically;
        verticalSmoothTime = defaultVerticalSmoothTime;
    }

    public void FocusOnSomethingElse(Transform otherStuff, float smoothTime)
    {
        focusingPlayer = false;
        target = otherStuff;
        offset = Vector2.zero;
        horizontalSmoothTime = smoothTime;
        verticalSmoothTime = smoothTime;
    }

    Vector2 Position2D { get { return new Vector2(myTransform.position.x, myTransform.position.y); } }

    private void FixedUpdate()
    {
        //Vector3 targetPosition = target.TransformPoint(offset.x, offset.y, 0f);
        Vector3 targetPosition = target.position;

        Vector2 diff = Vector2.zero;

        if (!followHorizontally)
        {
            targetPosition.x = cellSizeX * Mathf.RoundToInt(target.position.x / cellSizeX);
        }
        float x = 0f;
        x = Mathf.SmoothDamp(myTransform.position.x, targetPosition.x, ref velocityX, horizontalSmoothTime);
        diff.x = x - myTransform.position.x;

        float y = 0f;
        if (!followVertically)
        {
            targetPosition.y = cellSizeY * Mathf.Round(target.position.y / cellSizeY);
        }

        y = Mathf.SmoothDamp(myTransform.position.y, targetPosition.y, ref velocityY, verticalSmoothTime);
        diff.y = y - myTransform.position.y;

        myTransform.position = new Vector3(x, y, myTransform.position.z);
        ClampCamera(diff.x, diff.y);
    }

    void ClampCamera(float dx, float dy)
    {
        Vector3 position = myTransform.position;

        float width3 = (width - 2) / 2;
        float height3 = (height - 2) / 2;

        Vector2 up = Position2D + Vector2.up * (height - 2f) / 2;
        Vector2 right = Position2D + Vector2.right * (width - 2f) / 2;

        if (dx != 0)
        {

            for (int i = 0; i < 3; i++)
            {
                RaycastHit2D horizontalHit;

                if (dx > 0)
                {
                    horizontalHit = Physics2D.Raycast(
                        // Position2D,
                        up + i * height3 * Vector2.down,
                        Vector2.right,
                        width / 2 + Mathf.Abs(dx),
                        boundsLayer);
                }
                else
                {
                    horizontalHit = Physics2D.Raycast(
                       // Position2D,
                       up + i * height3 * Vector2.down,
                       Vector2.left,
                       width / 2 + Mathf.Abs(dx),
                       boundsLayer);
                }

                Debug.DrawRay(up + i * height3 * Vector2.down, Vector2.left * (width / 2 + Mathf.Abs(dx)), Color.red);

                if (horizontalHit)
                {
                    position.x = horizontalHit.point.x - Mathf.Sign(dx) * (width) / 2;
                    velocityX = 0f;
                    break;
                }
            }
        }


        for (int i = 0; i < 3; i++)
        {
            RaycastHit2D verticalHit;

            if (dy > 0)
            {
                verticalHit = Physics2D.Raycast(
                    // Position2D,
                    right + i * width3 * Vector2.left,
                    Vector2.up,
                    height / 2 + Mathf.Abs(dy),
                    boundsLayer);
            }
            else
            {
                verticalHit = Physics2D.Raycast(
                   // Position2D,
                   right + i * width3 * Vector2.left,
                   Vector2.down,
                   height / 2 + Mathf.Abs(dy),
                   boundsLayer);
            }

            Debug.DrawRay(right + i * width3 * Vector2.left, Vector2.down * (height / 2 + Mathf.Abs(dy)), Color.red);

            if (verticalHit)
            {
                position.y = verticalHit.point.y - Mathf.Sign(dy) * (height) / 2;
                velocityY = 0f;
                break;
            }
        }


        myTransform.position = position;
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                Gizmos.DrawWireCube(new Vector3(i * cellSizeX, -j * cellSizeY, 0),
                    new Vector3(cellSizeX, cellSizeY, 1));
            }
        }
    }
}
