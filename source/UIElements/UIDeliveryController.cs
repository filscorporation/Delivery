using System.Collections.Generic;
using Steel;

namespace SteelCustom.UIElements
{
    public class UIDeliveryController : ScriptComponent
    {
        private readonly List<UIDeliveryQueueItem> _queueItems = new List<UIDeliveryQueueItem>();

        public void Init()
        {
            GameController.Instance.DeliveryController.OnItemAdded += OnDeliveryItemAdded;
            GameController.Instance.DeliveryController.OnItemRemoved += OnDeliveryItemRemoved;
        }

        public void Dispose()
        {
            GameController.Instance.DeliveryController.OnItemAdded -= OnDeliveryItemAdded;
            GameController.Instance.DeliveryController.OnItemRemoved -= OnDeliveryItemRemoved;
        }

        private void OnDeliveryItemAdded(DeliveryItem newItem)
        {
            float K = GameController.Instance.UIController.K;
            
            UIImage itemImage = UI.CreateUIImage(ResourcesManager.GetImage("ui_frame.png"), "QueueItem", Entity);
            itemImage.RectTransform.AnchorMin = new Vector2(0, 1);
            itemImage.RectTransform.AnchorMax = new Vector2(0, 1);
            itemImage.RectTransform.Pivot = new Vector2(0, 1);
            itemImage.RectTransform.Size = new Vector2(36 * K, 14 * K);
            itemImage.RectTransform.AnchoredPosition = new Vector2(-36 * K, -2 * K - _queueItems.Count * (14 + 1) * K);

            var queueItem = itemImage.Entity.AddComponent<UIDeliveryQueueItem>();
            queueItem.SetTargetPosition(new Vector2(2 * K, -2 * K - _queueItems.Count * (14 + 1) * K));
            queueItem.Init(newItem);
            
            _queueItems.Add(queueItem);
        }

        private void OnDeliveryItemRemoved(DeliveryItem removedItem)
        {
            float K = GameController.Instance.UIController.K;
            
            int index = _queueItems.FindIndex(item => item.DeliveryItem == removedItem);
            _queueItems[index].SetTargetPosition(new Vector2(-40 * K, -2 * K - index * (14 + 1) * K));
            _queueItems[index].Entity.Destroy(3.0f);
            _queueItems.RemoveAt(index);

            for (int i = 0; i < _queueItems.Count; i++)
            {
                _queueItems[i].SetTargetPosition(new Vector2(2 * K, -2 * K - i * (14 + 1) * K));
            }
        }
    }
}