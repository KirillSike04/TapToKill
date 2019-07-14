using Ejaw.TapToKill.Game.Popups;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ejaw.TapToKill.Game.PointsManagment
{
    /// <summary>
    /// Класс, отвечающий за сохранение и запись 
    /// текущего количества очков и лучшего результата
    /// </summary>
    public class PointsManager : MonoBehaviour //Singleton<PointsManager>
    {
        //Ключ для записи в PlayerPrefs лучшего результата
        private const string BEST_SCORE_KEY = "BestScore";

        /// <summary>
        /// Текущее количество очков
        /// </summary>
        public int Points { get; private set; }
        /// <summary>
        /// Лучший результат
        /// </summary>
        public int BestScore { get; private set; }

        private void Awake()
        {
            PopupObject.OnPopupTaped += OnPopudTaped;
            GameManager.OnGameStateChanged += OnGameStateChanged;
        }      

        private void Start()
        {
            //Получить значение лучшего результата из записи
            BestScore = LoadFromPrefs();
        }

        private void OnPopudTaped( PopupObjectAsset obj )
        {
            //Прибавить количество очков, если был нажат Positive объект
            //иначе отнять это количество
            if ( obj.IsPositive )
            {
                Points += obj.Score;
            }
            else
            {
                Points -= obj.Score;
            }
        }

        private void OnGameStateChanged( GameState obj )
        {
            if ( obj == GameState.END )
            {
                //Сохранить лучший результат 
                //по окончанию игры
                SaveBestScore();                
            }
            else if ( obj == GameState.RESTART )
            {
                //При рестарте количество очков обнуляется
                Points = 0;
            }
        }

        private void OnDestroy()
        {
            PopupObject.OnPopupTaped -= OnPopudTaped;
            GameManager.OnGameStateChanged -= OnGameStateChanged;

            SaveBestScore();
        }

        private void SaveBestScore()
        {
            if ( Points > BestScore )
            {
                SaveToPrefs();
            }
        }

        //Сохранить результат
        private void SaveToPrefs()
        {
            PlayerPrefs.SetInt( BEST_SCORE_KEY, Points );
        }

        //Получить результат
        private int LoadFromPrefs()
        {
           return PlayerPrefs.GetInt( BEST_SCORE_KEY, 0 );
        }

        /// <summary>
        /// Поставить значение лучшего результата в 0
        /// </summary>
        [ContextMenu("ClearPrefs")]
        public void ClearPrefs()
        {
            PlayerPrefs.SetInt( BEST_SCORE_KEY, 0 );
        }
    }
}