﻿using UnityEngine;

namespace ActiveCharacters.Shared.Components
{
    public abstract class DetectionReaction : MonoBehaviour
    {
        public abstract void Follow(BoxCollider target);
        public abstract void Forget();
    }
}