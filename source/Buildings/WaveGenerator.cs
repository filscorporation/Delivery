using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Steel;
using SteelCustom.Enemies;

namespace SteelCustom.Buildings
{
    public class WaveGenerator : Building
    {
        public override BuildingType BuildingType => BuildingType.WaveGenerator;
        public override int MaxHealth => 5;
        public override int Price => 20;
        public override int DeliveryTime => 15;
        public override string SpritePath => "wave_generator.aseprite";
        public override string Name => "Wave generator";
        public override string Description => $"Generates force waves to push enemies from your base.\nPrice: {Price}, Delivery time: {DeliveryTime}";

        private Vector2 ShootPosition => (Vector2)Transformation.Position + new Vector2(3f / 32, 6f / 32);

        private float _shootTimer = 0;
        private Sprite _effectSprite;

        private const float SHOOT_SPEED = 5.0f;
        private const int SHOOT_FORCE = 5;
        private const int SHOOT_MAX_ENEMIES = 3;
        private const float SHOOT_RANGE = 2;

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
                    StartCoroutine(Shoot(enemyUnits.OrderBy(u => u.Transformation.Position.X).Take(SHOOT_MAX_ENEMIES).ToList()));
            }
        }

        private IEnumerator Shoot(List<EnemyUnit> enemyUnits)
        {
            _shootTimer = SHOOT_SPEED;

            if (_effectSprite == null)
                _effectSprite = ResourcesManager.GetImage("bullet.png");

            /*Entity entity = new Entity("Bullet");
            entity.Transformation.Position = (Vector3)ShootPosition + new Vector3(0, 0, -1);
            entity.AddComponent<SpriteRenderer>().Sprite = _bulletSprite;
            entity.AddComponent<Projectile>().Init(ShootPosition + new Vector2(distance - 5.0f / 32, 0), bulletDuration);
            entity.Destroy(bulletDuration);*/

            foreach (EnemyUnit enemyUnit in enemyUnits)
            {
                enemyUnit.AddKnockBack(SHOOT_FORCE);
            }
            
            yield return null;
        }
    }
}