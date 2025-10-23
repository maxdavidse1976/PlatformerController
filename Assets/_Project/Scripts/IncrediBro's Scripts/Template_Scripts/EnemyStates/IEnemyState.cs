// State interface for future Enemy refactoring
// Currently unused to avoid compilation errors

using UnityEngine;

namespace DragonspiritGames.PlatformerController
{
    public interface IEnemyState
    {
        void EnterState(Enemy enemy);
        void UpdateState(Enemy enemy);
        void ExitState(Enemy enemy);
    }
}