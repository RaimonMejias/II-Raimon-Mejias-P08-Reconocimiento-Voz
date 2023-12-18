using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SelectionController : MonoBehaviour
{

    [SerializeField] private GameObject arrow_;
    private GameObject selectedObject_;

    // Update is called once per frame
    void Update()
    {
        CheckKeys();
        if (selectedObject_ == null) { return; }
        arrow_.transform.position = selectedObject_.transform.position + Vector3.up;
        arrow_.SetActive(true);
    }

    private void CheckKeys() {
        if      (Input.GetKeyDown("1")) { selectedObject_ = GameObject.FindWithTag("SpiderA"); }
        else if (Input.GetKeyDown("2")) { selectedObject_ = GameObject.FindWithTag("SpiderB"); }
        else if (Input.GetKeyDown("3")) { selectedObject_ = GameObject.FindWithTag("SpiderC"); }
    }

    public void SetObjectText(string newText, bool isError = false) {
        if (selectedObject_ == null) { return; } 
        Transform canvas = selectedObject_.transform.GetChild(selectedObject_.transform.childCount - 1);
        canvas.GetChild(0).GetComponent<TMP_Text>().color = (isError)? Color.red : Color.white;
        canvas.GetChild(0).GetComponent<TMP_Text>().text = newText;
    }

    public string SetObjectAction(string action) {
        if (selectedObject_ == null) { return ""; }
        return selectedObject_.GetComponent<SpidersResponses>().Action(action);
    }
}
