using System;
using UnityEngine;

namespace Game.Entity
{
    public class HitHandler : MonoBehaviour
    {
        
        public bool isInvincible = false;
        public bool isDeath = false;
        public bool IsNotHittable => isInvincible || isDeath;
        
        public class HitEventArgs : EventArgs
        {
            public int dmg;
        }

        public EventHandler<HitEventArgs> Handler;

        public bool Raise(object sender, HitEventArgs hitEventArgs)
        {
            if (IsNotHittable)
                return false;
            Handler.Invoke(sender, hitEventArgs);
            return true;
        }
    }
}