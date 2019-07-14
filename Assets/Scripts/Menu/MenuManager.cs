using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ejaw.TapToKill.Menu
{
    public class MenuManager : MonoBehaviour
    {
        /// <summary>
        /// Название сцены меню
        /// </summary>
        public const string MENU_SCENE_NAME = "MainMenu";
        /// <summary>
        /// Название сцены, которая будет загружена при нажатии на кнопку в меню
        /// </summary>
        public const string GAME_SCENE_NAME = "Game";
       
        [SerializeField]
        private Network.NetworkConnection m_NetworkConnection;

        [Tooltip( "Отображается, если нет подключения к серверу" )]
        [SerializeField]
        private GameObject m_ConnectionPanel = null;

        [Tooltip( "Отображается, если удалось подключиться" )]
        [SerializeField]
        private GameObject m_MenuPanel = null;

        #region Mono

        private void OnValidate()
        {
            if ( !m_NetworkConnection )
            {
                m_NetworkConnection = FindObjectOfType<Network.NetworkConnection>();
            }
        }

        private void OnEnable()
        {
            //Дефолтные настройки
            //(скрыть кнопку старта)
            OnDisconnected();

            //Подписка на события
            m_NetworkConnection.OnConnectionToServer += OnConnected;
            m_NetworkConnection.OnDisconnectFromServer += OnDisconnected;
        }

        #endregion

        private void OnConnected()
        {
            m_ConnectionPanel?.SetActive( false );
            m_MenuPanel?.SetActive( true );
        }

        private void OnDisconnected()
        {
            m_ConnectionPanel?.SetActive( true );
            m_MenuPanel?.SetActive( false );
        }

        /// <summary>
        /// Загрузить игровую сцену
        /// </summary>
        public void LoadGameScene()
        {
            SceneManager.LoadScene( GAME_SCENE_NAME );
        }
    }
}