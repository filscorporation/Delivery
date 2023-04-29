namespace SteelCustom.Buildings
{
    public class ResearchStation : Building
    {
        protected override void OnPlaced()
        {
            GameController.Instance.Player.ResearchStationPlaced = true;
        }
    }
}