using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ejaw.TapToKill.Game.TimeManagment
{
    /// <summary>
    /// Таймер, отсчитывающий время до конца чего-либо
    /// </summary>
    public class Timer : MonoBehaviour
    {
        /// <summary>
        /// Время вышло!
        /// </summary>
        public event Action OnTimeIsOver;

        /// <summary>
        /// Будет ли работать данный таймер?
        /// </summary>
        public bool IsRunning;
        /// <summary>
        /// Общее время, которое отсчитывается
        /// </summary>
        public float TotalTime;

        [Tooltip( "Оставшееся время" )]
        [SerializeField]
        private float m_RemainingTime;
        /// <summary>
        /// Оставшееся время данного таймера
        /// </summary>
        public float RemainingTime
        {
            get { return m_RemainingTime; }
            protected set
            {
                m_RemainingTime = value;
            }
        }

        /// <summary>
        /// Время вышло?
        /// </summary>
        public bool TimeIsOver
        {
            get
            {
                return RemainingTime <= Time.deltaTime;
            }
        }

        /// <summary>
        /// Перезапустить таймер
        /// </summary>
        public void RestartTimer()
        {
            RemainingTime = TotalTime;
        }

        #region Mono

        protected virtual void Start()
        {
            RestartTimer();
        }

        protected virtual void Update()
        {
            //Если таймер работает - каждый кадр вычитаем время
            //иначе вызов события и остановка таймера
            if ( IsRunning )
            {
                if ( TimeIsOver )
                {
                    OnTimeIsOver?.Invoke();
                    RemainingTime = 0;
                    IsRunning = false;
                }
                else
                {
                    RemainingTime -= Time.deltaTime;
                }
            }
        }

        #endregion
    }
}