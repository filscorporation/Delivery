using Steel;

namespace SteelCustom.Buildings
{
    public class BuildingDeliveryItem : ScriptComponent
    {
        private DeliveryItem _deliveryItem;
        private Animator _parachuteAnimator;
        private bool _grounded = false;
        private bool _parachuteOpened = false;
        private float _speed = FALL_SPEED;
        private float _targetSpeed = FALL_SPEED;
        private float _timer = 0.0f;

        private const float FALL_SPEED = 3.0f;
        private const float GLIDE_SPEED = 0.5f;
        private const float SKY_HEIGHT = 3.0f;
        private const float OPEN_PARACHUTE_HEIGHT = 0.625f;

        public override void OnUpdate()
        {
            UpdateGrounding();
        }

        public void Init(DeliveryItem deliveryItem)
        {
            _deliveryItem = deliveryItem;

            Transformation.Position = new Vector3(deliveryItem.Position.X, SKY_HEIGHT);
            var sr = Entity.AddComponent<SpriteRenderer>();
            sr.Sprite = ResourcesManager.GetImage(deliveryItem.SpritePath);

            Entity parachuteEntity = ResourcesManager.GetAsepriteData("parachute.aseprite").CreateEntityFromAsepriteData();
            parachuteEntity.Parent = Entity;
            parachuteEntity.Transformation.LocalPosition = new Vector3(0, 0.8725f);
            _parachuteAnimator = parachuteEntity.GetComponent<Animator>();
        }

        private void UpdateGrounding()
        {
            if (_grounded)
            {
                _timer -= Time.DeltaTime;
                if (_timer <= 0.0f)
                {
                    GameController.Instance.BattleController.BuilderController.PlaceBuilding(_deliveryItem.BuildingType, _deliveryItem.Position);
                    Entity.Destroy();
                }
            }

            if (!_grounded)
            {
                Transformation.Position -= new Vector3(0, _speed * Time.DeltaTime);
                _speed = Math.Lerp(_speed, _targetSpeed, 4 * Time.DeltaTime);
            }
            
            if (!_parachuteOpened && Transformation.Position.Y < OPEN_PARACHUTE_HEIGHT)
            {
                _parachuteOpened = true;
                _parachuteAnimator.Play("Start");

                _targetSpeed = GLIDE_SPEED;
            }
            if (!_grounded && Transformation.Position.Y < BuilderController.GROUND_HEIGHT)
            {
                _grounded = true;
                _speed = 0.0f;
                _targetSpeed = 0.0f;
                _parachuteAnimator.Play("End");
                _timer = 0.5f;

                AsepriteData asepriteData = ResourcesManager.GetAsepriteData("building_landing_effect.aseprite");
                foreach (Sprite sprite in asepriteData.Sprites)
                    sprite.Pivot = new Vector2(0.5f, 0);
                Entity groundEffect = asepriteData.CreateEntityFromAsepriteData();
                groundEffect.Transformation.Position = Transformation.Position;
                groundEffect.GetComponent<Animator>().Play("Effect");
                
                groundEffect.AddComponent<AudioSource>().Volume = 2.0f;
                groundEffect.AddComponent<AudioSource>().Play(ResourcesManager.GetAudioTrack("building_landing.wav"));
                
                groundEffect.Destroy(0.7f);
            }
        }
    }
}