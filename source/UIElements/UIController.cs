﻿using System.Collections;
using Steel;

namespace SteelCustom.UIElements
{
    public class UIController : ScriptComponent
    {
        public float K { get; private set; }
        public static Color DarkColor => new Color(22, 15, 41);
        public static Color RedColor => new Color(141, 10, 10);
        
        public UIMenu Menu { get; private set; }
        public Entity UIRoot { get; private set; }

        private UIButton _openMotherShipButton;
        private UIButton _openPlanetButton;
        private UIMotherShipUpgrades _motherShipUpgrades;
        
        private UIButton _openOrdersButton;
        private UIImage _creditsIcon;
        private UIText _creditsText;
        private UIOrdersShop _ordersShop;
        private UIImage _deliveryLabel;
        private UIDeliveryController _deliveryController;
        private UIResearchProgress _researchProgress;

        public override void OnUpdate()
        {
            if (GameController.Instance.GameState == GameState.Battle
             || GameController.Instance.GameState == GameState.Win
             || GameController.Instance.GameState == GameState.OrderFirstTower)
                UpdateInGameUI();
        }

        public override void OnDestroy()
        {
            if (GameController.Instance.Player != null)
                GameController.Instance.Player.OnCreditsChanged -= OnCreditsChanged;
        }

        public void CreateMenu()
        {
            K = Screen.Width / GameController.MAP_SIZE;
            
            UIRoot = UI.CreateUIElement();
            UIRoot.GetComponent<RectTransformation>().AnchorMin = Vector2.Zero;
            UIRoot.GetComponent<RectTransformation>().AnchorMax = Vector2.One;
            
            Menu = UI.CreateUIElement("Menu").AddComponent<UIMenu>();
            Menu.Init();
        }

        public void CreateGameUI()
        {
            CreateOpenMotherShipButton();
            CreateOpenPlanetButton();
            CreateMotherShipUpgrades();
            
            CreateOpenOrdersShopButton();
            CreateCreditsInfo();
            CreateOrdersShop();
            CreateDeliveryController();
            CreateResearchUI();
        }

        public void EnableOpenOrdersShopButton()
        {
            _openOrdersButton.Entity.IsActiveSelf = true;
        }

        public void DisableOpenOrdersShopButton()
        {
            _openOrdersButton.Entity.IsActiveSelf = false;
        }

        public void EnableOpenMotherShipButton()
        {
            _openMotherShipButton.Entity.IsActiveSelf = true;
        }

        public void DisableOpenMotherShipButton()
        {
            _openMotherShipButton.Entity.IsActiveSelf = false;
        }

        public void EnableOpenPlanetButton()
        {
            _openPlanetButton.Entity.IsActiveSelf = true;
        }

        public void DisableOpenPlanetButton()
        {
            _openPlanetButton.Entity.IsActiveSelf = false;
        }

        public void EnableDeliveryQueue()
        {
            _deliveryController.Entity.IsActiveSelf = true;
            _deliveryLabel.Entity.IsActiveSelf = true;
        }

        public void DisableDeliveryQueue()
        {
            _deliveryController.Entity.IsActiveSelf = false;
            _deliveryLabel.Entity.IsActiveSelf = false;
        }

        private void UpdateInGameUI()
        {
            _creditsText.Text = GameController.Instance.Player.Credits.ToString();
        }
        
        private void CreateOpenMotherShipButton()
        {
            _openMotherShipButton = UI.CreateUIButton(ResourcesManager.GetImage("ui_ship_button.aseprite"), "OpenMotherShipButton", UIRoot);
            _openMotherShipButton.RectTransform.AnchorMin = new Vector2(0.5f, 1.0f);
            _openMotherShipButton.RectTransform.AnchorMax = new Vector2(0.5f, 1.0f);
            _openMotherShipButton.RectTransform.Pivot = new Vector2(1, 1);
            _openMotherShipButton.RectTransform.Size = new Vector2(56 * K, 21 * K);
            _openMotherShipButton.RectTransform.AnchoredPosition = new Vector2(-20 * K, 5);
            
            _openMotherShipButton.OnClick.AddCallback(OpenMotherShip);

            DisableOpenMotherShipButton();
        }
        
