using System;
using System.Collections.Generic;
using UnityEditor;
using TriInspector;
using UnityEngine;

public static class SearchableUtils
{
    public static bool HasSearchableAttribute(TriProperty property)
    {
        return property.TryGetAttribute<SearchableAttribute>(out _);
    }

    public static List<int> FilterIndices(IReadOnlyList<TriProperty> elements, string searchText)
    {
        var indices = new List<int>();

        if (string.IsNullOrEmpty(searchText))
        {
            for (int i = 0; i < elements.Count; i++)
                indices.Add(i);
            return indices;
        }

        for (int i = 0; i < elements.Count; i++)
        {
            if (IsPropertyMatch(elements[i], searchText))
                indices.Add(i);
        }
        return indices;
    }

    public static bool IsPropertyMatch(TriProperty property, string searchText)
    {
        if (string.IsNullOrEmpty(searchText))
            return true;

        if (!(property.PropertyType == TriPropertyType.Generic || property.PropertyType == TriPropertyType.Reference)
            || property.ChildrenProperties.Count == 0)
        {
            var val = property.GetValue(0)?.ToString();
            return val != null && val.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        foreach (var child in property.ChildrenProperties)
        {
            if (IsPropertyMatch(child, searchText))
                return true;
        }
        return false;
    }
}

