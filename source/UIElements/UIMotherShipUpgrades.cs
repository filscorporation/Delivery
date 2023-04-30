using System.Collections.Generic;
using Steel;
using SteelCustom.Upgrades;

namespace SteelCustom.UIElements
{
    public class UIMotherShipUpgrades : ScriptComponent
    {
        private readonly Dictionary<MotherShipUpgrade, (UIButton Button, UIText Text)> _upgradesButtons = new Dictionary<MotherShipUpgrade, (UIButton Button, UIText Text)>();

        public void Init()
        {
            float K = GameController.Instance.UIController.K;

            int i = 0;
            
            foreach (MotherShipUpgrade effect in GameController.Instance.MotherShip.Upgrades)
            {
                UIButton itemButton = UI.CreateUIButton(ResourcesManager.GetImage("ui_shop_item.aseprite"), "UpgradeItem", Entity);
                itemButton.RectTransform.AnchorMin = new Vector2(0, 1);
                itemButton.RectTransform.AnchorMax = new Vector2(0, 1);
                itemButton.RectTransform.Pivot = new Vector2(0, 1);
                itemButton.RectTransform.Size = new Vector2(58 * K, 14 * K);
                itemButton.RectTransform.AnchoredPosition = new Vector2(2 * K, -2 * K - i * (14 + 1) * K);
                itemButton.OnClick.AddCallback(
                    () =>
                    {
                        if (GameController.Instance.Player.TryBuyMotherShipUpgrade(effect))
                            UpdateState();
                    }
                );
                
                UIImage icon = UI.CreateUIImage(ResourcesManager.GetImage(effect.SpritePath), "UpgradeIcon", itemButton.Entity);
                icon.RectTransform.AnchorMin = new Vector2(0, 0);
                icon.RectTransform.AnchorMax = new Vector2(0, 0);
                icon.RectTransform.Pivot = new Vector2(0, 0);
                icon.RectTransform.Size = new Vector2(12 * K, 12 * K);
                icon.RectTransform.AnchoredPosition = new Vector2(1 * K, 1 * K);
                icon.ConsumeEvents = false;
                
                UIText text = UI.CreateUIText(effect.Description, "UpgradeText", itemButton.Entity);
                text.RectTransform.AnchorMin = new Vector2(0, 0);
                text.RectTransform.AnchorMax = new Vector2(1, 0);
                text.RectTransform.Pivot = new Vector2(0, 0);
                text.RectTransform.Size = new Vector2(0, 12 * K);
                text.RectTransform.AnchoredPosition = new Vector2(0, 1 * K);
                text.RectTransform.OffsetMin = new Vector2(12 * K + 2 * K, 0);
                text.Color = UIController.DarkColor;
                text.TextOverflowMode = OverflowMode.WrapByWords;
                text.TextSize = 16;

                _upgradesButtons[effect] = (itemButton, text);
                i++;
            }
            
            UpdateState();
        }

        public void UpdateState()
        {
            Player player = GameController.Instance.Player;
            foreach (var pair in _upgradesButtons)
            {
                pair.Value.Button.Interactable = player.CanBuyMotherShipUpgrade(pair.Key);
                pair.Value.Text.Text = pair.Key.Description;
            }
        }
    }
}