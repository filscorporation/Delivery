using System;
using System.Collections.Generic;
using System.Linq;
using Steel;
using SteelCustom.Buildings;
using SteelCustom.Effects;

namespace SteelCustom
{
    public class DeliveryController : ScriptComponent
    {
        public event Action<DeliveryItem> OnItemAdded;
        public event Action<DeliveryItem> OnItemRemoved;

        private readonly LinkedList<DeliveryItem> _deliveryQueue = new LinkedList<DeliveryItem>();

        public override void OnUpdate()
        {
            if (GameController.Instance.GameState != GameState.Intro
                && GameController.Instance.GameState != GameState.Win
                && GameController.Instance.GameState != GameState.Lose)
                UpdateQueue();
        }

        public void Init()
        {
        }

        public void AddItem(Building building, Vector2 position)
        {
            float modificator = GameController.Instance.MotherShip.BuildingDeliveryModifier;
            if (GameController.Instance.Player.ResearchStationPlaced)
                GameController.Instance.Player.FirstTowerOrdered = true;
            
            var item = new DeliveryItem { BuildingType = building.BuildingType, Position = position, TimeLeft = building.DeliveryTime * modificator, SpritePath = building.SpritePath };
            _deliveryQueue.AddLast(item);
            
            OnItemAdded?.Invoke(item);
        }

        public void AddItem(Effect effect, Vector2 position)
        {
            float modificator = GameController.Instance.MotherShip.EffectDeliveryModifier;
            var item = new DeliveryItem { Effect = effect, Position = position, TimeLeft = effect.DeliveryTime * modificator, SpritePath = effect.SpritePath };
            _deliveryQueue.AddLast(item);
            
            OnItemAdded?.Invoke(item);
        }

        public void OpenForFirstOrder()
        {
            GameController.Instance.UIController.OpenOrdersShop();
        }

        public List<DeliveryItem> GetDeliveryItems()
        {
            return _deliveryQueue.ToList();
        }

        private void UpdateQueue()
        {
            if (!_deliveryQueue.Any())
                return;

            LinkedListNode<DeliveryItem> node = _deliveryQueue.First;
            for (int i = 0; i < GameController.Instance.MotherShip.DeliveryQueueWorkers; i++)
            {
                node.Value.TimeLeft -= Time.DeltaTime;
                if (node.Value.TimeLeft <= 0)
                {
                    LinkedListNode<DeliveryItem> temp = node.Next;
                    Apply(node.Value);
                    _deliveryQueue.Remove(node);
                    node = temp;
                }
                else
                    node = node.Next;
                
                if (node == null)
                    break;
            }
        }

        private void Apply(DeliveryItem item)
        {
            if (item.IsBuilding)
            {
                var buildingDeliveryItem = new Entity("BuildingDeliveryItem").AddComponent<BuildingDeliveryItem>();
                buildingDeliveryItem.Init(item);
            }
            else
            {
                item.Effect.Apply();
            }
            
            OnItemRemoved?.Invoke(item);
        }
    }
}