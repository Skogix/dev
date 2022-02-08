using ECS;

namespace SimpleGameUI {
	internal class TileSystem : ISystem, InitSystem {
		public Entity[,] Tiles { get; set; }
		public TileSystem(World world, int width, int height) {
			World = world;
			Width = width;
			Height = height;
			Tiles = new Entity[width,height];
		}
		private int Width { get; set; }
		private int Height { get; set; }
		private World World { get; set; }
		private const int _roomWidth = 5;
		private const int _roomHeight = 5;
		public void Init() {
			CreateWalls();
			CreateFloors();
		}
		public void CreateFloors() {
			for (int x = 0; x < _roomWidth; x++) {
				for (int y = 0; y < _roomHeight; y++) {
					var entity = Tiles[x, y];
					entity.GetComponent<DrawableComponent>().Glyph = '.';
					entity.GetComponent<NameComponent>().Name = "Floor";
				}
			}
		}
		public void CreateWalls() {
			for (int x = 0; x < Width; x++) {
				for (int y = 0; y < Height; y++) {
					Tiles[x,y] = World.EntityFactory.Get(new TileTemplate(x, y));
				}
			}
		}
	}
}