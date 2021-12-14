using System;
using System.Collections;
using System.Collections.Generic;
using ArchToolkit.Character;
using UnityEngine;

namespace ArchToolkit.InputSystem{
    public class SetActiveIfCameraType : MonoBehaviour
    {
        public MovementType type;

        void Start() {
            ArchToolkitManager.Instance.visitor.OnMovementTypeChanged+=this.OnMovementTypeChanged;
            this.OnMovementTypeChanged(ArchToolkitManager.Instance.visitor.MovementType);
        }

        private void OnMovementTypeChanged(MovementType t)
        {
            this.gameObject.SetActive(t==type);
        }
    }
}

