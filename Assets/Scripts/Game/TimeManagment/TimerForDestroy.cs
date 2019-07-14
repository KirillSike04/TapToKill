using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ejaw.TapToKill.Game.TimeManagment
{
    /// <summary>
    /// Таймер, по истечению которого объект уничтожится
    /// </summary>
    public class TimerForDestroy : Timer
    {
        protected virtual void Awake()
        {
            //Подписка на событие
            OnTimeIsOver += Destroy;
        }

        private void Destroy()
        {
            //Уничтожение
            Destroy( gameObject );
        }
    }
}