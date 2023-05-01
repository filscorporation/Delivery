using System.Collections;
using System.Collections.Generic;
using Steel;
using SteelCustom.Enemies;

namespace SteelCustom.Buildings
{
    public class MineThrower : Building
    {
        public override BuildingType BuildingType => BuildingType.MineThrower;
        public override int MaxHealth => 5;
        public override int Price => 30;
        public override int DeliveryTime => 20;
        public override string SpritePath => "mine_thrower.aseprite";
        public override string Name => "Mine thrower";
        public override string Description => $"Deals area damage.\nPrice: {Price}, Delivery time: {DeliveryTime}";

        private Vector2 ExplosionPosition => (Vector2)Transformation.Position + new Vector2(34f / 32, 0f / 32);

        private float _shootTimer = 0;
        private AsepriteData _explosionSprite;

        private const float SHOOT_SPEED = 6.0f;
        private const int EXPLOSION_DAMAGE = 3;
        private const float EXPLOSION_RANGE = 0.4f;
        private const float EXPLOSION_DELAY = 1.1f;

        protected override void UpdateBuilding()
        {
            UpdateShoot();
        }

        private void UpdateShoot()
        {
            _shootTimer -= Time.DeltaTime;
            if (_shootTimer <= 0)
            {
                StartCoroutine(Shoot());
            }
        }

        private IEnumerator Shoot()
        {
            _shootTimer = SHOOT_SPEED;

            if (_explosionSprite == null)
                _explosionSprite = ResourcesManager.GetAsepriteData("mine_thrower_attack.aseprite");

            Entity entity = _explosionSprite.CreateEntityFromAsepriteData();
            entity.Transformation.Position = Transformation.Position + new Vector3(17f / 32, 12f / 32, 2);
            entity.GetComponent<Animator>().Play("Effect");
            entity.Destroy(1.6f);
            
            yield return new WaitForSeconds(EXPLOSION_DELAY);
            
            List<EnemyUnit> enemyUnits = new List<EnemyUnit>();
            foreach (Entity hitEntity in Physics.AABBCast(ExplosionPosition, new Vector2(EXPLOSION_RANGE, EXPLOSION_RANGE)))
            {
                EnemyUnit enemyUnit = EnemyUnit.GetEnemyUnit(hitEntity);
                if (enemyUnit != null)
                    enemyUnits.Add(enemyUnit);
            }

            foreach (EnemyUnit enemyUnit in enemyUnits)
            {
                if (enemyUnit.Entity == null || enemyUnit.Entity.IsDestroyed())
                    yield break;
                enemyUnit.TakeDamage(EXPLOSION_DAMAGE);
            }
            
            entity.AddComponent<AudioSource>().Play(ResourcesManager.GetAudioTrack("mine_explosion.wav"));
        }
    }
}