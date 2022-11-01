using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class JsonDataHandler : MonoBehaviour
{
    DataScript dataScript;
    [SerializeField]
    GameObject uiCardSuit;
    [SerializeField]
    GameObject uiCardValue;

    public TextAsset jsonFile;
    
    // Start is called before the first frame update
    void Start()
    {
        dataScript= new DataScript();
       
        dataScript = JsonUtility.FromJson<DataScript>(jsonFile.text);
        uiCardSuit.transform.GetComponent<TextMeshProUGUI>().SetText(dataScript.suit);
        uiCardValue.transform.GetComponent<TextMeshProUGUI>().SetText(dataScript.value);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
