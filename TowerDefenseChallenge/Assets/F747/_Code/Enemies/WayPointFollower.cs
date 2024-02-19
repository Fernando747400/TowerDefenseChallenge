using Lean.Pool;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class WayPointFollower : MonoBehaviour
{
    [HideInInspector] public UnityEvent OnArriveToLastCheckpoint = new UnityEvent();

    private float _moveSpeed = 1f;
    private float _rotationForce = 1f;
    private float _arriveDistance = 1f;
    private WayPointManager _waypointsToFollow; 

    private Rigidbody _rigidBody;
    private bool _isLastWaypoint;
    private Transform _currentWaypoint;
    private Vector3 _waypointPosition;
    private Vector3 _destinationPosition;
    private Vector3 _directionToWaypoint;

    public float MoveSpeed { get => _moveSpeed; set => _moveSpeed = value; }
    public float RotationForce { get => _rotationForce; set => _rotationForce = value; }
    public float ArriveDistance { get => _arriveDistance; set => _arriveDistance = value; }
    public WayPointManager WaypointsToFollow { get => _waypointsToFollow; set => _waypointsToFollow = value; }

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Spawn();
    }

    private void FixedUpdate()
    {
        UpdateDirection();
        LookAtWaypoint();
        MoveToWaypoint();
        GetNextWaypointOnArrival();
    }

    public void Spawn()
    {
        _currentWaypoint = _waypointsToFollow.GetFirstWaypoint();
        UpdateWaypointPosition();
        MoveToSpawn();
        _isLastWaypoint = false;
    }

    private void UpdateWaypointPosition()
    {
        _waypointPosition = _currentWaypoint.position;
    }

    private void UpdateDirection()
    {
        _destinationPosition = new Vector3(_waypointPosition.x, transform.position.y, _waypointPosition.z);
        _directionToWaypoint = _destinationPosition - transform.position;
    }

    private void GetNextWaypointOnArrival()
    {
        if (Vector3.Distance(transform.position, _destinationPosition) > _arriveDistance) return;

        if (!_isLastWaypoint)
        {
            _currentWaypoint = _waypointsToFollow.GetNextWaypoint(_currentWaypoint);
            UpdateWaypointPosition();
            _isLastWaypoint = _waypointsToFollow.IsLastWaypoint(_currentWaypoint);
        }
        else
        {
            ArriveToLastCheckpoint();
        }
    }

    private void LookAtWaypoint()
    {
        Quaternion targetRotation = Quaternion.LookRotation(_directionToWaypoint, Vector3.up);
        _rigidBody.rotation = Quaternion.Lerp(_rigidBody.rotation, targetRotation, Time.deltaTime * _rotationForce * 10);
    }


    private void MoveToWaypoint()
    {
       _rigidBody.velocity = _directionToWaypoint.normalized * _moveSpeed;
    }

    private void MoveToSpawn()
    {
        transform.position = new Vector3(_waypointPosition.x, transform.position.y, _waypointPosition.z);
    }

    private void ArriveToLastCheckpoint()
    {
        OnArriveToLastCheckpoint.Invoke();
        LeanPool.Despawn(gameObject);
    }
}
