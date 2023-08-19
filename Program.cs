using BattleBitAPI;
using BattleBitAPI.Common;
using BattleBitAPI.Server;
using System.Linq.Expressions;
using System.Net;
using System.Numerics;
using System.Threading.Channels;
using System.Xml;

class Program
{
    static void Main(string[] args)
    {
        var listener = new ServerListener<MyPlayer, MyGameServer>();
        listener.OnCreatingGameServerInstance += OnCreatingGameServerInstance;
        listener.OnCreatingPlayerInstance += OnCreatingPlayerInstance;
        listener.Start(29294);

        Thread.Sleep(-1);
    }

    private static MyPlayer OnCreatingPlayerInstance()
    {
        return new MyPlayer("asdasd");
    }

    private static MyGameServer OnCreatingGameServerInstance()
    {
        return new MyGameServer("mysecretDBpass");
    }
}
class MyPlayer : Player<MyPlayer>
{
    private string mydb;
    public MyPlayer(string mydb)
    {
        this.mydb = mydb;
    }

    public override async Task OnSpawned()
    {
    }
}
class MyGameServer : GameServer<MyPlayer>
{
    private string myDbConnection;
    public MyGameServer(string mySecretDBConnection)
    {
        this.myDbConnection = mySecretDBConnection;
    }

    public override async Task OnConnected()
    {
        ForceStartGame();
        ServerSettings.PlayerCollision = true;
    }
    public override async Task OnTick()
    {
    }
    public override async Task<OnPlayerSpawnArguments> OnPlayerSpawning(MyPlayer player, OnPlayerSpawnArguments request)
    {
        request.Wearings.Eye = "Eye_Zombie_01";
        request.Wearings.Face = "Face_Zombie_01";
        request.Wearings.Face = "Hair_Zombie_01";
        request.Wearings.Skin = "Zombie_01";
        request.Wearings.Uniform = "ANY_NU_Uniform_Zombie_01";
        request.Wearings.Head = "ANV2_Universal_Zombie_Helmet_00_A_Z";
        request.Wearings.Belt = "ANV2_Universal_All_Belt_Null";
        request.Wearings.Backbag = "ANV2_Universal_All_Backpack_Null";
        request.Wearings.Chest = "ANV2_Universal_All_Armor_Null";

        return request;
    }

    public override async Task OnPlayerConnected(MyPlayer player)
    {
        await Console.Out.WriteLineAsync("Connected: " + player);
        // send message
         player.Message("Welcome to シSTRANGE_S ∄ ∀ Lシ Server, " + player.Name + "");
    }
    public override async Task OnPlayerSpawned(MyPlayer player)
    {
        await Console.Out.WriteLineAsync("Spawned: " + player);
    }
    public override async Task OnAPlayerDownedAnotherPlayer(OnPlayerKillArguments<MyPlayer> args)
    {
        await Console.Out.WriteLineAsync("Downed: " + args.Victim);
        args.Killer.Message("You downed " + args.Victim.Name + " Fucker!");
        args.Victim.Message("You got abs smoked by" + args.Killer.Name);
    }
    public override async Task OnPlayerGivenUp(MyPlayer player)
    {
        await Console.Out.WriteLineAsync("Giveup: " + player);
    }
    public override async Task OnPlayerDied(MyPlayer player)
    {
        await Console.Out.WriteLineAsync("Died: " + player);
    }
    public override async Task OnAPlayerRevivedAnotherPlayer(MyPlayer from, MyPlayer to)
    {
        await Console.Out.WriteLineAsync(from + " revived " + to);
    }
    public override async Task OnPlayerDisconnected(MyPlayer player)
    {
        await Console.Out.WriteLineAsync("Disconnected: " + player);
    }

    public async Task OnRoundStarted()
    {
        var playersList = AllPlayers.ToList();
        var random = new Random();
        foreach (var player in playersList)
        {
            // now change the players health
            player.HP = 300;
            player.Modifications.JumpHeightMultiplier = Single.MaxValue;
        }
        
    }
}
