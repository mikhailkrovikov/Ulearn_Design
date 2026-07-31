namespace Inheritance.MapObjects;

public interface IAssignable
{
    public int Owner { get; set; }
}
public interface ICollecatble
{
    public Treasure Treasure { get; set; }
}
public interface IFightable
{
    public Army Army { get; set; }
}

public class Dwelling : IAssignable
{
    public int Owner { get; set; }
}

public class Mine : IAssignable, IFightable, ICollecatble
{
    public int Owner { get; set; }
    public Army Army { get; set; }
    public Treasure Treasure { get; set; }
}

public class Creeps : IFightable, ICollecatble
{
    public Army Army { get; set; }
    public Treasure Treasure { get; set; }
}

public class Wolves : IFightable
{
    public Army Army { get; set; }
}

public class ResourcePile : ICollecatble
{
    public Treasure Treasure { get; set; }
}

public static class Interaction
{
    public static void Make(Player player, object mapObject)
    {
        if (mapObject is IAssignable assignable and IFightable fightable1 and ICollecatble collecatble2)
        {
            HandleFightAndAssignAndCollect(player, assignable, fightable1, collecatble2);
            return;
        }
        if (mapObject is IFightable fightable2 and ICollecatble collecatble1)
        {
            HandleFightAndCollect(player, fightable2, collecatble1);
            return;
        }
        if (mapObject is IFightable fightable)
            HandleFight(player, fightable);
        if (mapObject is ICollecatble collecatble)
        {
            HandleCollecatble(player, collecatble);
            return;
        }
        if (mapObject is IAssignable dwellingObj)
        {
            HandleAssign(player, dwellingObj);
            return;
        }
    }

    private static void HandleFight(Player player, IFightable fightable)
    {
        if (!player.CanBeat(fightable.Army))
            player.Die();
    }

    private static void HandleAssign(Player player, IAssignable assignable)
    {
        assignable.Owner = player.Id;
    }

    private static void HandleCollecatble(Player player, ICollecatble collecatble)
    {
        player.Consume(collecatble.Treasure);
    }

    private static void HandleFightAndCollect(Player player, IFightable fightable, ICollecatble collecatble)
    {
        if (player.CanBeat(fightable.Army))
            player.Consume(collecatble.Treasure);
        else
            player.Die();
    }

    private static void HandleFightAndAssignAndCollect(Player player,
        IAssignable assignable,
        IFightable fightable,
        ICollecatble collecatble)
    {
        if (player.CanBeat(fightable.Army))
        {
            assignable.Owner = player.Id;
            player.Consume(collecatble.Treasure);
        }
        else player.Die();
    }
}