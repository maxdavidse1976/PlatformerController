using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DragonspiritGames.PlatformerController
{
    public class MovingPlatform : MonoBehaviour
    {
        [SerializeField] Transform[] _waypoints;   
        [SerializeField] float _speed = 2f;        
        [SerializeField] float _waitTime = 1f;     
        [SerializeField] bool _loop = true;        
        [SerializeField] bool _pingpong = false;   

        int _currentWaypointIndex = 0;   
        bool _moving = true;             
        float _timer = 0f;               
        bool _movingForward = true;      

        void Start()
        {
            if (_waypoints.Length == 0)
            {
                enabled = false;
            }
        }

        void Update()
        {
            if (_moving)
            {
                MovePlatform();
            }
            else
            {
                WaitAtWaypoint();
            }
        }

        // TODO: Refactor this
        void MovePlatform()
        {
            Vector3 targetPosition = _waypoints[_currentWaypointIndex].position;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, _speed * Time.deltaTime);

            if (transform.position == targetPosition)
            {
                if (_pingpong)
                {
                    if (_movingForward)
                    {
                        _currentWaypointIndex++;
                    }
                    else
                    {
                        _currentWaypointIndex--;
                    }

                    if (_currentWaypointIndex == _waypoints.Length - 1)
                    {
                        _movingForward = false;
                    }
                    else if (_currentWaypointIndex == 0)
                    {
                        _movingForward = true;
                    }
                }
                else
                {
                    if (_currentWaypointIndex == _waypoints.Length - 1)
                    {
                        if (_loop)
                        {
                            _currentWaypointIndex = 0;
                        }
                        else
                        {
                            ResetPlatform();
                            return;
                        }
                    }
                    else
                    {
                        _currentWaypointIndex++;
                    }
                }

                _timer = 0f;
                _moving = false;
            }
        }

        void WaitAtWaypoint()
        {
            _timer += Time.deltaTime;

            if (_timer >= _waitTime)
            {
                _moving = true;
            }
        }

        void ResetPlatform()
        {
            _currentWaypointIndex = 0;
            transform.position = _waypoints[_currentWaypointIndex].position;
            _moving = true;
        }

        void OnDrawGizmos()
        {
            for (int i = 0; i < _waypoints.Length - 1; i++)
            {
                Gizmos.DrawLine(_waypoints[i].position, _waypoints[i + 1].position);
            }

            if (_loop && _waypoints.Length > 1)
            {
                Gizmos.DrawLine(_waypoints[_waypoints.Length - 1].position, _waypoints[0].position);
            }
        }
    }
}
