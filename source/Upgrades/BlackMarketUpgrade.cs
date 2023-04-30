namespace SteelCustom.Upgrades
{
    public class BlackMarketUpgrade : MotherShipUpgrade
    {
        public override int Price => 60;
        public override string SpritePath => "black_market.aseprite";
        public override string Description => $"Black market.\nGain 50% more credits for killing.\n{(IsSold ? "Already upgraded" : $"Price: {Price}")}";
        
        public override void Apply()
        {
            GameController.Instance.MotherShip.CreditGainModifier = 1.5f;
            IsSold = true;
        }
    }
}