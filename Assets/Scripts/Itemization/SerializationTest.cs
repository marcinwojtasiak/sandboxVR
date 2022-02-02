using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.Newtonsoft.Json;

public class SerializationTest : MonoBehaviour
{
    public ItemData data;
    private SerializableItem serializable;
    private string jsonString;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            serializable = data.Serialize();
            jsonString = JsonConvert.SerializeObject(serializable);
            Debug.LogWarning(jsonString);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            serializable = JsonConvert.DeserializeObject<SerializableGun>(jsonString);
            GameObject obj = serializable.Deserialize();
            obj.transform.position = Vector3.zero;
        }
    }

}
