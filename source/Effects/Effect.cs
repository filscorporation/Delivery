namespace SteelCustom.Effects
{
    public abstract class Effect
    {
        public abstract EffectType EffectType { get; }
        public abstract int Price { get; }
        public abstract int DeliveryTime { get; }
        public abstract string SpritePath { get; }
        public abstract string Description { get; }
        public abstract void Apply();
    }
}