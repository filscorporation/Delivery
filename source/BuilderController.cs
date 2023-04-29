using System;
using Steel;
using SteelCustom.Buildings;

namespace SteelCustom
{
    public class BuilderController : ScriptComponent
    {
        public Building DraftBuilding { get; private set; }
        public bool IsInBuildMode => DraftBuilding != null;

        public const float GROUND_HEIGHT = -0.75f;

        private bool _placingConstraints;
        private float _placingPosition;
        private float _placingRange;

        public override void OnUpdate()
        {
            if (IsInBuildMode)
            {
                UpdateDraft();
            }
        }

        public void StartPlacingBuilding(BuildingType buildingType)
        {
            ClearDraft();

            DraftBuilding = CreateBuilding(buildingType);
            DraftBuilding.Init(buildingType);
            UpdateDraft();
        }

        public void ClearDraft()
        {
            if (DraftBuilding == null)
                return;
            
            DraftBuilding.Entity.Destroy();
            DraftBuilding = null;
        }

        public void SetPlacingConstraints(float position, float range)
        {
            _placingConstraints = true;
            _placingPosition = position;
            _placingRange = range;
        }

        public void ClearPlacingConstraints()
        {
            _placingConstraints = false;
        }

        private void UpdateDraft()
        {
            Vector2 position = Camera.Main.ScreenToWorldPoint(Input.MousePosition).SetY(GROUND_HEIGHT);
            DraftBuilding.Transformation.Position = position;

            bool checkDraft = CheckDraft();
            DraftBuilding.SetDraftState(checkDraft);

            if (Input.IsMouseJustPressed(MouseCodes.ButtonLeft))
            {
                if (checkDraft)
                    PlaceDraft();
            }
        }

        private bool CheckDraft()
        {
            if (_placingConstraints)
            {
                float x = DraftBuilding.Transformation.Position.X;
                if (x < _placingPosition - _placingRange || x > _placingPosition + _placingRange)
                    return false;
            }
            
            return true;
        }

        private void PlaceDraft()
        {
            DraftBuilding.Place();
            DraftBuilding = null;
        }

        private Building CreateBuilding(BuildingType buildingType)
        {
            Entity entity = new Entity(buildingType.ToString(), Entity);
            switch (buildingType)
            {
                case BuildingType.ResearchStation:
                    return entity.AddComponent<ResearchStation>();
                case BuildingType.Turret:
                    return entity.AddComponent<Turret>();
                case BuildingType.Wall:
                    return entity.AddComponent<Wall>();
                case BuildingType.RocketLauncher:
                    return entity.AddComponent<RocketLauncher>();
                case BuildingType.WaveGenerator:
                    return entity.AddComponent<WaveGenerator>();
                case BuildingType.CreditsMiner:
                    return entity.AddComponent<CreditsMiner>();
                case BuildingType.MineThrower:
                    return entity.AddComponent<MineThrower>();
                default:
                    throw new ArgumentOutOfRangeException(nameof(buildingType), buildingType, null);
            }
        }
    }
}