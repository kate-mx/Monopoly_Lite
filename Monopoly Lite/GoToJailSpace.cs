using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Monopoly
{
    class GoToJailSpace : GameSpace
    {
        /// <summary>
        /// Определяет игрока в тюрьму и уведомляет игроков об этом.
        /// </summary>
        /// <param name="playerNum"> Текщий  индекс положения игроков </param>
        /// <param name="curPlayer"> Текущий объект игроков.</param>
        /// <returns> </returns>
        public override int doAction(ref int playerNum, ref Player curPlayer)
        {
            curPlayer.moveTo(4);
            MessageBox.Show("Вы попали в тюрьму!");
            return 0;
        }
    }
}
