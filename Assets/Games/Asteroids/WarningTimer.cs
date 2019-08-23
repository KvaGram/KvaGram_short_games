using UnityEngine;

namespace KvaGames.Asteroids
{
    public class WarningTimer
    {
        public WarningType w;
        public float t;
        public WarningTimer(WarningType w, float t)
        {
            this.w = w;
            this.t = t;
        }

        public bool Tick()
        {
            t -= Time.deltaTime;
            if (t <= 0)
                return true;
            return false;
        }
    }
}