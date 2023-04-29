using System.Linq;
using Steel;

namespace SteelCustom
{
    public class DeliveryController : ScriptComponent
    {
        public void Init()
        {
        }

        public void OpenForFirstOrder()
        {
            GameController.Instance.UIController.EnableOpenOrdersShopButton();
        }
    }
}