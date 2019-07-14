using Ejaw.TapToKill.Game.Popups;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Ejaw.TapToKill.Game
{
    /// <summary>
    /// InstantiateManager отвечает за создание
    /// объектов, на которые пользователь будет нажимать
    /// </summary>
    public class InstantiateManager : MonoBehaviour //Singleton<InstantiateManager>
    {
        //Парент для всех объектов, чтобы не засорять иерархию объектов
        private const string POPUPS_PARENT_NAME = "[POPUPS]";
        /// <summary>
        /// Максимальное количество одновременно существующих объектов,
        /// если не указано иное
        /// </summary>
        public const int DEFAULT_MAX_POPUPS_COUNT = 10;

        /// <summary>
        /// Центральная точка зоны респа
        /// </summary>
        [Tooltip( "Центральная точка зоны респа" )]
        public Vector3 CenterPoint;
        /// <summary>
        /// Площадь респа
        /// </summary>
        [Tooltip( "Площадь респа" )]
        public Vector2 Area;

        /// <summary>
        /// Максимальное количество одновременно существующих объектов,
        /// дефолтно - <see cref="DEFAULT_MAX_POPUPS_COUNT"/>
        /// </summary>
        [Tooltip( "Максимальное количество одновременно существующих объектов" )]
        public int MaxPopupsCount = DEFAULT_MAX_POPUPS_COUNT;

        /// <summary>
        /// Остановить создание новых объектов
        /// </summary>
        public bool StopInstantiate = false;

        [Space(5)]

        [Tooltip( "Список префабов" )]
        [SerializeField]
        private PopupObject[] m_PopupPrefabs;

        //Парент для созданных объектов
        private Transform m_PopupsParent;

        /// <summary>
        /// Список всех созданных объектов
        /// </summary>
        public List<PopupObject> AllPopups { get; private set; } = new List<PopupObject>();

        #region Mono

        private void Awake()
        {
            //Подписка на уничтожение объекта
            PopupObject.OnPopupDestroy += OnPopupsDestroy;
        }

        private void OnPopupsDestroy( PopupObject obj )
        {
            //чтобы удалить его из списка
            AllPopups.Remove( obj );
        }

        private void Start()
        {
            //Удаление "пустых" объектов
            RemoveNullPopups();
            //Сортировка по вероятности
            SortAtPercentChance();

            //Создание парента
            m_PopupsParent = new GameObject( POPUPS_PARENT_NAME ).transform;
            m_PopupsParent.SetAsLastSibling();
        }

        private void Update()
        {
            if ( !StopInstantiate )
            {
                if ( AllPopups.Count < MaxPopupsCount )
                {
                    InstantiateRandomPopup();
                }
            }
        }

        //Отрисовка Gizmos для отображения зоны респа
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube( CenterPoint, new Vector3( Area.x, 1, Area.y ) );
        }

        #endregion

        private void RemoveNullPopups()
        {
            //Убрать объекты с неуказанным PopupObjectAsset
            m_PopupPrefabs = m_PopupPrefabs.Where( e => e.PopupObjectAsset ).ToArray();
        }

        //Сортировка по вероятности появлению данного типа объекта
        private void SortAtPercentChance()
        {
            //Сортировка нужна, чтобы была бОльшая возможность
            //для создания объектов с низким шансом появления
            m_PopupPrefabs = m_PopupPrefabs.OrderBy( e => e.PopupObjectAsset.PercentChance ).ToArray();
        }

        #region Public methods

        /// <summary>
        /// Получить случайный объект, с учётом вероятности его выпадения
        /// </summary>
        /// <returns>Случайный префаб из списка</returns>
        public PopupObject GetRandomPopup()
        {
            if ( m_PopupPrefabs == null || m_PopupPrefabs.Length == 0 )
                return null;

            PopupObject randomPopup = null;
            while ( randomPopup == null )
            {
                foreach ( var popup in m_PopupPrefabs )
                {
                    if ( Random.Range( 0, 100 + 1 ) < popup.PopupObjectAsset.PercentChance )
                    {
                        return popup;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Случайная точка на поле
        /// </summary>
        public Vector3 GetRandomPositionAtArea()
        {
            Vector2 zone = Area / 2;
            float x = Random.Range( CenterPoint.x - zone.x, CenterPoint.x + zone.x );
            float y = CenterPoint.y;
            float z = Random.Range( CenterPoint.z - zone.y, CenterPoint.z + zone.y );
            return new Vector3( x, y, z );
        }

        /// <summary>
        /// Создать случайный объект из списка префабов
        /// </summary>
        /// <returns></returns>
        public PopupObject InstantiateRandomPopup()
        {
            PopupObject popupObject = Instantiate( GetRandomPopup(), GetRandomPositionAtArea(),
                Quaternion.identity, m_PopupsParent );
            AllPopups.Add( popupObject );
            return popupObject;
        }

        /// <summary>
        /// Уничтожить все объекты
        /// </summary>
        public void DestroyAllPopups()
        {
            AllPopups.ForEach( e => Destroy( e.gameObject ) );
            AllPopups.Clear();
        }

        #endregion
    }
}