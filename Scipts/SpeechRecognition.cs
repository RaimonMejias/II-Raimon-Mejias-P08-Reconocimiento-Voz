using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using HuggingFace.API;

public class SpeechRecognition : MonoBehaviour {

    private AudioClip clip;
    private byte[] bytes;
    private bool recording;
    private SelectionController selectionController_;
    private GameObject microphoneImage_;
    
    private void Start() {
        selectionController_ = GameObject.FindWithTag("SelectionController").GetComponent<SelectionController>();
        microphoneImage_ = GameObject.FindWithTag("MicrophoneImg").transform.GetChild(0).gameObject;
        Debug.Log(microphoneImage_);
    }

    private void Update() {

        if (Input.GetKeyDown("r")) { 
            StartRecording(); 
            microphoneImage_.SetActive(true);
        }
        else if (Input.GetKeyDown("s")) { 
            StopRecording();  
            microphoneImage_.SetActive(false);
        }
        if (recording && Microphone.GetPosition(null) >= clip.samples) {
            StopRecording();
        }
    }

    private void StartRecording() {
        clip = Microphone.Start(null, false, 10, 44100);
        recording = true;
    }

    private void StopRecording() {
        var position = Microphone.GetPosition(null);
        Microphone.End(null);
        var samples = new float[position * clip.channels];
        clip.GetData(samples, 0);
        bytes = EncodeAsWAV(samples, clip.frequency, clip.channels);
        recording = false;
        SendRecording();
    }

    private void SendRecording() {
        HuggingFaceAPI.AutomaticSpeechRecognition(bytes, 
            response => {
                selectionController_.SetObjectText(selectionController_.SetObjectAction(response));
            }, error => {
                selectionController_.SetObjectText(error, true);
            }
        );
    }
    
    private byte[] EncodeAsWAV(float[] samples, int frequency, int channels) {
        using (var memoryStream = new MemoryStream(44 + samples.Length * 2)) {
            using (var writer = new BinaryWriter(memoryStream)) {
                writer.Write("RIFF".ToCharArray());
                writer.Write(36 + samples.Length * 2);
                writer.Write("WAVE".ToCharArray());
                writer.Write("fmt ".ToCharArray());
                writer.Write(16);
                writer.Write((ushort)1);
                writer.Write((ushort)channels);
                writer.Write(frequency);
                writer.Write(frequency * channels * 2);
                writer.Write((ushort)(channels * 2));
                writer.Write((ushort)16);
                writer.Write("data".ToCharArray());
                writer.Write(samples.Length * 2);
                foreach (var sample in samples) {
                    writer.Write((short)(sample * short.MaxValue));
                }
            }
            return memoryStream.ToArray();
        }
    }
}
