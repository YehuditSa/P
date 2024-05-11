using System.Collections;
using System.Runtime.CompilerServices;
using System.Text;

namespace BO;
internal static class Tools
{

    /// <summary>
    /// A class that implementats IEqualityComparer for List<int?>
    /// </summary>
    public class NullableListIntComparer : IEqualityComparer<List<int?>>
    {
        public bool Equals(List<int?>? x, List<int?>? y)
        {
            if (x is not null && y is not null)
                return x.SequenceEqual(y);
            if (x is null && y is null)
                return true;
            return false;
        }

        //Return sum of elements in list
        public int GetHashCode(List<int?> obj)
        {
            int sum = 0;
            foreach (int? num in obj)
            {
                sum += num ?? default(int);
            }
            return sum;
        }
    }

    /// <summary>
    /// Creating a string with data on any entity - by Reflection
    /// </summary>
    /// <param name="obj"></param>
    /// <returns>String with data on any entity</returns>
    public static string ToStringProperty(this object obj)
    {
        StringBuilder sb = new StringBuilder();
        foreach (System.Reflection.PropertyInfo property in obj.GetType().GetProperties())
        {
            sb.Append(property.Name);
            sb.Append(": ");
            if (property.GetValue(obj, null) is null)
            {
                sb.Append("null");
            }
            else if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
            {
                var collection = (IEnumerable)property.GetValue(obj, null)!;
                sb.Append(System.Environment.NewLine);
                foreach (var elem in collection)
                {
                    sb.Append("***************" + System.Environment.NewLine + elem.ToString());
                }
            }
            else
            {
                sb.Append((property.GetValue(obj, null)));
            }

            sb.Append(System.Environment.NewLine);
        }

        return sb.ToString();
    }
}



