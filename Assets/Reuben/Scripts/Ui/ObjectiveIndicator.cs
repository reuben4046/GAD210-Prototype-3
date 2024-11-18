using System.Collections.Generic;
using UnityEngine;

public class ObjectiveIndicator : MonoBehaviour
{
    List<Transform> objectivesToPointAt = new List<Transform>();

    private Transform playerTransform;

    [SerializeField] private float indicatorTurnSpeed = 10f;

    void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    void OnEnable()
    {
        EventSystem.OnChunkSpawned += OnChunkSpawned;
    }

    void OnDisable()
    {
        EventSystem.OnChunkSpawned -= OnChunkSpawned;
    }

    void OnChunkSpawned(List<Transform> objectives)
    {
        foreach (Transform objective in objectives)
        {
            objectivesToPointAt.Add(objective);
        }
    }

    void Update()
    {
        LookAtObjective(FindClosestObjective());
    }

    void LookAtObjective(Transform objective)
    {
        Vector3 direction = objective.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), Time.deltaTime * indicatorTurnSpeed);
    }

    Transform FindClosestObjective()
    {
        if (objectivesToPointAt.Count > 0)
        {
            List<Transform> oldObjectives = new List<Transform>();
            Transform closestObjective = null;
            float closestDistance = Mathf.Infinity;
            foreach (Transform objective in objectivesToPointAt)
            {
                if (playerTransform.position.x > objective.position.x)
                {
                    oldObjectives.Add(objective);
                }
                float distance = Vector3.Distance(playerTransform.position, objective.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestObjective = objective;
                }
            }

            foreach (Transform objective in oldObjectives)
            {
                objectivesToPointAt.Remove(objective);
            }
            oldObjectives.Clear();

            return closestObjective;
        }
        return null;
    }

}
