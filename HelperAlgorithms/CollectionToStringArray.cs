using System.Reflection;

namespace FastTrackEServices.HelperAlgorithms;

public class CollectionToStringArray : ITransform {

    public string[] ConvertCollection<T>(ICollection<T> collection)
    {
        // Assuming we are working with models
        string[] arr = new string[collection.Count];
        int i = 0;
        MethodInfo toString = typeof(T).GetMethod("ArrayName");
        foreach (T o in collection)
        {
            string output = (string) toString.Invoke(o, null);
            arr[i] = output;
            i++;
        }
        return arr;
    }
}