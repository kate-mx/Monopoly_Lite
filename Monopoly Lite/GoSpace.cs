using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly
{
    class GoSpace : GameSpace
    {
        /// <summary>
        /// Прибавляет деньги к балансу игрока, если он встал на клетку "Старт"
        /// </summary>
        /// <param name="playerNum"> Индекс текущего игрока. </param>
        /// <param name="curPlayer"> Объект текущего игрока. </param>
        /// <returns></returns>
        public override int doAction(ref int playerNum, ref Player curPlayer)
        {
            curPlayer.updateMoney(100);
            return 0;
        }
    }
}
