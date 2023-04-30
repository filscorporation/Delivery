using System;
using Steel;
using SteelCustom.Buildings;
using SteelCustom.Effects;

namespace SteelCustom
{
    public class Player :ScriptComponent
    {
        public event Action OnCreditsChanged;

        public int Credits { get; private set; } = 500; // TODO: remove

        public bool ResearchStationPlaced { get; set; } = false;
        public bool FirstTowerOrdered { get; set; } = false;

        public void GainCredits(int reward, Vector2 sourcePosition)
        {
            Credits += reward;
            OnCreditsChanged?.Invoke();
            
            // TODO: animate sourcePosition
        }
        
        public void SpendCredits(int amount)
        {
            Credits -= amount;
            OnCreditsChanged?.Invoke();
            
            if (Credits < 0)
                Log.LogError($"Credits less than zero: {Credits} (-{amount})");
        }

        public bool CanOrderBuilding(Building building)
        {
            return Credits >= building.Price;
        }

        public bool CanOrderEffect(Effect effect)
        {
            return Credits >= effect.Price;
        }

        public bool TryOrderBuilding(Building building)
        {
            if (!CanOrderBuilding(building))
                return false;

            SpendCredits(building.Price);
            GameController.Instance.BattleController.BuilderController.StartPlacingBuilding(building.BuildingType);

            return true;
        }

        public bool TryOrderEffect(Effect effect)
        {
            if (!CanOrderEffect(effect))
                return false;

            SpendCredits(effect.Price);
            GameController.Instance.BattleController.BuilderController.StartPlacingEffect(effect.EffectType);

            return true;
        }
    }
}