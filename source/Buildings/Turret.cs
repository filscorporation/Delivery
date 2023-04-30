using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Steel;
using SteelCustom.Enemies;

namespace SteelCustom.Buildings
{
    public class Turret : Building
    {
        public override BuildingType BuildingType => BuildingType.Turret;
        public override int MaxHealth => 5;
        public override int Price => 10;
        public override int DeliveryTime => 10;
        public override string SpritePath => "turret.aseprite";
        public override string Name => "Turret";
        public override string Description => $"Deals average damage at low range.\nPrice: {Price}, Delivery time: {DeliveryTime}";

        private Vector2 ShootPosition => (Vector2)Transformation.Position + new Vector2(3.0f / 32, 6.5f / 32);

        private float _shootTimer = 0;
        private Sprite _bulletSprite;

        private const float SHOOT_SPEED = 1.0f;
        private const int SHOOT_DAMAGE = 1;
        private const float SHOOT_RANGE = 0.7f;
        private const float BULLET_SPEED = 5.0f;

        protected override void UpdateBuilding()
        {
            UpdateShoot();
        }

        private void UpdateShoot()
        {
            _shootTimer -= Time.DeltaTime;
            if (_shootTimer <= 0)
            {
                List<EnemyUnit> enemyUnits = new List<EnemyUnit>();
                Vector2 start = ShootPosition;
                foreach (RayCastHit hit in Physics.LineCast(start, start + new Vector2(SHOOT_RANGE, 0)))
                {
                    EnemyUnit enemyUnit = EnemyUnit.GetEnemyUnit(hit.Entity);
                    if (enemyUnit != null)
                        enemyUnits.Add(enemyUnit);
                }
                
                if (enemyUnits.Any())
                    StartCoroutine(Shoot(enemyUnits.OrderBy(u => u.Transformation.Position.X).First()));
            }
        }

        private IEnumerator Shoot(EnemyUnit enemyUnit)
        {
            _shootTimer = SHOOT_SPEED;
            
            float distance = Math.Abs(Transformation.Position.X - enemyUnit.Transformation.Position.X);
            float bulletDuration = distance / BULLET_SPEED;

            if (_bulletSprite == null)
                _bulletSprite = ResourcesManager.GetImage("bullet.png");

            Entity entity = new Entity("Bullet");
            entity.Transformation.Position = (Vector3)ShootPosition + new Vector3(0, 0, -1);
            entity.AddComponent<SpriteRenderer>().Sprite = _bulletSprite;
            entity.AddComponent<Projectile>().Init(ShootPosition + new Vector2(distance - 5.0f / 32, 0), bulletDuration);
            entity.Destroy(bulletDuration);
            
            yield return new WaitForSeconds(bulletDuration);
            
            if (enemyUnit.Entity == null || enemyUnit.Entity.IsDestroyed())
                yield break;
            enemyUnit.TakeDamage(SHOOT_DAMAGE);
        }
    }
}