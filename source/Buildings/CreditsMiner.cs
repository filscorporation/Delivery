using Steel;

namespace SteelCustom.Buildings
{
    public class CreditsMiner : Building
    {
        public override BuildingType BuildingType => BuildingType.CreditsMiner;
        public override int MaxHealth => 5;
        public override int Price => 10;
        public override int DeliveryTime => 20;
        public override string SpritePath => "miner.aseprite";
        public override string Name => "Credits miner";
        public override string Description => $"Slowly mines credits.\nPrice: {Price}, Delivery time: {DeliveryTime}";

        private float _miningTimer = MINING_SPEED;

        private const float MINING_SPEED = 5.0f;
        private const int MINING_GAIN = 1;

        private void UpdateMining()
        {
            _miningTimer -= Time.DeltaTime;
            if (_miningTimer <= 0)
            {
                _miningTimer = MINING_SPEED;
                
                GameController.Instance.Player.GainCredits(MINING_GAIN, Transformation.Position);
            }
        }

        protected override void UpdateBuilding()
        {
            UpdateMining();
        }
    }
}