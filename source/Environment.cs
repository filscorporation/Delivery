using Steel;

namespace SteelCustom
{
    public class Environment : ScriptComponent
    {
        public void Init()
        {
            SpriteRenderer sr = new Entity("Background", Entity).AddComponent<SpriteRenderer>();
            sr.GetComponent<Transformation>().Position = new Vector3(0, 0, -2);
            sr.Sprite = ResourcesManager.GetImage("background.aseprite");
        }
    }
}