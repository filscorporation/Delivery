using System;
using System.Collections;
using Steel;
using SteelCustom.UIElements;

namespace SteelCustom
{
    public class GameController : ScriptComponent
    {
        public static GameController Instance;
        
        public const float DEFAULT_VOLUME = 0.25f;
        public static bool SoundOn { get; set; } = true;
        public const float MAP_SIZE = 320;

        public Player Player { get; private set; }
        public CameraController CameraController { get; private set; }
        public UIController UIController { get; private set; }
        public DialogController DialogController { get; private set; }
        public BattleController BattleController { get; private set; }
        public MotherShip MotherShip { get; private set; }
        public DeliveryController DeliveryController { get; private set; }

        public GameState GameState { get; private set; }
        private bool _changeState = false;
        private bool _startGame = false;
        private bool _winGame = false;
        private bool _loseGame = false;
        
        private bool _shipUpgradeDialog = false;
        private bool _halfCompletedDilog = false;
        
        public override void OnCreate()
        {
            Instance = this;
            
            Screen.Color = new Color(243, 223, 193);
            Screen.Width = 1600;
            Screen.Height = 900;
            Camera.Main.ResizingMode = CameraResizingMode.KeepWidth;
            Camera.Main.Width = MAP_SIZE / 32;

            StartCoroutine(IntroCoroutine());
        }

        public override void OnUpdate()
        {
            if (_changeState)
            {
                _changeState = false;
                
                switch (GameState)
                {
                    case GameState.Intro:
                        StartCoroutine(PlaceResearchStationCoroutine());
                        break;
                    case GameState.PlaceResearchStation:
                        StartCoroutine(OrderFirstTowerCoroutine());
                        break;
                    case GameState.OrderFirstTower:
                        StartCoroutine(StartBattleCoroutine());
                        break;
                    case GameState.Battle:
                        if (_loseGame)
                            StartCoroutine(LoseGameCoroutine());
                        else if (_winGame)
                            StartCoroutine(WinGameCoroutine());
                        break;
                    case GameState.Win:
                        break;
                    case GameState.Lose:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (GameState == GameState.Battle)
            {
                if (!_shipUpgradeDialog && BattleController.EnemyController.CurrentWave > 5)
                {
                    StartCoroutine(ShowShipUpgradesCoroutine());
                    _shipUpgradeDialog = true;
                }

                if (!_halfCompletedDilog && BattleController.EnemyController.AttackCompletion >= 0.5f)
                {
                    StartCoroutine(ShowHalfCompleteCoroutine());
                    _halfCompletedDilog = true;
                }
            }
        }

        public void StartGame()
        {
            _startGame = true;
        }

        public void RestartGame()
        {
            SceneManager.SetActiveScene(SceneManager.GetActiveScene());
        }

        public void ExitGame()
        {
            Application.Quit();
        }

        public void WinGame()
        {
            _winGame = true;
            _changeState = true;
        }

        public void LoseGame()
        {
            if (_winGame)
            {
                ExitGame();
                return;
            }
            
            _loseGame = true;
            _changeState = true;
        }

        private IEnumerator IntroCoroutine()
        {
            GameState = GameState.Intro;
            Log.LogInfo("Start Intro state");

            new Entity("Environment").AddComponent<Environment>().Init();

            CameraController = Camera.Main.Entity.AddComponent<CameraController>();
            CameraController.Init();
            CameraController.ToTopScene(true);
            
            UIController = new Entity("UI controller").AddComponent<UIController>();
            UIController.CreateMenu();

            yield return new WaitWhile(() => !_startGame);

            Player = new Entity("Player").AddComponent<Player>();
            DialogController = new Entity("DialogController").AddComponent<DialogController>();
            BattleController = new Entity("BattleController").AddComponent<BattleController>();
            MotherShip = new Entity("MotherShip").AddComponent<MotherShip>();
            DeliveryController = new Entity("DeliveryController").AddComponent<DeliveryController>();
            
            Player.Init();
            DialogController.Init();
            BattleController.Init();
            DeliveryController.Init();

            yield return new WaitForSeconds(1.0f);

            DialogController.ShowIntroDialog();
            yield return new WaitWhile(() => DialogController.ShowingDialog);
            
            CameraController.ToBottomScene();

            yield return new WaitForSeconds(2.0f);

            Log.LogInfo("End Intro state");
            _changeState = true;
        }

        private IEnumerator PlaceResearchStationCoroutine()
        {
            GameState = GameState.PlaceResearchStation;
            Log.LogInfo("Start PlaceResearchStation state");
            
            UIController.CreateGameUI();
            
            DialogController.ShowPlaceResearchStationDialog();
            yield return new WaitWhile(() => DialogController.ShowingDialog);
            
            BattleController.PlaceResearchStation();
            
            yield return new WaitWhile(() => !Player.ResearchStationPlaced);
            
            UIController.OpenResearchProgress();
            BattleController.EndPlaceResearchStation();
            
            yield return new WaitForSeconds(1.0f);

            Log.LogInfo("End PlaceResearchStation state");
            _changeState = true;
        }

        private IEnumerator OrderFirstTowerCoroutine()
        {
            GameState = GameState.OrderFirstTower;
            Log.LogInfo("Start OrderFirstTower state");
            
            DialogController.ShowOrderFirstTowerDialog();
            yield return new WaitWhile(() => DialogController.ShowingDialog);
            
            DeliveryController.OpenForFirstOrder();
            
            yield return new WaitWhile(() => !Player.FirstTowerOrdered);
            
            UIController.EnableDeliveryQueue();

            Log.LogInfo("End OrderFirstTower state");
            _changeState = true;
        }

        private IEnumerator StartBattleCoroutine()
        {
            DialogController.ShowBeforeBattleDialog();
            
            yield return new WaitWhile(() => DialogController.ShowingDialog);
            
            GameState = GameState.Battle;
            Log.LogInfo("Start Battle state");

            BattleController.StartBattle();
        }

        private IEnumerator ShowShipUpgradesCoroutine()
        {
            DialogController.ShowUpgradeMotherShipDialog();
            yield return new WaitWhile(() => DialogController.ShowingDialog);
            
            UIController.EnableOpenMotherShipButton();
        }

        private IEnumerator ShowHalfCompleteCoroutine()
        {
            DialogController.ShowResearchHalfDownDialog();
            yield return new WaitWhile(() => DialogController.ShowingDialog);
        }

        private IEnumerator LoseGameCoroutine()
        {
            GameState = GameState.Lose;
            Log.LogInfo("Start Lose state");
            
            UIController.OpenPlanet();

            UIController.CloseOrdersShop();
            UIController.DisableOpenOrdersShopButton();
            UIController.DisableOpenMotherShipButton();
            
            DialogController.ShowLoseDialog();
            yield return new WaitWhile(() => DialogController.ShowingDialog);
            
            yield return new WaitForSeconds(0.5f);
            
            UIController.Menu.OpenOnLoseScreen();
        }

        private IEnumerator WinGameCoroutine()
        {
            GameState = GameState.Win;
            Log.LogInfo("Start Win state");
            
            UIController.OpenPlanet();

            UIController.CloseOrdersShop();
            UIController.DisableOpenOrdersShopButton();
            UIController.DisableOpenMotherShipButton();

            DialogController.ShowWinDialog();
            yield return new WaitWhile(() => DialogController.ShowingDialog);
            
            /*yield return new WaitForSeconds(2.0f);
            
            Application.Quit();*/
        }
    }
}