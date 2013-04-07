﻿using System;
using System.Collections.Generic;

namespace XamlRemover.Extensions
{
    public static class EnumerableExtensions
    {
         public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
         {
             foreach (var item in enumerable)
                 action(item);
         }
    }
}