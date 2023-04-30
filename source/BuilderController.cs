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

        public const float GROUND_HEIGHT = -1.25f;
        
        private Building _draftBuilding;
        private Effect _draftEffect;
        private Transformation _draftEffectObject;
        private bool _placingConstraints;
        private float _placingPosition;
        private float _placingRange;

        private List<Building> _sortedBuildingsCache = new List<Building>();

        public override void OnUpdate()
        {
            if (_draftBuilding != null)
            {
                UpdateDraftBuilding();
            }
            if (_draftEffect != null)
            {
                UpdateDraftEffect();
            }
        }

        public void StartPlacingBuilding(BuildingType buildingType)
        {
            ClearDraft();

            _draftBuilding = CreateBuilding(buildingType);
            _draftBuilding.Init();
            UpdateDraftBuilding();
        }

        public void StartPlacingEffect(Effect effect)
        {
            if (!effect.NeedTarget)
            {
                GameController.Instance.DeliveryController.AddItem(effect, Vector2.Zero);
                return;
            }
            
            ClearDraft();

            _draftEffectObject = new Entity("EffectObject").Transformation;
            _draftEffectObject.Entity.AddComponent<SpriteRenderer>().Sprite = ResourcesManager.GetImage("place_draft_effect.aseprite");
            _draftEffect = effect;
            UpdateDraftEffect();
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
            
            ReplaceBuildings(building.Transformation.Position, building.ColliderSize, building.Entity);
            
            _sortedBuildingsCache.Add(building);
            
            BuildingsChanged();
        }

        private void ClearDraft()
        {
            if (_draftBuilding != null)
            {
                _draftBuilding.Entity.Destroy();
                _draftBuilding = null;
            }
            if (_draftEffect != null)
            {
                _draftEffect = null;
                _draftEffectObject.Entity.Destroy();
                _draftEffectObject = null;
            }
        }

        private Building GetBuilding(Entity entity)
        {
            if (entity == null || entity.IsDestroyed())
                return null;
            // No inheritance in engine :(
            if (entity.HasComponent<ResearchStation>())
                return entity.GetComponent<ResearchStation>();
            if (entity.HasComponent<Turret>())
                return entity.GetComponent<Turret>();
            if (entity.HasComponent<Wall>())
                return entity.GetComponent<Wall>();
            if (entity.HasComponent<RocketLauncher>())
                return entity.GetComponent<RocketLauncher>();
            if (entity.HasComponent<WaveGenerator>())
                return entity.GetComponent<WaveGenerator>();
            if (entity.HasComponent<CreditsMiner>())
                return entity.GetComponent<CreditsMiner>();
            if (entity.HasComponent<MineThrower>())
                return entity.GetComponent<MineThrower>();
            return null;
        }

        private void ReplaceBuildings(Vector2 center, Vector2 size, Entity entity)
        {
            foreach (Entity collidedEntity in Physics.AABBCast(center, size))
            {
                if (!collidedEntity.Equals(entity))
                {
                    Building building = GetBuilding(collidedEntity);
                    if (building != null && building.IsPlaced)
                    {
                        ReplaceBuilding(building);
                    }
                }
            }
        }

        private void ReplaceBuilding(Building building)
        {
            building.DestroyBuilding();
        }

        private void UpdateDraftBuilding()
        {
            Vector2 position = Camera.Main.ScreenToWorldPoint(Input.MousePosition).SetY(GROUND_HEIGHT);
            _draftBuilding.Transformation.Position = new Vector3(position.X, position.Y, 0.5f);

            bool checkDraft = CheckDraftBuilding();
            _draftBuilding.SetDraftState(checkDraft);

            if (!UI.IsPointerOverUI() && Input.IsMouseJustPressed(MouseCodes.ButtonLeft))
            {
                if (checkDraft)
                    PlaceDraftBuilding();
            }
        }

        private void UpdateDraftEffect()
        {
            Vector2 position = Camera.Main.ScreenToWorldPoint(Input.MousePosition).SetY(GROUND_HEIGHT);
            _draftEffectObject.Transformation.Position = new Vector3(position.X, position.Y, 0.5f);

            if (!UI.IsPointerOverUI() && Input.IsMouseJustPressed(MouseCodes.ButtonLeft))
            {
                PlaceDraftEffect();
            }
        }

        private bool CheckDraftBuilding()
        {
            if (_placingConstraints)
            {
                float x = _draftBuilding.Transformation.Position.X;
                if (x < _placingPosition - _placingRange || x > _placingPosition + _placingRange)
                    return false;
            }

            bool result = true;
            foreach (Entity collidedEntity in Physics.AABBCast(_draftBuilding.Transformation.Position, _draftBuilding.ColliderSize))
            {
                if (!collidedEntity.Equals(_draftBuilding.Entity))
                {
                    Building building = GetBuilding(collidedEntity);
                    if (building == null)
                        continue;
                    
                    if (building is ResearchStation)
                        result = false;
                    else
                        building.ShowWillGetReplaced();
                }
            }
            
            return result;
        }

        private void PlaceDraftBuilding()
        {
            GameController.Instance.DeliveryController.AddItem(_draftBuilding, _draftBuilding.Transformation.Position);
            
            Entity effect = ResourcesManager.GetAsepriteData("place_draft_effect.aseprite").CreateEntityFromAsepriteData();
            effect.Transformation.Position = _draftBuilding.Transformation.Position + new Vector3(0, 0.3f, -1);
            effect.GetComponent<Animator>().Play("Effect");
            effect.Destroy(0.7f);

            ClearDraft();
        }

        private void PlaceDraftEffect()
        {
            GameController.Instance.DeliveryController.AddItem(_draftEffect, _draftEffectObject.Position);
            
            Entity effect = ResourcesManager.GetAsepriteData("place_draft_effect.aseprite").CreateEntityFromAsepriteData();
            effect.Transformation.Position = _draftEffectObject.Transformation.Position + new Vector3(0, 0.3f, -1);
            effect.GetComponent<Animator>().Play("Effect");
            effect.Destroy(0.7f);

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