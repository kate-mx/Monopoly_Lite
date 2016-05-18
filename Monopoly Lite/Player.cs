using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly
{
    public class Player
    {
        // Деньги игроков
        int money;

        // Текущая позиция игрока на поле
        int boardSpace;

        // Сколько ходов игрок провёл в тюрьме.
        int turnsMissed;

        // Количество дублей, выпавших за один ход у игрока
        int numDoubles;

        // Флаг (в тюрьме игрок или нет)
        bool inJail;

        // Флаг проигрыша игрока
        bool gameOver;

        // Флаг (если игрок попал в тюрьму в этот ход)
        bool justInJail;

        /// <summary>
        /// Задаёт начальные значения для игроков
        /// </summary>
        public Player()
        {
            money = 1000;
            boardSpace = 0;
            turnsMissed = 0;
            numDoubles = 0;
            inJail = false;
            gameOver = false;
        }

        /// <summary>
        /// Возвращает клетку на поле.
        /// </summary>
        /// <returns> В какой клетке находится игрок </returns>
        public int getBoardSpace() { return boardSpace; }

        /// <summary>
        /// Возвращает деньги игрока
        /// </summary>
        /// <returns> Баланс игрока </returns>
        public int getMoney() { return money; }

        /// <summary>
        /// Возвращает если игрок в тюрьме.
        /// </summary>
        /// <returns> Если игрок в тюрьме </returns>
        public bool getInJail() { return inJail; }

        /// <summary>
        /// Возвращает количество пропущенных ходов.
        /// </summary>
        /// <returns> Пропущенные ходы </returns>
        public int getTurnsMissed() { return turnsMissed; }

        /// <summary>
        /// Возвращает количество выпавших дублей за ход
        /// </summary>
        /// <returns> Дубли за этот ход. </returns>
        public int getNumDoubles() { return numDoubles; }

        /// <summary>
        /// Обновление баланса игроков с учетом итога хода.
        /// </summary>
        /// <param name="amount"> Сумма добавляется к балансу </param>
        internal void updateMoney(int amount) { this.money += amount; }

        /// <summary>
        /// Set Играет ли ещё игрок или нет.
        /// </summary>
        /// <param name="gameover"> Будет ли игра для игрока закончена. </param>
        public void setGameOver(bool gameover) { gameOver = gameover; }

        /// <summary>
        /// Get Возвращает game over
        /// </summary>
        /// <returns> Закончена ли игра для игрока </returns>
        public bool getGameOver() { return gameOver; }

        /// <summary>
        /// Sets Попадает ли игрок в тюрьму на этом ходе.
        /// </summary>
        /// <param name="justinjail"> Попадает ли игрок в тюрьму на этом ходе. </param>
        public void setJustInJail(bool justinjail) { justInJail = justinjail; }

        /// <summary>
        /// Gets Попадает ли игрок в тюрьму на этом ходе.
        /// </summary>
        /// <returns> Попадает ли игрок в тюрьму на этом ходе. </returns>
        public bool getJustInJail() { return justInJail; }


        /// <summary>
        /// Перемещает игрока на данную клетку
        /// </summary>
        /// <param name="space"> Клетка для перемещения. </param>
        internal void moveTo(int space)
        {
            this.boardSpace = space;
            if (boardSpace == 4)
            {
                inJail = true;
                justInJail = true;
                turnsMissed = 0;
                numDoubles = 0;
            }
        }

        /// <summary>
        /// Передвижение на клетку + текущая клетка, 
        /// проверка застревает ли игрок в тюрьме.
        /// </summary>
        /// <param name="spaces"> Клетка для движения </param>
        /// <param name="isDouble"> Выпал ли у игрока дубль. </param>
        internal void move(int spaces, bool isDouble)
        {
            // Результат если игрок в тюрьме
            if (inJail)
            {
                if (isDouble || turnsMissed == 2)
                    inJail = false;
                else
                {
                    turnsMissed++;
                    return;
                }
            }

            // Изменение игрового поля
            boardSpace += spaces;

            // Если игрок попал на "Старт"
            if (boardSpace > 15)
            {
                boardSpace -= 16;
                money += 100;
            }

            // Если дубли
            if (isDouble && numDoubles == 2) // 3 дубля
            {
                moveTo(4);
                numDoubles = 0;
            }
            else if (isDouble)  // 1 или 2 дубля
                numDoubles++;
            else
                numDoubles = 0;
        }
    }
}
