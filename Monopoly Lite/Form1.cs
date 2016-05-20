using System;
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
            btnBalAve.Text = playerPosition[1];
            btnChance1.Text = playerPosition[2];
            btnConAve.Text = playerPosition[3];
            btnJail.Text = playerPosition[4];
            btnVAAve.Text = playerPosition[5];
            btnChance2.Text = playerPosition[6];
            btnNYAve.Text = playerPosition[7];
            btnFP.Text = playerPosition[8];
            btnILAve.Text = playerPosition[9];
            btnChance3.Text = playerPosition[10];
            btnMG.Text = playerPosition[11];
            btnGoToJail.Text = playerPosition[12];
            btnPennAve.Text = playerPosition[13];
            btnChance4.Text = playerPosition[14];
            btnBW.Text = playerPosition[15];

            // Possesion Color  Присвоение цвета

            for (int i = 0; i < QPlayers; i++)
            {
                if (spaceArray[1].getOwner() == i)
                    btnBalAve.BackColor = colorArray[i];
                if (spaceArray[3].getOwner() == i)
                    btnConAve.BackColor = colorArray[i];
                if (spaceArray[5].getOwner() == i)
                    btnVAAve.BackColor = colorArray[i];
                if (spaceArray[7].getOwner() == i)
                    btnNYAve.BackColor = colorArray[i];
                if (spaceArray[9].getOwner() == i)
                    btnILAve.BackColor = colorArray[i];
                if (spaceArray[11].getOwner() == i)
                    btnMG.BackColor = colorArray[i];
                if (spaceArray[13].getOwner() == i)
                    btnPennAve.BackColor = colorArray[i];
                if (spaceArray[15].getOwner() == i)
                    btnBW.BackColor = colorArray[i];
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
                    btnBalAve.Enabled = true;
                    btnChance1.Enabled = true;
                    btnConAve.Enabled = true;
                    btnJail.Enabled = true;
                    btnVAAve.Enabled = true;
                    btnChance2.Enabled = true;
                    btnNYAve.Enabled = true;
                    btnFP.Enabled = true;
                    btnILAve.Enabled = true;
                    btnChance3.Enabled = true;
                    btnMG.Enabled = true;
                    btnGoToJail.Enabled = true;
                    btnPennAve.Enabled = true;
                    btnChance4.Enabled = true;
                    btnBW.Enabled = true;

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
            btnBalAve.Text = "";
            btnChance1.Text = "";
            btnConAve.Text = "";
            btnJail.Text = "";
            btnVAAve.Text = "";
            btnChance2.Text = "";
            btnNYAve.Text = "";
            btnFP.Text = "";
            btnILAve.Text = "";
            btnChance3.Text = "";
            btnMG.Text = "";
            btnGoToJail.Text = "";
            btnPennAve.Text = "";
            btnChance4.Text = "";
            btnBW.Text = "";

            btnGO.Enabled = false;
            btnBalAve.Enabled = false;
            btnChance1.Enabled = false;
            btnConAve.Enabled = false;
            btnJail.Enabled = false;
            btnVAAve.Enabled = false;
            btnChance2.Enabled = false;
            btnNYAve.Enabled = false;
            btnFP.Enabled = false;
            btnILAve.Enabled = false;
            btnChance3.Enabled = false;
            btnMG.Enabled = false;
            btnGoToJail.Enabled = false;
            btnPennAve.Enabled = false;
            btnChance4.Enabled = false;
            btnBW.Enabled = false;

            btnGO.BackColor = SystemColors.Control;
            btnBalAve.BackColor = SystemColors.Control;
            btnChance1.BackColor = SystemColors.Control;
            btnConAve.BackColor = SystemColors.Control;
            btnJail.BackColor = SystemColors.Control;
            btnVAAve.BackColor = SystemColors.Control;
            btnChance2.BackColor = SystemColors.Control;
            btnNYAve.BackColor = SystemColors.Control;
            btnFP.BackColor = SystemColors.Control;
            btnILAve.BackColor = SystemColors.Control;
            btnChance3.BackColor = SystemColors.Control;
            btnMG.BackColor = SystemColors.Control;
            btnGoToJail.BackColor = SystemColors.Control;
            btnPennAve.BackColor = SystemColors.Control;
            btnChance4.BackColor = SystemColors.Control;
            btnBW.BackColor = SystemColors.Control;

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

        private void btnBW_Click(object sender, EventArgs e)
        {
            spaceArray[playerArray[currentPlayer].getBoardSpace()].addHouse(
                currentPlayer, ref playerArray[currentPlayer]);
        }

        private void btnPennAve_Click(object sender, EventArgs e)
        {
            spaceArray[playerArray[currentPlayer].getBoardSpace()].addHouse(
                currentPlayer, ref playerArray[currentPlayer]);
        }

        private void btnMG_Click(object sender, EventArgs e)
        {
            spaceArray[playerArray[currentPlayer].getBoardSpace()].addHouse(
                currentPlayer, ref playerArray[currentPlayer]);
        }

        private void btnILAve_Click(object sender, EventArgs e)
        {
            spaceArray[playerArray[currentPlayer].getBoardSpace()].addHouse(
                currentPlayer, ref playerArray[currentPlayer]);
        }

        private void btnNYAve_Click(object sender, EventArgs e)
        {
            spaceArray[playerArray[currentPlayer].getBoardSpace()].addHouse(
                currentPlayer, ref playerArray[currentPlayer]);
        }

        private void btnVAAve_Click(object sender, EventArgs e)
        {
            spaceArray[playerArray[currentPlayer].getBoardSpace()].addHouse(
                currentPlayer, ref playerArray[currentPlayer]);
        }

        private void btnConAve_Click(object sender, EventArgs e)
        {
            spaceArray[playerArray[currentPlayer].getBoardSpace()].addHouse(
                currentPlayer, ref playerArray[currentPlayer]);
        }

        private void btnBalAve_Click(object sender, EventArgs e)
        {
            spaceArray[playerArray[currentPlayer].getBoardSpace()].addHouse(
                currentPlayer, ref playerArray[currentPlayer]);
        }

    }
}
