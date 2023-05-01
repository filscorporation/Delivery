using System.Collections.Generic;
using Steel;
using SteelCustom.Upgrades;

namespace SteelCustom
{
    public class MotherShip : ScriptComponent
    {
        public int DeliveryQueueWorkers { get; set; } = 1;
        public float BuildingDeliveryModifier { get; set; } = 1.0f;
        public float EffectDeliveryModifier { get; set; } = 1.0f;
        public float CreditGainModifier { get; set; } = 1.0f;

        public List<MotherShipUpgrade> Upgrades => new List<MotherShipUpgrade>(_upgrades);
        private readonly List<MotherShipUpgrade> _upgrades = new List<MotherShipUpgrade> { new ParallelDeliveryUpgrade(), new AutomatedAssemblyLineUpgrade(), new SystemsWarmUpUpgrade(), new BlackMarketUpgrade() };
    }
}