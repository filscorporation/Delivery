using System;
using System.Collections;
using Steel;
using SteelCustom.Buildings;
using Math = Steel.Math;

namespace SteelCustom.Enemies
{
    public abstract class EnemyUnit : ScriptComponent
    {
        public EnemyType EnemyType { get; private set; }
        
        public int Health { get; protected set; }
        public abstract int MaxHealth { get; }
        public abstract float Speed { get; }
        public abstract int Reward { get; }
        public abstract int Damage { get; }
        public abstract float AttackDelay { get; }
        public abstract float AttackRange { get; }

        public virtual bool IsGround => true;
        protected virtual Vector2 ColliderSize => new Vector2(0.3f, 0.3f);

        private Building _target = null;
        private float _attackCooldown = 0.0f;

        public override void OnUpdate()
        {
            _attackCooldown -= Time.DeltaTime;
            
            UpdateAttack();
        }

        public void Init(EnemyType enemyType)
        {
            EnemyType = enemyType;
            Health = MaxHealth;

            Sprite sprite = ResourcesManager.GetImage(EnemyTypeToSpritePath(EnemyType));
            sprite.Pivot = new Vector2(0.5f, 0.0f);
            Entity.AddComponent<SpriteRenderer>().Sprite = sprite;
            /*Entity.AddComponent<BoxCollider>().Size = ColliderSize;
            var rb = Entity.AddComponent<RigidBody>();
            rb.RigidBodyType = RigidBodyType.Static;*/
        }

        public void TakeDamage(int damage)
        {
            Health = Math.Max(0, Health - damage);
            if (Health <= 0)
                Die();
        }

        private void Die()
        {
            GameController.Instance.Player.GainCredits(Reward, Transformation.Position);
            Entity.Destroy();
        }

        private void UpdateAttack()
        {
            if (_target == null || _target.Entity.IsDestroyed())
                _target = GameController.Instance.BattleController.BuilderController.GetClosestBuilding(Transformation.Position.X);
            
            if (_target == null)
                return;

            if (Math.Abs(Transformation.Position.X - _target.Transformation.Position.X) > AttackRange)
            {
                Move();
                return;
            }

            if (_attackCooldown <= 0.0f)
            {
                StartCoroutine(AttackCoroutine());
                return;
            }
        }

        private IEnumerator AttackCoroutine()
        {
            _attackCooldown = AttackDelay;

            yield return new WaitForSeconds(0.3f);

            Attack();
        }

        protected virtual void Attack()
        {
            if (_target != null && !_target.Entity.IsDestroyed())
            {
                _target.TakeDamage(this, Damage);
            }
        }

        private void Move()
        {
            Transformation.Position -= new Vector3(Speed * Time.DeltaTime, 0);
        }

        private static string EnemyTypeToSpritePath(EnemyType buildingType)
        {
            switch (buildingType)
            {
                case EnemyType.Soldier:
                    return "soldier.aseprite";
                case EnemyType.Tank:
                    return "tank.aseprite";
                case EnemyType.Runner:
                    return "runner.aseprite";
                case EnemyType.Flying:
                    return "flying.aseprite";
                default:
                    throw new ArgumentOutOfRangeException(nameof(buildingType), buildingType, null);
            }
        }
    }
}