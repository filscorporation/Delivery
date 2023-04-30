namespace SteelCustom
{
    public class Wave
    {
        public float Delay { get; }
        public EnemyType EnemyType { get; }
        public int EnemyCount { get; }

        public Wave(float delay, EnemyType enemyType, int enemyCount)
        {
            Delay = delay;
            EnemyType = enemyType;
            EnemyCount = enemyCount;
        }
    }
}