using SteelCustom.Buildings;

namespace SteelCustom.Effects
{
    public class Repair : Effect
    {
        public override EffectType EffectType => EffectType.Repair;
        public override int Price => 20;
        public override int DeliveryTime => 15;
        public override string SpritePath => "repair.aseprite";
        public override string Description => $"Heals all of your buildings.\nPrice: {Price}, Delivery time: {DeliveryTime}";
        public override void Apply()
        {
            foreach (Building building in GameController.Instance.BattleController.BuilderController.GetBuildings())
            {
                building.Repair();
            }
        }
    }
}