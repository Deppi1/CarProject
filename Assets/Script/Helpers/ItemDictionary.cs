using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
///  Ключ-значение в словаре. Универсальный класс.
/// </summary>
/// <typeparam name="TKey">Ключ.</typeparam>
/// <typeparam name="TValue">Значение.</typeparam>
[Serializable]
public class ItemDictionary<TKey, TValue>
{
    public TKey Key;
    public TValue Value;
}
