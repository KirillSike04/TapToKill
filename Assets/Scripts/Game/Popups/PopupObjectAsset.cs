using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ejaw.TapToKill.Game.Popups
{
    /// <summary>
    /// ScriptableObject с информацией о типе объекта
    /// </summary>
    [CreateAssetMenu]
    public class PopupObjectAsset : ScriptableObject
    {
        /// <summary>
        /// Это добавляет очков пользователю?
        /// </summary>
        [Tooltip( "Добавляет счёт пользователя?" )]
        public bool IsPositive = true;
        /// <summary>
        /// Цвет окраски объекта
        /// </summary>
        [Tooltip( "Цвет окраски объекта" )]
        public Color Color;
        /// <summary>
        /// Значение, на которое изменится общий счёт
        /// </summary>
        [Tooltip( "Значение, на которое изменится общий счёт" )]
        public byte Score;
        /// <summary>
        /// Вероятность выпадения данного объекта
        /// </summary>
        [Tooltip( " Вероятность выпадения данного объекта" )]
        public byte PercentChance = 50;
        /// <summary>
        /// Автоматическое уничтожение объекта через
        /// </summary>
        [Tooltip( "Автоматическое уничтожение объекта" )]
        public byte TimeForSelfDestroy = 5;
    }
}