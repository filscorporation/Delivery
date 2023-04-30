using Steel;

namespace SteelCustom
{
    public class CameraController : ScriptComponent
    {
        private Vector3 _startPosition;
        private Vector3? _targetPosition;
        private float _progress;

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
                    Transformation.Position = Math.Lerp(_startPosition, _targetPosition.Value, Blend(_progress));
            }
        }

        public void ToBottomScene(bool instant = false)
        {
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