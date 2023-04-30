using Steel;
using SteelCustom.Enemies;

namespace SteelCustom.Effects
{
    public class BigRocket : ScriptComponent
    {
        private Vector3 _target;
        private int _damage;
        private float _range;

        private const float SPEED = 3;

        public override void OnUpdate()
        {
            UpdateFlight();
        }

        public void Init(Vector3 target, int damage, float range)
        {
            _target = target;
            _damage = damage;
            _range = range;

            Entity.AddComponent<SpriteRenderer>().Sprite = ResourcesManager.GetImage("rocket.aseprite");

            Transformation.Position = target + new Vector3(-Camera.Main.Height, Camera.Main.Height);
        }

        private void UpdateFlight()
        {
            Transformation.Position += new Vector3(SPEED * Time.DeltaTime, -SPEED * Time.DeltaTime);
            
            if (Vector2.Distance(Transformation.Position, _target) < 0.1f)
            {
                Explode();
            }
        }

        private void Explode()
        {
            foreach (Entity hitEntity in Physics.AABBCast(_target, new Vector2(_range, _range)))
            {
                EnemyUnit enemyUnit = EnemyUnit.GetEnemyUnit(hitEntity);
                if (enemyUnit != null && !enemyUnit.Entity.IsDestroyed())
                    enemyUnit.TakeDamage(_damage);
            }
            
            Entity.Destroy();
            
            Entity entity = ResourcesManager.GetAsepriteData("big_rocket_effect.aseprite").CreateEntityFromAsepriteData();
            entity.Transformation.Position = Transformation.Position + new Vector3(0.125f, 0.625f, 0.5f);
            entity.GetComponent<Animator>().Play("Effect");
            entity.Destroy(0.5f);
        }
    }
}