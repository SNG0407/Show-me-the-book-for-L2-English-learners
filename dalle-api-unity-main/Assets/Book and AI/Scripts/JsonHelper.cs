using System;
using UnityEngine;

public static class JsonHelper
{
    [Serializable]
    public class ArrayWrapper<T>
    {
        public T[] items;
    }

    public static T[] FromJson<T>(string json)
    {
        ArrayWrapper<T> wrapper = JsonUtility.FromJson<ArrayWrapper<T>>(json);
        return wrapper.items;
    }

    public static string ToJson<T>(T[] array)
    {
        ArrayWrapper<T> wrapper = new ArrayWrapper<T>();
        wrapper.items = array;
        return JsonUtility.ToJson(wrapper);
    }
}
