using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{

    [SerializeField]
    private NavMeshAgent agent;

    [SerializeField]
    private Collider collider;


    public Vector3 walkPoint;
    public bool walkPointSet;
    private Vector3 lastPlayerPositon;
    private float rotateDeltaTime;


    [SerializeField]
    private float rotateSeconds;

    [SerializeField]
    private float lastPlayerPositionDistanceWhenChangePatrol;

    [SerializeField]
    private float walkPointRange;

    [SerializeField]
    private float playerInSightRange;

    [SerializeField]
    private EnemyStates state;

    [SerializeField]
    private float visibleDistance;

    [SerializeField]
    private float distanceMagnitude;

    [SerializeField]
    private Canvas chaseCanvas;


    [SerializeField]
    private DeadAnimScript deadAnimScript;


    private void Awake()
    {
     
    }

    // Start is called before the first frame update
    void Start()
    {

        this.chaseCanvas.gameObject.SetActive(false);
    }

    private Vector3? GetLastPlayerPositionIfVisible()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, visibleDistance))
        {
            if (hitInfo.collider != null)
            {
                if (hitInfo.collider.CompareTag(Tags.Player.ToString()))
                {
                    Vector3 playerPosition = hitInfo.collider.gameObject.transform.position;

                    Vector3 searchedPlayerPosition = new Vector3(playerPosition.x, 0, playerPosition.z);

                    return searchedPlayerPosition;            
                }
            }
        }

        return null;
    }

    private void SetChaseStateIfPlayerVisible()
    {
        Vector3? lastPlayerPos = GetLastPlayerPositionIfVisible();
        if (lastPlayerPos.HasValue)
        {
            this.lastPlayerPositon = lastPlayerPos.Value;
            this.state = EnemyStates.Chase;
            this.chaseCanvas.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (this.state == EnemyStates.Patrol)
        {
            Patrolling();
            SetChaseStateIfPlayerVisible();
        }
        else if (this.state == EnemyStates.Chase)
        {
            ChasePlayer();
        }
        else if (this.state == EnemyStates.Rotate)
        {
            transform.RotateAround(this.transform.position, Vector3.up, agent.angularSpeed * Time.deltaTime);

            this.rotateDeltaTime += Time.deltaTime;

            if (this.rotateDeltaTime >= this.rotateSeconds)
            {
                this.state = EnemyStates.Patrol;
                this.rotateDeltaTime = 0;
            }

            SetChaseStateIfPlayerVisible();
        }
    }

    private void Patrolling()
    {

       if (!walkPointSet)
       {
            SearchWalkPoint();
       }

       if (walkPointSet)
       {
            Quaternion newRotation = Quaternion.LookRotation(walkPoint - this.transform.position);

            this.transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 10);

            agent.SetDestination(walkPoint);
       }

       Vector3 distanceToWalkPoint = transform.position - walkPoint;

       if (distanceToWalkPoint.magnitude < this.distanceMagnitude)
       {
           // search the new random walkpoint
           walkPointSet = false;
       }

    }

    private void SearchWalkPoint()
    {
        // Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        walkPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(this.lastPlayerPositon);
       
        float dist = Vector3.Distance(this.lastPlayerPositon, this.transform.position);

        if (dist <= lastPlayerPositionDistanceWhenChangePatrol)
        {
            this.state = EnemyStates.Rotate;
            this.rotateDeltaTime = 0;
            this.chaseCanvas.gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(Tags.bullet.ToString()))
        {  
            this.state = EnemyStates.NotMove;
            this.chaseCanvas.gameObject.SetActive(false);

            this.collider.enabled = false;
            // Debug.Log("Bullet hit!");

            this.deadAnimScript.Play();

            agent.velocity = Vector3.zero;
            agent.isStopped = true;
            agent.ResetPath();

        }
        else if (collision.gameObject.CompareTag(Tags.Player.ToString()))
        {
            GameObject player = collision.gameObject;
            
            var playerController = player.GetComponent<PlayerController>();
            playerController.StopAndDisableMoveAndShoot();

            // Debug.Log("Game over!");
            MenuController.Instance.ShowGameOverMenu();

        }

    }

}
