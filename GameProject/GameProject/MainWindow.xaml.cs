using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GameMaps;

namespace GameProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        UniversalMap_Wpf map;
        TimerController timer = new TimerController();
        int FireX = 0;
        int FireY = 0;
        int FireSaveX = 0;
        int FireSaveY = 0;
        int[] SetGemX = new int [3];
        int[] SetGemY = new int [3];
        int WallX = 200;
        int WallY = 300;
       
        Random r = new Random();
        



        public MainWindow()
        {

            InitializeComponent();
            map = MapCreator.CreateMap(this, 38, 20, 50);
            
            map.Library.AddPicture("wall", "wall.png");
            map.Library.AddPicture("fire", "fire.png");
            map.Library.AddPicture("Gem0", "gem_green.png");
            map.Library.AddPicture("Gem1", "gem_blue.png");
            map.Library.AddPicture("Gem2", "gem_red.png");
            map.Library.AddPicture("stones", "stones.jpg");
            map.SetMapBackground("stones");
            
            map.Library.AddContainer("fire", "fire");
            map.ContainerSetSize("fire", 50, 50);
            map.ContainerSetCoordinate("fire", FireX, FireY);
            map.ContainerSetZIndex("fire", 102);
            map.ContainerSetIndents("fire", 5, 5);
            for (int i = 0; i <= 2; i++)
            {
                int random1 = r.Next(1, 5);
                int random2 = r.Next(1, 5);
                SetGemX[i] = 500 - random1 * 100;
                SetGemY[i] = 500 + random2 * 100;
                SetGemX[i] = 500 + random1 * 10;
                SetGemY[i] = 500 - random2 * 10;
            }
            
           
            map.Library.AddContainer("wall", "wall");
            map.ContainerSetSize("wall", 50, 50);
            map.ContainerSetCoordinate("wall", WallX, WallY);
                        
          
           
            for(int i=0;i<=2;i++)
            {
                CreateContainers("Gem"+i.ToString(), SetGemX[0], SetGemY[0], 101, "Gem"+i.ToString());
            }
            
            timer.AddAction(CheckKey, 10);
            //Done:Доделать движение: устранить эффект перепрыгивания стены, вернув координаты в исходное состояние; добавить 2 кнопки.
            //Dont know how:Сделать функцию, которая принимает все параметры контейнера и создает его, чтобы в основной программе можно было создать контейнер в одну строчку.
            //Done:Сделать так, чтобы после сбора кристалла он появлялся в случайном месте, но не ближе 100 пикселей к игроку.
            //Создать класс для кристала.
            //* Продумать механику игры.

        }
        /// <summary>
        /// Функция создания контейнера 
        /// </summary>
        /// <param name="ContainerName"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z">Индекс</param>
        /// <param name="Picturename">Название Картинки из биоблитеки</param>
        void CreateContainers(string ContainerName,int x,int y,int z, string Picturename)
        {
            map.Library.AddContainer(ContainerName, Picturename);
            map.ContainerSetSize(ContainerName, 50, 50);
            map.ContainerSetIndents(ContainerName, 5, 5);
            map.ContainerSetCoordinate(ContainerName, x, y);
            map.ContainerSetZIndex(ContainerName, z);
        }
         
        void CheckKey()
        {
            FireSaveX = FireX;
            FireSaveY = FireY;
            bool Moving = false;
            if (map.Keyboard.IsKeyPressed(Key.W))
            {
                FireY--;
                FireY--;
                FireY--;
                Moving = true;
            }
            if (map.Keyboard.IsKeyPressed(Key.A))
            {
                FireX--;
                FireX--;
                FireX--;
                Moving = true;
            }
            if (map.Keyboard.IsKeyPressed(Key.D))
            {
                FireX++;
                FireX++;
                FireX++;
                Moving = true;
            }
            if (map.Keyboard.IsKeyPressed(Key.S))
            {
                FireY++;
                FireY++;
                FireY++;
                Moving = true;
            }
            if (Moving)
            {
                map.ContainerMovePreview("fire", FireX, FireY, 0);
                if (!(map.CollisionContainers("fire", "wall", true)))
                {
                        
                    map.ContainerSetCoordinate("fire", FireX, FireY);
                    if (map.CollisionContainers("fire", "Gem0"))
                    {
                            for (int i = 0; i <= 2; i++)
                            {
                                SetGemX[i] = r.Next(0, 1820);
                                SetGemY[i] = r.Next(0, 980);

                                if (FireX - SetGemX[i] <= 100 || FireY - SetGemY[i] <= 100 || SetGemX[i] - FireX <= 100 || SetGemY[i] - FireY <= 100)
                                {
                                    if (FireX < 100 || FireY < 100)
                                    {
                                        SetGemX[i] +=101;
                                        SetGemY[i] +=101;
                                    }
                                    if (FireX > 1720 || FireY > 880)
                                    {
                                        SetGemX[i] -=101;
                                        SetGemY[i] -=101;
                                    }
                                    if (FireX > 1720 || FireY < 100)
                                    {
                                        SetGemX[i] -=101;
                                        SetGemY[i] +=101;
                                    }
                                    if (FireX < 100 || FireY > 880)
                                    {
                                        SetGemX[i] +=101;
                                        SetGemY[i] -=101;
                                    }

                                }
                                if (SetGemX[i] >= 1820 || SetGemY[i] >= 980 || SetGemX[i] < 0 || SetGemY[i] < 0)
                                {
                                    int random1 = r.Next(1, 5);
                                    int random2 = r.Next(1, 5);
                                    SetGemX[i] = 500 - random1 * 100;
                                    SetGemY[i] = 500 + random2 * 100;
                                    SetGemX[i] = 500 + random1 * 10;
                                    SetGemY[i] = 500 - random2 * 10;
                            }
                                if (i == 0)
                                    map.ContainerSetCoordinate("Gem0", SetGemX[i], SetGemY[i]);
                                if (i == 1)
                                    map.ContainerSetCoordinate("Gem1", SetGemX[i], SetGemY[i]);
                                if (i == 2)
                                    map.ContainerSetCoordinate("Gem2", SetGemX[i], SetGemY[i]);
                            }
                        }
                    }
                  else
                  {
                    FireX = FireSaveX;
                    FireY = FireSaveY;
                    map.ContainerSetCoordinate("fire", FireX, FireY);
                  }

            }

        }
    }
}
