using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Steel;
using SteelCustom.UIElements;

namespace SteelCustom
{
    public class CreditsAnimator : ScriptComponent
    {
        private Sprite _effectSprite;
        private readonly Queue<(int Reward, Vector2 SourcePosition)> _queue = new Queue<(int reward, Vector2 sourcePosition)>();

        public override void OnUpdate()
        {
            while (_queue.Any())
            {
                (int Reward, Vector2 SourcePosition) item = _queue.Dequeue();
                AnimateInner(item.Reward, item.SourcePosition);
            }
        }

        public void Init()
        {
            _effectSprite = ResourcesManager.GetImage("ui_credits_effect.aseprite");
        }
        
        public void Animate(int reward, Vector2 sourcePosition)
        {
            _queue.Enqueue((reward, sourcePosition));
        }
        
        private void AnimateInner(int reward, Vector2 sourcePosition)
        {
            float K = GameController.Instance.UIController.K;
            
            UIImage image = UI.CreateUIImage(_effectSprite, "CreditsEffect", GameController.Instance.UIController.UIRoot);
            image.RectTransform.AnchoredPosition = Camera.Main.WorldToScreenPoint(sourcePosition + new Vector2(0, 0.4f));
            image.RectTransform.Size = new Vector2(9 * K, 6 * K);
            
            UIText text = UI.CreateUIText($"+{reward}", "Text", image.Entity);
            text.RectTransform.AnchorMin = new Vector2(0, 0);
            text.RectTransform.AnchorMax = new Vector2(1, 1);
            text.RectTransform.AnchoredPosition = Vector2.Zero;
            text.RectTransform.Size = Vector2.Zero;
            text.Color = UIController.DarkColor;
            text.TextSize = 32;
            text.TextAlignment = AlignmentType.CenterMiddle;
            
            //image.Entity.AddComponent<AudioSource>().Play(ResourcesManager.GetAudioTrack("gain_credits.wav"));

            float duration = 0.3f;
            if (reward > 10)
                duration = 1.0f;
            else if (reward > 1)
                duration = 0.6f;
            StartCoroutine(AnimateCoroutine(image.RectTransform, duration));
        }

        private IEnumerator AnimateCoroutine(RectTransformation rectTransformation, float duration)
        {
            float timer = 0.0f;
            while (timer < duration)
            {
                rectTransformation.AnchoredPosition += new Vector2(0, (duration - timer) * Time.DeltaTime * 300.0f);
                
                timer += Time.DeltaTime;
                yield return null;
            }
            
            rectTransformation.Entity.Destroy();
        }
    }
}