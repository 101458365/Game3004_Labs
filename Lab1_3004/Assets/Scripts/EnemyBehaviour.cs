using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    Transform player;
    NavMeshAgent agent;

    private void Start()
    {
        player = FindObjectOfType<PlayerBehaviour>().transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        agent.SetDestination(player.position);
        Debug.Log(player.position);
    }
}