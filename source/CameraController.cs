using Steel;

namespace SteelCustom
{
    public class CameraController : ScriptComponent
    {
        private Vector3 _startPosition;
        private Vector3? _targetPosition;
        private float _progress;

        private bool _toShip = false;
        private AudioSource _backgroundPlanetMusic;
        private AudioSource _backgroundShipMusic;

        private const float VOLUME = 0.05f;
        private const float DURATION = 2.0f;
        
        public override void OnUpdate()
        {
            if (_targetPosition.HasValue)
            {
                _progress += Time.DeltaTime / DURATION;
                
                if (Vector3.Distance(Transformation.Position, _targetPosition.Value) < 0.001f)
                {
                    Transformation.Position = _targetPosition.Value;
                    _targetPosition = null;
                }
                else
                {
                    Transformation.Position = Math.Lerp(_startPosition, _targetPosition.Value, Blend(_progress));

                    _backgroundPlanetMusic.Volume = !_toShip ? _progress * VOLUME : (1 - _progress) * VOLUME;
                    _backgroundShipMusic.Volume = _toShip ? _progress * VOLUME : (1 - _progress) * VOLUME;
                }
            }
        }

        public void Init()
        {
            Camera.Main.Entity.GetComponent<AudioListener>().Volume = GameController.DEFAULT_VOLUME;
            
            Entity entity1 = new Entity();
            _backgroundPlanetMusic = entity1.AddComponent<AudioSource>();
            _backgroundPlanetMusic.Loop = true;
            _backgroundPlanetMusic.Play(ResourcesManager.GetAudioTrack("background_planet.wav"));
            _backgroundPlanetMusic.Volume = 0;
            
            Entity entity2 = new Entity();
            _backgroundShipMusic = entity2.AddComponent<AudioSource>();
            _backgroundShipMusic.Loop = true;
            _backgroundShipMusic.Play(ResourcesManager.GetAudioTrack("background_ship.wav"));
            _backgroundShipMusic.Volume = VOLUME;
        }

        public void ToBottomScene(bool instant = false)
        {
            _toShip = false;
            
            _startPosition = Transformation.Position;
            _targetPosition = new Vector3(0, 0, Transformation.Position.Z);
            _progress = 0.0f;
            
            if (instant)
            {
                Transformation.Position = _targetPosition.Value;
                _targetPosition = null;
            }
        }

        public void ToTopScene(bool instant = false)
        {
            _toShip = true;
            
            _startPosition = Transformation.Position;
            _targetPosition = new Vector3(0, 360f / 32, Transformation.Position.Z);
            _progress = 0.0f;
            
            if (instant)
            {
                Transformation.Position = _targetPosition.Value;
                _targetPosition = null;
            }
        }
        
        private static float Blend(float t)
        {
            float sqt = t * t;
            return sqt / (2.0f * (sqt - t) + 1.0f);
        }
    }
}