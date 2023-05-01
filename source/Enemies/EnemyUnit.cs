using System;
using System.Collections;
using System.Linq;
using Steel;
using SteelCustom.Buildings;
using Math = Steel.Math;

namespace SteelCustom.Enemies
{
    public abstract class EnemyUnit : ScriptComponent
    {
        public EnemyType EnemyType { get; private set; }
        public abstract string SpritePath { get; }

        public bool IsDead => Health <= 0;
        public int Health { get; protected set; }
        public abstract int MaxHealth { get; }
        public abstract float Speed { get; }
        public abstract int Reward { get; }
        public abstract int Damage { get; }
        public abstract float AttackDelay { get; }
        public abstract float AttackRange { get; }

        public virtual bool IsGround => true;
        protected virtual Vector2 ColliderSize => new Vector2(0.3f, 0.5f);

        private Action _deadCallback;
        private Building _target = null;
        private float _attackCooldown = 0.0f;
        private Animator _animator;
        private bool _isWalking = false;
        private bool _isAttacking = false;
        private float _knockBackForce;

        public override void OnUpdate()
        {
            if (IsDead)
                return;
            
            _attackCooldown -= Time.DeltaTime;
            
            UpdateAttack();
        }

        public override void OnCreate()
        {
            GameController.Instance.BattleController.BuilderController.OnBuildingsChanged += OnBuildingsChanged;
        }

        public override void OnDestroy()
        {
            GameController.Instance.BattleController.BuilderController.OnBuildingsChanged -= OnBuildingsChanged;
        }

        public void Init(EnemyType enemyType, Action deadCallback)
        {
            EnemyType = enemyType;
            Health = MaxHealth;
            _deadCallback = deadCallback;

            Entity.AddComponent<RigidBody>().RigidBodyType = RigidBodyType.Static;
            Entity.AddComponent<BoxCollider>().Size = ColliderSize;

            AsepriteData data = ResourcesManager.GetAsepriteData(SpritePath, true);
            data.Animations.Last().Loop = false;
            foreach (Sprite sprite in data.Sprites)
            {
                sprite.Pivot = new Vector2(0.5f, 0.0f);
            }
            Entity.AddComponent<SpriteRenderer>().Sprite = data.Sprites.First();

            _animator = Entity.AddComponent<Animator>();
            _animator.AddAnimations(data.Animations);
        }

        public static EnemyUnit GetEnemyUnit(Entity entity)
        {
            if (entity == null || entity.IsDestroyed())
                return null;
            if (entity.HasComponent<Soldier>())
                return entity.GetComponent<Soldier>();
            if (entity.HasComponent<Runner>())
                return entity.GetComponent<Runner>();
            if (entity.HasComponent<Tank>())
                return entity.GetComponent<Tank>();
            if (entity.HasComponent<Flying>())
                return entity.GetComponent<Flying>();
            return null;
        }

        public void TakeDamage(int damage)
        {
            if (IsDead)
                return;
            
            Health = Math.Max(0, Health - damage);
            
            GameController.Instance.BattleController.DamageAnimator.Animate(damage, Transformation.Position, true);
            
            if (Health <= 0)
                Die();
        }

        public void AddKnockBack(float force)
        {
            if (MaxHealth >= 20)
                force *= 0.5f;
            _knockBackForce += force;
        }

        private void Die()
        {
            int reward = (int)Math.Ceiling(Reward * GameController.Instance.MotherShip.CreditGainModifier);
            GameController.Instance.Player.GainCredits(reward, Transformation.Position);
            Entity.Destroy(0.5f);
            _animator.Play("Death");
            _deadCallback?.Invoke();
        }

        private void UpdateAttack()
        {
            if (_knockBackForce > 0)
            {
                Transformation.Position += new Vector3(_knockBackForce * Time.DeltaTime, 0);
                _knockBackForce -= Time.DeltaTime * 10.0f;
            }
            
            if (_target == null || _target.Entity.IsDestroyed())
                _target = GetTarget();

            if (_target == null)
            {
                _isWalking = false;
                return;
            }

            if (!CanAttack(_target))
            {
                Move();
                return;
            }

            if (_attackCooldown <= 0.0f)
            {
                _isWalking = false;
                StartCoroutine(AttackCoroutine());
                return;
            }
        }

        private bool CanAttack(Building building)
        {
            return Math.Abs(Transformation.Position.X - building.Transformation.Position.X) <= AttackRange;
        }

        private IEnumerator AttackCoroutine()
        {
            _attackCooldown = AttackDelay;
            _animator.Play("Attack");
            _isAttacking = true;

            yield return new WaitForSeconds(0.3f);
            
            if (IsDead)
                yield break;

            Attack();
            
            yield return new WaitForSeconds(0.4f);
            
            if (IsDead)
                yield break;
            
            _animator.Play("Idle");
            _isAttacking = false;
        }

        protected virtual void Attack()
        {
            if (_target != null && !_target.Entity.IsDestroyed() && CanAttack(_target))
            {
                _target.TakeDamage(this, Damage);
                Entity.AddComponent<AudioSource>().Play(ResourcesManager.GetAudioTrack(MaxHealth >= 20 ? "big_hit.wav" : "small_hit.wav"));
            }
        }

        private void Move()
        {
            if (_isAttacking)
                return;
            if (!_isWalking)
            {
                _isWalking = true;
                _animator.Play("Walk");
            }
            Transformation.Position -= new Vector3(Speed * Time.DeltaTime, 0);
        }

        private void OnBuildingsChanged()
        {
            _target = GetTarget();
        }

        protected virtual Building GetTarget()
        {
            return GameController.Instance.BattleController.BuilderController.GetClosestBuilding(Transformation.Position.X);
        }
    }
}