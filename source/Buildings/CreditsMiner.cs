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
    }
}