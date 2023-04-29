namespace SteelCustom.Buildings
{
    public class RocketLauncher : Building
    {
        public override int MaxHealth => 5;
        public override int Price => 30;
        public override int DeliveryTime => 20;
        public override string SpritePath => "rocket_launcher.aseprite";
        public override string Name => "Rocket launcher";
        public override string Description => "";
    }
}