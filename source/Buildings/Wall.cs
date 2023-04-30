using Steel;

namespace SteelCustom.Buildings
{
    public class Wall : Building
    {
        public override BuildingType BuildingType => BuildingType.Wall;
        public override Vector2 ColliderSize => new Vector2(0.1f, 0.3f);
        public override int MaxHealth => 20;
        public override int Price => 10;
        public override int DeliveryTime => 1;//10; TODO
        public override string SpritePath => "wall.aseprite";
        public override string Name => "Wall";
        public override string Description => $"Stops enemies from passing.\nPrice: {Price}, Delivery time: {DeliveryTime}";
    }
}