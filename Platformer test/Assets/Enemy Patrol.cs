using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class EnemyPatrol : MonoBehaviour

{

    [Header("PATROL")]

    public Transform pointA;

    public Transform pointB;

    public float speed = 2f;

    [Header("PLAYER DETECTION")]

    public Transform player;

    public float sightRange = 5f;

    public float sightAngle = 60f;

    public LayerMask obstacleLayer;

    [Header("GAME OVER")]

    public GameObject caughtPopup;

    private Rigidbody2D rb;

    private Transform target;

    private float facingDirection = 1f;

    private bool caught = false;

    void Start()

    {

        rb = GetComponent<Rigidbody2D>();

        if (player == null) return;

        {

            Debug.LogError("Assign both patrol points!", this);

            enabled = false;

            return;

        }

        target = pointB;

        facingDirection = Mathf.Sign(

            pointB.position.x - pointA.position.x

        );

        if (caughtPopup != null)

            caughtPopup.SetActive(false);

    }

    void Update()

    {

        if (caught) return;

        CheckForPlayer();

    }

    void FixedUpdate()

    {

        if (caught) return;

        Patrol();

    }

    void Patrol()

    {

        float targetX = target.position.x;

        float currentX = rb.position.x;

        float newX = Mathf.MoveTowards(

            currentX,

            targetX,

            speed * Time.fixedDeltaTime

        );

        rb.MovePosition(

            new Vector2(newX, rb.position.y)

        );

        if (Mathf.Abs(targetX - currentX) < 0.05f)

        {

            target = target == pointA ? pointB : pointA;

            facingDirection = Mathf.Sign(

                target.position.x - rb.position.x

            );

        }

    }

    void CheckForPlayer()

    {

        if (player == null) return;

        Vector2 direction =

            (Vector2)player.position - rb.position;

        float distance = direction.magnitude;

        if (distance > sightRange || distance == 0f)

            return;

        direction.Normalize();

        Vector2 forward = new Vector2(

            facingDirection, 0f

        );

        float angle = Vector2.Angle(forward, direction);

        if (angle > sightAngle / 2f)

            return;

        RaycastHit2D hit = Physics2D.Raycast(

            rb.position,

            direction,

            distance,

            obstacleLayer

        );

        if (hit.collider != null)

            return;

        CatchPlayer();

    }

    void CatchPlayer()

    {

        caught = true;

        rb.linearVelocity = Vector2.zero;

        if (caughtPopup != null)

            caughtPopup.SetActive(true);

        Time.timeScale = 0f;

        Debug.Log("PLAYER CAUGHT!");

    }

}

