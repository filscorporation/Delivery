using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Steel;
using SteelCustom.UIElements;

namespace SteelCustom
{
    public class DamageAnimator : ScriptComponent
    {
        private readonly Queue<(int Damage, Vector2 SourcePosition, bool IsEnemy)> _queue = new Queue<(int Damage, Vector2 SourcePosition, bool IsEnemy)>();

        public override void OnUpdate()
        {
            while (_queue.Any())
            {
                (int Damage, Vector2 SourcePosition, bool IsEnemy) item = _queue.Dequeue();
                AnimateInner(item.Damage, item.SourcePosition, item.IsEnemy);
            }
        }
        
        public void Animate(int damage, Vector2 sourcePosition, bool isEnemy)
        {
            _queue.Enqueue((-damage, sourcePosition, isEnemy));
        }
        
        public void AnimateRepair(int repair, Vector2 sourcePosition)
        {
            _queue.Enqueue((repair, sourcePosition, false));
        }
        
        private void AnimateInner(int damage, Vector2 sourcePosition, bool isEnemy)
        {
            float K = GameController.Instance.UIController.K;
            bool isRepair = damage > 0;
            
            RectTransformation rectTransform = UI.CreateUIElement("DamageEffect", GameController.Instance.UIController.UIRoot).GetComponent<RectTransformation>();
            rectTransform.AnchoredPosition = Camera.Main.WorldToScreenPoint(sourcePosition + new Vector2(0, 0.45f));
            rectTransform.Size = new Vector2(9 * K, 6 * K);
            
            UIText text = UI.CreateUIText(isRepair ? $"+{damage}" : $"-{-damage}", "Text", rectTransform.Entity);
            text.RectTransform.AnchorMin = new Vector2(0, 0);
            text.RectTransform.AnchorMax = new Vector2(1, 1);
            text.RectTransform.AnchoredPosition = Vector2.Zero;
            text.RectTransform.Size = Vector2.Zero;
            text.Color = isRepair || isEnemy ? UIController.DarkColor : UIController.RedColor;
            text.TextSize = 32;
            text.TextAlignment = AlignmentType.CenterMiddle;
            
            //rectTransform.Entity.AddComponent<AudioSource>().Play(ResourcesManager.GetAudioTrack("damage.wav"));

            float duration = 0.3f;
            StartCoroutine(AnimateCoroutine(rectTransform, duration));
        }

        private IEnumerator AnimateCoroutine(RectTransformation rectTransformation, float duration)
        {
            float timer = 0.0f;
            float xOffset = Random.NextFloat(-50, 50);
            while (timer < duration)
            {
                rectTransformation.AnchoredPosition += new Vector2(xOffset * Time.DeltaTime, (duration - timer) * Time.DeltaTime * 300.0f);
                
                timer += Time.DeltaTime;
                yield return null;
            }
            
            rectTransformation.Entity.Destroy();
        }
    }
}