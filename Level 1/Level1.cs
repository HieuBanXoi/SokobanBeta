﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Level_1
{
    public partial class Level1 : Form
    {
        private char[,] map = {
            //{ '#', '#', '#', '#', '#', '#', '#' },
            //{ '#', ' ', 'G', 'G', 'G', ' ', '#' },
            //{ '#', 'P', ' ', 'G', ' ', ' ', '#' },
            //{ '#', ' ', 'B', 'G', 'G', 'G', '#' },
            //{ '#', ' ', ' ', 'G', ' ', ' ', '#' },
            //{ '#', ' ', ' ', 'G', ' ', ' ', '#' },
            //{ '#', '#', '#', '#', '#', '#', '#' }

            { '#', '#', '#', '#', '#' },
            { '#', 'P', ' ', ' ', '#' },
            { '#', ' ', 'B', 'G', '#' },
            { '#', '#', '#', '#', '#' }
        };

        private int playerX = 1, playerY = 1; // Vị trí bắt đầu của người chơi
        private int cellSize = 64;           // Kích thước mỗi ô
        private Image wallImage;
        private Image boxImage;
        private Image goalImage;
        private Image playerImage;
        enum TrangThai { OnGoal, OutGoal };

        TrangThai p_TrangThai = TrangThai.OutGoal;
        TrangThai b_TrangThai = TrangThai.OutGoal;

        public Level1()
        {
            InitializeComponent();
            playerImage = Image.FromFile("C:\\Users\\Administrator\\source\\repos\\SokobanBeta\\Level 1\\Resources\\player.png");
            goalImage = Image.FromFile("C:\\Users\\Administrator\\source\\repos\\SokobanBeta\\Level 1\\Resources\\goal.png");
            boxImage = Image.FromFile("C:\\Users\\Administrator\\source\\repos\\SokobanBeta\\Level 1\\Resources\\box.png");
            wallImage = Image.FromFile("C:\\Users\\Administrator\\source\\repos\\SokobanBeta\\Level 1\\Resources\\wall.png");
            this.Width = map.GetLength(1) * cellSize + 16; // Độ rộng form
            this.Height = map.GetLength(0) * cellSize + 39; // Chiều cao form
            this.Text = "Level1";
            this.KeyDown += SokobanForm_KeyDown;
            this.Paint += SokobanForm_Paint;
        }
        private void SokobanForm_KeyDown(object sender, KeyEventArgs e)
        {
            int dx = 0, dy = 0;
            if (e.KeyCode == Keys.Up) dx = -1;
            else if (e.KeyCode == Keys.Down) dx = 1;
            else if (e.KeyCode == Keys.Left) dy = -1;
            else if (e.KeyCode == Keys.Right) dy = 1;

            int newX = playerX + dx, newY = playerY + dy;

            // Kiểm tra di chuyển hợp lệ
            if ( map[newX, newY] == 'G')
            {
                if (p_TrangThai == TrangThai.OnGoal)
                {
                    map[playerX, playerY] = 'G';
                }
                else map[playerX, playerY] = ' ';
                playerX = newX;
                playerY = newY;
                map[playerX, playerY] = 'P';
                p_TrangThai = TrangThai.OnGoal;
            }
            if (map[newX, newY] == ' ')
            {
                switch (p_TrangThai)
                {
                    case TrangThai.OutGoal:
                        map[playerX, playerY] = ' ';
                        break;
                    case TrangThai.OnGoal:
                        map[playerX, playerY] = 'G';
                        p_TrangThai=TrangThai.OutGoal;
                        break;
                    default:
                        break;
                }
                playerX = newX;
                playerY = newY;
                map[playerX, playerY] = 'P';
            }
            else if (map[newX, newY] == 'B')
            {
                // Kiểm tra nếu hộp có thể được đẩy
                int boxNewX = newX + dx, boxNewY = newY + dy;
                if(map[boxNewX, boxNewY] == 'G')
                {
                    if (b_TrangThai == TrangThai.OnGoal)
                    {
                        if (p_TrangThai == TrangThai.OnGoal)
                        {
                            map[playerX, playerY] = 'G';
                        }
                        else
                        {
                            map[playerX, playerY] = ' ';
                        }
                        p_TrangThai = TrangThai.OnGoal;
                    }
                    else
                    {
                        if (p_TrangThai == TrangThai.OnGoal)
                        {
                            map[playerX, playerY] = 'G';
                        }
                        else
                        {
                            map[playerX, playerY] = ' ';
                        }
                        p_TrangThai = TrangThai.OutGoal;
                    }
                    map[newX, newY] = 'P';            // Vị trí mới của người chơi
                    map[boxNewX, boxNewY] = 'B';      // Vị trí mới của hộp
                    playerX = newX;
                    playerY = newY;
                    b_TrangThai = TrangThai.OnGoal;
                }
                if (map[boxNewX, boxNewY] == ' ' )
                {
                    if (b_TrangThai == TrangThai.OnGoal)
                    {
                        if (p_TrangThai == TrangThai.OnGoal)
                        {
                            map[playerX, playerY] = 'G';
                        }
                        else
                        {
                            map[playerX, playerY] = ' ';
                        }
                        p_TrangThai = TrangThai.OnGoal;
                    }
                    else
                    {
                        if (p_TrangThai == TrangThai.OnGoal)
                        {
                            map[playerX, playerY] = 'G';
                        }
                        else
                        {
                            map[playerX, playerY] = ' ';
                        }
                        p_TrangThai = TrangThai.OutGoal;
                    }
                    map[newX, newY] = 'P';            // Vị trí mới của người chơi
                    map[boxNewX, boxNewY] = 'B';      // Vị trí mới của hộp
                    playerX = newX;
                    playerY = newY;
                    b_TrangThai = TrangThai.OutGoal;
                }
            }

            // Kiểm tra hoàn thành level
            CheckWinCondition();

            this.Invalidate(); // Vẽ lại màn hình
        }
        private bool IsGoal(int x, int y)
        {
            return map[x, y] == 'G'  ; // Ô đó là mục tiêu hoặc người chơi trên mục tiêu
        }


        // Hàm kiểm tra điều kiện chiến thắng
        private void CheckWinCondition()
        {
                for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    if (map[x, y] == 'G'||p_TrangThai==TrangThai.OnGoal ) // Nếu còn mục tiêu chưa có hộp
                    {
                        return; // Chưa hoàn thành
                    }
                }
            }
            this.Invalidate();
            // Nếu tất cả mục tiêu đã được lấp đầy
            MessageBox.Show("You Win!", "Congratulations");
            Application.Exit();
        }

        // Sự kiện vẽ giao diện trò chơi
        private void SokobanForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    Rectangle rect = new Rectangle(y * cellSize, x * cellSize, cellSize, cellSize);
                    char cell = map[x, y];

                    if (cell == '#')
                        g.DrawImage(wallImage, rect);  // Vẽ tường
                    else if (cell == 'P')
                        g.DrawImage(playerImage, rect);  // Vẽ người chơi
                    else if (cell == 'B')
                        g.DrawImage(boxImage, rect);  // Vẽ hộp
                    else if (cell == 'G')
                        g.DrawImage(goalImage, rect);  // Vẽ mục tiêu
                    else
                        g.FillRectangle(Brushes.White, rect); // Ô trống

                    g.DrawRectangle(Pens.Gray, rect); // Viền ô
                }
            }
        }

        


    }
}
