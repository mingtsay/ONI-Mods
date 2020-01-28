﻿using PeterHan.PLib.UI;
using Pholib;
using UnityEngine;

namespace Notepad
{
    public class NotepadSideScreen : SideScreenContent
    {

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            titleKey = PHO_STRINGS.NOTEPAD.NAME;
            activateOnSpawn = true;


            /*editButton.onClick += delegate()
            {
                ManagementMenu.Instance.SendMessage("Coucou Pholith!");
            };*/
        }

        public override bool IsValidForTarget(GameObject target)
        {
            return target.GetComponent<Notepad>() != null;
        }

        public override void SetTarget(GameObject target)
        {
            if (target == null)
            {
                Debug.LogError("Invalid gameObject received");
                return;
            }
            targetNotepad = target.GetComponent<Notepad>();
            if (targetNotepad == null)
            {
                Debug.LogError("The gameObject received does not contain a Notepad component");
                return;
            }
            UpdateLabels();
        }

        private void UpdateLabels()
        {
            Logs.Log("update");
            Logs.Log(targetNotepad.activateText);
            Logs.Log(Control.DescriptionField.Text);
            Control.DescriptionField.Text = targetNotepad.activateText;
            Control.DescriptionField.OnTextChanged += (go, text) => targetNotepad.activateText = Control.DescriptionField.Text;
        }

        public Notepad targetNotepad;

        public NotepadControl Control { get; set; }
    }
}
