# Player prefs inspired save system

Save data is organized into contexts, which are keyed by strings. This allows for saving and 
loading into save slots, or saving and loading a "root" context.

## Usage Example


This script will maintain a list of strings that have been seen, and save them to the save system.
The "seen" strings can be queried from outside, of managed with logic contained to this class.

```csharp
public class SomethingThatSavesData : MonoBehaviour
{
    // To use Save Slots, this context may be set externally.
    //    or queried from the game object heirarchy
    [SerializeField] private string saveContextName = "SomeContextName";
    private readonly string _saveKeyRoot = $"{nameof(SomethingThatSavesData)}_SeenStrings";
    
    private ISaveDataContext _saveContext;
    private ISaveDataContext SaveContext
    {
        get
        {
            // the _saveContext may be invalidated, in which case, we need a new one
            if (_saveContext?.IsAlive == false) _saveContext = null;
            
            if (_saveContext == null)
            {
                // if we don't have a context, use the SingletonLocator<ISaveDataContextProvider>
                //  utility to get one, using the current context name
                _saveContext = SingletonLocator<ISaveDataContextProvider>.Instance
                    .GetContext(seenAbilitiesContextName);
            }

            return _saveContext;
        }
    }
    
    private bool HasSeen(string thing)
    {
        var key = _saveKeyRoot + "_" + "set";
        if(SaveContext.TryLoad(key, out List<string> seenSet))
        {
            return seenSet.Contains(thing);
        }

        return false;
    }

    private void SetSeen(string thing, bool seen = true)
    {
        var key = _saveKeyRoot + "_" + "set";
        if(!SaveContext.TryLoad(key, out List<string> seenSet))
        {
            seenSet = new List<string>();
        }
        var containsThing = seenSet.Contains(thing);
        if (seen)
        {
            if (containsThing) return;
            seenSet.Add(thing);
        }

        if (!seen)
        {
            if (!containsThing) return;
            seenSet.Remove(thing);
        }
        
        SaveContext.Save(key, seenSet);
    }
}
```

The json will be saved to `{Application.persistentDataPath}/SaveContexts/SomeContextName.json`, in this format:
```json
{
  "SomethingThatSavesData_SeenStrings_set": [
    "thing1",
    "thing_Two",
    "another thingamajig"
  ]
}
```