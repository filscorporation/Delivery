﻿namespace SteelCustom.Effects
{
    public class Rocket : Effect
    {
        public override EffectType EffectType => EffectType.Rocket;
        public override int Price => 10;
        public override int DeliveryTime => 8;
        public override string SpritePath => "rocket.aseprite";
        public override string Description => $"Deal massive damage in the small area.\nPrice: {Price}, Delivery time: {DeliveryTime}";
        public override void Apply()
        {
            
        }
    }
}