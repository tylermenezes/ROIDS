﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace UICore
{
    /// <summary>
    /// A UI Element such as a button or an information bar etc.
    /// </summary>
    /// 
    public delegate void ControlEventHandler(Control sender);
    public abstract class Control : GUIElement
    {
        public GUIElement Parent { get; private set; }

        public event ControlEventHandler Selected;
        public event ControlEventHandler Deselected;

        public void Select()
        {
            //if (Parent is Control)
            //    ((Control)Parent).Parent.ActiveControl = (Control)Parent;
            if (Selected != null)
                Selected(this);
        }

        public void Deselect()
        {
            //ActiveControl = null;
            if (Deselected != null)
                Deselected(this);
        }
        /// <summary>
        /// Parent can only be set once!
        /// </summary>
        public void setParent(GUIElement parent)
        {
            if (Parent != null)
                throw new Exception("Control already has a parent!");
            Parent = parent;
        }
        public Control(Vector2 location, Vector2 size)
        {
            Controls = new List<Control>();
            Location = location;
            ElementSize = size;
        }

        
        public Vector2 RelativeLocation
        {
            get
            {
                return Parent.GetRelativeLocation(this.Location);
            }
            set
            {
                this.Location = Parent.GetAbsoluteLocation(value);
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            Parent.ActiveControl = this;
            base.OnMouseClick(e);
        }
        protected void Close()
        {
            Parent.RemoveControl(this);
        }
    }
}
