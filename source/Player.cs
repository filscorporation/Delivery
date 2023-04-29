using Steel;

namespace SteelCustom
{
    public class Player :ScriptComponent
    {
        public int Credits { get; private set; }

        public bool ResearchStationPlaced { get; set; } = false;
    }
}