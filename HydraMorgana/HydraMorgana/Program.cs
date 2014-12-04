using System;
using LeagueSharp;
using LeagueSharp.Common;
using HydraMorgana;

namespace HydraMorgana
{
	class Program
	{
		public const int LocalVersion = 1;
        public const String Version = "1.0.*";

		public static Champion Champion;
		public static Menu Menu;
		public static Orbwalking.Orbwalker Orbwalker;
        public static Helper Helper;
		private static void Main(string[] args)
		{
			CustomEvents.Game.OnGameLoad  += Game_OnGameLoad;
		}

		private static void Game_OnGameLoad(EventArgs args)
		{
			//AutoUpdater.InitializeUpdater();
			Helper = new Helper();

            Menu = new Menu("HydraMorgana", "HydraMorgana_" + ObjectManager.Player.ChampionName, true);

			var targetSelectorMenu = new Menu("Target Selector", "TargetSelector");
			SimpleTs.AddToMenu(targetSelectorMenu);
			Menu.AddSubMenu(targetSelectorMenu);
			{
				var orbwalking = Menu.AddSubMenu(new Menu("Orbwalking", "Orbwalking"));
				Orbwalker = new Orbwalking.Orbwalker(orbwalking);
				Menu.Item("FarmDelay").SetValue(new Slider(0, 0, 200));
			}
			var potionManager = new PotionManager();
		
			try
			{
                var handle = System.Activator.CreateInstance(null, "HydraMorgana." + ObjectManager.Player.ChampionName);
				Champion = (Champion) handle.Unwrap();
			}
			catch (Exception)
			{
			}
					
			Menu.AddToMainMenu();
		}
	}
}
