using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ejaw.TapToKill.Game.UI
{
    /// <summary>
    /// Текст, состоящий из двух строк:
    /// описания и выводимого значения
    /// </summary>
    [RequireComponent( typeof( Text ) )]
    public abstract class InfoValueText : MonoBehaviour
    {
        //Формат выводимого текста
        private const string FORMAT_TEXT = "{0} {1}";

        [Tooltip( "Текст-описание" )]
        [SerializeField]
        private string m_InfoText;
        [Tooltip( "Значение" )]
        [SerializeField]
        private string m_ValueText;

        /// <summary>
        /// Первая часть текста - описание
        /// </summary>
        public string InfoText
        {
            get { return m_InfoText; }
            set
            {
                m_InfoText = value;
                SetText();
            }
        }

        /// <summary>
        /// Вторая часть текста - значение
        /// </summary>
        public string ValueText
        {
            get { return m_ValueText; }
            set
            {
                m_ValueText = value;
                SetText();
            }
        }

        private Text m_Text;

        protected virtual void Awake()
        {
            m_Text = GetComponent<Text>();
        }

        /// <summary>
        /// Установить текст
        /// </summary>
        public void SetText()
        {
            m_Text.text = string.Format( FORMAT_TEXT, InfoText, ValueText );
        }
    }
}