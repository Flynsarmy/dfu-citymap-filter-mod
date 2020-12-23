// Project:         Daggerfall Tools For Unity
// Copyright:       Copyright (C) 2009-2019 Daggerfall Workshop
// Web Site:        http://www.dfworkshop.net
// License:         MIT License (http://www.opensource.org/licenses/mit-license.php)
// Source Code:     https://github.com/Interkarma/daggerfall-unity
// Original Author: Gavin Clayton (interkarma@dfworkshop.net)
// Contributors: InconsolableCellist, Hazelnut, Numidium
// Extended:     Asesino, Pango
// Notes:
//

using UnityEngine;
using System;
using System.Collections.Generic;
using DaggerfallConnect.Arena2;
using DaggerfallConnect.Utility;
using DaggerfallWorkshop.Utility;
using DaggerfallWorkshop.Game.UserInterface;
using DaggerfallWorkshop.Game.Entity;
using DaggerfallWorkshop.Game.Items;
using DaggerfallWorkshop.Game.Utility;
using DaggerfallWorkshop.Game.Questing;
using DaggerfallWorkshop.Game.Banking;
using System.Linq;
using DaggerfallConnect;
using DaggerfallWorkshop.Game.Formulas;


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

            if (localFilterTextBox.HasFocus() && (Input.GetKeyDown(KeyCode.Return)))
                SetFocus(null);
        }

        protected void SetupTargetIconPanelFilterBox()
        {
            string toolTipText = string.Empty;
            toolTipText = "Press Filter Button to Open Filter Text Box.\rAnything typed into text box will autofilter.";

            localFilterTextBox = DaggerfallUI.AddTextBoxWithFocus(new Rect(new Vector2(15, 24), new Vector2(47, 8)), "filter pattern", NativePanel);
            localFilterTextBox.VerticalAlignment = VerticalAlignment.Bottom;
            localFilterTextBox.OnType += LocalFilterTextBox_OnType;
            localFilterTextBox.OverridesHotkeySequences = true;

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


        public override void OnPop()
        {
            ClearFilterFields();
            base.OnPop();
        }

        public new void UpdateAutomapView()
        {
            base.UpdateAutomapView();
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
            if (String.IsNullOrEmpty(filterString))
                return true;

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

            UpdateAutomapView();
        }
    }
}