        private void CreateOpenPlanetButton()
        {
            _openPlanetButton = UI.CreateUIButton(ResourcesManager.GetImage("ui_planet_button.aseprite"), "OpenPlanetButton", UIRoot);
            _openPlanetButton.RectTransform.AnchorMin = new Vector2(0.5f, 0.0f);
            _openPlanetButton.RectTransform.AnchorMax = new Vector2(0.5f, 0.0f);
            _openPlanetButton.RectTransform.Pivot = new Vector2(1, 0);
            _openPlanetButton.RectTransform.Size = new Vector2(58 * K, 21 * K);
            _openPlanetButton.RectTransform.AnchoredPosition = new Vector2(-20 * K, 5);
            
            _openPlanetButton.OnClick.AddCallback(OpenPlanet);

            DisableOpenPlanetButton();
        }
        
        private void CreateMotherShipUpgrades()
        {
            Entity uiEntity = UI.CreateUIElement("MotherShipUpgrades", UIRoot);
            _motherShipUpgrades = uiEntity.AddComponent<UIMotherShipUpgrades>();
            uiEntity.AddComponent<UIImage>().Sprite = ResourcesManager.GetImage("ui_frame.png");
            RectTransformation rt = uiEntity.GetComponent<RectTransformation>();
            rt.AnchorMin = new Vector2(0.0f, 0.5f);
            rt.AnchorMax = new Vector2(0.0f, 0.5f);
            rt.Size = new Vector2(62 * K, 63 * K);
            rt.Pivot = new Vector2(0.0f, 1.0f);
            rt.AnchoredPosition = new Vector2(1 * K, -1 * K);
            uiEntity.IsActiveSelf = false;

            _motherShipUpgrades.Init();
        }
        
        private void CreateOpenOrdersShopButton()
        {
            _openOrdersButton = UI.CreateUIButton(ResourcesManager.GetImage("ui_order_button.aseprite"), "OpenOrdersButton", UIRoot);
            _openOrdersButton.RectTransform.AnchorMin = new Vector2(1.0f, 1.0f);
            _openOrdersButton.RectTransform.AnchorMax = new Vector2(1.0f, 1.0f);
            _openOrdersButton.RectTransform.Pivot = new Vector2(1, 1);
            _openOrdersButton.RectTransform.Size = new Vector2(56 * K, 21 * K);
            _openOrdersButton.RectTransform.AnchoredPosition = new Vector2(-40, 5);
            
            _openOrdersButton.OnClick.AddCallback(OpenOrdersShop);

            DisableOpenOrdersShopButton();
        }
        
        private void CreateCreditsInfo()
        {
            _creditsIcon = UI.CreateUIImage(ResourcesManager.GetImage("ui_credits_icon.aseprite"), "CreditsIcon", UIRoot);
            _creditsIcon.RectTransform.AnchorMin = new Vector2(0.5f, 1.0f);
            _creditsIcon.RectTransform.AnchorMax = new Vector2(0.5f, 1.0f);
            _creditsIcon.RectTransform.Pivot = new Vector2(0, 1);
            _creditsIcon.RectTransform.Size = new Vector2(31 * K, 21 * K);
            _creditsIcon.RectTransform.AnchoredPosition = new Vector2(-14 * K, 5);

            _creditsText = UI.CreateUIText("0", "CreditsText", UIRoot);
            _creditsText.RectTransform.AnchorMin = new Vector2(0.5f, 1.0f);
            _creditsText.RectTransform.AnchorMax = new Vector2(0.5f, 1.0f);
            _creditsText.RectTransform.Pivot = new Vector2(1, 1);
            _creditsText.RectTransform.Size = new Vector2(32 * K, 21 * K);
            _creditsText.RectTransform.AnchoredPosition = new Vector2(10 * K - 2 * K, 5);

            _creditsText.TextAlignment = AlignmentType.CenterRight;
            _creditsText.TextSize = (int)Math.Round(16 * K);
            _creditsText.Color = DarkColor;
        }
        
        private void CreateOrdersShop()
        {
            Entity uiEntity = UI.CreateUIElement("OrdersShop", UIRoot);
            _ordersShop = uiEntity.AddComponent<UIOrdersShop>();
            uiEntity.AddComponent<UIImage>().Sprite = ResourcesManager.GetImage("ui_frame.png");
            RectTransformation rt = uiEntity.GetComponent<RectTransformation>();
            rt.AnchorMin = new Vector2(1.0f, 1.0f);
            rt.AnchorMax = new Vector2(1.0f, 1.0f);
            rt.Size = new Vector2(121 * K, 93 * K);
            rt.Pivot = new Vector2(1.0f, 1.0f);
            rt.AnchoredPosition = new Vector2(-1 * K, -1 * K);
            uiEntity.IsActiveSelf = false;

            _ordersShop.Init();

            GameController.Instance.Player.OnCreditsChanged += OnCreditsChanged;
        }
        
