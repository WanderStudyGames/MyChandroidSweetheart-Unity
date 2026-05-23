using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GameAction
{
    public readonly Action action;
    public string ScriptName { get; set; }
    public GameAction(Action f, string n) { action = f; ScriptName = n; }
    public void InvokeAction()
    {
        action.Invoke();
    }

}
public class GameAction<T>
{
    public readonly Action<T> action;
    public string ActionName { get; set; }
    public GameAction(Action<T> f, string n) { action = f; ActionName = n; }
    public void InvokeAction(T t)
    {
        action.Invoke(t);
    }
}

public class GameFunc<T, TResult>
{
    public Func<T, TResult> func;
    public string ScriptName { get; set; }
    public GameFunc(Func<T, TResult> f, string n) { func = f; ScriptName = n; }
    public TResult InvokeFunc(T t)
    {
        return func(t);
    }
}

