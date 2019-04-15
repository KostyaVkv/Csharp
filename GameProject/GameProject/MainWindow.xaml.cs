﻿using System;
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
using GameMaps.Layouts;
namespace GameProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IGameScreenLayout Lay;
        CellMapInfo MapInfo;
        UniversalMap_Wpf map;
        InventoryPanel Inventory;
        TimerController timer = new TimerController();
        Players Player = new Players();
        List<Enemy> Enemneis=new List<Enemy>();
        Random r = new Random();
        Gem[] Gems = new Gem[3];
        public MainWindow()
        {
            InitializeComponent();
            Lay = LayoutsFactory.GetLayout(LayoutType.Vertical, this.Content);
            MapInfo = new CellMapInfo(38, 20, 50, 5);
            map = MapCreator.GetUniversalMap(this, MapInfo);
            Lay.Attach(map, 0);
            Inventory = new InventoryPanel(map.Library, 50, 14);
            Lay.Attach(Inventory, 1);
            Helper.map = map;
            map.Library.ImagesFolder = new PathInfo { Path = "..\\..\\images", Type = PathType.Relative };
            map.Library.AddPicture("wall", "wall.png");
            map.Library.AddPicture("fire", "fire.png");
            map.Library.AddPicture("Gem0", "gem_green.png");
            map.Library.AddPicture("Gem1", "gem_blue.png");
            map.Library.AddPicture("Gem2", "gem_red.png");
            map.Library.AddPicture("stones", "stones.jpg");
            map.Library.AddPicture("ghost0", "GHOST.png");
            map.Library.AddPicture("ghost1", "GHOST1.png");
            map.Library.AddPicture("ghost2", "GHOST2.png");
            map.Library.AddPicture("gate closed", "gate_closed.png");
            string[] exp = new string[10]; 
            for(int i=0; i<=9;i++)
            {
                exp[i] = "exp" + i.ToString();
                map.Library.AddPicture(exp[i], exp[i] + ".png");
            }
            map.Library.AddPicture("fon", "Fon.jpg");
            map.SetMapBackground("stones");
            Inventory.AddItem("Lives", "fire");
            Inventory.SetBackground(Brushes.Transparent);
            AnimationDefinition a = new AnimationDefinition();
            a.AddEqualFrames(50, exp);
            a.LastFrame = "exp9";
            map.Library.AddAnimation("Explosion", a);
            Helper.PlayerRun = Helper.PlayerWalk * 2;
            //map.Library.AddContainer("wall", "wall");
            //map.ContainerSetSize("wall", 50, 50);
            //map.ContainerSetCoordinate("wall", WallX, WallY);
            CreateGems();
            CreatePlayer();
            for(int i=0;i<3;i++)
            {
                CreateEnemy();
            }
            CreateWalls(20,500,50,1020);
            CreateWalls(975, 20, 1860, 50);
            CreateWalls(1880,530,50,975);
            CreateWalls(950,978,1815,50);
            CreateWalls(500, 500, 250, 250);
            CreateContainers("Gate", 101, "gate closed");
            map.ContainerSetCoordinate("Gate", 1000, 700);
            //map.Library.AddContainer("fon", "wall", ContainerType.TiledImage);
            //map.ContainerSetSize("fon", 50, 500);
            //map.ContainerSetTileSize("fon", 50, 50);
            //map.ContainerSetCoordinate("fon",25, 25);
            Player.SetCoordinates(80, 80);
            timer.AddAction(CheckKey, 10);
            timer.AddAction(MoveEnemies, 10);
            timer.AddAction(PlayerPlusEnemy, 10);
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
        void CreateContainers(string ContainerName, int z, string Picturename)
        {
            map.Library.AddContainer(ContainerName, Picturename);
            map.ContainerSetSize(ContainerName, 50, 50);
            map.ContainerSetIndents(ContainerName, 5, 5);
            map.ContainerSetZIndex(ContainerName, z);
        }
        void CreateEnemy()
        {
            Enemy Ghost = new Enemy();
            Ghost.ContainerName = "Ghost" + Helper.EnemyCounter.ToString();
            Ghost.Picture ="ghost"+ Helper.EnemyCounter.ToString() ;
            CreateContainers(Ghost.ContainerName, 101, Ghost.Picture);
            Ghost.SetCoordinates(r.Next(100,1600), r.Next(100,850));
            Ghost.SetGoal();
            Helper.EnemyCounter++;
            Enemneis.Add(Ghost);
        }
        void MoveEnemies()
        {
            for (int f=0;f<Enemneis.Count;f++)
            {
                Enemneis[f].EnemyMove();
                Enemneis[f].CheckGoal();
            }
        }
        void CreateWalls(int CentreX,int CentreY,int SizeX,int SizeY)
        {
            string WallName = "Wall" + Helper.WallCounter.ToString();
            map.Library.AddContainer(WallName, "wall", ContainerType.TiledImage);
            map.ContainerSetSize(WallName, SizeX, SizeY);
            map.ContainerSetTileSize(WallName, 50, 50);
            map.ContainerSetCoordinate(WallName, CentreX, CentreY);
            Helper.WallCounter++;
        }
       void UpdateLives ()
       {
            Inventory.SetText("Lives", Player.Lives.ToString());
       }
        //TODO:Сделать проверку кристалов и стен
        void CreatePlayer() 
        {
            Player.ContainerName = "Fire";
            Player.Picture = "fire";
            CreateContainers(Player.ContainerName, 102, Player.Picture);
            map.ContainerSetCoordinate(Player.ContainerName, Player.X, Player.Y);
            UpdateLives();
        }
        void PlayerPlusEnemy()
        {
            int x=Player.X;
            int y=Player.Y;
            for (int r = 0; r < Enemneis.Count; r++)
            {
                if (map.CollisionContainers(Player.ContainerName, Enemneis[r].ContainerName))
                {
                    x = 75;
                    y = 75;
                    Player.Lives--;
                    UpdateLives();
                }
            }
            Player.SetCoordinates(x, y);
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
                if (map.CollisionContainers("Fire", "Gem" + i.ToString()))
                {
                    SetGemRandomCoordinate(Gems[i]);
                    map.AnimationStart(Gems[i].ContainerName, "Explosion", 2);
                }
            }
        }
       
        void SetGemRandomCoordinate(Gem G)
        {
            while (true)
            {


                int x = r.Next(0, 1820);
                int y = r.Next(0, 980);
                int i = r.Next(1, 5);
                if (Helper.FirstGemPosition == 0)
                {
                    int FirstX = 500 + r.Next(1, 5) * 100;
                    int FirstY = 500 - r.Next(1, 5) * 100;
                    FirstX = FirstX - r.Next(1, 5) * 10;
                    FirstY = FirstY - r.Next(1, 5) * 10;
                    x = FirstX;
                    y = FirstY;
                    Helper.FirstGemPosition++;
                }
                else
                {
                    if (Player.X - x <= 100 || Player.Y - y <= 100 || x - Player.X <= 100 || y - Player.Y <= 100)
                    {
                        if (Player.X < 100 || Player.Y < 100)
                        {
                            x += 101;
                            y += 101;
                        }
                        if (Player.X > 1720 || Player.Y > 880)
                        {
                            x -= 101;
                            y -= 101;
                        }
                        if (Player.X > 1720 || Player.Y < 100)
                        {
                            x -= 101;
                            y += 101;
                        }
                        if (Player.X < 100 || Player.Y > 880)
                        {
                            x += 101;
                            y -= 100;
                        }
                    }
                    if (x >= 1820 || y >= 980 || x < 0 || y < 0)
                    {
                        x = 500 - i * 100;
                        y = 500 + i * 100;
                        x = 500 + i * 10;
                        y = 500 - i * 10;
                    }
                }
                //TODO: Проверить коррекность работы пересечений.
                if (!(Helper.CollisionWalls(x, y, G.ContainerName)))
                {
                    G.SetCoordinates(x, y);
                    break;
                }
            }
            //Создаём рандомные координаты.
            //Проверка попадания в стену.
        }
        void PlayerMove(int nx,int ny)
        {
            if (!(Helper.CollisionWalls(nx, ny, Player.ContainerName) ))
            {
                Player.SetCoordinates(nx, ny);
                CollectGems();
            }
            //else
            //{
            //    x = Player.SaveX;
            //    y = Player.SaveY;
            //    map.ContainerSetCoordinate("Fire", x, y);
            //}
            //Player.PlayerMoving(x, y, xs, ys);
        }
        void CheckKey()
        {
            bool Moving = false;
            int Speed;
            int x = Player.X;
            int y = Player.Y;
            if (map.Keyboard.IsKeyPressed(Key.LeftShift))
                Speed = Helper.PlayerRun;
            else
            {
                Speed = Helper.PlayerWalk;
            }
            if (map.Keyboard.IsKeyPressed(Key.W))
            {
                y -= Speed;
                Moving = true;
            }
            if (map.Keyboard.IsKeyPressed(Key.A))
            {
                x -= Speed;
                Moving = true;
            }
            if (map.Keyboard.IsKeyPressed(Key.D))
            {
                x += Speed;
                Moving = true;
            }
            if (map.Keyboard.IsKeyPressed(Key.S))
            {
                y += Speed;
                Moving = true;
            }
            if (Moving)
            {
                PlayerMove(x,y);
            }
        }
    }
    class Gem
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public string Picture;
        public string ContainerName;
        public void SetCoordinates(int x, int y)
        {
            X = x;
            Y = y;
            Helper.map.ContainerSetCoordinate(ContainerName, x, y);
        }
    }
    class Players
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public string Picture;
        public int Lives = 3;
        public string ContainerName;
        public void SetCoordinates(int x, int y)
        {
            X = x;
            Y = y;
            Helper.map.ContainerSetCoordinate(ContainerName, x, y);
        }
    }
    class Enemy
    { 
        public int X { get; private set; }
        public int Y { get; private set; }
        public int GoalX { get; private set; }
        public int GoalY { get; private set; }
        int EnemySpeed=2;
        static Random r = new Random();
        public string Picture;
        public string ContainerName;
        public void SetCoordinates(int x, int y)
        {
            X = x;
            Y = y;
            Helper.map.ContainerSetCoordinate(ContainerName, x, y);
        }
        public void SetGoal()
        {
             GoalX = r.Next(200, 1700);
             GoalY = r.Next(50, 950);
        }
        public void CheckGoal()
        {
            if ((Math.Abs(X - GoalX) <= 2)
                 && (Math.Abs(Y - GoalY) <= 2))
            {
               SetGoal();
            }
        }
        public void EnemyMove()
        {
            int SaveX = X;
            int SaveY = Y;
            if (X > GoalX)
            {
                X -= EnemySpeed;
            }
            if (X < GoalX)
            {
                X += EnemySpeed;
            }
            if (Y > GoalY)
            {
                Y -= EnemySpeed;
            }
            if (Y < GoalY)
            {
                Y += EnemySpeed;
            }
            if (!(Helper.CollisionWalls(X, Y, ContainerName)))
            {
                Helper.map.ContainerSetCoordinate(ContainerName, X, Y);
            }
            else
            {
                X = SaveX;
                Y = SaveY;
                SetGoal();
            }
        }
    }
    class Helper
    {
        static public UniversalMap_Wpf map;
        static public int EnemyCounter = 0;
        static public int WallCounter = 0;
        static public int PlayerWalk = 3;
        static public int PlayerRun;
        static public int FirstGemPosition = 0;
        static public  bool CollisionWalls(int x, int y, string ContainerName)
      {
            map.ContainerMovePreview(ContainerName, x, y, 0);
            bool MoveInWalls = false;
            for (int i = 0; i < WallCounter; i++)
            {
                if (map.CollisionContainers(ContainerName, "Wall" + i.ToString(), true))
                {
                    MoveInWalls = true;
                    break;
                }
            }
            return MoveInWalls;
        }
    }
}