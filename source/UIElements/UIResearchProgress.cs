using Steel;

namespace SteelCustom.UIElements
{
    public class UIResearchProgress : ScriptComponent
    {
        private UIText _progressText;

        public override void OnUpdate()
        {
            UpdateProgress();
        }

        public void Init()
        {
            _progressText = UI.CreateUIText("Research", "Time", Entity);
            _progressText.RectTransform.AnchorMin = new Vector2(0, 0);
            _progressText.RectTransform.AnchorMax = new Vector2(1, 1);
            _progressText.Color = UIController.DarkColor;
            _progressText.TextSize = 48;
            _progressText.TextAlignment = AlignmentType.BottomLeft;

            UpdateProgress();
        }

        private void UpdateProgress()
        {
            if (GameController.Instance.GameState != GameState.Battle
                || GameController.Instance.BattleController.EnemyController == null)
                return;
            
            int progress = (int)(GameController.Instance.BattleController.EnemyController.AttackCompletion * 100);
            if (GameController.Instance.BattleController.EnemyController.AttackCompleted)
                progress = 100;
            _progressText.Text = $"Research: {progress}%";
        }
    }
}