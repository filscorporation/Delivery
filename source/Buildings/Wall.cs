namespace SteelCustom.Buildings
{
    public class Wall : Building
    {
        public override int MaxHealth => 20;
        public override int Price => 10;
        public override int DeliveryTime => 10;
        public override string SpritePath => "wall.aseprite";
        public override string Name => "Wall";
        public override string Description => "";
    }
}