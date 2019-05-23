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
using GameMaps.Layouts;

namespace GameProjectWithErrors
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
        Random r = new Random();
        bool ChestExp = false;
        int[,] PortalsCoordinates = new int[4, 2];
        Player player;
        public MainWindow()
        {
            InitializeComponent();
            Lay = LayoutsFactory.GetLayout(LayoutType.Vertical, this.Content);
            MapInfo = new CellMapInfo(10, 10, 40, 5);
            map = MapCreator.GetUniversalMap(this, MapInfo);
            Lay.Attach(map, 0);
            Inventory = new InventoryPanel(map.Library, 50, 14);
            Lay.Attach(Inventory, 1);
            map.Library.ImagesFolder = new PathInfo { Path = "..\\..\\images", Type = PathType.Relative };
            map.Library.AddPicture("wall", "wall.png");
            map.Library.AddPicture("player", "evil.png");
            map.Library.AddPicture("gem", "gem_green.png");

            player.CreatePlayer(map);

            map.Library.AddContainer("gem", "gem");
            map.ContainerSetSize("gem", 40);
            map.ContainerSetCoordinate("gem", 180, 200);


            timer.AddAction(KeyCheck, 50);


          
        }
        void KeyCheck()
        {
            bool isMoved = false;
            if (map.Keyboard.IsKeyPressed(Key.W))
            {
                isMoved = true;
                player.DrawPlayer(player.X + 10, player.Y);
            }
            if (map.Keyboard.IsKeyPressed(Key.A))
            {
                isMoved = true;
                player.DrawPlayer(player.X - 10, player.Y);
            }
            if (map.Keyboard.IsKeyPressed(Key.S))
            {
                isMoved = true;
                player.DrawPlayer(player.X, player.Y + 1);
            }
            if (map.Keyboard.IsKeyPressed(Key.D))
                isMoved = true;
                player.DrawPlayer(player.X + 10, player.Y);
            if(isMoved)
            {
                if(map.CollisionContainers("player", "player"))
                {
                    map.ContainerRemove("gem");
                }
            }
        }
    }

    class Player
    {
        UniversalMap_Wpf Map;
        public int X;
        public int Y;
        public string ContainerName;
        public void CreatePlayer(UniversalMap_Wpf map)
        {
            Map = map;
            X = 50;
            Y = 50;
            map.Library.AddContainer("player", "player");
            map.ContainerSetSize("player", 40);
            map.ContainerSetCoordinate("player", X, Y);

        }
        public void DrawPlayer(int x, int y)
        {
            X = x;
            y = y;
            Map.ContainerSetCoordinate("player", X, Y);
        }
    }
}
