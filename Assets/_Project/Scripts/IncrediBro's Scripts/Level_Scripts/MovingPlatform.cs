using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DragonspiritGames.PlatformerController
{
    public class MovingPlatform : MonoBehaviour
    {
        [SerializeField] private Transform[] waypoints;   
        [SerializeField] private float speed = 2f;        
        [SerializeField] private float waitTime = 1f;     
        [SerializeField] private bool loop = true;        
        [SerializeField] private bool pingpong = false;   

        private int currentWaypointIndex = 0;   
        private bool moving = true;             
        private float timer = 0f;               
        private bool movingForward = true;      

        private void Start()
        {
            if (waypoints.Length == 0)
            {
                enabled = false;
            }
        }

        private void Update()
        {
            if (moving)
            {
                MovePlatform();
            }
            else
            {
                WaitAtWaypoint();
            }
        }

        private void MovePlatform()
        {
            Vector3 targetPosition = waypoints[currentWaypointIndex].position;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            if (transform.position == targetPosition)
            {
                if (pingpong)
                {
                    if (movingForward)
                    {
                        currentWaypointIndex++;
                    }
                    else
                    {
                        currentWaypointIndex--;
                    }

                    if (currentWaypointIndex == waypoints.Length - 1)
                    {
                        movingForward = false;
                    }
                    else if (currentWaypointIndex == 0)
                    {
                        movingForward = true;
                    }
                }
                else
                {
                    if (currentWaypointIndex == waypoints.Length - 1)
                    {
                        if (loop)
                        {
                            currentWaypointIndex = 0;
                        }
                        else
                        {
                            ResetPlatform();
                            return;
                        }
                    }
                    else
                    {
                        currentWaypointIndex++;
                    }
                }

                timer = 0f;
                moving = false;
            }
        }

        private void WaitAtWaypoint()
        {
            timer += Time.deltaTime;

            if (timer >= waitTime)
            {
                moving = true;
            }
        }

        private void ResetPlatform()
        {
            currentWaypointIndex = 0;
            transform.position = waypoints[currentWaypointIndex].position;
            moving = true;
        }

        private void OnDrawGizmos()
        {
            for (int i = 0; i < waypoints.Length - 1; i++)
            {
                Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
            }

            if (loop && waypoints.Length > 1)
            {
                Gizmos.DrawLine(waypoints[waypoints.Length - 1].position, waypoints[0].position);
            }
        }
    }
}
