namespace SteelCustom.Enemies
{
    public class Runner : EnemyUnit
    {
        public override string SpritePath => "runner.aseprite";
        public override int MaxHealth => 2;
        public override float Speed => 0.7f;
        public override int Reward => 5;
        public override int Damage => 1;
        public override float AttackDelay => 1.5f;
        public override float AttackRange => 0.2f;
    }
}