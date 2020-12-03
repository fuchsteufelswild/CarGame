using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection;


public static class ExtensionMethods
{
    public static T SwapIndices<T>(this T[] array, int firstIndex, int secondIndex)
    {
        T temp = array[firstIndex];
        array[firstIndex] = array[secondIndex];
        array[secondIndex] = temp;

        return temp;
    }

    public static IEnumerable<FieldInfo> GetConstants(this System.Type type)
    {
        var infos = type.GetFields(BindingFlags.Public |
                                   BindingFlags.Static |
                                   BindingFlags.FlattenHierarchy);

        return infos.Where(fi => fi.IsLiteral && !fi.IsInitOnly);
    }

    public static IEnumerable<T> GetConstantValues<T>(this System.Type type) where T : class
    {
        var infos = GetConstants(type);

        return infos.Select(fi => fi.GetRawConstantValue() as T);
    }

}

