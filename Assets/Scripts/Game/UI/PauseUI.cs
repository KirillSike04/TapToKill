using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ejaw.TapToKill.Game.UI
{
    /// <summary>
    /// Панелька с кнопками продолжения и рестарта игры
    /// </summary>
    public class PauseUI : MonoBehaviour
    {       
        [SerializeField]
        private GameManager m_GameManager;

        #region Mono

        protected void OnValidate()
        {
            if ( !m_GameManager )
            {
                m_GameManager = FindObjectOfType<GameManager>();
            }
        }

        protected void Awake()
        {            
            GameManager.OnGameStateChanged += OnGameStateChanged;
        }

        protected void Start()
        {
            
        }

        protected void OnDestroy()
        {
            GameManager.OnGameStateChanged -= OnGameStateChanged;
        }

        #endregion

        private void OnGameStateChanged( GameState obj )
        {
            switch ( obj )
            {
                case GameState.PAUSE:
                    //Отображение панели,
                    //если игра перешла в режим паузы
                    ShowPanel();
                    break;
                case GameState.PLAY:
                    //Скрытие панели,
                    //если игра перешла в режим игры
                    HidePanel();
                    break;
            }
        }

        #region Методы для кнопок

        //Вызов методов на кнопках:
        public void ContinueGame()
        {
            m_GameManager.ContinueGame();
        }

        public void HidePanel()
        {
            gameObject.SetActive( false );
        }

        public void RestartGame()
        {
            m_GameManager.RestartGame();
        }

        public void ShowPanel()
        {
            gameObject.SetActive( true );
        }

        public void ExitToMenu()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene( Menu.MenuManager.MENU_SCENE_NAME );
        }

        #endregion
    }
}