using System.Collections.Generic;
using SimpleJSON;

/// <summary>
/// This class provides utility functions for JSON values.
/// </summary>
public class JSONUtil
{
    /// <summary>
    /// Private constructor to prevent initialization.
    /// </summary>
    private JSONUtil() {}

    /// <summary>
    /// Converts a JSONArray to a List with strings.
    /// </summary>
    /// <param name="array">The JSONArray to be converted.</param>
    /// <returns>A List filled with all of the values in the JSONArray represented as strings.</returns>
    public static List<string> arrayToList(JSONArray array)
    {
        List<string> list = new List<string>();

        for (int i = 0; i < array.Count; i++)
        {
            list.Add(array[i]);
        }

        return list;
    }
}