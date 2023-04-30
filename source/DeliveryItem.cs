using Steel;
using SteelCustom.Buildings;
using SteelCustom.Effects;

namespace SteelCustom
{
    public class DeliveryItem
    {
        public bool IsBuilding => Effect == null;
        
        public BuildingType BuildingType { get; set; }
        public Vector2 Position { get; set; }
        public Effect Effect { get; set; }
        public float TimeLeft { get; set; }
        public string SpritePath { get; set; }
    }
}