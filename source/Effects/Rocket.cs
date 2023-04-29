namespace SteelCustom.Effects
{
    public class Rocket : Effect
    {
        public override EffectType EffectType => EffectType.Rocket;
        public override int Price => 10;
        public override int DeliveryTime => 8;
        public override string SpritePath => "rocket.aseprite";
        public override string Description => $"Deals massive damage at the small area.\nPrice: {Price}, Delivery time: {DeliveryTime}";
    }
}