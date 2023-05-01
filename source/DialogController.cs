using System;
using System.Collections;
using Steel;
using SteelCustom.UIElements;

namespace SteelCustom
{
    public class DialogController : ScriptComponent
    {
        private UIDialog _uiDialog;
        private Action _defferedAction = null;

        public bool ShowingDialog { get; private set; } = false;

        public override void OnUpdate()
        {
            if (_defferedAction != null)
            {
                _defferedAction?.Invoke();
                _defferedAction = null;
            }
        }

        public void Init()
        {
            float K = GameController.Instance.UIController.K;
            
            UIImage image = UI.CreateUIImage(ResourcesManager.GetImage("ui_frame.png"), "UIDialog", GameController.Instance.UIController.UIRoot);
            image.RectTransform.AnchorMin = new Vector2(1.0f, 0.0f);
            image.RectTransform.AnchorMax = new Vector2(1.0f, 0.0f);
            image.RectTransform.Size = new Vector2(250 * K, 26 * K);
            image.RectTransform.Pivot = new Vector2(1.0f, 0.0f);
            image.RectTransform.AnchoredPosition = new Vector2(-1 * K, 1 * K);

            _uiDialog = image.Entity.AddComponent<UIDialog>();
            _uiDialog.Init();

            _uiDialog.Entity.IsActiveSelf = false;
        }

        public void ShowIntroDialog()
        {
            StopAllCoroutines();
            ShowingDialog = true;
            _defferedAction = () => StartCoroutine(ShowIntroDialogCoroutine());
        }
        
        private IEnumerator ShowIntroDialogCoroutine()
        {
            _uiDialog.Entity.IsActiveSelf = true;
            
            _uiDialog.SetText("Greetings, capitan. We are currently on the orbit of LD-0053.");

            yield return new WaitWhile(() => !Input.IsMouseJustPressed(MouseCodes.ButtonLeft));
            yield return null;
            
            _uiDialog.SetText("Scanners of our cruiser showed enormous amount of plutonium in the top layers of this planet, but we can't risk and need to ensure if this data is correct.");

            yield return new WaitWhile(() => !Input.IsMouseJustPressed(MouseCodes.ButtonLeft));
            yield return null;
            
            _uiDialog.SetText("You've been assigned to a mission of extreme importance. Land on the planet, do research of the surface composition and transfer data to the cruiser.");

            yield return new WaitWhile(() => !Input.IsMouseJustPressed(MouseCodes.ButtonLeft));
            yield return null;
            
            _uiDialog.SetText("Every minute costs us money that we could have already earned on the market. So let's not waste any time.");

            yield return new WaitWhile(() => !Input.IsMouseJustPressed(MouseCodes.ButtonLeft));
            yield return null;

            _uiDialog.Entity.IsActiveSelf = false;
            
            ShowingDialog = false;
        }

        public void ShowPlaceResearchStationDialog()
        {
            StopAllCoroutines();
            ShowingDialog = true;
            _defferedAction = () => StartCoroutine(ShowPlaceResearchStationDialogCoroutine());
        }
        
        private IEnumerator ShowPlaceResearchStationDialogCoroutine()
        {
            _uiDialog.Entity.IsActiveSelf = true;
            
            _uiDialog.SetText("First, choose location for your Research Station.");

            yield return new WaitWhile(() => !Input.IsMouseJustPressed(MouseCodes.ButtonLeft));
            yield return null;
            
            _uiDialog.SetText("On the West is a good spot, it will leave us place for defensive structures.");

            yield return new WaitWhile(() => !Input.IsMouseJustPressed(MouseCodes.ButtonLeft));
            yield return null;

            _uiDialog.Entity.IsActiveSelf = false;
            
            ShowingDialog = false;
        }

        public void ShowOrderFirstTowerDialog()
        {
            StopAllCoroutines();
            ShowingDialog = true;
            _defferedAction = () => StartCoroutine(ShowOrderFirstTowerDialogCoroutine());
        }
        
        private IEnumerator ShowOrderFirstTowerDialogCoroutine()
        {
            _uiDialog.Entity.IsActiveSelf = true;
            
            _uiDialog.SetText("We detected hostile lifeform all over the place where the most of plutonium is concentrated.");

            yield return new WaitWhile(() => !Input.IsMouseJustPressed(MouseCodes.ButtonLeft));
            yield return null;
            
            _uiDialog.SetText("To complete research you will need to defend the station. For this purpose we grand you the access to the delivery interface.");

            yield return new WaitWhile(() => !Input.IsMouseJustPressed(MouseCodes.ButtonLeft));
            yield return null;
            
            _uiDialog.SetText("Use it carefully - it takes time to process your orders and get them to the planet surface.");

            yield return new WaitWhile(() => !Input.IsMouseJustPressed(MouseCodes.ButtonLeft));
            yield return null;
            
            _uiDialog.SetText("Now order and place your first turret. Be careful with coordinates of placement - buildings will destroy others if landing on top.");

            yield return new WaitWhile(() => !Input.IsMouseJustPressed(MouseCodes.ButtonLeft));
            yield return null;

            _uiDialog.Entity.IsActiveSelf = false;
            
            ShowingDialog = false;
        }

