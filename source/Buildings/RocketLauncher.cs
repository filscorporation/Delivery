using System.Collections.Generic;
using System.Linq;
using Steel;
using SteelCustom.Enemies;

namespace SteelCustom.Buildings
{
    public class RocketLauncher : Building
    {
        public override BuildingType BuildingType => BuildingType.RocketLauncher;
        public override int MaxHealth => 5;
        public override int Price => 30;
        public override int DeliveryTime => 20;
        public override string SpritePath => "rocket_launcher.aseprite";
        public override string Name => "Rocket launcher";
        public override string Description => $"Fires rockets at high range, can hit flying enemies.\nPrice: {Price}, Delivery time: {DeliveryTime}";

        private Vector2 ShootPosition => (Vector2)Transformation.Position + new Vector2(0, 8f / 32);

        private float _shootTimer = 0;
        private float _searchTimer = 0;
        private Sprite _rocketSprite;

        private const float SHOOT_SPEED = 6.0f;
        private const int ROCKET_DAMAGE = 5;
        private const float SHOOT_RANGE = 4.0f;

        protected override void UpdateBuilding()
        {
            UpdateShoot();
        }

        private void UpdateShoot()
        {
            _shootTimer -= Time.DeltaTime;
            _searchTimer -= Time.DeltaTime;
            if (_shootTimer <= 0 && _searchTimer <= 0)
            {
                EnemyUnit target = FindTarget();
                if (target != null)
                    Shoot(target);
            }
        }

        private EnemyUnit FindTarget()
        {
            _searchTimer = 1.0f;
            
            List<Flying> flyingUnits = FindAllOfType<Flying>().OrderBy(u => u.Transformation.Position.X).ToList();
            if (flyingUnits.Any())
            {
                return flyingUnits.First();
            }
            
            List<Tank> tankUnits = FindAllOfType<Tank>().OrderBy(u => u.Transformation.Position.X).ToList();
            if (tankUnits.Any())
            {
                return tankUnits.First();
            }
            
            List<Soldier> soldierUnits = FindAllOfType<Soldier>().OrderBy(u => u.Transformation.Position.X).ToList();
            if (soldierUnits.Any())
            {
                return soldierUnits.First();
            }
            
            List<Runner> runnerUnits = FindAllOfType<Runner>().OrderBy(u => u.Transformation.Position.X).ToList();
            if (runnerUnits.Any())
            {
                return runnerUnits.First();
            }
            
            return null;
        }

        private void Shoot(EnemyUnit enemyUnit)
        {
            _shootTimer = SHOOT_SPEED;

            if (_rocketSprite == null)
                _rocketSprite = ResourcesManager.GetImage("small_rocket.png");

            Entity entity = new Entity("SmallRocket");
            entity.Transformation.Position = (Vector3)ShootPosition + new Vector3(0, 0, -1);
            entity.AddComponent<SpriteRenderer>().Sprite = _rocketSprite;
            entity.AddComponent<SmallRocket>().Init(enemyUnit, ROCKET_DAMAGE);
        }
    }
}