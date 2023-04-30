using System;
using System.Collections.Generic;
using System.Linq;
using Steel;
using SteelCustom.Enemies;
using Random = Steel.Random;

namespace SteelCustom
{
    public class EnemyController : ScriptComponent
    {
        public float AttackCompletion => Steel.Math.Clamp(_attackProgress / _attackLength, 0, 1);
        public float CurrentWave => _currentWave;
        public bool AttackCompleted { get; private set; } = false;
        public List<EnemyUnit> Enemies => new List<EnemyUnit>(_enemies);

        private readonly LinkedList<EnemyUnit> _enemies = new LinkedList<EnemyUnit>();
        private List<Wave> _waves;
        private int _currentWave = 0;
        private bool _isAttacking = false;
        private float _attackProgress;
        private float _lastAttackTime;
        private float _attackLength;

        private const float FLY_HEIGHT = -0.75f;

        public override void OnUpdate()
        {
            if (Input.IsKeyJustPressed(KeyCode.W)) // TODO: remove
            {
                _currentWave = _waves.Count;
                _attackProgress = _attackLength;
                AttackCompleted = true;
            }
            
            if (!_isAttacking)
                return;
            
            if (!AttackCompleted)
                UpdateAttack();
        }

        public void Init()
        {
            GenerateScenario();
            _attackProgress = 0;
            _lastAttackTime = 0;
            _attackLength = _waves.Sum(w => w.Delay);
            _isAttacking = true;

            SpawnUnit(EnemyType.Soldier);
            SpawnUnit(EnemyType.Tank);
            SpawnUnit(EnemyType.Runner);
            SpawnUnit(EnemyType.Flying);
        }

        private void UpdateAttack()
        {
            _attackProgress += Time.DeltaTime;

            while (_attackProgress >= _lastAttackTime + _waves[_currentWave].Delay)
            {
                _lastAttackTime += _waves[_currentWave].Delay;

                for (int i = 0; i < _waves[_currentWave].EnemyCount; i++)
                {
                    SpawnUnit(_waves[_currentWave].EnemyType);
                }
                
                _currentWave++;
                if (_currentWave >= _waves.Count)
                {
                    AttackCompleted = true;
                    return;
                }
            }
        }

        private void SpawnUnit(EnemyType enemyType)
        {
            EnemyUnit unit = CreateUnit(enemyType);
            LinkedListNode<EnemyUnit> node = _enemies.AddLast(unit);
            unit.Init(enemyType, () => _enemies.Remove(node));

            float y = unit.IsGround ? BuilderController.GROUND_HEIGHT : FLY_HEIGHT;
            y += 1.0f / 32;
            unit.Transformation.Position = new Vector3(6 + Random.NextFloat(-0.5f, 0.5f), y, 1);
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

        private void GenerateScenario()
        {
            _waves = new List<Wave>
            {
                new Wave(5, EnemyType.Soldier, 1),
                new Wave(5, EnemyType.Soldier, 1),
                new Wave(10, EnemyType.Soldier, 2),
                new Wave(10, EnemyType.Soldier, 2),
                new Wave(10, EnemyType.Soldier, 3),
                
                new Wave(10, EnemyType.Tank, 1),
                new Wave(5, EnemyType.Runner, 1),
                new Wave(15, EnemyType.Soldier, 3),
                new Wave(10, EnemyType.Tank, 1),
                new Wave(5, EnemyType.Runner, 3),
                
                new Wave(20, EnemyType.Soldier, 1),
                new Wave(2, EnemyType.Soldier, 1),
                new Wave(2, EnemyType.Soldier, 1),
                new Wave(2, EnemyType.Soldier, 1),
                new Wave(2, EnemyType.Soldier, 1),
                new Wave(2, EnemyType.Soldier, 1),
                new Wave(3, EnemyType.Tank, 1),
                new Wave(3, EnemyType.Tank, 1),
                new Wave(5, EnemyType.Runner, 1),
                new Wave(1, EnemyType.Runner, 1),
                new Wave(1, EnemyType.Runner, 1),
                
                new Wave(20, EnemyType.Flying, 1),
                new Wave(5, EnemyType.Tank, 1),
                new Wave(5, EnemyType.Tank, 1),
                new Wave(5, EnemyType.Soldier, 3),
                new Wave(5, EnemyType.Soldier, 1),
                new Wave(1, EnemyType.Soldier, 1),
                new Wave(1, EnemyType.Soldier, 1),
                new Wave(1, EnemyType.Soldier, 1),
                new Wave(1, EnemyType.Soldier, 1),
                new Wave(1, EnemyType.Soldier, 1),
                new Wave(5, EnemyType.Flying, 1),
                new Wave(5, EnemyType.Flying, 1),
                new Wave(5, EnemyType.Flying, 1),
                new Wave(1, EnemyType.Runner, 1),
                new Wave(1, EnemyType.Runner, 1),
                new Wave(1, EnemyType.Runner, 1),
                new Wave(5, EnemyType.Tank, 1),
                new Wave(5, EnemyType.Tank, 1),
            };
        }
    }
}