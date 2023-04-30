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
            
            SpriteRenderer sr2 = new Entity("BackgroundTop", Entity).AddComponent<SpriteRenderer>();
            sr2.GetComponent<Transformation>().Position = new Vector3(0, (180f + 360f) * 0.5f / 32, -2);
            sr2.Sprite = ResourcesManager.GetImage("background_top.aseprite");
        }
    }
}