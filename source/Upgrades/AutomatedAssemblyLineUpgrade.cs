namespace SteelCustom.Upgrades
{
    public class AutomatedAssemblyLineUpgrade : MotherShipUpgrade
    {
        public override int Price => 30;
        public override string SpritePath => "automated_assembly_line.aseprite";
        public override string Description => $"Automated assembly line.\nBuildings delivery is 33% faster.\n{(IsSold ? "Already upgraded" : $"Price: {Price}")}";
        
        public override void Apply()
        {
            GameController.Instance.MotherShip.BuildingDeliveryModifier = 0.67f;
            IsSold = true;
        }
    }
}