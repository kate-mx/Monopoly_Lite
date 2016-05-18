using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly
{//olololol
    // ololololo2
       // ololololo3
    class Dice
    {
        // Игральная кость
        int DieA;
        int DieB;

        // Генерируем игр. кость
        Random rand = new Random();

        /// <summary>
        /// Уставливаем номер по умолчанию для игры в кости. Sets default number for dice.
        /// </summary>
        public Dice()
        {
            DieA = 1;
            DieB = 1;
        }

        /// <summary>
        /// Кидаем кости.
        /// </summary>
        public void RollDice()
        {
            // Changed to 4 sided die in an attempt to stop such drastic movements
            // Измененный на 4 примкнувших умирают в попытке остановить такие решительные движения
            DieA = rand.Next(1, 5);
            DieB = rand.Next(1, 5);
        }

        /// <summary>
        /// Получаем два числа(результаты броска). Gets the total of the two die.
        /// </summary>
        /// <returns> The total of the dice for movement. </returns>
        public int GetTotal() { return DieA + DieB; }

        /// <summary>
        /// Получает результат сброса для каждого кубика (результат броска)
        /// </summary>
        /// <param name="index"> Индекс кубика </param>
        /// <returns> Число, которое выпало на кубике.
        /// Если индекс не указан возвращаем -1. </returns>
        public int getNumber(int index)
        {
            if (index == 1)
                return DieA;
            else if (index == 2)
                return DieB;
            return -1;
        }

        /// <summary>
        /// Проверка на выпадание дубля.
        /// </summary>
        /// <returns> Если выпал дубль </returns>
        public bool isDoubles() { return DieA == DieB; }
    }
}
