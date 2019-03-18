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
        int[] SetGemX = new int[3];
        int[] SetGemY = new int[3];
        int WallX = 200;
        int WallY = 300;
        int PlayerWalk=3;
        int PlayerRun;
        Random r = new Random();
        Gem[] Gems = new Gem[3];



        public MainWindow()
        {

            InitializeComponent();
            map = MapCreator.CreateMap(this, 38, 20, 50);
            Gem.map = map;
            map.Library.ImagesFolder = new PathInfo { Path = "..\\..\\images", Type = PathType.Relative };
            map.Library.AddPicture("wall", "wall.png");
            map.Library.AddPicture("fire", "fire.png");
            map.Library.AddPicture("Gem0", "gem_green.png");
            map.Library.AddPicture("Gem1", "gem_blue.png");
            map.Library.AddPicture("Gem2", "gem_red.png");
            map.Library.AddPicture("stones", "stones.jpg");
            map.SetMapBackground("stones");
            PlayerRun = PlayerWalk * 2;
            map.Library.AddContainer("fire", "fire");
            map.ContainerSetSize("fire", 50, 50);
            map.ContainerSetCoordinate("fire", FireX, FireY);
            map.ContainerSetZIndex("fire", 102);
            map.ContainerSetIndents("fire", 5, 5);



            map.Library.AddContainer("wall", "wall");
            map.ContainerSetSize("wall", 50, 50);
            map.ContainerSetCoordinate("wall", WallX, WallY);


            CreateGems();
            

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
        void CreateContainers(string ContainerName,  int z, string Picturename)
        {
            map.Library.AddContainer(ContainerName, Picturename);
            map.ContainerSetSize(ContainerName, 50, 50);
            map.ContainerSetIndents(ContainerName, 5, 5);
         
            map.ContainerSetZIndex(ContainerName, z);
        }
        void CreateGems()
        {
            for (int i = 0; i <= 2; i++)
            {
                Gems[i] = new Gem();
                Gems[i].ContainerName = "Gem" + i.ToString();
                Gems[i].Picture = "Gem" + i.ToString();
               
                CreateContainers(Gems[i].ContainerName, 101, Gems[i].Picture);
                SetGemRandomCoordinate(Gems[i]);

            }
        }
        void CollectGems()
        {
            for (int i = 0; i < Gems.Length; i++)
            {
                if (map.CollisionContainers("fire", "Gem" + i.ToString()))
                {
                    SetGemRandomCoordinate(Gems[i]);
                }
            }
        }
        
        void SetGemRandomCoordinate(Gem G)
        {
            //TODO:Сделать нормальнеы рандомные координаты
            G.SetCoordinates(r.Next(0, 500), r.Next(0, 500));
        }
        void CheckKey()
        {
            FireSaveX = FireX;
            FireSaveY = FireY;
            bool Moving = false;
            int Speed;
            if (map.Keyboard.IsKeyPressed(Key.LeftShift))
                Speed = PlayerRun;
            else
                Speed = PlayerWalk;
            if (map.Keyboard.IsKeyPressed(Key.W))
            {
                FireY -= Speed;

                Moving = true;
            }
            if (map.Keyboard.IsKeyPressed(Key.A))
            {
                FireX -= Speed;

                Moving = true;
            }
            if (map.Keyboard.IsKeyPressed(Key.D))
            {
                FireX += Speed;

                Moving = true;
            }
            if (map.Keyboard.IsKeyPressed(Key.S))
            {
                FireY += Speed;

                Moving = true;
            }

            if (Moving)
            {
                map.ContainerMovePreview("fire", FireX, FireY, 0);
                if (!(map.CollisionContainers("fire", "wall", true)))
                {
                    map.ContainerSetCoordinate("fire", FireX, FireY);
                    CollectGems();
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
    class Gem
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        static public UniversalMap_Wpf map;
        public string Picture ;
        public string ContainerName;
        public void SetCoordinates(int x,int y)
        {
            X = x;
            Y = y;
            map.ContainerSetCoordinate(ContainerName,x,y);
        }
    }

    
    class Coordinates
    {
        public int X = 0;
        public int Y = 0;
        Random r = new Random();
        Coordinate[] SetGem = new Coordinate[3];
        public void GetGem(int FireX, int FireY, int i)
        {
            SetGem[i] = new Coordinate();

            SetGem[i].X = r.Next(0, 1820);
            SetGem[i].Y = r.Next(0, 980);
            if (FireX - SetGem[i].X <= 100 || FireY - SetGem[i].Y <= 100 || SetGem[i].X - FireX <= 100 || SetGem[i].Y - FireY <= 100)
            {
                if (FireX < 100 || FireY < 100)
                {
                    SetGem[i].X += 101;
                    SetGem[i].Y += 101;
                }
                if (FireX > 1720 || FireY > 880)
                {
                    SetGem[i].X -= 101;
                    SetGem[i].Y -= 101;
                }
                if (FireX > 1720 || FireY < 100)
                {
                    SetGem[i].X -= 101;
                    SetGem[i].Y += 101;
                }
                if (FireX < 100 || FireY > 880)
                {
                    SetGem[i].X += 101;
                    SetGem[i].Y -= 100;
                }
            }
            if (SetGem[i].X >= 1820 || SetGem[i].Y >= 980 || SetGem[i].X < 0 || SetGem[i].Y < 0)
            {
                SetGem[i].X = 500 - i * 100;
                SetGem[i].Y = 500 + i * 100;
                SetGem[i].X = 500 + i * 10;
                SetGem[i].Y = 500 - i * 10;

            }

        }


    }
}