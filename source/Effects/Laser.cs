using Steel;
using SteelCustom.Enemies;

namespace SteelCustom.Effects
{
    public class Laser : Effect
    {
        public override EffectType EffectType => EffectType.Laser;
        public override bool NeedTarget => false;
        public override int Price => 10;
        public override int DeliveryTime => 4;
        public override string SpritePath => "laser.aseprite";
        public override string Description => $"Deal damage to all enemies.\nPrice: {Price}, Delivery time: {DeliveryTime}";

        private const int DAMAGE = 2;
        
        public override void Apply()
        {
            foreach (EnemyUnit enemyUnit in GameController.Instance.BattleController.EnemyController.Enemies)
            {
                if (enemyUnit.Entity == null || enemyUnit.Entity.IsDestroyed())
                    continue;
                
                enemyUnit.TakeDamage(DAMAGE);
            }

            Entity effect = ResourcesManager.GetAsepriteData("laser_effect.aseprite").CreateEntityFromAsepriteData();
            effect.Transformation.Position = new Vector3(0, 0, 2.5f);
            effect.GetComponent<Animator>().Play("Effect");
            effect.AddComponent<AudioSource>().Play(ResourcesManager.GetAudioTrack("laser.wav"));
            effect.Destroy(1.5f);
        }
    }
}