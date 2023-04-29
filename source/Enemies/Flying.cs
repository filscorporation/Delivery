namespace SteelCustom.Enemies
{
    public class Flying : EnemyUnit
    {
        public override bool IsGround => false;
        
        public override int MaxHealth => 10;
        public override float Speed => 0.3f;
        public override int Reward => 10;
        public override int Damage => 2;
        public override float AttackDelay => 3;
        public override float AttackRange => 0.2f;
    }
}