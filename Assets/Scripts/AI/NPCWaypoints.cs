using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCWaypoints : MonoBehaviour
{
    public List<Transform> waypoints;
    public int timeBetweenMovementsMin;
    public int timeBetweenMovementsMax;
    private Vector3 currentGoalPos;
    NavMeshAgent agent;
    private Vector2 dir;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        StartCoroutine(MakeMove());
    }

    private void Update()
    {
        dir = currentGoalPos - transform.position;
        if (Vector2.Distance(transform.position, currentGoalPos) < 0.05f)
        {
            dir = Vector2.zero;
            currentGoalPos = new Vector2(-1000, -1000);
            StartCoroutine(MakeMove());
        }
    }

    IEnumerator MakeMove()
    {
        float delay = Random.Range(timeBetweenMovementsMin, timeBetweenMovementsMax);
        yield return new WaitForSeconds(delay);
        int wayPointIndex = Mathf.RoundToInt(Random.Range(0, waypoints.Count));
        currentGoalPos = waypoints[wayPointIndex].position;
        agent.SetDestination(currentGoalPos);
    }
}
