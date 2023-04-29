using System;
using System.Collections;
using System.Linq;
using Steel;
using SteelCustom.UIElements;

namespace SteelCustom
{
    public class GameController : ScriptComponent
    {
        public static GameController Instance;
        
        public const float DEFAULT_VOLUME = 0.15f;
        public static bool SoundOn { get; set; } = true;
        public const float MAP_SIZE = 320;

        public Player Player { get; private set; }
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
        }

        public void StartGame()
        {
            _startGame = true;
        }

        public void RestartGame()
        {
            Dispose();
            SceneManager.SetActiveScene(SceneManager.GetActiveScene());
        }

        public void ExitGame()
        {
            Dispose();
            Application.Quit();
        }

        public void WinGame()
        {
            _winGame = true;
            _changeState = true;
        }

        public void LoseGame()
        {
            _loseGame = true;
            _changeState = true;
        }

        private void Dispose()
        {
            UIController?.Dispose();
        }

        private IEnumerator IntroCoroutine()
        {
            GameState = GameState.Intro;
            Log.LogInfo("Start Intro state");
            
            Camera.Main.Entity.GetComponent<AudioListener>().Volume = DEFAULT_VOLUME;
            Entity backgroundMusic = new Entity();
            AudioSource source = backgroundMusic.AddComponent<AudioSource>();
            source.Loop = true;
            //source.Play(ResourcesManager.GetAudioTrack("background_music.wav"));
            source.Volume = 0.1f;
            
            UIController = new Entity("UI controller").AddComponent<UIController>();
            UIController.CreateMenu();

            _startGame = true; // TODO: menu
            yield return new WaitWhile(() => !_startGame);

            Player = new Entity("Player").AddComponent<Player>();
            DialogController = new Entity("DialogController").AddComponent<DialogController>();
            BattleController = new Entity("BattleController").AddComponent<BattleController>();
            MotherShip = new Entity("MotherShip").AddComponent<MotherShip>();
            DeliveryController = new Entity("DeliveryController").AddComponent<DeliveryController>();

            new Entity("Environment").AddComponent<Environment>().Init();
            
            BattleController.Init();
            DeliveryController.Init();

            yield return DialogController.ShowIntroDialog();

            Log.LogInfo("End Intro state");
            _changeState = true;
        }

        private IEnumerator PlaceResearchStationCoroutine()
        {
            GameState = GameState.PlaceResearchStation;
            Log.LogInfo("Start PlaceResearchStation state");
            
            UIController.CreateGameUI();
            BattleController.PlaceResearchStation();

            yield return DialogController.ShowPlaceResearchStationDialog();
            
            yield return new WaitWhile(() => !Player.ResearchStationPlaced);
            
            BattleController.EndPlaceResearchStation();
            
            yield return new WaitForSeconds(1.0f);

            Log.LogInfo("End PlaceResearchStation state");
            _changeState = true;
        }

        private IEnumerator OrderFirstTowerCoroutine()
        {
            GameState = GameState.OrderFirstTower;
            Log.LogInfo("Start OrderFirstTower state");
            
            DeliveryController.OpenForFirstOrder();

            yield return DialogController.ShowOrderFirstTowerDialog();
            
            yield return new WaitWhile(() => !Player.FirstTowerOrdered);
            
            UIController.EnableDeliveryQueue();

            Log.LogInfo("End OrderFirstTower state");
            _changeState = true;
        }

        private IEnumerator StartBattleCoroutine()
        {
            GameState = GameState.Battle;
            Log.LogInfo("Start Battle state");

            yield return DialogController.ShowBeforeBattleDialog();

            BattleController.StartBattle();
            
            yield return new WaitForSeconds(1.0f);
        }

        private IEnumerator LoseGameCoroutine()
        {
            GameState = GameState.Lose;
            Log.LogInfo("Start Lose state");

            yield return DialogController.ShowLoseDialog();
            
            yield return new WaitForSeconds(0.1f);
            
            //Entity.AddComponent<AudioSource>().Play(ResourcesManager.GetAudioTrack("end_game.wav"));
            //UIController.Menu.OpenOnLoseScreen();
        }

        private IEnumerator WinGameCoroutine()
        {
            GameState = GameState.Win;
            Log.LogInfo("Start Win state");

            yield return DialogController.ShowWinDialog();
            
            yield return new WaitForSeconds(0.1f);
            
            //Entity.AddComponent<AudioSource>().Play(ResourcesManager.GetAudioTrack("end_game.wav"));
            //UIController.Menu.OpenOnWinScreen();
        }
    }
}