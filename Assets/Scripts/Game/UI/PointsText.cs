using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ejaw.TapToKill.Game.UI
{
    /// <summary>
    /// Текст набранных игровых очков
    /// </summary>
    public class PointsText : InfoValueText
    {
        [Tooltip( "Ссылка на менеджер, отвечающий на подсчёт очков" )]
        [SerializeField]
        private PointsManagment.PointsManager m_PointsManager;

        //Флаг конца игры
        private bool m_IsGameEnd = false;

        #region Mono

        protected void OnValidate()
        {
            if ( !m_PointsManager )
            {
                m_PointsManager = FindObjectOfType<PointsManagment.PointsManager>();
            }
        }

        protected override void Awake()
        {
            base.Awake();
           
            GameManager.OnGameStateChanged += OnGameStateChanged;
        }      

        protected void Update()
        {
            if ( !m_IsGameEnd )
            {
                ValueText = m_PointsManager.Points.ToString();
            }
        }

        protected void OnDestroy()
        {
            GameManager.OnGameStateChanged -= OnGameStateChanged;
        }

        #endregion

        private void OnGameStateChanged( GameState obj )
        {
            if ( obj == GameState.END )
            {
                //Добавление в результат лучший набранный игроком счёт
                string bestScore = string.Format( " (BestScore: {0})", m_PointsManager.BestScore );
                ValueText += bestScore;

                m_IsGameEnd = true;
            }
            else
            {
                m_IsGameEnd = false;
            }
        }
    }
}