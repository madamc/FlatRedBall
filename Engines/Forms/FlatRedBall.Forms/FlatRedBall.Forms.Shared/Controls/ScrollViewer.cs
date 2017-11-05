﻿using Gum.Wireframe;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlatRedBall.Forms.Controls
{
    public class ScrollViewer : FrameworkElement
    {
        #region Fields/Properties

        ScrollBar verticalScrollBar;

        GraphicalUiElement innerPanel;
        public GraphicalUiElement InnerPanel => innerPanel;

        GraphicalUiElement clipContainer;

        #endregion

        #region Initialize

        protected override void ReactToVisualChanged()
        {
            verticalScrollBar = new ScrollBar();
            verticalScrollBar.Visual = Visual.GetGraphicalUiElementByName("VerticalScrollBarInstance");
            verticalScrollBar.ValueChanged += HandleVerticalScrollBarValueChanged;

            innerPanel = Visual.GetGraphicalUiElementByName("InnerPanelInstance");
            clipContainer = Visual.GetGraphicalUiElementByName("ClipContainerInstance");

            UpdateVerticalScrollBarValues();

            base.ReactToVisualChanged();
        }

        #endregion

        #region Event Handlers

        private void HandleVerticalScrollBarValueChanged(object sender, EventArgs e)
        {
            innerPanel.Y = -(float)verticalScrollBar.Value;
        }

        #endregion

        #region UpdateTo methods

        // Currently this is public because Gum objects don't have events
        // when positions and sizes change. Eventually, we'll have this all
        // handled internally and this can be made private.
        public void UpdateVerticalScrollBarValues()
        {
            verticalScrollBar.Minimum = 0;
            verticalScrollBar.ViewportSize = clipContainer.GetAbsoluteHeight();
            var maxValue = innerPanel.GetAbsoluteHeight() - clipContainer.GetAbsoluteHeight();

            maxValue = System.Math.Max(0, maxValue);

            verticalScrollBar.Maximum = maxValue;

            verticalScrollBar.SmallChange = 10;
            verticalScrollBar.LargeChange = verticalScrollBar.ViewportSize;


        }

        #endregion
    }
}
