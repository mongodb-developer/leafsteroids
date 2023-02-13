using System;
using System.Collections.Generic;
using Game;
using TMPro;
using UnityEngine;

namespace ReplaySystem
{
    public class Recorder : MonoBehaviour
    {
        [SerializeField] private Pacman pacman;
        [SerializeField] private Ghost chuck;
        [SerializeField] private Ghost nic;
        [SerializeField] private Ghost hubert;
        [SerializeField] private Ghost dominic;

        [SerializeField] private TMP_Text logTextField;
        [SerializeField] private AtlasHelper atlasHelper;

        public readonly List<Snapshot> Snapshots = new();

        private string log = "foo";

        public void StartNewRecording()
        {
            Snapshots!.Clear();
        }

        public void PersistRecording()
        {
            var recording = new Recording
            {
                DateTime = DateTime.Now,
                Snapshots = Snapshots
            };
            Debug.Log("Recorder.PersistRecording");
            log = atlasHelper!.PersistRecording(recording);
            Debug.Log(log);
        }

        private void Start()
        {
            Debug.Log(log);
            InvokeRepeating(nameof(CreateSnapshot), 0f, Constants.RecordingSpeed);
            PersistRecording();
        }

        private void Update()
        {
            if (logTextField!.text == log) return;
            logTextField!.text = log;

            Debug.Log(log);
        }

        private void CreateSnapshot()
        {
            var snapshot = new Snapshot
            {
                PacManPosition = Helper.InvertY(pacman!.transform.position),
                ChuckPosition = Helper.InvertY(chuck!.transform.position),
                NicPosition = Helper.InvertY(nic!.transform.position),
                HubertPosition = Helper.InvertY(hubert!.transform.position),
                DominicPosition = Helper.InvertY(dominic!.transform.position)
            };
            Snapshots!.Add(snapshot);
        }
    }
}