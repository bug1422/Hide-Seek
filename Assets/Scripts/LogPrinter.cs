using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogPrinter : MonoBehaviour
{
    public static LogPrinter Instance = new LogPrinter();
    public void Print(params object[] objects)
    {
        foreach (object o in objects)
        {
            print(o);
        }
    }
}
