using Steel;
using SteelCustom.Enemies;

namespace SteelCustom.Buildings
{
    public class SmallRocket : ScriptComponent
    {
        private Vector3 TargetPosition => _target.Transformation.Position + new Vector3(0, 0.2f);
        
        private EnemyUnit _target;
        private int _damage;
        private float _speed = 0;
        private float _verticalSpeed = SPEED;

        private const float SPEED = 3;

        public override void OnUpdate()
        {
            UpdateFlight();
        }

        public void Init(EnemyUnit target, int damage)
        {
            _target = target;
            _damage = damage;
        }

        private void UpdateFlight()
        {
            if (_target == null || _target.Entity == null || _target.Entity.IsDestroyed())
            {
                Explode();
                return;
            }

            Vector3 direction = new Vector3(0, _verticalSpeed * Time.DeltaTime, 0) + (TargetPosition - Transformation.Position).Normalize() * _speed * Time.DeltaTime;
            Transformation.Position += direction;
            Transformation.Rotation = new Vector3(0, 0, Math.Atan2(direction.Normalize().Y, direction.Normalize().X) - Math.Pi * 0.5f);
            
            _verticalSpeed = Math.Lerp(_verticalSpeed, 0, Time.DeltaTime * 2);
            _speed = Math.Lerp(_speed, SPEED, Time.DeltaTime * 2);
            
            if (Vector2.Distance(Transformation.Position, TargetPosition) < 0.05f)
            {
                _target.TakeDamage(_damage);
                Explode();
            }
        }

        private void Explode()
        {
            Entity.Destroy();
            
            Entity entity = ResourcesManager.GetAsepriteData("small_rocket_effect.aseprite").CreateEntityFromAsepriteData();
            entity.Transformation.Position = Transformation.Position + new Vector3(0, 0, 0.5f);
            entity.GetComponent<Animator>().Play("Effect");
            entity.Destroy(0.5f);
        }
    }
}