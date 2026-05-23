using System.Collections.Generic;
public class TaggedValue<T>
{
    private readonly T _value;
    public T Value => _value;
    private readonly string _name;
    public string Name => _name;
    public TaggedValue(T value, string name)
    {
        _value = value;
        _name = name;
    }
}
public static class GameActionListMethods
{
    public static bool AddUniqueGameAction(this List<GameAction> list, GameAction ga)
    {
        bool replacedold = false;
        for (int i = list.Count - 1; i >= 0; i--)
        {
            if (list[i].ScriptName == ga.ScriptName)
            {
                list.Remove(list[i]);
                replacedold = true;
                break;
            }
        }
        list.Add(ga);
        return replacedold;
    }
    public static bool AddUniqueGameAction<T>(this List<GameAction<T>> list, GameAction<T> ga)
    {
        bool replacedold = false;
        for (int i = list.Count - 1; i >= 0; i--)
        {
            if (list[i].ActionName == ga.ActionName)
            {
                list.Remove(list[i]);
                replacedold = true;
                break;
            }
        }
        list.Add(ga);
        return replacedold;
    }
    public static bool AddUniqueGameFunc<T, TResult>(this List<GameFunc<T, TResult>> list, GameFunc<T, TResult> gf)
    {
        bool replacedOld = false;
        for (int i = list.Count - 1; i >= 0; i--)
        {
            if (list[i].ScriptName == gf.ScriptName)
            {
                list.Remove(list[i]);
                replacedOld = true;
                break;
            }
        }
        list.Add(gf);
        return replacedOld;
    }
    public static bool RemoveGameFunc<T, TResult>(this List<GameFunc<T,TResult>> list, string gfName)
    {
        bool success = false;
        for (int i = list.Count - 1; i >= 0; i--)
        {
            if (list[i].ScriptName == gfName) { success = list.Remove(list[i]); }
        }
        return success;
    }
    public static bool AddUnique<T>(this List<TaggedValue<T>> list, TaggedValue<T> item)
    {
        bool replacedold = false;
        for (int i = list.Count - 1; i >= 0; i--)
        {
            if (list[i].Name == item.Name)
            {
                list.Remove(list[i]);
                replacedold = true;
                break;
            }
        }
        list.Add(item);
        return replacedold;
    }
    public static bool RemoveByName<T>(this List<TaggedValue<T>> list, string name)
    {
        bool success = false;
        for (int i = list.Count - 1; i >= 0; i--)
        {
            if (list[i].Name == name) { success = list.Remove(list[i]); }
        }
        return success;
    }
    public static bool RemoveGameAction(this List<GameAction> list, string gaName)
    {
        bool success = false;
        for (int i = list.Count - 1; i >= 0; i--)
        {
            if (list[i].ScriptName == gaName) { success = list.Remove(list[i]); }
        }
        return success;
    }
    public static bool RemoveGameAction<T>(this List<GameAction<T>> list, string gaName)
    {
        bool success = false;
        for (int i = list.Count - 1; i >= 0; i--)
        {
            if (list[i].ActionName == gaName) { success = list.Remove(list[i]); }
        }
        return success;
    }
    public static void Execute<T>(this List<GameAction<T>> list, T t)
    {
        for (int i = list.Count - 1; i >= 0; i--)
        {
            list[i].action.Invoke(t);
        }
    }
    public static void Execute(this List<GameAction> list)
    {
        for (int i = list.Count - 1; i >= 0; i--)
        {
            list[i].action.Invoke();
        }
    }

    public static string[] GetNames(this List<GameAction> list)
    {
        List<string> strings = new List<string>();
        foreach (GameAction action in list)
        {
            strings.Add(action.ScriptName);
        }
        return strings.ToArray();
    }
    public static void LogNames(this List<GameAction> list)
    {
        foreach (GameAction action in list)
        {
            Logger.Log(action.ScriptName, new(logLevel: LogLevel.Debug));
        }
    }
}
