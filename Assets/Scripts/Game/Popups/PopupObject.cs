using Ejaw.TapToKill.Game.TimeManagment;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ejaw.TapToKill.Game.Popups
{
    /// <summary>
    /// Объекты, на которые пользователь должен нажимать
    /// </summary>
    public class PopupObject : MonoBehaviour
    {
        //Объект будет помечен данным тегом
        public const string POPUP_TAG = "Popup";

        /// <summary>
        /// Вызывается при нажатии на данный объект
        /// </summary>
        public static event Action<PopupObjectAsset> OnPopupTaped;
        /// <summary>
        /// Вызывается при уничтожении объекта
        /// </summary>
        public static event Action<PopupObject> OnPopupDestroy;

        [Tooltip( "Asset с информацией о типе данного объекта" )]
        [SerializeField]
        private PopupObjectAsset m_PopupObjectAsset;

        /// <summary>
        /// Asset с информацией о типе данного объекта
        /// </summary>
        public PopupObjectAsset PopupObjectAsset
        {
            get
            {
                return m_PopupObjectAsset;
            }
            protected set
            {
                m_PopupObjectAsset = value;
            }
        }

        private Material m_Material;

        #region Mono

        protected void Awake()
        {
            //Объект уничтожается в случае отсутствия ссылки
            //на требуемый ScriptableObject
            if ( m_PopupObjectAsset == null )
            {
                Destroy( gameObject );
                return;
            }

            m_Material = GetComponent<MeshRenderer>().material;
            tag = POPUP_TAG;
            
        }

        protected void OnEnable()
        {
            //Установить цвет материала в соответствии с настройками
            m_Material.color = m_PopupObjectAsset.Color;

            //Установить время для самоуничтожения, если значение не 0
            //иначе сделать его неавтонеуничтожаемым
            Timer timer = GetComponent<Timer>();
            if ( timer )
            {
                if ( m_PopupObjectAsset.TimeForSelfDestroy > 0 )
                {
                    timer.TotalTime = m_PopupObjectAsset.TimeForSelfDestroy;
                }
                else
                {
                    timer.IsRunning = false;
                }               
            }            
        }      

        protected void OnDestroy()
        {
            OnPopupDestroy?.Invoke( this );
        }

        protected void OnTriggerEnter( Collider collider )
        {
            //Уничтожить объект
            //при соприкосновении с другим таким же объектом
            if ( collider.tag == POPUP_TAG )
            {
                Destroy( gameObject );
            }
        }

        //Работает и на мобильных устройствах,
        //однако желательно кидать райкасты и проверять попадание в объекты,
        //так как ожидается уменьшение производительности
        private void OnMouseDown()
        {
            OnTappedAtObject();
        }

        #endregion

        private void OnTappedAtObject()
        {
            //Не стоит уничтожать объекты когда время остановилось
            if ( Time.deltaTime == 0 )
            {
                return;
            }

            //Вызвать событие нажатия на объект, уничтожив его
            OnPopupTaped?.Invoke( m_PopupObjectAsset );
            Destroy( gameObject );
        }
    }
}