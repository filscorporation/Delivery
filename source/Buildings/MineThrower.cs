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
    }
}