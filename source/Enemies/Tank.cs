﻿namespace SteelCustom.Enemies
{
    public class Tank : EnemyUnit
    {
        public override string SpritePath => "tank.aseprite";
        public override int MaxHealth => 30;
        public override float Speed => 0.1f;
        public override int Reward => 30;
        public override int Damage => 5;
        public override float AttackDelay => 5;
        public override float AttackRange => 0.3f;
    }
}