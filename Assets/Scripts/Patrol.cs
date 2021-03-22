using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Patrol : MonoBehaviour
{

    public Transform[] points;
    private int destPoint = 0;
    private int curPoint = 0;
    private NavMeshAgent agent;
    private bool _patroling = false;

    /**public Patrol(Transform[] path)
    {
        points = path;
    }*/

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Disabling auto-braking allows for continuous movement
        // between points (ie, the agent doesn't slow down as it
        // approaches a destination point).
        agent.autoBraking = false;

        GotoNextPoint();
    }


    public void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (points.Length == 0)
            return;

        // Set the agent to go to the currently selected destination.
        agent.destination = points[destPoint].position;

        curPoint = destPoint;
        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = (destPoint + 1) % points.Length;
    }


    void Update()
    {
        // Choose the next destination point when the agent gets
        // close to the current one.
        if (!agent.pathPending && agent.remainingDistance < 0.5f && _patroling)
        {
            transform.localEulerAngles = points[curPoint].localEulerAngles;
            GotoNextPoint();
        }
    }

    public void StartPatrol()
    {
        _patroling = true;
    }

    public void StopPatrol()
    {
        _patroling = false;
    }

    public void ChangeRoute(Transform[] newPoints)
    {
        points = newPoints;
        destPoint = 0;
        curPoint = 0;
    }
}