        public void ShowBeforeBattleDialog()
        {
            StopAllCoroutines();
            ShowingDialog = true;
            _defferedAction = () => StartCoroutine(ShowBeforeBattleDialogCoroutine());
        }
        
        private IEnumerator ShowBeforeBattleDialogCoroutine()
        {
            _uiDialog.Entity.IsActiveSelf = true;
            
            _uiDialog.SetText("Defend the station, build your base, finish research. Success of our mission depends on you.");

            yield return new WaitWhile(() => !Input.IsMouseJustPressed(MouseCodes.ButtonLeft));
            yield return null;

            _uiDialog.Entity.IsActiveSelf = false;
            
            ShowingDialog = false;
        }

        public void ShowUpgradeMotherShipDialog()
        {
            StopAllCoroutines();
            ShowingDialog = true;
            _defferedAction = () => StartCoroutine(ShowUpgradeMotherShipDialogCoroutine());
        }
        
        private IEnumerator ShowUpgradeMotherShipDialogCoroutine()
        {
            _uiDialog.Entity.IsActiveSelf = true;
            
            _uiDialog.SetText("To increase our chances of success we discovered different ways of upgrading effectiveness of ship's system. Take a look.");

            yield return new WaitWhile(() => !Input.IsMouseJustPressed(MouseCodes.ButtonLeft));
            yield return null;

            _uiDialog.Entity.IsActiveSelf = false;
            
            ShowingDialog = false;
        }

        public void ShowResearchHalfDownDialog()
        {
            StopAllCoroutines();
            ShowingDialog = true;
            _defferedAction = () => StartCoroutine(ShowResearchHalfDownDialogCoroutine());
        }
        
        private IEnumerator ShowResearchHalfDownDialogCoroutine()
        {
            _uiDialog.Entity.IsActiveSelf = true;
            
            _uiDialog.SetText("Good job, we're half way through, keep it up, capitan.");

            yield return new WaitWhile(() => !Input.IsMouseJustPressed(MouseCodes.ButtonLeft));
            yield return null;

            _uiDialog.Entity.IsActiveSelf = false;
            
            ShowingDialog = false;
        }

        public void ShowLoseDialog()
        {
            StopAllCoroutines();
            ShowingDialog = true;
            _defferedAction = () => StartCoroutine(ShowLoseDialogCoroutine());
        }
        
        private IEnumerator ShowLoseDialogCoroutine()
        {
            _uiDialog.Entity.IsActiveSelf = true;
            
            _uiDialog.SetText("Research station is destroyed. Mission failed. Maybe we overestimated you.");

            yield return new WaitWhile(() => !Input.IsMouseJustPressed(MouseCodes.ButtonLeft));
            yield return null;

            _uiDialog.Entity.IsActiveSelf = false;
            
            ShowingDialog = false;
        }

        public void ShowWinDialog()
        {
            StopAllCoroutines();
            ShowingDialog = true;
            _defferedAction = () => StartCoroutine(ShowWinDialogCoroutine());
        }
        
        private IEnumerator ShowWinDialogCoroutine()
        {
            _uiDialog.Entity.IsActiveSelf = true;
            
            _uiDialog.SetText("Congratulation, capitan. Mission completed. We gathered enough information to plan our next campaign.");
            
            yield return new WaitForSeconds(2.0f);
            yield return new WaitWhile(() => !Input.IsMouseJustPressed(MouseCodes.ButtonLeft));
            yield return null;
            
            _uiDialog.SetText("Unfortunately estimated costs of research exceeded our expectations, and now we made a hard decision to leave station and all defensive structures on the surface.");

            yield return new WaitForSeconds(2.0f);
            yield return new WaitWhile(() => !Input.IsMouseJustPressed(MouseCodes.ButtonLeft));
            yield return null;
            
            yield return new WaitForSeconds(2.0f);
            _uiDialog.SetText("This means you are also staying there.");

            yield return new WaitWhile(() => !Input.IsMouseJustPressed(MouseCodes.ButtonLeft));
            yield return null;
            
            _uiDialog.SetText("Thank you for your service. Goodbye.");

            yield return new WaitForSeconds(2.0f);
            yield return new WaitWhile(() => !Input.IsMouseJustPressed(MouseCodes.ButtonLeft));
            yield return null;

            _uiDialog.Entity.IsActiveSelf = false;
            
            ShowingDialog = false;
        }
    }
}