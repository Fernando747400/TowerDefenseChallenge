using System.Collections.Generic;
using UnityEngine;

public class WayPointManager : MonoBehaviour
{
    [SerializeField] private List<Transform> _wayPoints = new List<Transform>();

    public Transform GetFirstWaypoint()
    {
        if (CheckForEmpty()) return null;
        return _wayPoints[0];
    }

    public Transform GetNextWaypoint(Transform currentWaypoint)
    {
        if (CheckForEmpty()) return null;

        int currentIndex = _wayPoints.IndexOf(currentWaypoint);

        if (currentIndex == _wayPoints.Count - 1) return null;

        return _wayPoints[currentIndex + 1];
    }

    public Queue<Transform> GetWayPointsQueue()
    {
        if(CheckForEmpty()) return null;

        Queue<Transform> wayPointsQueue = new Queue<Transform>();
        foreach(Transform currentWaypoint in _wayPoints)
        {
            wayPointsQueue.Enqueue(currentWaypoint);
        }

        return wayPointsQueue;
    }

    public bool IsLastWaypoint(Transform currentWaypoint)
    {
        return (GetNextWaypoint(currentWaypoint) == null);
    }

    private bool CheckForEmpty()
    {
        return (_wayPoints.Count <= 0);
    }
}
