using Steel;

namespace SteelCustom.Buildings
{
    public class Turret : Building
    {
        protected override Vector2 ColliderSize => new Vector2(0.3f, 0.3f);
        public override int MaxHealth => 5;
        public override int Price => 10;
        public override int DeliveryTime => 10;
        public override string SpritePath => "turret.aseprite";
        public override string Name => "Turret";
        public override string Description => "";
    }
}