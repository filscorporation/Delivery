using System;
using Steel;

namespace SteelCustom.Buildings
{
    public abstract class Building : ScriptComponent
    {
        public bool IsPlaced { get; private set; } = false;
        public BuildingType BuildingType { get; private set; }
        
        public void Init(BuildingType buildingType)
        {
            BuildingType = buildingType;
            
            Entity.AddComponent<SpriteRenderer>().Sprite = ResourcesManager.GetImage(BuildingTypeToSpritePath(BuildingType));
        }

        public void Place()
        {
            IsPlaced = true;

            OnPlaced();
        }

        public void SetDraftState(bool checkDraft)
        {
            
        }
        
        protected virtual void OnPlaced() { }

        private static string BuildingTypeToSpritePath(BuildingType buildingType)
        {
            switch (buildingType)
            {
                case BuildingType.ResearchStation:
                    return "research_station.aseprite";
                case BuildingType.Turret:
                    return "turret.aseprite";
                case BuildingType.Wall:
                    return "wall.aseprite";
                case BuildingType.RocketLauncher:
                    return "rocket_launcher.aseprite";
                case BuildingType.WaveGenerator:
                    return "wave_generator.aseprite";
                case BuildingType.CreditsMiner:
                    return "credits_miner.aseprite";
                case BuildingType.MineThrower:
                    return "mine_thrower.aseprite";
                default:
                    throw new ArgumentOutOfRangeException(nameof(buildingType), buildingType, null);
            }
        }
    }
}