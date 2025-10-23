using UnityEngine;

namespace DragonspiritGames.PlatformerController
{
    public class EnemyIdleState : IEnemyState
    {
        public void EnterState(Enemy enemy)
        {
            // Initialize idle behavior
        }

        public void UpdateState(Enemy enemy)
        {
            // if (enemy.StateTimer >= enemy.GetEnemyData().wanderTimer)
            // {
            //     enemy.ChangeState(EnemyState.Wandering);
            // }
        }

        public void ExitState(Enemy enemy)
        {
            // Cleanup idle behavior
        }
    }
}