using System;
using Steel;
using SteelCustom.Buildings;
using SteelCustom.Effects;
using SteelCustom.Upgrades;

namespace SteelCustom
{
    public class Player : ScriptComponent
    {
        public event Action OnCreditsChanged;

        public int Credits { get; private set; } = 500; // TODO: remove

        public bool ResearchStationPlaced { get; set; } = false;
        public bool FirstTowerOrdered { get; set; } = false;

        private CreditsAnimator _creditsAnimator;

        public void Init()
        {
            _creditsAnimator = new Entity("CreditsAnimator", Entity).AddComponent<CreditsAnimator>();
            _creditsAnimator.Init();
        }

        public void GainCredits(int reward, Vector2 sourcePosition)
        {
            Credits += reward;
            OnCreditsChanged?.Invoke();

            _creditsAnimator.Animate(reward, sourcePosition);
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

        public bool CanBuyMotherShipUpgrade(MotherShipUpgrade upgrade)
        {
            return !upgrade.IsSold && Credits >= upgrade.Price;
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
            
            GameController.Instance.BattleController.BuilderController.StartPlacingEffect(effect);

            return true;
        }

        public bool TryBuyMotherShipUpgrade(MotherShipUpgrade upgrade)
        {
            if (!CanBuyMotherShipUpgrade(upgrade))
                return false;

            SpendCredits(upgrade.Price);
            
            upgrade.Apply();

            return true;
        }
    }
}