using Ejaw.TapToKill.Game.TimeManagment;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ejaw.TapToKill.Game
{
    /// <summary>
    /// Состояние игры
    /// </summary>
    public enum GameState
    {
        /// <summary>
        /// Состояние паузы
        /// </summary>
        PAUSE,
        /// <summary>
        /// Игра была перезапущена ->
        /// состояние <see cref="PLAY"/>
        /// </summary>
        RESTART,
        /// <summary>
        /// Состояние готовности игры
        /// </summary>
        PLAY,
        /// <summary>
        /// Игра завершена
        /// </summary>
        END
    }

    /// <summary>
    /// GameManager отвечает за поведение всей логики игры
    /// </summary>
    [RequireComponent( typeof( Timer ) )]
    public class GameManager : MonoBehaviour //Singleton<GameManager>
    {
        /// <summary>
        /// Вызывается при смене состояния игры
        /// </summary>
        public static event Action<GameState> OnGameStateChanged;

        //Текущее состояние игры
        private GameState m_GameState = GameState.PLAY;

        [SerializeField]
        private InstantiateManager m_InstantiateManager;

        /// <summary>
        /// Счётчик, по истечению которого завершится игра
        /// </summary>
        public Timer GameTimer { get; private set; }

        #region Mono

        private void OnValidate()
        {
            if ( !m_InstantiateManager )
            {
                m_InstantiateManager = FindObjectOfType<InstantiateManager>();
            }
        }

        private void Awake()
        {
            if ( !GameTimer )
            {
                GameTimer = GetComponent<Timer>();
            }
            GameTimer.OnTimeIsOver += OnTimeIsOver;            
        }       

        private void Start()
        {
            //Установить дефолтные настройки
            ContinueGame();
        }

        // Update is called once per frame
        private void Update()
        {
            //Управление состояниями игры
            if ( Input.GetKeyDown( KeyCode.Escape ) )
            {
                if ( m_GameState == GameState.PAUSE )
                {
                    ContinueGame();
                }
                else if ( m_GameState == GameState.PLAY )
                {
                    PauseGame();
                }
                else
                {
                    RestartGame();
                }
            }
        }

        #endregion

        private void OnTimeIsOver()
        {
            //Когда время вышло
            //это равносильно концу игры
            EndGame();
        }

        #region Действия с игрой

        /// <summary>
        /// Возобновить игровой процесс после паузы
        /// </summary>
        public void ContinueGame()
        {
            m_GameState = GameState.PLAY;
            OnGameStateChanged?.Invoke( GameState.PLAY );
            m_InstantiateManager.StopInstantiate = false;
            GameTimer.IsRunning = true;
            Time.timeScale = 1;
        }

        /// <summary>
        /// Поставить игру на паузу
        /// </summary>
        public void PauseGame()
        {
            m_GameState = GameState.PAUSE;
            OnGameStateChanged?.Invoke( GameState.PAUSE );
            m_InstantiateManager.StopInstantiate = true;
            GameTimer.IsRunning = false;
            Time.timeScale = 0;
        }

        /// <summary>
        /// Начать игру заново
        /// </summary>
        public void RestartGame()
        {
            m_GameState = GameState.PLAY;
            OnGameStateChanged?.Invoke( GameState.RESTART );
            GameTimer.RestartTimer();
            GameTimer.IsRunning = true;
            m_InstantiateManager.StopInstantiate = false;
            m_InstantiateManager.DestroyAllPopups();
            GameTimer.IsRunning = true;
            Time.timeScale = 1;
        }

        /// <summary>
        /// Перезапуск уровня
        /// </summary>
        [Obsolete( "Use RestartGame() instead." )]
        public void ReloadGameLevel()
        {
            SceneManager.LoadScene( SceneManager.GetActiveScene().name );
        }

        //Конец игры (завершается таймером)
        private void EndGame()
        {
            m_GameState = GameState.END;
            OnGameStateChanged?.Invoke( GameState.END );
            GameTimer.RestartTimer();
            GameTimer.IsRunning = false;
            m_InstantiateManager.StopInstantiate = true;
            m_InstantiateManager.DestroyAllPopups();
            GameTimer.IsRunning = false;
            Time.timeScale = 0;
        }

        #endregion
    }
}