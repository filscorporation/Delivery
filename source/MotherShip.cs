using Steel;

namespace SteelCustom
{
    public class MotherShip : ScriptComponent
    {
        public int DeliveryQueueWorkers { get; set; } = 2; //1
        public float BuildingDeliveryModifier { get; set; } = 0.1f; //1.0f;
        public float EffectDeliveryModifier { get; set; } = 0.1f; //1.0f;
        public float CreditGainModifier { get; set; } = 1.0f;
    }
}