using UnityEngine;

namespace DragonspiritGames.PlatformerController
{
    public class EnemyWanderingState : IEnemyState
    {
        public void EnterState(Enemy enemy)
        {
            // Set new wander target
            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            // We can't directly access wanderTarget, so we'll let ChangeState handle it
        }

        public void UpdateState(Enemy enemy)
        {
            // if (Vector2.Distance(enemy.transform.position, enemy.WanderTarget) < 0.5f || 
            //     enemy.StateTimer >= enemy.GetEnemyData().wanderTimer * 2)
            // {
            //     enemy.ChangeState(EnemyState.Idle);
            //     return;
            // }
            //
            // enemy.MoveTowards(enemy.WanderTarget);
        }

        public void ExitState(Enemy enemy)
        {
            // Cleanup wandering behavior
        }
    }
}