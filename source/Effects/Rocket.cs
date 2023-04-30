using Steel;

namespace SteelCustom.Effects
{
    public class Rocket : Effect
    {
        public override EffectType EffectType => EffectType.Rocket;
        public override bool NeedTarget => true;
        public override int Price => 10;
        public override int DeliveryTime => 8;
        public override string SpritePath => "rocket.aseprite";
        public override string Description => $"Deal massive damage in the small area.\nPrice: {Price}, Delivery time: {DeliveryTime}";
        
        private const int DAMAGE = 10;
        private const float RANGE = 0.5f;
        
        public override void Apply()
        {
            BigRocket bigRocket = new Entity("BigRocket").AddComponent<BigRocket>();
            bigRocket.Init(Target, DAMAGE, RANGE);
        }
    }
}