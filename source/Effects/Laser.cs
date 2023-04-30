namespace SteelCustom.Effects
{
    public class Laser : Effect
    {
        public override EffectType EffectType => EffectType.Laser;
        public override int Price => 10;
        public override int DeliveryTime => 4;
        public override string SpritePath => "laser.aseprite";
        public override string Description => $"Deal damage in a big area.\nPrice: {Price}, Delivery time: {DeliveryTime}";
        public override void Apply()
        {
            
        }
    }
}