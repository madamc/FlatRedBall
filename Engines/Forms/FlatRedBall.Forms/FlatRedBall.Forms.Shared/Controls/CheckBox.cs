﻿using FlatRedBall.Forms.Controls.Primitives;
using FlatRedBall.Gui;
using Gum.Wireframe;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlatRedBall.Forms.Controls
{
    public class CheckBox : ToggleButton
    {
        #region Fields/Properties

        private GraphicalUiElement textComponent;

        private RenderingLibrary.Graphics.Text coreTextObject;

        public string Text
        {
            get
            {
#if DEBUG
                ReportMissingTextInstance();
#endif
                return coreTextObject.RawText;
            }
            set
            {
#if DEBUG
                ReportMissingTextInstance();
#endif
                // go through the component instead of the core text object to force a layout refresh if necessary
                textComponent.SetProperty("Text", value);
            }
        }

        public override bool IsEnabled
        {
            get
            {
                return base.IsEnabled;
            }
            set
            {
                base.IsEnabled = value;
                if (!IsEnabled)
                {
                    // todo - to add focus eventually
                    //HasFocus = false;
                }
                UpdateState();
            }
        }

        #endregion

        #region Initialize Methods

        protected override void ReactToVisualChanged()
        {
            // text component is optional:
            textComponent = base.Visual.GetGraphicalUiElementByName("TextInstance");

            base.ReactToVisualChanged();

            // In case the check is visible - the checkbox starts in a IsChecked = false state:
            UpdateState();
        }

        #endregion

        #region UpdateTo Methods

        protected override void UpdateState()
        {
            if (Visual == null) //don't try to update the UI when the UI is not set yet, mmmmkay?
                return;

            var cursor = GuiManager.Cursor;

            if (IsChecked == true)
            {
                if (IsEnabled == false)
                {
                    Visual.SetProperty("CheckBoxCategoryState", "DisabledOn");
                }
                //else if (HasFocus)
                //{
                //    Visual.SetProperty("TextBoxCategoryState", "Selected");
                //}
                else if (GetIfIsOnThisOrChildVisual(cursor))
                {
                    if (cursor.WindowPushed == Visual && cursor.PrimaryDown)
                    {
                        Visual.SetProperty("CheckBoxCategoryState", "PushedOn");
                    }
                    else
                    {
                        Visual.SetProperty("CheckBoxCategoryState", "HighlightedOn");
                    }
                }
                else
                {
                    Visual.SetProperty("CheckBoxCategoryState", "EnabledOn");
                }
            }
            else if (IsChecked == false)
            {
                if (IsEnabled == false)
                {
                    Visual.SetProperty("CheckBoxCategoryState", "DisabledOff");
                }
                //else if (HasFocus)
                //{
                //    Visual.SetProperty("TextBoxCategoryState", "Selected");
                //}
                else if (Visual.HasCursorOver(cursor))
                {
                    if (cursor.WindowPushed == Visual && cursor.PrimaryDown)
                    {
                        Visual.SetProperty("CheckBoxCategoryState", "PushedOff");
                    }
                    else
                    {
                        Visual.SetProperty("CheckBoxCategoryState", "HighlightedOff");
                    }
                }
                else
                {
                    Visual.SetProperty("CheckBoxCategoryState", "EnabledOff");
                }
            }
            else
            {
                // todo - handle the indeterminate state here
            }

        }

        #endregion

        protected override void OnClick()
        {
            if(IsChecked == true)
            {
                IsChecked = false;
            }
            else // false or indeterminte
            {
                IsChecked = true;
            }
        }

        #region Utilities

#if DEBUG
        private void ReportMissingTextInstance()
        {
            if (textComponent == null)
            {
                throw new Exception(
                    "This button was created with a Gum component that does not have an instance called 'text'. A 'text' instance must be added to modify the radio button's Text property.");
            }
        }
#endif

        #endregion
    }
}