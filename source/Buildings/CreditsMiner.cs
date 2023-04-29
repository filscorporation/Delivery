namespace SteelCustom.Buildings
{
    public class CreditsMiner : Building
    {
        public override int MaxHealth => 5;
        public override int Price => 10;
        public override int DeliveryTime => 20;
        public override string SpritePath => "credits_miner.aseprite";
        public override string Name => "Credits miner";
        public override string Description => "";
    }
}