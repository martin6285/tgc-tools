﻿using TGC.Tools.Utils.Modifiers;

namespace TGC.Tools.TerrainEditor.Panel
{
    public class TerrainEditorModifier : TgcModifierPanel
    {
        public TerrainEditorModifier(string varName, TgcTerrainEditor creator)
            : base(varName)
        {
            Control = new TerrainEditorControl(creator);
            contentPanel.Controls.Add(Control);
        }

        public TerrainEditorControl Control { get; }

        public override object getValue()
        {
            return null;
        }

        public void dispose()
        {
            Control.dispose();
        }
    }
}