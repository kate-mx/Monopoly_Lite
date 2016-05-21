﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Monopoly
{
    public partial class Form1 : Form
    {
        //StreamWriter myOutFile = new StreamWriter("out.txt");

        // Массив игроков. Варьируется в размерах (2 - 4).
        Player[] playerArray;

        // Массив пространств на доске.
        GameSpace[] spaceArray = new GameSpace[16];

        // Выброс костей
        Dice dice = new Dice();

        // Массив цветов, в котором текущий индекс игрока могут быть взаимозаменяемыми.
        // Ex. color[currentPlayer] текщий цыет игроков
        Color[] colorArray = new Color[4];

        // Количество игроков
        int QPlayers;

        // Индекс текущего игрока.
        int currentPlayer;

        // Индекс победителя.
        int winner;

        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// выводит информацию об игроке в файл для целей отладки.
        /// </summary>
        /// <param name="mPlayer"> Ссылка на игрока, чтобы получить информацию от </param>
        void showPlayer(ref Player mPlayer)
        {
            /*
            myOutFile.WriteLine("Money: " + mPlayer.getMoney().ToString());
            myOutFile.WriteLine("BoardSpace: " + mPlayer.getBoardSpace().ToString());
            myOutFile.WriteLine("In Jail: " + mPlayer.getInJail().ToString());
            myOutFile.WriteLine("Doubles: " + mPlayer.getNumDoubles().ToString());
            myOutFile.WriteLine("Turns Missed: " + mPlayer.getTurnsMissed().ToString());
            myOutFile.WriteLine("------");
             * */
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Принимает все статистику игроков и выводит их в соответствующие текстовые поля
        /// </summary>
        private void updateStats()
        {
            int numPlayers = Int32.Parse(QPlayersIn.Text);

            txtCurPlayer.Text = (currentPlayer + 1).ToString();
            if (numPlayers >= 1) txtPlayer1M.Text = playerArray[0].getMoney().ToString();
            if (numPlayers >= 2) txtPlayer2M.Text = playerArray[1].getMoney().ToString();
            if (numPlayers >= 3) txtPlayer3M.Text = playerArray[2].getMoney().ToString();
            if (numPlayers == 4) txtPlayer4M.Text = playerArray[3].getMoney().ToString();
        }

        /// <summary>
        /// Изменение представления о местонахождении игроков в нужное
        /// место и добавляет / удаляет любые представления игроков имущества.
        /// </summary>
        private void playerPosition()
        {
            // Позиция

            string[] playerPosition = new string[16];
            for (int i = 0; i < QPlayers; i++)
                playerPosition[playerArray[i].getBoardSpace()] += (i + 1).ToString();
            btnGO.Text = playerPosition[0];
            btnSvobody.Text = playerPosition[1];
            btnChance1.Text = playerPosition[2];
            btnPushkina.Text = playerPosition[3];
            btnJail.Text = playerPosition[4];
            btnGagarina.Text = playerPosition[5];
            btnChance2.Text = playerPosition[6];
            btnYubiley.Text = playerPosition[7];
            btnFP.Text = playerPosition[8];
            btnLenina.Text = playerPosition[9];
            btnChance3.Text = playerPosition[10];
            btnProf.Text = playerPosition[11];
            btnGoToJail.Text = playerPosition[12];
            btnTverskaya.Text = playerPosition[13];
            btnChance4.Text = playerPosition[14];
            btnSovetskaya.Text = playerPosition[15];

            // Possesion Color  Присвоение цвета

            for (int i = 0; i < QPlayers; i++)
            {
                if (spaceArray[1].getOwner() == i)
                    btnSvobody.BackColor = colorArray[i];
                if (spaceArray[3].getOwner() == i)
                    btnPushkina.BackColor = colorArray[i];
                if (spaceArray[5].getOwner() == i)
                    btnGagarina.BackColor = colorArray[i];
                if (spaceArray[7].getOwner() == i)
                    btnYubiley.BackColor = colorArray[i];
                if (spaceArray[9].getOwner() == i)
                    btnLenina.BackColor = colorArray[i];
                if (spaceArray[11].getOwner() == i)
                    btnProf.BackColor = colorArray[i];
                if (spaceArray[13].getOwner() == i)
                    btnTverskaya.BackColor = colorArray[i];
                if (spaceArray[15].getOwner() == i)
                    btnSovetskaya.BackColor = colorArray[i];
            }
                    
        }

        /// <summary>
        /// Изменяет текст текущих игроков в назначенный цвет, чтобы указать их очередь.
        /// </summary>
        private void changeTurn()
        {
            // По умолчанию
            lblP1Turn.ForeColor = Color.Black;
            lblP2Turn.ForeColor = Color.Black;
            lblP3Turn.ForeColor = Color.Black;
            lblP4Turn.ForeColor = Color.Black;

            switch (currentPlayer)
            {
                case 0:
                    lblP1Turn.ForeColor = Color.Green;
                    break;
                case 1:
                    lblP2Turn.ForeColor = Color.Blue;
                    break;
                case 2:
                    lblP3Turn.ForeColor = Color.Red;
                    break;
                case 3:
                    lblP4Turn.ForeColor = Color.Violet;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Check to see if a player has lost 
        /// Проверка проигрыша игрока
        /// </summary>
        private void checkLoss()
        {
            for (int i = 0; i < QPlayers; i++)
                if (playerArray[i].getMoney() <= 0)
                    playerArray[i].setGameOver(true);
        }

        /// <summary>
        /// Checks if anyone of the players has caused bankrupcy among all of the other players.
        /// Проверка, является ли кто-либо из игроков банкротом.
        /// </summary>
        private int checkWin()
        {
            int lossCount = 0;
            int winner = -1;

            for (int i = 0; i < QPlayers; i++)
            {
                if (playerArray[i].getGameOver())
                    lossCount++;
                else
                    winner = i;
            }

            if (lossCount == QPlayers - 1)
                return winner;

            return -1;
        }
        
        /// <summary>
        /// Сообщение о конце игры.
        /// </summary>
        private void gameOver()
        {
            MessageBox.Show("Поздравляем Игрока " + (winner + 1).ToString() + ", Вы выйграли!");
        }

        /// <summary>
        /// Обновляет статистику игры и проверяет победителя для завершения игры.
        /// </summary>
        private void updateGame()
        {
            playerPosition();

            // Проверяет наличие неактивного (игрока, который потерял).
            while (true)
            {
                if (currentPlayer >= QPlayers)
                    currentPlayer = 0;

                if (playerArray[currentPlayer].getGameOver())
                    currentPlayer++;
                else break;
            }

            updateStats();
            checkLoss();
            winner = checkWin();

            // If only 1 player left.
            // 
            if (winner != -1)
            {
                gameOver();
                this.Close();
            }

            changeTurn();
        }

        /// <summary>
        /// Returns if requirments are not met.  Calls both rolling dice methods 
        /// (1 will return if not correct).  Then updates the game.
        /// Возвращает, если требования не будут выполнены. 
        /// Вызывает как прокатные методы кости (1 будет вернуться, если не правильно). 
        /// Затем обновляет игру.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRollDice_Click(object sender, EventArgs e)
        {
            if (playerArray[currentPlayer].getGameOver()) return;
            if (playerArray[currentPlayer].getJustInJail())
            {
                playerArray[currentPlayer].setJustInJail(false);
                btnEndTurn.Visible = true;
                btnRollDice.Visible = false;
                updateGame();
                return;
            }

            btnRollDiceJail();
            btnRollDiceFree();

            updateGame();

            if (!dice.isDoubles())
            {
                btnEndTurn.Visible = true;
                btnRollDice.Visible = false;
            }
        }

        /// <summary>
        /// Roll the dice for player if they are in jail to test for doubles.
        /// Кидаем кости для игрока, который находится в тюрьме, чтобы проверить дубли.
        /// </summary>
        private void btnRollDiceJail()
        {
            if (!playerArray[currentPlayer].getInJail()) return;

            dice.RollDice();

            txtA.Text = dice.getNumber(1).ToString();
            txtB.Text = dice.getNumber(2).ToString();

            if (dice.isDoubles())
                MessageBox.Show("Вы освобождены из тюрьмы!");
            else
                MessageBox.Show("Дубля не выпало!");

            playerArray[currentPlayer].move(dice.GetTotal(), dice.isDoubles());
        }

        /// <summary>
        /// Бросок костей для игрока, который свободен (не в тюрьме).
        /// </summary>
        private void btnRollDiceFree()
        {
            if (playerArray[currentPlayer].getInJail()) return;

            dice.RollDice();

            txtA.Text = dice.getNumber(1).ToString();
            txtB.Text = dice.getNumber(2).ToString();

            int tempPlayer = currentPlayer;

            playerArray[currentPlayer].move(dice.GetTotal(), dice.isDoubles());

            // Выполнение действий на поле (пространстве)
            int rent = spaceArray[playerArray[currentPlayer].getBoardSpace()].doAction(ref currentPlayer, ref playerArray[currentPlayer]);


            // Updates the owner of the space's money.
            // Обновляем владельца денег клетки(пространства)
            playerArray[currentPlayer].updateMoney(rent);

            currentPlayer = tempPlayer;
        }

        /// <summary>
        /// Инициализирует игру и создает все игровые переменные.
        ///     - playerArray - массив игроков
        ///     - spaceArray - массив клеток(пространств)
        ///     - обновление поля со статистикой и информацией
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNewGame_Click(object sender, EventArgs e)
        {
            bool checkClick = true; //проверка, совершено ли было нажатие


            colorArray[0] = Color.Green;
            colorArray[1] = Color.Blue;
            colorArray[2] = Color.Red;
            colorArray[3] = Color.Purple;

            if (checkClick == true && QPlayersIn.ReadOnly == true)
            {
                DialogResult dialog = MessageBox.Show("Вы уверены, что хотите завершить текущую игру?", "Завершение текущей игры", MessageBoxButtons.YesNo);
                if (dialog == System.Windows.Forms.DialogResult.Yes)
                {
                    backToStart();
                    btnNewGame.Text = "Новая игра";
                }

            }
            else
            {
                try
                {
                    QPlayers = Int32.Parse(QPlayersIn.Text);

                    if (QPlayers <= 1 || QPlayers >= 5)
                    {
                        MessageBox.Show("Введите число в интервале 2 - 4!");
                        checkClick = false;
                    }
                    else
                    {
                        QPlayersIn.ReadOnly = true;//отключение выбора кол-ва игроков
                        btnNewGame.Text = "Завершить игру";

                        playerArray = new Player[QPlayers];
                        for (int i = 0; i < QPlayers; i++)
                            playerArray[i] = new Player();
                    }
                }
                catch (FormatException)
                {
                    MessageBox.Show("Пожалуйста, введите число в интервале 2 - 4 в поле!");
                    QPlayersIn.ReadOnly = false;
                    return;
                }

                if (checkClick == true)
                {
                    // Инициализация игровых клеток 
                    spaceArray[0] = new GoSpace();
                    spaceArray[1] = new PropertySpace(25);
                    spaceArray[2] = new DrawCardSpace();
                    spaceArray[3] = new PropertySpace(50);
                    spaceArray[4] = new GameSpace("default");
                    spaceArray[5] = new PropertySpace(75);
                    spaceArray[6] = new DrawCardSpace();
                    spaceArray[7] = new PropertySpace(100);
                    spaceArray[8] = new GameSpace("default");
                    spaceArray[9] = new PropertySpace(125);
                    spaceArray[10] = new DrawCardSpace();
                    spaceArray[11] = new PropertySpace(150);
                    spaceArray[12] = new GoToJailSpace();
                    spaceArray[13] = new PropertySpace(175);
                    spaceArray[14] = new DrawCardSpace();
                    spaceArray[15] = new PropertySpace(200);

                    // Включение кнопок с клетками
                    btnGO.Enabled = true;
                    btnSvobody.Enabled = true;
                    btnChance1.Enabled = true;
                    btnPushkina.Enabled = true;
                    btnJail.Enabled = true;
                    btnGagarina.Enabled = true;
                    btnChance2.Enabled = true;
                    btnYubiley.Enabled = true;
                    btnFP.Enabled = true;
                    btnLenina.Enabled = true;
                    btnChance3.Enabled = true;
                    btnProf.Enabled = true;
                    btnGoToJail.Enabled = true;
                    btnTverskaya.Enabled = true;
                    btnChance4.Enabled = true;
                    btnSovetskaya.Enabled = true;

                    btnRollDice.Visible = true;
                    updateStats();
                    changeTurn();
                    playerPosition();
                }
            }
        }

     
        
        /// <summary>
        /// Возвращает поле в стартовый вид
        /// </summary>
        private void backToStart()
        {
            //foreach (Control btn in form.Controls)
            //{
            //    if (btn.GetType().ToString().IndexOf("Button") > -1)
            //    {
            //        Button btn1 = (Button)btn;
            //        btn1.Text = "lol";
            //    }
            //}    
            currentPlayer = 0;

            btnGO.Text = "";
            btnSvobody.Text = "";
            btnChance1.Text = "";
            btnPushkina.Text = "";
            btnJail.Text = "";
            btnGagarina.Text = "";
            btnChance2.Text = "";
            btnYubiley.Text = "";
            btnFP.Text = "";
            btnLenina.Text = "";
            btnChance3.Text = "";
            btnProf.Text = "";
            btnGoToJail.Text = "";
            btnTverskaya.Text = "";
            btnChance4.Text = "";
            btnSovetskaya.Text = "";

            btnGO.Enabled = false;
            btnSvobody.Enabled = false;
            btnChance1.Enabled = false;
            btnPushkina.Enabled = false;
            btnJail.Enabled = false;
            btnGagarina.Enabled = false;
            btnChance2.Enabled = false;
            btnYubiley.Enabled = false;
            btnFP.Enabled = false;
            btnLenina.Enabled = false;
            btnChance3.Enabled = false;
            btnProf.Enabled = false;
            btnGoToJail.Enabled = false;
            btnTverskaya.Enabled = false;
            btnChance4.Enabled = false;
            btnSovetskaya.Enabled = false;

            btnGO.BackColor = SystemColors.Control;
            btnSvobody.BackColor = SystemColors.Control;
            btnChance1.BackColor = SystemColors.Control;
            btnPushkina.BackColor = SystemColors.Control;
            btnJail.BackColor = SystemColors.Control;
            btnGagarina.BackColor = SystemColors.Control;
            btnChance2.BackColor = SystemColors.Control;
            btnYubiley.BackColor = SystemColors.Control;
            btnFP.BackColor = SystemColors.Control;
            btnLenina.BackColor = SystemColors.Control;
            btnChance3.BackColor = SystemColors.Control;
            btnProf.BackColor = SystemColors.Control;
            btnGoToJail.BackColor = SystemColors.Control;
            btnTverskaya.BackColor = SystemColors.Control;
            btnChance4.BackColor = SystemColors.Control;
            btnSovetskaya.BackColor = SystemColors.Control;

            btnRollDice.Visible = false;
            btnEndTurn.Visible = false;

            txtA.Text = "";
            txtB.Text = "";
            txtCurPlayer.Text = "";
            txtPlayer1M.Text = "";
            txtPlayer2M.Text = "";
            txtPlayer3M.Text = "";
            txtPlayer4M.Text = "";

            lblP1Turn.ForeColor = Color.Black;
            lblP2Turn.ForeColor = Color.Black;
            lblP3Turn.ForeColor = Color.Black;
            lblP4Turn.ForeColor = Color.Black;

            QPlayersIn.Text = "";
            QPlayersIn.ReadOnly = false;
        }


        /// <summary>
        /// Increments player upon call. Приращивает игрока при вызове.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEndTurn_Click(object sender, EventArgs e)
        {
            currentPlayer++;
            updateGame();
            btnEndTurn.Visible = false;
            btnRollDice.Visible = true;
           
        }

        // Добавление дома по щелчку.

        private void btnSovetskaya_Click(object sender, EventArgs e)
        {
            spaceArray[playerArray[currentPlayer].getBoardSpace()].addHouse(
                currentPlayer, ref playerArray[currentPlayer]);
            rent15.Text = spaceArray[15].getRent().ToString();
            
        }

        private void btnTverskaya_Click(object sender, EventArgs e)
        {
            spaceArray[playerArray[currentPlayer].getBoardSpace()].addHouse(
                currentPlayer, ref playerArray[currentPlayer]);
            rent13.Text = spaceArray[13].getRent().ToString();
            
        }

        private void btnProf_Click(object sender, EventArgs e)
        {
            spaceArray[playerArray[currentPlayer].getBoardSpace()].addHouse(
                currentPlayer, ref playerArray[currentPlayer]);
            rent11.Text = spaceArray[11].getRent().ToString();
        }

        private void btnLenina_Click(object sender, EventArgs e)
        {
            spaceArray[playerArray[currentPlayer].getBoardSpace()].addHouse(
                currentPlayer, ref playerArray[currentPlayer]);
            rent9.Text = spaceArray[9].getRent().ToString();
        }

        private void btnYubiley_Click(object sender, EventArgs e)
        {
            spaceArray[playerArray[currentPlayer].getBoardSpace()].addHouse(
                currentPlayer, ref playerArray[currentPlayer]);
            rent7.Text = spaceArray[7].getRent().ToString();
        }

        private void btnGagarina_Click(object sender, EventArgs e)
        {
            spaceArray[playerArray[currentPlayer].getBoardSpace()].addHouse(
                currentPlayer, ref playerArray[currentPlayer]);
            rent5.Text = spaceArray[5].getRent().ToString();
        }

        private void btnPushkina_Click(object sender, EventArgs e)
        {
            spaceArray[playerArray[currentPlayer].getBoardSpace()].addHouse(
                currentPlayer, ref playerArray[currentPlayer]);
            rent3.Text = spaceArray[3].getRent().ToString();
        }

        private void btnSvobody_Click(object sender, EventArgs e)
        {
            spaceArray[playerArray[currentPlayer].getBoardSpace()].addHouse(
                currentPlayer, ref playerArray[currentPlayer]);
            rent1.Text = spaceArray[1].getRent().ToString();
        }

    }
}
