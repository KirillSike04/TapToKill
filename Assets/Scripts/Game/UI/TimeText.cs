using Ejaw.TapToKill.Game.TimeManagment;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ejaw.TapToKill.Game.UI
{
    /// <summary>
    /// Текст оставшегося времени
    /// </summary>
    public class TimeText : InfoValueText
    {
        //Маска для вывода секунд на экран
        private const string TIME_MASK = "0.00";

        [SerializeField]
        private GameManager m_GameManager;

        protected void OnValidate()
        {
            if ( !m_GameManager )
            {
                m_GameManager = FindObjectOfType<GameManager>();
            }
        }

        #region Mono

        protected override void Awake()
        {
            base.Awake();            
        }

        protected void Update()
        {
            ValueText = m_GameManager.GameTimer.RemainingTime.ToString( TIME_MASK );
        }

        #endregion
    }
}