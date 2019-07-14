using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace Ejaw.TapToKill.Network
{
    public class NetworkConnection : MonoBehaviourPunCallbacks
    {
        [Tooltip( "Автоматически переподключиться в случае дисконнекта" )]
        public bool ReconnectAfterDisconnect = true;

        /// <summary>
        /// Вызывается при успешном подключении
        /// </summary>
        public event Action OnConnectionToServer;
        /// <summary>
        /// Вызывается при дисконнекте
        /// </summary>
        public event Action OnDisconnectFromServer;

        private IEnumerator Start()
        {
            //Ожидание соединения
            yield return WaitingInternetReachable();
            //Подключение
            Connect();
        }

        private void Update()
        {

        }

        /// <summary>
        /// Подключиться к серверу
        /// </summary>
        public void Connect()
        {
            if ( !PhotonNetwork.IsConnected )
            {
                PhotonNetwork.ConnectUsingSettings();
            }
            else
            {
                OnConnectionToServer?.Invoke();
            }
        }

        /// <summary>
        /// Переподключиться к серверу
        /// </summary>
        public void Reconnect()
        {
            StartCoroutine( ReconnectCoroutine() );
        }

        // Короутина переподключения к серверу
        private IEnumerator ReconnectCoroutine()
        {
            //Ожидание соединения
            yield return WaitingInternetReachable();
            //Переподключение
            PhotonNetwork.Reconnect();
        }

        private WaitWhile WaitingInternetReachable()
        {
            return new WaitWhile( () =>
            Application.internetReachability == NetworkReachability.NotReachable );
        }

        override public void OnConnected()
        {
            base.OnConnected();
        }

        override public void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();
            Debug.Log( "Connected." );

            OnConnectionToServer?.Invoke();
            PhotonNetwork.JoinLobby();
        }

        public override void OnDisconnected( DisconnectCause cause )
        {
            base.OnDisconnected( cause );

            OnDisconnectFromServer?.Invoke();

            if ( ReconnectAfterDisconnect )
            {
                Reconnect();
            }
        }
    }
}
