using Steel;
using SteelCustom.Enemies;

namespace SteelCustom.Buildings
{
    public class ResearchStation : Building
    {
        protected override Vector2 ColliderSize => new Vector2(0.9f, 0.9f);
        
        public override int MaxHealth => 30;
        public override int Price => int.MaxValue;
        public override int DeliveryTime => 2;
        public override string SpritePath => "research_station.aseprite";
        public override string Name => "Research station";
        public override string Description => "";

        private const int COUNTER_DAMAGE = 1;

        protected override void OnPlaced()
        {
            GameController.Instance.Player.ResearchStationPlaced = true;
            GameController.Instance.Player.GainCredits(15, Transformation.Position);
        }

        protected override void OnTakeDamage(EnemyUnit attacker)
        {
            attacker.TakeDamage(COUNTER_DAMAGE);
        }

        protected override void OnBuildingDestroyed()
        {
            GameController.Instance.LoseGame();
        }
    }
}