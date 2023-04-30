namespace SteelCustom.Upgrades
{
    public class SystemsWarmUpUpgrade : MotherShipUpgrade
    {
        public override int Price => 40;
        public override string SpritePath => "systems_warm_up.aseprite";
        public override string Description => $"Systems warm up.\nDouble ship attacks and repair delivery speed.\n{(IsSold ? "Already upgraded" : $"Price: {Price}")}";
        
        public override void Apply()
        {
            GameController.Instance.MotherShip.EffectDeliveryModifier = 0.5f;
            IsSold = true;
        }
    }
}