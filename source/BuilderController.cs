using System;
using System.Collections.Generic;
using System.Linq;
using Steel;
using SteelCustom.Buildings;
using SteelCustom.Effects;

namespace SteelCustom
{
    public class BuilderController : ScriptComponent
    {
        public event Action OnBuildingsChanged;
        
        public Building DraftBuilding { get; private set; }
        public bool IsInBuildMode => DraftBuilding != null;

        public const float GROUND_HEIGHT = -1.25f;

        private bool _placingConstraints;
        private float _placingPosition;
        private float _placingRange;

        private List<Building> _sortedBuildingsCache = new List<Building>();

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
            DraftBuilding.Init();
            UpdateDraft();
        }

        public void StartPlacingEffect(EffectType effectType)
        {
            // TODO:
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

        public void OnBuildingDestroyed(Building building)
        {
            _sortedBuildingsCache.Remove(building);
            BuildingsChanged();
        }

        public List<Building> GetBuildings()
        {
            return new List<Building>(_sortedBuildingsCache);
        }

        public Building GetClosestBuilding(float x, BuildingType? ignoreType = null)
        {
            if (!_sortedBuildingsCache.Any())
                return null;

            for (int i = _sortedBuildingsCache.Count - 1; i >= 0; i--)
            {
                if (ignoreType.HasValue && _sortedBuildingsCache[i].BuildingType == ignoreType.Value)
                    continue;
                if (_sortedBuildingsCache[i].Transformation.Position.X < x)
                    return _sortedBuildingsCache[i];
            }

            return null;
        }

        public void PlaceBuilding(BuildingType buildingType, Vector2 position)
        {
            Building building = CreateBuilding(buildingType);
            building.Init();
            building.Transformation.Position = new Vector3(position.X, position.Y, 0.5f);
            building.Place();
            
            _sortedBuildingsCache.Add(building);
            
            BuildingsChanged();
        }

        private void UpdateDraft()
        {
            Vector2 position = Camera.Main.ScreenToWorldPoint(Input.MousePosition).SetY(GROUND_HEIGHT);
            DraftBuilding.Transformation.Position = new Vector3(position.X, position.Y, 0.5f);;

            bool checkDraft = CheckDraft();
            DraftBuilding.SetDraftState(checkDraft);

            if (!UI.IsPointerOverUI() && Input.IsMouseJustPressed(MouseCodes.ButtonLeft))
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

            foreach (Entity collidedEntity in Physics.AABBCast(DraftBuilding.Transformation.Position, DraftBuilding.ColliderSize))
            {
                if (!collidedEntity.Equals(DraftBuilding.Entity))
                    return false;
            }
            
            return true;
        }

        private void PlaceDraft()
        {
            GameController.Instance.DeliveryController.AddItem(DraftBuilding, DraftBuilding.Transformation.Position);
            
            ClearDraft();
        }

        private void BuildingsChanged()
        {
            _sortedBuildingsCache = _sortedBuildingsCache.OrderBy(b => b.Transformation.Position.X).ToList();
            OnBuildingsChanged?.Invoke();
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