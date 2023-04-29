using Steel;
using SteelCustom.Enemies;
using Math = Steel.Math;

namespace SteelCustom.Buildings
{
    public abstract class Building : ScriptComponent
    {
        public bool IsPlaced { get; private set; } = false;
        public BuildingType BuildingType { get; private set; }
        protected virtual Vector2 ColliderSize => new Vector2(0.3f, 0.3f);
        
        public int Health { get; protected set; }
        public abstract int MaxHealth { get; }
        public abstract int Price { get; }
        public abstract int DeliveryTime { get; }
        public abstract string SpritePath { get; }
        public abstract string Name { get; }
        public abstract string Description { get; }

        public void Init(BuildingType buildingType)
        {
            BuildingType = buildingType;
            Health = MaxHealth;

            Sprite sprite = ResourcesManager.GetImage(SpritePath);
            sprite.Pivot = new Vector2(0.5f, 0.0f);
            Entity.AddComponent<SpriteRenderer>().Sprite = sprite;
            Entity.AddComponent<BoxCollider>().Size = ColliderSize;
            Entity.AddComponent<RigidBody>().RigidBodyType = RigidBodyType.Static;
        }

        public void Place()
        {
            IsPlaced = true;

            OnPlaced();
        }

        public void SetDraftState(bool checkDraft)
        {
            
        }

        public void TakeDamage(EnemyUnit attacker, int damage)
        {
            Health = Math.Max(0, Health - damage);
            OnTakeDamage(attacker);
            
            if (Health <= 0)
                DestroyBuilding();
        }

        protected virtual void OnTakeDamage(EnemyUnit attacker) { }

        private void DestroyBuilding()
        {
            GameController.Instance.BattleController.BuilderController.OnBuildingDestroyed(this);
            OnBuildingDestroyed();
            
            Entity.Destroy();
        }

        protected virtual void OnBuildingDestroyed() { }
        
        protected virtual void OnPlaced() { }
    }
}