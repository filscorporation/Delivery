using Steel;
using SteelCustom.Buildings;
using SteelCustom.Enemies;

namespace SteelCustom
{
    public class BattleController :  ScriptComponent
    {
        public BuilderController BuilderController { get; private set; }
        public EnemyController EnemyController { get; private set; }

        public override void OnUpdate()
        {
            if (GameController.Instance.GameState == GameState.Battle)
            {
                if (EnemyController.AttackCompleted)
                    GameController.Instance.WinGame();
            }
            
            // TODO: remove cheat
            if (Input.IsKeyJustPressed(KeyCode.Q))
            {
                foreach (EnemyUnit enemyUnit in EnemyController.Enemies)
                {
                    enemyUnit.TakeDamage(999);
                }
            }
        }

        public void Init()
        {
            BuilderController = new Entity("BuilderController").AddComponent<BuilderController>();
        }

        public void PlaceResearchStation()
        {
            BuilderController.SetPlacingConstraints(-4.5f, 0.5f);
            BuilderController.StartPlacingBuilding(BuildingType.ResearchStation);
        }

        public void EndPlaceResearchStation()
        {
            BuilderController.ClearPlacingConstraints();
        }

        public void StartBattle()
        {
            EnemyController = new Entity("EnemyController").AddComponent<EnemyController>();
            EnemyController.Init();
        }
    }
}