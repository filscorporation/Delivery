namespace SteelCustom.Upgrades
{
    public abstract class MotherShipUpgrade
    {
        public abstract int Price { get; }
        public abstract string SpritePath { get; }
        public abstract string Description { get; }
        public bool IsSold { get; protected set; }
        public abstract void Apply();
    }
}