using System;
using UnityEngine;

public class TrashStation : MonoBehaviour, IInteractable
    {
        public EventHandler OnUse;

        public void Interact()
        {
            if (!Player.Instance.HandleSystem.HaveAnyItemSelected()) return;
            Player.Instance.HandleSystem.GetSelectedItem().DestroySelf();
            OnUse?.Invoke(this, EventArgs.Empty);
        }
    }
