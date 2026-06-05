

using UnityEngine;

namespace DeathbulgeArchipelagoClient;

class Utils
{
    public static string GetHierarchyPath(Transform t)
    {
        if (t.parent == null)
            return t.name;

        return GetHierarchyPath(t.parent) + "/" + t.name;
    }
}