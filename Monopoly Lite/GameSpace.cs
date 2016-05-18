using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly
{
    public class GameSpace
    {

        // Строка с типом клетки.
        private string SpaceType;

        // Случайный объект для клетки "Шанс" 
        protected Random rand = new Random();

        /// <summary>
        /// Пустой параметр без перегрузки
        /// </summary>
        public GameSpace()
        {
        }

        /// <summary>
        /// Определяет тип клетки
        /// </summary>
        /// <param name="iniSpaceType"> Тип клетки </param>
        public GameSpace(string iniSpaceType)
        {
            SpaceType = iniSpaceType;
        }

        /// <summary>
        /// Функции различны в производных классах. Ничего не делает здесь.
        /// Functions differently in derived classes. Does nothing here.
        /// </summary>
        /// <param name="PlayerNum"> Индекс текущего игрока. </param>
        /// <param name="CurPlayer"> Объект текущего игрока. </param>
        /// <returns> </returns>
        public virtual int doAction(ref int PlayerNum, ref Player CurPlayer) { return 0; }

        /// <summary>
        /// Ничего не делает
        /// </summary>
        /// <param name="playerNum"> Индекс текущего игрока. </param>
        /// <param name="curPlayer"> Объект текущего игрока. </param>
        public virtual void addHouse(int playerNum, ref Player curPlayer) { return; }

        /// <summary>
        /// Ничего не делает
        /// </summary>
        /// <returns> </returns>
        public virtual int getOwner() { return -1; }

        /// <summary>
        /// Возвращает тип клетка
        /// </summary>
        /// <returns> Какого типа клетка (возвр). </returns>
        public string GetSpaceType()
        {
            return SpaceType;
        }
    }
}
