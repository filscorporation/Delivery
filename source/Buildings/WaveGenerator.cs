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
    }
}