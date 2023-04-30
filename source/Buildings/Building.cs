using Steel;
using SteelCustom.Enemies;
using Math = Steel.Math;

namespace SteelCustom.Buildings
{
    public abstract class Building : ScriptComponent
    {
        public bool IsPlaced { get; private set; } = false;
        public abstract BuildingType BuildingType { get; }
        public virtual Vector2 ColliderSize => new Vector2(0.175f, 0.2f);
        
        public int Health { get; protected set; }
        public abstract int MaxHealth { get; }
        public abstract int Price { get; }
        public abstract int DeliveryTime { get; }
        public abstract string SpritePath { get; }
        public abstract string Name { get; }
        public abstract string Description { get; }
        
        private bool _showWillGetReplaced;
        private Entity _redCross;

        public override void OnUpdate()
        {
            _redCross.IsActiveSelf = _showWillGetReplaced;
            if (_showWillGetReplaced)
                _showWillGetReplaced = false;
            
            if (IsPlaced)
                UpdateBuilding();
        }

        public void Init()
        {
            Health = MaxHealth;

            Sprite sprite = ResourcesManager.GetImage(SpritePath);
            sprite.Pivot = new Vector2(0.5f, 0.0f);
            Entity.AddComponent<SpriteRenderer>().Sprite = sprite;
            Entity.AddComponent<BoxCollider>().Size = ColliderSize;
            Entity.AddComponent<RigidBody>().RigidBodyType = RigidBodyType.Static;

            _redCross = new Entity("RedCross", Entity);
            _redCross.Transformation.LocalPosition = new Vector3(0, 0, 1.5f);
            Sprite redCrossSprite = ResourcesManager.GetImage("red_cross.png");
            redCrossSprite.Pivot = new Vector2(0.5f, 0.0f);
            _redCross.AddComponent<SpriteRenderer>().Sprite = redCrossSprite;
        }

        public void Place()
        {
            IsPlaced = true;

            OnPlaced();
        }

        public void SetDraftState(bool checkDraft)
        {
            if (!checkDraft)
                ShowWillGetReplaced();
        }

        public void Repair(int healthBoost)
        {
            if (Health >= MaxHealth)
                Health += healthBoost;
            else
                Health = MaxHealth;
        }

        public void ShowWillGetReplaced()
        {
            if (_showWillGetReplaced)
                return;

            _showWillGetReplaced = true;
            _redCross.IsActiveSelf = _showWillGetReplaced;
        }

        public void TakeDamage(EnemyUnit attacker, int damage)
        {
            Health = Math.Max(0, Health - damage);
            OnTakeDamage(attacker);
            
            GameController.Instance.BattleController.DamageAnimator.Animate(damage, Transformation.Position, false);
            
            if (Health <= 0)
                DestroyBuilding();
        }
        
        protected virtual void UpdateBuilding() { }

        protected virtual void OnTakeDamage(EnemyUnit attacker) { }

        public void DestroyBuilding()
        {
            GameController.Instance.BattleController.BuilderController.OnBuildingDestroyed(this);
            OnBuildingDestroyed();
            
            Entity.Destroy();
        }

        protected virtual void OnBuildingDestroyed() { }
        
        protected virtual void OnPlaced() { }
    }
}