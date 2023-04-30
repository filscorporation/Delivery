using Steel;

namespace SteelCustom.UIElements
{
    public class UIDeliveryQueueItem : ScriptComponent
    {
        public DeliveryItem DeliveryItem { get; private set; }

        private UIText _timeText;
        private RectTransformation _rectTransformation;
        private Vector2? _targetPosition;

        public override void OnUpdate()
        {
            if (DeliveryItem != null)
            {
                _timeText.Text = Math.Ceiling(DeliveryItem.TimeLeft).ToString();
            }

            if (_targetPosition.HasValue)
            {
                if (Vector2.Distance(_rectTransformation.AnchoredPosition, _targetPosition.Value) < 2)
                {
                    _rectTransformation.AnchoredPosition = _targetPosition.Value;
                    _targetPosition = null;
                }
                else
                    _rectTransformation.AnchoredPosition = Math.Lerp(_rectTransformation.AnchoredPosition, _targetPosition.Value, Time.DeltaTime * 3);
            }
        }

        public void Init(DeliveryItem deliveryItem)
        {
            DeliveryItem = deliveryItem;
            _rectTransformation = GetComponent<RectTransformation>();
            
            float K = GameController.Instance.UIController.K;
            
            Sprite sprite = ResourcesManager.GetImage(deliveryItem.SpritePath);
            sprite.Pivot = new Vector2(0.5f, 0);
            UIImage icon = UI.CreateUIImage(sprite, "ItemIcon", Entity);
            icon.RectTransform.AnchorMin = new Vector2(0, 0);
            icon.RectTransform.AnchorMax = new Vector2(0, 0);
            icon.RectTransform.Pivot = new Vector2(0, 0);
            icon.RectTransform.Size = new Vector2(12 * K, 12 * K);
            icon.RectTransform.AnchoredPosition = new Vector2(1 * K, 1 * K - icon.RectTransform.Size.Y * 0.5f);
            icon.ConsumeEvents = false;
            
            _timeText = UI.CreateUIText(Math.Ceiling(DeliveryItem.TimeLeft).ToString(), "Time", Entity);
            _timeText.RectTransform.AnchorMin = new Vector2(0, 0);
            _timeText.RectTransform.AnchorMax = new Vector2(1, 0);
            _timeText.RectTransform.Pivot = new Vector2(0, 0);
            _timeText.RectTransform.Size = new Vector2(0, 12 * K);
            _timeText.RectTransform.AnchoredPosition = new Vector2(0, 1 * K);
            _timeText.RectTransform.OffsetMin = new Vector2(12 * K + 2 * K, 0);
            _timeText.RectTransform.OffsetMax = new Vector2(2 * K, 0);
            _timeText.Color = UIController.DarkColor;
            _timeText.TextSize = 64;
            _timeText.TextAlignment = AlignmentType.CenterRight;
        }

        public void SetTargetPosition(Vector2 targetPosition)
        {
            _targetPosition = targetPosition;
        }
    }
}