using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace NavKeypad
{
    public class Keypad : MonoBehaviour
    {
        [Header("Puzzle Config")]
        private GameManager gameManager;
        [SerializeField] private int maxNumber = 9;
        [SerializeField] private int totalSequences = 3;
        [SerializeField] private float numberDisplayTime = 2f;
        [SerializeField] private float timeToRespond = 6f;
        [SerializeField] private float timeForTryAgain = 4f;
        private int currentNumber;
        private int currentSequence = 0;
        private int clickCount = 0;
        private bool isAwaitingInput = false;
        private Coroutine responseTimer;

        // Codigo ja existente
        [Header("Events")]
        [SerializeField] private UnityEvent onAccessGranted;
        [SerializeField] private UnityEvent onAccessDenied;

        [Header("Settings")]
        [SerializeField] private string accessGrantedText = "TOWER ON";
        [SerializeField] private string accessDeniedText = "ERROR MEM";

        [Range(0, 5)]
        [SerializeField] private float screenIntensity = 2.5f;
        [Header("Colors")]
        [SerializeField] private Color screenNormalColor = new Color(0.98f, 0.50f, 0.032f, 1f); //orangy
        [SerializeField] private Color screenDeniedColor = new Color(1f, 0f, 0f, 1f); //red
        [SerializeField] private Color screenGrantedColor = new Color(0f, 0.62f, 0.07f); //greenish
        [Header("SoundFx")]
        [SerializeField] private AudioClip buttonClickedSfx;
        [SerializeField] private AudioClip accessDeniedSfx;
        [SerializeField] private AudioClip accessGrantedSfx;
        [Header("Component References")]
        [SerializeField] private Renderer panelMesh;
        [SerializeField] private TMP_Text keypadDisplayText;
        [SerializeField] private AudioSource audioSource;


        private string currentInput;
        private bool displayingResult = false;
        private bool accessWasGranted = false;

        public void StartPuzzle()
        {
            gameManager = GameObject.Find("FpsController").GetComponent<GameManager>();
            StartNewSequence();
        }

        private void StartNewSequence()
        {
            if(currentSequence >= totalSequences)
            {
                StartCoroutine("CompletedPuzzle");
                return;
            }

            panelMesh.material.SetVector("_EmissionColor", screenNormalColor * screenIntensity);
            currentSequence++;
            GenerateRandomNumber();
        }

        private void GenerateRandomNumber()
        {
            currentNumber = UnityEngine.Random.Range(1, maxNumber);
            keypadDisplayText.text = "MEM: " + currentNumber.ToString();

            // Mostre o numero por um breve periodo e depois oculta-o
            StartCoroutine("DisplayNumberRoutine");
        }

        IEnumerator DisplayNumberRoutine()
        {
            yield return new WaitForSeconds(numberDisplayTime);
            keypadDisplayText.text = "WAITING"; // Hide number
            StartResponsePhase();
        }

        private void StartResponsePhase()
        {
            isAwaitingInput = true;
            clickCount = 0;

            // Inicie um cronometro para o jogador responder
            responseTimer = StartCoroutine("ResponseTimer");
        }

        IEnumerator ResponseTimer()
        {
            yield return new WaitForSeconds(timeToRespond);

            // O tempo acabou, falha
            StartCoroutine("FailPuzzle");
        }

        public void OnButtonPressed()
        {
            if (!isAwaitingInput) return;

            audioSource.PlayOneShot(buttonClickedSfx);
            clickCount++;

            if (clickCount == currentNumber)
            {
                // Entrada correta para esta sequencia
                StopCoroutine(responseTimer);
                isAwaitingInput = false;
                StartNewSequence();
            }
            else if (clickCount > currentNumber)
            {
                // Muitos cliques, falha
                StartCoroutine("FailPuzzle");
            }
        }

        IEnumerator FailPuzzle()
        {
            isAwaitingInput = false;
            audioSource.PlayOneShot(accessDeniedSfx);
            keypadDisplayText.text = accessDeniedText;
            panelMesh.material.SetVector("_EmissionColor", screenDeniedColor * screenIntensity);
            yield return new WaitForSeconds(timeForTryAgain);
            ResetPuzzle();
        }

        private void ResetPuzzle()
        {
            panelMesh.material.SetVector("_EmissionColor", screenNormalColor * screenIntensity);
            currentSequence = 0;
            StartNewSequence();
        }

        private void CompletedPuzzle()
        {
            accessWasGranted = true;
            keypadDisplayText.text = accessGrantedText;
            panelMesh.material.SetVector("_EmissionColor", screenGrantedColor * screenIntensity);
            audioSource.PlayOneShot(accessGrantedSfx);
            gameManager.Desafio3Finalizado();
        }
    }
}