using System.Collections.Generic;
using Steel;
using SteelCustom.Buildings;
using SteelCustom.Effects;

namespace SteelCustom.UIElements
{
    public class UIOrdersShop : ScriptComponent
    {
        private List<Building> _buildingsToSell = new List<Building>();
        private List<Effect> _effectsToSell = new List<Effect>();
        
        private Dictionary<Building, UIButton> _buildingsButtons = new Dictionary<Building, UIButton>();
        private Dictionary<Effect, UIButton> _effectsButtons = new Dictionary<Effect, UIButton>();

        public void Init()
        {
            _buildingsToSell = new List<Building> { new Turret(), new Wall(), new RocketLauncher(), new WaveGenerator(), new CreditsMiner(), new MineThrower() };
            _effectsToSell = new List<Effect> { new Rocket(), new Laser(), new Repair() };

            float K = GameController.Instance.UIController.K;
            
            UIButton closeButton = UI.CreateUIButton(ResourcesManager.GetImage("ui_close_button.aseprite"), "CloseButton", Entity);
            closeButton.RectTransform.AnchorMin = new Vector2(1, 0);
            closeButton.RectTransform.AnchorMax = new Vector2(1, 0);
            closeButton.RectTransform.Pivot = new Vector2(1, 0);
            closeButton.RectTransform.Size = new Vector2(14 * K, 14 * K);
            closeButton.RectTransform.AnchoredPosition = new Vector2(-2 * K, 2 * K);
            closeButton.OnClick.AddCallback(GameController.Instance.UIController.CloseOrdersShop);

            int i = 0;
            foreach (Building building in _buildingsToSell)
            {
                UIButton itemButton = UI.CreateUIButton(ResourcesManager.GetImage("ui_shop_item.aseprite"), "ShopItem", Entity);
                itemButton.RectTransform.AnchorMin = new Vector2(0, 1);
                itemButton.RectTransform.AnchorMax = new Vector2(0, 1);
                itemButton.RectTransform.Pivot = new Vector2(0, 1);
                itemButton.RectTransform.Size = new Vector2(58 * K, 14 * K);
                itemButton.RectTransform.AnchoredPosition = new Vector2(2 * K, -2 * K - i * (14 + 1) * K);
                itemButton.OnClick.AddCallback(
                    () =>
                    {
                        if (GameController.Instance.Player.TryOrderBuilding(building))
                            GameController.Instance.UIController.CloseOrdersShop();
                    }
                );

                Sprite sprite = ResourcesManager.GetImage(building.SpritePath);
                sprite.Pivot = new Vector2(0.5f, 0);
                UIImage icon = UI.CreateUIImage(sprite, "ShopItemIcon", itemButton.Entity);
                icon.RectTransform.AnchorMin = new Vector2(0, 0);
                icon.RectTransform.AnchorMax = new Vector2(0, 0);
                icon.RectTransform.Pivot = new Vector2(0, 0);
                icon.RectTransform.Size = new Vector2(12 * K, 12 * K);
                icon.RectTransform.AnchoredPosition = new Vector2(1 * K, 1 * K - icon.RectTransform.Size.Y * 0.5f);
                
                UIText text = UI.CreateUIText(building.Description, "ShopItemIcon", itemButton.Entity);
                text.RectTransform.AnchorMin = new Vector2(0, 0);
                text.RectTransform.AnchorMax = new Vector2(1, 0);
                text.RectTransform.Pivot = new Vector2(0, 0);
                text.RectTransform.Size = new Vector2(0, 12 * K);
                text.RectTransform.AnchoredPosition = new Vector2(0, 1 * K);
                text.RectTransform.OffsetMin = new Vector2(12 * K + 2 * K, 0);
                text.Color = UIController.DarkColor;
                text.TextOverflowMode = OverflowMode.WrapByWords;
                text.TextSize = 16;

                _buildingsButtons[building] = itemButton;
                i++;
            }

            i = 0;
            foreach (Effect effect in _effectsToSell)
            {
                UIButton itemButton = UI.CreateUIButton(ResourcesManager.GetImage("ui_shop_item.aseprite"), "ShopItem", Entity);
                itemButton.RectTransform.AnchorMin = new Vector2(0, 1);
                itemButton.RectTransform.AnchorMax = new Vector2(0, 1);
                itemButton.RectTransform.Pivot = new Vector2(0, 1);
                itemButton.RectTransform.Size = new Vector2(58 * K, 14 * K);
                itemButton.RectTransform.AnchoredPosition = new Vector2(58 * K + 3 * K, -2 * K - i * (14 + 1) * K);
                itemButton.OnClick.AddCallback(
                    () =>
                    {
                        if (GameController.Instance.Player.TryOrderEffect(effect))
                            GameController.Instance.UIController.CloseOrdersShop();
                    }
                );
                
                UIImage icon = UI.CreateUIImage(ResourcesManager.GetImage(effect.SpritePath), "ShopItemIcon", itemButton.Entity);
                icon.RectTransform.AnchorMin = new Vector2(0, 0);
                icon.RectTransform.AnchorMax = new Vector2(0, 0);
                icon.RectTransform.Pivot = new Vector2(0, 0);
                icon.RectTransform.Size = new Vector2(12 * K, 12 * K);
                icon.RectTransform.AnchoredPosition = new Vector2(1 * K, 1 * K);
                
                UIText text = UI.CreateUIText(effect.Description, "ShopItemIcon", itemButton.Entity);
                text.RectTransform.AnchorMin = new Vector2(0, 0);
                text.RectTransform.AnchorMax = new Vector2(1, 0);
                text.RectTransform.Pivot = new Vector2(0, 0);
                text.RectTransform.Size = new Vector2(0, 12 * K);
                text.RectTransform.AnchoredPosition = new Vector2(0, 1 * K);
                text.RectTransform.OffsetMin = new Vector2(12 * K + 2 * K, 0);
                text.Color = UIController.DarkColor;
                text.TextOverflowMode = OverflowMode.WrapByWords;
                text.TextSize = 16;

                _effectsButtons[effect] = itemButton;
                i++;
            }
            
            UpdateState();
        }

        public void UpdateState()
        {
            Player player = GameController.Instance.Player;
            foreach (var pair in _buildingsButtons)
            {
                pair.Value.Interactable = player.CanOrderBuilding(pair.Key);
            }
            foreach (var pair in _effectsButtons)
            {
                pair.Value.Interactable = player.CanOrderEffect(pair.Key);
            }
        }
    }
}