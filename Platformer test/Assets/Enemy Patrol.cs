using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class EnemyPatrol : MonoBehaviour

{

    [Header("PATROL")]

    public Transform pointA;

    public Transform pointB;

    public float speed = 3.5f;

    public float arrivalDistance = 0.15f;

    [Header("VISION")]

    public Transform player;

    public float sightRange = 5f;

    public float sightAngle = 60f;

    public LayerMask obstacleLayer;

    [Header("GAME OVER")]

    public GameObject caughtPopup;

    private Rigidbody2D rb;

    private Transform target;

    private float patrolDirection;

    private bool playerCaught = false;

    void Awake()

    {

        rb = GetComponent<Rigidbody2D>();

    }

    void Start()

    {

        if (pointA  null || pointB  null)

        {

            Debug.LogError("Assign PatrolPointA and PatrolPointB!", this);

            enabled = false;

            return;

        }

        target = pointB;

        patrolDirection = Mathf.Sign(

            pointB.position.x - pointA.position.x

        );

        if (patrolDirection == 0)

            patrolDirection = 1;

        if (caughtPopup != null)

            caughtPopup.SetActive(false);

    }

    void Update()

    {

        if (playerCaught)

            return;

        CheckForPlayer();

    }

    void FixedUpdate()

    {

        if (playerCaught)

            return;

        Patrol();

    }

    void Patrol()

    {

        float distance = target.position.x - rb.position.x;

        float stoppingDistance = Mathf.Max(

            arrivalDistance,

            speed * Time.fixedDeltaTime

        );

        // Reached the patrol point

        if (Mathf.Abs(distance) <= stoppingDistance)

        {

            target = target == pointA ? pointB : pointA;

            patrolDirection = Mathf.Sign(

                target.position.x - rb.position.x

            );

            rb.velocity = new Vector2(0f, rb.velocity.y);

            return;

        }

        // Move horizontally while preserving vertical physics

        rb.velocity = new Vector2(

            patrolDirection * speed,

            rb.velocity.y

        );

    }

    void CheckForPlayer()

    {

        if (player == null)

            return;

        Vector2 origin = rb.position;

        Vector2 directionToPlayer =

            (Vector2)player.position - origin;

        float distance = directionToPlayer.magnitude;

        if (distance > sightRange || distance == 0)

            return;

        directionToPlayer.Normalize();

        // Check that the player is in front of the enemy

        Vector2 forward = new Vector2(patrolDirection, 0f);

        float dot = Vector2.Dot(forward, directionToPlayer);

        float requiredDot = Mathf.Cos(

            sightAngle  0.5f  Mathf.Deg2Rad

        );

        if (dot < requiredDot)

            return;

        // Check whether a wall or carriage blocks vision

        if (obstacleLayer.value != 0)

        {

            RaycastHit2D hit = Physics2D.Raycast(

                origin,

                directionToPlayer,

                distance,

                obstacleLayer

            );

            if (hit.collider != null)

                return;

        }

        CatchPlayer();

    }

    void CatchPlayer()

    {

        if (playerCaught)

            return;

        playerCaught = true;

        rb.velocity = Vector2.zero;

        if (caughtPopup != null)

            caughtPopup.SetActive(true);

        Time.timeScale = 0f;

        Debug.Log("Player caught by enemy!");

    }

}

