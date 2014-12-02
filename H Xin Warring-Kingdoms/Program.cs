using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;
namespace Hxin
{
    class Program
    {
        public static string ChampName = "XinZhao";
        public static Orbwalking.Orbwalker Orbwalker;
        public static Obj_AI_Base Player = ObjectManager.Player;
        public static Spell Q, W, E, R;
        public static Items.Item hydra = new Items.Item(3074, 400);
        public static Items.Item tiamat = new Items.Item(3077, 400);
        public static Items.Item BoRK = new Items.Item(3153, 400);
        private static SpellSlot IgniteSlot;


        public static Menu HXin;
        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        static void Game_OnGameLoad(EventArgs args)
        {
            if (Player.BaseSkinName != ChampName) return;

            Q = new Spell(SpellSlot.Q, 0);
            W = new Spell(SpellSlot.W, 0);
            E = new Spell(SpellSlot.E, 600);
            R = new Spell(SpellSlot.R, 180);


            HXin = new Menu("H Xin", ChampName, true);
            HXin.AddSubMenu(new Menu("Orbwalker", "Orbwalker"));
            Orbwalker = new Orbwalking.Orbwalker(HXin.SubMenu("Orbwalker"));
            var ts = new Menu("Target Selector", "Target Selector");
            SimpleTs.AddToMenu(ts);
            HXin.AddSubMenu(ts);
            HXin.AddSubMenu(new Menu("Combo", "Combo"));
            HXin.SubMenu("Combo").AddItem(new MenuItem("useQ", "Use Q").SetValue(true));
            HXin.SubMenu("Combo").AddItem(new MenuItem("useW", "Use W").SetValue(true));
            HXin.SubMenu("Combo").AddItem(new MenuItem("useE", "Use E").SetValue(true));
            HXin.SubMenu("Combo").AddItem(new MenuItem("useR", "Use R").SetValue(true));
            HXin.SubMenu("Combo").AddItem(new MenuItem("ComboActive", "Combo").SetValue(new KeyBind(32, KeyBindType.Press)));
            

            HXin.AddSubMenu(new Menu("KillSteal", "Ks"));
            HXin.SubMenu("Ks").AddItem(new MenuItem("ActiveKs", "Use KillSteal")).SetValue(true);
            HXin.SubMenu("Ks").AddItem(new MenuItem("UseRKs", "Use R")).SetValue(true);
            HXin.AddToMainMenu();


            Drawing.OnDraw += Drawing_OnDraw; 
            Game.OnGameUpdate += Game_OnGameUpdate;

            Game.PrintChat("H Xin Warring-Kingdoms loaded!");
        }

        static void Game_OnGameUpdate(EventArgs args)
        {
            if (HXin.Item("ComboActive").GetValue<KeyBind>().Active)
            {
                Combo();
            }
            if (HXin.Item("ActivateKs").GetValue<bool>())
            {
                KillSteal();
            }

        }

        static void Drawing_OnDraw(EventArgs args)
        {
            Utility.DrawCircle(Player.Position, E.Range, Color.Crimson);
        }

        public static void Combo()
        {
            var target = SimpleTs.GetTarget(E.Range, SimpleTs.DamageType.Physical);
            if (target == null) return;

            if (target.IsValidTarget(hydra.Range) && hydra.IsReady())
                hydra.Cast();

            if (target.IsValidTarget(tiamat.Range) && tiamat.IsReady())
                tiamat.Cast();

            if (target.IsValidTarget(BoRK.Range) && BoRK.IsReady())
                BoRK.Cast(target);

            if (target.IsValidTarget(E.Range) && Q.IsReady())
            {
                Q.Cast();

            }
            if (target.IsValidTarget(E.Range) && W.IsReady())
            {
                W.Cast();
            }
            if (target.IsValidTarget(E.Range) && E.IsReady())
            {
                E.Cast(target);
            }
            var RDmg = DamageLib.getDmg(target, DamageLib.SpellType.R);
            if (target.IsValidTarget(R.Range) && R.IsReady() && Player.Distance(target)>= R.Range)
              if (target.Health < RDmg)
             {
              R.Cast();
             }
        }

        public static void KillSteal()
        {
            var target = SimpleTs.GetTarget(R.Range, SimpleTs.DamageType.Physical);
            var igniteDmg = DamageLib.getDmg(target, DamageLib.SpellType.IGNITE);
            var RDmg = DamageLib.getDmg(target, DamageLib.SpellType.R);

            {
                if (HXin.Item("UseRKS").GetValue<bool>() && target != null && R.IsReady() && target.IsValidTarget(180))
                {
                    if (target.Health < RDmg)
                    {
                        R.Cast();
                    }
                }
            }




        }

    }
}
