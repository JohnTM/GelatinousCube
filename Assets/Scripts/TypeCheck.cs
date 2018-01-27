using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TypeCheck : MonoBehaviour {

    public string requiredType;

    public UnityEvent TypeCorrect;
    public UnityEvent TypeIncorrect;


    public void CheckType()
    {
        Transmissible thisTransmissable = GetComponent<Transmissible>();
        if (thisTransmissable.type.name == requiredType)
        {
            TypeCorrect.Invoke();
        }
        else if (thisTransmissable.type.name != requiredType)
        {
            TypeIncorrect.Invoke();
        }
    }
}
