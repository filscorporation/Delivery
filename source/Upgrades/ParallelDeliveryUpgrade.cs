namespace SteelCustom.Upgrades
{
    public class ParallelDeliveryUpgrade : MotherShipUpgrade
    {
        public override int Price => 50;
        public override string SpritePath => "parallel_delivery.aseprite";
        public override string Description => $"Parallel delivery.\nIncrease max amount of simultaneous delivery by 1.\n{(IsSold ? "Already upgraded" : $"Price: {Price}")}";
        
        public override void Apply()
        {
            GameController.Instance.MotherShip.DeliveryQueueWorkers = 2;
            IsSold = true;
        }
    }
}