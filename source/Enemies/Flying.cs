﻿using SteelCustom.Buildings;

namespace SteelCustom.Enemies
{
    public class Flying : EnemyUnit
    {
        public override string SpritePath => "flying.aseprite";
        public override bool IsGround => false;
        
        public override int MaxHealth => 10;
        public override float Speed => 0.3f;
        public override int Reward => 10;
        public override int Damage => 2;
        public override float AttackDelay => 3;
        public override float AttackRange => 0.2f;

        protected override Building GetTarget()
        {
            return GameController.Instance.BattleController.BuilderController.GetClosestBuilding(Transformation.Position.X, BuildingType.Wall);
        }
    }
}