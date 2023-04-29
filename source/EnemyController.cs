using System;
using Steel;
using SteelCustom.Enemies;

namespace SteelCustom
{
    public class EnemyController : ScriptComponent
    {
        private bool _isAttacking = false;

        private const float FLY_HEIGHT = 1f;

        public override void OnUpdate()
        {
            if (!_isAttacking)
                return;
            
            
        }

        public void Init()
        {
            _isAttacking = true;

            SpawnUnit(EnemyType.Soldier);
        }

        private void SpawnUnit(EnemyType enemyType)
        {
            EnemyUnit unit = CreateUnit(enemyType);
            unit.Init(enemyType);

            float y = unit.IsGround ? BuilderController.GROUND_HEIGHT : FLY_HEIGHT;
            unit.Transformation.Position = new Vector3(6, y);
        }

        private EnemyUnit CreateUnit(EnemyType enemyType)
        {
            Entity entity = new Entity(enemyType.ToString(), Entity);
            switch (enemyType)
            {
                case EnemyType.Soldier:
                    return entity.AddComponent<Soldier>();
                case EnemyType.Tank:
                    return entity.AddComponent<Tank>();
                case EnemyType.Runner:
                    return entity.AddComponent<Runner>();
                case EnemyType.Flying:
                    return entity.AddComponent<Flying>();
                default:
                    throw new ArgumentOutOfRangeException(nameof(enemyType), enemyType, null);
            }
        }
    }
}