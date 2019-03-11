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

namespace WpfApp1
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
                SetGemX[i] = 500 + i * 10;
                SetGemY[i] = 500 - i * 10;
            }

            map.Library.AddContainer("wall", "wall");
            map.ContainerSetSize("wall", 50, 50);
            map.ContainerSetCoordinate("wall", WallX, WallY);
                        
            CreateContainers("Gem0", SetGemX[0], SetGemY[0],101);
            map.ContainerSetZIndex("Gem0", 101);
            CreateContainers("Gem1", SetGemX[1], SetGemY[1],101);
            map.ContainerSetZIndex("Gem1", 101);
            CreateContainers("Gem2", SetGemX[2], SetGemY[2],101);
            map.ContainerSetZIndex("Gem2", 101);
            
            timer.AddAction(CheckKey, 10);
            //Done:Доделать движение: устранить эффект перепрыгивания стены, вернув координаты в исходное состояние; добавить 2 кнопки.
            //Dont know how:Сделать функцию, которая принимает все параметры контейнера и создает его, чтобы в основной программе можно было создать контейнер в одну строчку.
            //Done:Сделать так, чтобы после сбора кристалла он появлялся в случайном месте, но не ближе 100 пикселей к игроку.
            //* Продумать механику игры.

        }
        void CreateContainers(string name,int x,int y,int z)
        {
            map.Library.AddContainer(name, name);
            map.ContainerSetSize(name, 50, 50);
            map.ContainerSetIndents(name, 5, 5);
            map.ContainerSetCoordinate(name, x, y);
            map.ContainerSetZIndex(name, z);
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
                                        SetGemX[i] = +101;
                                        SetGemY[i] = +101;
                                    }
                                    if (FireX > 1720 || FireY > 880)
                                    {
                                        SetGemX[i] = -101;
                                        SetGemY[i] = -101;
                                    }
                                    if (FireX > 1720 || FireY < 100)
                                    {
                                        SetGemX[i] = -101;
                                        SetGemY[i] = +101;
                                    }
                                    if (FireX < 100 || FireY > 880)
                                    {
                                        SetGemX[i] = +100;
                                        SetGemY[i] = -100;
                                    }

                                }
                                if (SetGemX[i] >= 1820 || SetGemY[i] >= 980 || SetGemX[i] < 0 || SetGemY[i] < 0)
                                {
                                    SetGemX[i] = 500 - i * 10;
                                    SetGemY[i] = 500 + i * 10;
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
