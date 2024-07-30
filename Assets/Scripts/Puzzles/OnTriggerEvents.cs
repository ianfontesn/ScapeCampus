using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnTriggerEvents : MonoBehaviour
{
    [SerializeField] private UnityEvent<string, string> OnTriggerEnterEvent;
    [SerializeField] private UnityEvent<string, string> OnTriggerExitEvent;



    private void OnTriggerEnter(Collider other)
    {
        OnTriggerEnterEvent?.Invoke(other.tag, gameObject.tag);
    }

    private void OnTriggerExit(Collider other)
    {
        OnTriggerExitEvent?.Invoke(other.tag, gameObject.tag);
    }
}
