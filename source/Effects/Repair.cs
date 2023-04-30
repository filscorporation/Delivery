using SteelCustom.Buildings;

namespace SteelCustom.Effects
{
    public class Repair : Effect
    {
        public override EffectType EffectType => EffectType.Repair;
        public override bool NeedTarget => false;
        public override int Price => 20;
        public override int DeliveryTime => 15;
        public override string SpritePath => "repair.aseprite";
        public override string Description => $"Heal all of your buildings and increase their health.\nPrice: {Price}, Delivery time: {DeliveryTime}";

        private const int HEALTH_BOOST = 5;
        
        public override void Apply()
        {
            foreach (Building building in GameController.Instance.BattleController.BuilderController.GetBuildings())
            {
                building.Repair(HEALTH_BOOST);
            }
        }
    }
}