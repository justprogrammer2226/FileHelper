using System.Collections.Generic;

namespace PasswordHelper
{
    internal interface IMenu
    {
        /// <summary> Заголовок меню. </summary>
        /// <remarks> Заголовок меню будет отображаться при показе меню. </remarks>
        string Title { get; }

        /// <summary> Список опций меню. </summary>
        List<Option> Options { get; }

        /// <summary> Показать меню. </summary>
        void Show();
    }
}
