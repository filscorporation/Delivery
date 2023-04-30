using Steel;

namespace SteelCustom.UIElements
{
    public class UIDialog : ScriptComponent
    {
        private UIText _text;
        private string _targetText;
        private int _currentLength;
        private float _textTimer;

        private const float LETTER_DELAY = 0.025f;

        public override void OnUpdate()
        {
            UpdateText();
        }

        public void Init()
        {
            float K = GameController.Instance.UIController.K;
            
            _text = UI.CreateUIText("", "Text", Entity);
            _text.RectTransform.AnchorMin = new Vector2(0, 0);
            _text.RectTransform.AnchorMax = new Vector2(1, 1);
            _text.RectTransform.OffsetMin = new Vector2(26 * K + 2 * K, 2 * K);
            _text.RectTransform.OffsetMax = new Vector2(2 * K, 2 * K);
            _text.Color = UIController.DarkColor;
            _text.TextOverflowMode = OverflowMode.WrapByWords;
            _text.TextAlignment = AlignmentType.TopLeft;
            _text.TextSize = 32;
        }

        public void SetText(string targetText)
        {
            _text.Text = "";
            _targetText = targetText;
            _currentLength = 0;
            _textTimer = 0.0f;
        }
        
        public void Skip()
        {
            _text.Text = _targetText;
            _currentLength = _targetText.Length;
            _textTimer = 0.0f;
        }

        private void UpdateText()
        {
            if (_currentLength >= _targetText.Length)
                return;

            if (_textTimer > 0.0f)
            {
                _textTimer -= Time.DeltaTime;
                return;
            }

            _textTimer = LETTER_DELAY;
            _currentLength++;
            _text.Text = _targetText.Substring(0, _currentLength);
        }
    }
}