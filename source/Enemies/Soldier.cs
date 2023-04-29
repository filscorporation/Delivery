namespace SteelCustom.Enemies
{
    public class Soldier : EnemyUnit
    {
        public override int MaxHealth => 5;
        public override float Speed => 0.4f;
        public override int Reward => 5;
        public override int Damage => 1;
        public override float AttackDelay => 2;
        public override float AttackRange => 0.2f;
    }
}