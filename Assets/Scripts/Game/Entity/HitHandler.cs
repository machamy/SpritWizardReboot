using System;
using UnityEngine;

namespace Game.Entity
{
    public class HitHandler : MonoBehaviour
    {
        public class HitEventArgs : EventArgs
        {
            public int dmg;
        }

        public EventHandler<HitEventArgs> Handler;

        public void Raise(object sender, HitEventArgs hitEventArgs) => Handler.Invoke(sender, hitEventArgs);
    }
}