using UnityEngine;

public class SimpleHashTableTest : MonoBehaviour
{
    private void Start()
    {
        SimpleHashTable<string, int> hashTable = new SimpleHashTable<string, int>();
        hashTable.Add("One", 1);
        hashTable.Add("Two", 2);
        hashTable.Add("Three", 3);
        
        var array = hashTable.GetData();
        
        for (int i = 0; i < array.Length; i++)
        {
            var entry = array[i];
            if (entry.IsOccupied)
            {
                Debug.Log($"{i}번째: Key: {entry.Key}, Value: {entry.Value}");
            }
            else
            {
                Debug.Log($"{i}번째: 키없음");
            }
        }
    }
}