        private void CreateDeliveryController()
        {
            _deliveryLabel = UI.CreateUIImage(ResourcesManager.GetImage("ui_delivery_label.aseprite"), "DeliveryLabel", UIRoot);
            _deliveryLabel.RectTransform.AnchorMin = new Vector2(0.0f, 1.0f);
            _deliveryLabel.RectTransform.AnchorMax = new Vector2(0.0f, 1.0f);
            _deliveryLabel.RectTransform.Size = new Vector2(67 * K, 21 * K);
            _deliveryLabel.RectTransform.Pivot = new Vector2(0.0f, 1.0f);
            _deliveryLabel.RectTransform.AnchoredPosition = new Vector2(1 * K, -1 * K);
            _deliveryLabel.Entity.IsActiveSelf = false;
            
            Entity uiEntity = UI.CreateUIElement("DeliveryController", UIRoot);
            _deliveryController = uiEntity.AddComponent<UIDeliveryController>();
            RectTransformation rt = uiEntity.GetComponent<RectTransformation>();
            rt.AnchorMin = new Vector2(0.0f, 1.0f);
            rt.AnchorMax = new Vector2(0.0f, 1.0f);
            rt.Size = new Vector2(38 * K, 159 * K);
            rt.Pivot = new Vector2(0.0f, 1.0f);
            rt.AnchoredPosition = new Vector2(1 * K, -21 * K - 2 * K);
            uiEntity.IsActiveSelf = false;

            _deliveryController.Init();
        }

        private void CreateResearchUI()
        {
            _researchProgress = UI.CreateUIElement("ResearchUI", UIRoot).AddComponent<UIResearchProgress>();
            var rt = _researchProgress.GetComponent<RectTransformation>();
            rt.AnchorMin = new Vector2(0.0f, 0.0f);
            rt.AnchorMax = new Vector2(0.0f, 0.0f);
            rt.Pivot = new Vector2(0, 0);
            rt.Size = new Vector2(70 * K, 14 * K);
            rt.AnchoredPosition = new Vector2(1 * K, 1 * K);
            
            _researchProgress.Init();
            
            CloseResearchProgress();
        }

        private void OnCreditsChanged()
        {
            _ordersShop.UpdateState();
            _motherShipUpgrades.UpdateState();
        }

        public void OpenOrdersShop()
        {
            DisableOpenOrdersShopButton();

            _ordersShop.Entity.IsActiveSelf = true;
        }

        public void CloseOrdersShop()
        {
            _ordersShop.Entity.IsActiveSelf = false;
            EnableOpenOrdersShopButton();
        }

        private void OpenMotherShipUpgrades()
        {
            _motherShipUpgrades.Entity.IsActiveSelf = true;
        }

        private void CloseMotherShipUpgrades()
        {
            _motherShipUpgrades.Entity.IsActiveSelf = false;
        }

        public void OpenResearchProgress()
        {
            _researchProgress.Entity.IsActiveSelf = true;
        }

        private void CloseResearchProgress()
        {
            _researchProgress.Entity.IsActiveSelf = false;
        }

        public void OpenMotherShip()
        {
            GameController.Instance.CameraController.ToTopScene();

            StartCoroutine(OpenMotherShipCoroutine());
        }

        private IEnumerator OpenMotherShipCoroutine()
        {
            GameController.Instance.CameraController.ToTopScene();
            
            DisableOpenMotherShipButton();

            CloseOrdersShop();
            DisableDeliveryQueue();
            DisableOpenOrdersShopButton();
            CloseResearchProgress();

            yield return new WaitForSeconds(2.0f);
            
            EnableOpenPlanetButton();
            OpenMotherShipUpgrades();
        }

        public void OpenPlanet()
        {
            GameController.Instance.CameraController.ToBottomScene();

            StartCoroutine(OpenPlanetCoroutine());
        }

        private IEnumerator OpenPlanetCoroutine()
        {
            GameController.Instance.CameraController.ToBottomScene();
            
            DisableOpenPlanetButton();
            CloseMotherShipUpgrades();

            yield return new WaitForSeconds(2.0f);
            
            if (GameController.Instance.GameState == GameState.Win || GameController.Instance.GameState == GameState.Lose)
                yield break;
            
            EnableOpenMotherShipButton();
            
            EnableDeliveryQueue();
            EnableOpenOrdersShopButton();
            OpenResearchProgress();
        }
    }
}