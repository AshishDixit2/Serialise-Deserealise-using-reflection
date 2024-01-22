using System;
using System.Reflection;
using System.Text;
 
public class CustomSerializer
{
    // Serialize object to JSON using reflection
    public static string Serialize<T>(T obj)
    {
        StringBuilder jsonBuilder = new StringBuilder("{");

        PropertyInfo[] properties = typeof(T).GetProperties();
        foreach (PropertyInfo property in properties)
        {
            jsonBuilder.Append($"\"{property.Name}\": \"{property.GetValue(obj)}\", ");
        }

        // Remove the trailing comma and space if there are properties
        if (properties.Length > 0)
            jsonBuilder.Remove(jsonBuilder.Length - 2, 2);

        jsonBuilder.Append("}");

        return jsonBuilder.ToString();
    }

    // Deserialize JSON to object using reflection
    public static T Deserialize<T>(string jsonString) where T : new()
    {
        T obj = new T();
        string[] keyValuePairs = jsonString.Trim('{', '}').Split(',');

        foreach (string keyValuePair in keyValuePairs)
        {
            string[] parts = keyValuePair.Split(':');
            string propertyName = parts[0].Trim('\"', ' ');
            string propertyValue = parts[1].Trim('\"', ' ');

            PropertyInfo property = typeof(T).GetProperty(propertyName);
            if (property != null)
            {
                Type propertyType = property.PropertyType;
                object value = Convert.ChangeType(propertyValue, propertyType);
                property.SetValue(obj, value);
            }
        }

        return obj;
    }
}

class Program
{
    static void Main()
    {
        // Example 
        MyClass myObject = new MyClass { Id = 1, Name = "Ashish", Gender = "Male" };

        // Serialize
        string jsonString = CustomSerializer.Serialize(myObject);
        Console.WriteLine("Serialized JSON: " + jsonString);

        // Deserialize
        MyClass deserializedObject = CustomSerializer.Deserialize<MyClass>(jsonString);
        Console.WriteLine($"Deserialized Object: Id={deserializedObject.Id}, Name={deserializedObject.Name}, Gender={deserializedObject.Gender}");
    }
}

public class MyClass
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Gender { get; set; }
}


























