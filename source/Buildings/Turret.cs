using Steel;

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
    }
}