using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpidersResponses : MonoBehaviour
{

    private Animation anim;

    private void Start() {
        anim = GetComponent<Animation>();
    }

    public string Action(string action) {
        action = action.Substring(0, action.Length - 1);
        string result;
        switch(action) {
   
            case " Jump":
                Debug.Log("Ejecutando la acción JUMP");
                anim.Play("jump");
                result = "Saltando";
                break;

            case " Attack":
                Debug.Log("Ejecutando la acción ATTACK");
                anim.Play("attack1");
                anim.PlayQueued("attack2", QueueMode.CompleteOthers);
                result = "Atacando";
                break;

            case " Run":
                Debug.Log("Ejecutando la acción RUN");
                anim.Play("run");
                result = "Corriendo"; 
                break;

            case " Stop":
                Debug.Log("Ejecutando la acción STOP");
                anim.Stop();
                result = "Quieto";
                return result;

            case " Taunt":
                Debug.Log("Ejecutando la acción TAUNT");
                anim.Play("taunt");
                result = "Burlando";
                break;

            case " Kill":
                Debug.Log("Ejecutando la acción DEATH");
                anim.Play("death1");
                result = "Muriendo";
                break;

            default:
                Debug.Log("No se ha reconocido la acción!");
                result = "No te entiendo!";
                break;
        }

        anim.PlayQueued("idle", QueueMode.CompleteOthers);
        return result;
    }
}
