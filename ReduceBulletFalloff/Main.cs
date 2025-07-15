using BepInEx;
using MonoMod.Cil;

namespace ReduceBulletFalloff
{
  [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
  public class Main : BaseUnityPlugin
  {
    public const string PluginGUID = PluginAuthor + "." + PluginName;
    public const string PluginAuthor = "Nuxlar";
    public const string PluginName = "ReduceBulletFalloff";
    public const string PluginVersion = "1.0.0";

    internal static Main Instance { get; private set; }
    public static string PluginDirectory { get; private set; }

    public void Awake()
    {
      Instance = this;

      Log.Init(Logger);

      IL.RoR2.BulletAttack.CalcFalloffFactor += ChangeFalloff;
    }

    private void ChangeFalloff(ILContext il)
    {
      ILCursor c = new ILCursor(il);
      if (c.TryGotoNext(MoveType.Before,
     x => x.MatchLdcR4(60f)
      ))
      {
        c.Next.Operand = 120f;
        c.GotoNext(MoveType.Before, x => x.MatchLdcR4(25f));
        c.Next.Operand = 50f;
      }
      else
        Log.Error("ReduceBulletFalloff: Failed to increase bullet falloff range");
    }
  }
}