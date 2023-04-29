using Steel;
using SteelCustom.Buildings;

namespace SteelCustom
{
    public class BattleController :  ScriptComponent
    {
        public BuilderController BuilderController { get; private set; }
        public EnemyController EnemyController { get; private set; }
        
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
        }
    }
}