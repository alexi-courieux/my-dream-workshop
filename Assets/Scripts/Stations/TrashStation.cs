using System;
using UnityEngine;

public class TrashStation : MonoBehaviour, IInteractable
    {
        public EventHandler OnUse;

        public void Interact()
        {
            if (!Player.Instance.HandleSystem.HaveAnyItems()) return;
            Player.Instance.HandleSystem.GetItem().DestroySelf();
            OnUse?.Invoke(this, EventArgs.Empty);
        }
    }
