using UnityEngine;
using System;
using DaggerfallWorkshop.Game.UserInterface;
using System.Linq;


namespace DaggerfallWorkshop.Game.UserInterfaceWindows
{
    public class FlynsarmyExteriorAutomapWindow : DaggerfallExteriorAutomapWindow
    {
        protected static TextBox localFilterTextBox;
        protected static Button localFilterButton;
        protected static string filterString = null;

        public FlynsarmyExteriorAutomapWindow(IUserInterfaceManager uiManager)
            : base(uiManager)
        {

        }

        protected override void Setup()
        {
            base.Setup();
            SetupTargetIconPanelFilterBox();
        }

        public override void Update()
        {
            base.Update();

            FilterAutomapView();
        }

        protected void SetupTargetIconPanelFilterBox()
        {
            string toolTipText = string.Empty;
            toolTipText = "Press Filter Button to Open Filter Text Box.\rAnything typed into text box will autofilter.";

            localFilterTextBox = DaggerfallUI.AddTextBoxWithFocus(new Rect(new Vector2(15, 24), new Vector2(47, 8)), "", NativePanel);
            localFilterTextBox.VerticalAlignment = VerticalAlignment.Bottom;
            localFilterTextBox.OnType += LocalFilterTextBox_OnType;
            localFilterTextBox.OverridesHotkeySequences = true;
            localFilterTextBox.Enabled = true;
            localFilterTextBox.SetFocus();

            // This button doesn't do anything. It just adds the appearance of a label beside
            // our text field
            localFilterButton = DaggerfallUI.AddButton(new Rect(40, 25, 15, 8), NativePanel);
            localFilterButton.Label.Text = "Filter";
            localFilterButton.ToolTip = defaultToolTip;
            localFilterButton.ToolTipText = toolTipText;
            localFilterButton.Label.TextScale = 0.75f;
            localFilterButton.Label.ShadowColor = Color.black;
            localFilterButton.VerticalAlignment = VerticalAlignment.Bottom;
            localFilterButton.HorizontalAlignment = HorizontalAlignment.Left;
            localFilterButton.BackgroundColor = new Color(0.5f, 0.5f, 0.5f, 0.75f);
        }

        /// <summary>
        /// called when automap window is pushed - resets automap settings to default settings and signals ExteriorAutomap class
        /// </summary>
        public override void OnPush()
        {
            // Save the current automap hotkey
            KeyCode automapKeyCode = InputManager.Instance.GetBinding(InputManager.Actions.AutoMap);
            Debug.Log("Keycode found: Setting to slash");
            Debug.Log(automapKeyCode);
            // Remove the binding
            InputManager.Instance.SetBinding(KeyCode.Slash, InputManager.Actions.AutoMap);
            InputManager.Instance.SaveKeyBinds();
            Debug.Log("New Keycode found:");
            Debug.Log(InputManager.Instance.GetBinding(InputManager.Actions.AutoMap));

            // Base class saves the current binding and uses it to hide the automap in
            // Update() method.
            base.OnPush();

            // Return the binding to what it was set to by the user
            InputManager.Instance.SetBinding(automapKeyCode, InputManager.Actions.AutoMap);
            InputManager.Instance.SaveKeyBinds();
        }


        public override void OnPop()
        {
            ClearFilterFields();
            base.OnPop();
        }

        public void FilterAutomapView()
        {
            if (String.IsNullOrEmpty(filterString))
            {
                return;
            }

            int hidden = 0;
            int count = PanelRenderAutomap.Components.Count();

            foreach (TextLabel label in PanelRenderAutomap.Components.ToList())
            {
                if (!NameplatePassesFilter(label.Text))
                {
                    PanelRenderAutomap.Components.Remove(label);
                    hidden++;
                }
            }

            Debug.Log("CityMapFilter: Hid " + hidden + "/" + count + " leaving " + PanelRenderAutomap.Components.Count() + " with filter " + filterString);
        }

        protected bool NameplatePassesFilter(String nameplateText)
        {
            return nameplateText.IndexOf(filterString, StringComparison.OrdinalIgnoreCase) != -1;
        }

        private void ClearFilterFields()
        {
            filterString = null;
            localFilterTextBox.Text = string.Empty;
        }

        private void LocalFilterTextBox_OnType()
        {
            filterString = localFilterTextBox.Text.ToLower();

            Debug.Log("CityMapFilter: Filtering to " + filterString);

            FilterAutomapView();
        }
    }
}
