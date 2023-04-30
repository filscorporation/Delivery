using Steel;

namespace SteelCustom.Buildings
{
    public class Projectile : ScriptComponent
    {
        private Vector2 _startPosition;
        private Vector2 _targetPosition;
        private float _duration;
        private float _progress;
        
        public override void OnUpdate()
        {
            _progress += Time.DeltaTime / _duration;
            Transformation.Position = Math.Lerp(_startPosition, _targetPosition, _progress);
        }

        public void Init(Vector2 target, float duration)
        {
            _startPosition = Transformation.Position;
            _targetPosition = target;
            _duration = duration;
            _progress = 0.0f;
        }
    }
}