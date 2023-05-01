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
        private List<Wave> _endlessWaves;
        private int _currentWave = 0;
        private int _currentEndlessWave = 0;
        private bool _isAttacking = false;
        private float _attackProgress;
        private float _endlessAttackProgress;
        private float _lastAttackTime;
        private float _lastEndlessAttackTime;
        private float _attackLength;

        private const float FLY_HEIGHT = -0.75f;

        public override void OnUpdate()
        {
            /*if (Input.IsKeyJustPressed(KeyCode.W))
            {
                _currentWave = _waves.Count;
                _attackProgress = _attackLength;
                AttackCompleted = true;
            }*/
            
            if (!_isAttacking)
                return;
            
            if (!AttackCompleted)
                UpdateAttack();
            else
                UpdateEndlessAttack();
        }

        public void Init()
        {
            GenerateScenario();
            _attackProgress = 0;
            _lastAttackTime = 0;
            _attackLength = _waves.Sum(w => w.Delay);
            _isAttacking = true;
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

        private void UpdateEndlessAttack()
        {
            _endlessAttackProgress += Time.DeltaTime;

            while (_endlessAttackProgress >= _lastEndlessAttackTime + _endlessWaves[_currentEndlessWave].Delay)
            {
                _lastEndlessAttackTime += _endlessWaves[_currentEndlessWave].Delay;

                for (int i = 0; i < _endlessWaves[_currentEndlessWave].EnemyCount; i++)
                {
                    SpawnUnit(_endlessWaves[_currentEndlessWave].EnemyType);
                }
                
                _currentEndlessWave++;
                if (_currentEndlessWave >= _endlessWaves.Count)
                {
                    _currentEndlessWave = 0;
                    _endlessAttackProgress = 0.0f;
                    _lastEndlessAttackTime = 0.0f;
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
                new Wave(10, EnemyType.Soldier, 1),
                new Wave(20, EnemyType.Soldier, 1),
                new Wave(15, EnemyType.Soldier, 1),
                new Wave(20, EnemyType.Soldier, 2),
                new Wave(15, EnemyType.Soldier, 2),
                new Wave(15, EnemyType.Soldier, 3),
                
                new Wave(20, EnemyType.Tank, 1),
                new Wave(20, EnemyType.Runner, 1),
                new Wave(20, EnemyType.Soldier, 2),
                new Wave(20, EnemyType.Soldier, 2),
                new Wave(25, EnemyType.Tank, 1),
                new Wave(15, EnemyType.Runner, 2),
                new Wave(10, EnemyType.Soldier, 2),
                new Wave(20, EnemyType.Tank, 1),
                new Wave(10, EnemyType.Runner, 2),
                
                new Wave(20, EnemyType.Soldier, 2),
                new Wave(4, EnemyType.Soldier, 1),
                new Wave(4, EnemyType.Soldier, 1),
                new Wave(4, EnemyType.Soldier, 1),
                new Wave(4, EnemyType.Soldier, 1),
                new Wave(4, EnemyType.Soldier, 1),
                new Wave(10, EnemyType.Tank, 1),
                new Wave(10, EnemyType.Tank, 1),
                new Wave(5, EnemyType.Runner, 1),
                new Wave(1, EnemyType.Runner, 1),
                new Wave(1, EnemyType.Runner, 1),
                
                new Wave(30, EnemyType.Flying, 1),
                new Wave(10, EnemyType.Tank, 2),
                new Wave(15, EnemyType.Runner, 1),
                new Wave(10, EnemyType.Soldier, 5),
                new Wave(10, EnemyType.Soldier, 5),
                new Wave(10, EnemyType.Tank, 3),
                new Wave(5, EnemyType.Runner, 4),
                new Wave(5, EnemyType.Soldier, 3),
                new Wave(10, EnemyType.Flying, 1),
                new Wave(10, EnemyType.Tank, 2),
                new Wave(10, EnemyType.Runner, 3),
                
                new Wave(10, EnemyType.Flying, 2),
                new Wave(2, EnemyType.Flying, 2),
                new Wave(10, EnemyType.Tank, 2),
                new Wave(2, EnemyType.Tank, 2),
                new Wave(10, EnemyType.Runner, 4),
                new Wave(2, EnemyType.Runner, 4),
                new Wave(2, EnemyType.Runner, 4),
                new Wave(2, EnemyType.Runner, 4),
                new Wave(5, EnemyType.Soldier, 8),
                new Wave(5, EnemyType.Soldier, 8),
                new Wave(5, EnemyType.Soldier, 8),
                new Wave(7, EnemyType.Tank, 3),
                new Wave(7, EnemyType.Tank, 3),
                new Wave(7, EnemyType.Tank, 3),
                new Wave(1, EnemyType.Runner, 2),
                new Wave(1, EnemyType.Runner, 2),
                new Wave(1, EnemyType.Runner, 2),
                new Wave(1, EnemyType.Runner, 2),
                new Wave(1, EnemyType.Runner, 2),
                new Wave(1, EnemyType.Runner, 2),
                new Wave(10, EnemyType.Flying, 6),
                
                new Wave(15, EnemyType.Runner, 1),
            };

            _endlessWaves = new List<Wave>
            {
                new Wave(10, EnemyType.Soldier, 15),
                new Wave(10, EnemyType.Tank, 5),
                new Wave(5, EnemyType.Soldier, 15),
                new Wave(5, EnemyType.Flying, 5),
                new Wave(10, EnemyType.Runner, 10),
                new Wave(10, EnemyType.Tank, 8),
            };
        }
    }
}