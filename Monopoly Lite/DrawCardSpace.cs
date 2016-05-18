using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly
{
    class DrawCardSpace : GameSpace
    {
        /// <summary>
        /// Кидает кубик и 2 использует первое,
        /// чтобы решить ситуацию, а второй, чтобы определить величину ситуации.
        /// </summary>
        /// <param name="playerNum"> Индекс текущего игрока </param>
        /// <param name="curPlayer"> Текущий объект игрока </param>
        /// <returns> </returns>
        public override int doAction(ref int playerNum, ref Player curPlayer)
        {
            int rn1 = rand.Next(0, 2);
            int rn2 = rand.Next(0, 15);

            // Вычитаем деньги
            if (rn1 == 0)
            {
                rn2 *= -10;
                curPlayer.updateMoney(rn2);
            }

            // Прибавляем деньги
            else if (rn1 == 1)
            {
                rn2 *= 10;
                curPlayer.updateMoney(rn2);
            }

            // Перемещение в пространство
            else if (rn1 == 2)
                curPlayer.moveTo(rn2);

            return 0;
        }
    }
}
