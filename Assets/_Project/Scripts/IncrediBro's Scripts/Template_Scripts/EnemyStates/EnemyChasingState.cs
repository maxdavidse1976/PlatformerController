using UnityEngine;

namespace DragonspiritGames.PlatformerController
{
    public class EnemyChasingState : IEnemyState
    {
        public void EnterState(Enemy enemy)
        {
            // Initialize chasing behavior
        }

        public void UpdateState(Enemy enemy)
        {
            // if (enemy.Player == null)
            // {
            //     enemy.ChangeState(EnemyState.Idle);
            //     return;
            // }
            //
            // float distanceToPlayer = Vector2.Distance(enemy.transform.position, enemy.Player.position);
            //
            // if (distanceToPlayer > enemy.GetEnemyData().detectionRange * 1.5f)
            // {
            //     // Lost player, return to wandering
            //     enemy.ChangeState(EnemyState.Wandering);
            //     return;
            // }
            //
            // if (distanceToPlayer <= enemy.GetEnemyData().attackRange && 
            //     Time.time >= enemy.LastAttackTime + enemy.GetEnemyData().attackCooldown)
            // {
            //     enemy.ChangeState(EnemyState.Attacking);
            //     return;
            // }
            //
            // enemy.MoveTowards(enemy.Player.position);
        }

        public void ExitState(Enemy enemy)
        {
            // Cleanup chasing behavior
        }
    }
}