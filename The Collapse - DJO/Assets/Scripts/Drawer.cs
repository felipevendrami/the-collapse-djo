using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;

public class Drawer : MonoBehaviour
    {
        public Animator animator;
        public bool activate;
        public bool toggleState;
        public GameObject icon;

        public AudioClip sound_A;
        public AudioClip sound_B;

        AudioSource audioSource;

        // Start is called before the first frame update
        void Start()
        {
            activate = false;
            audioSource = GetComponent<AudioSource>();
        }

        // Update is called once per frame
        void Update()
        {
            if (activate)
            {
                icon.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (toggleState)
                    {
                        animator.Play("CloseDrawer");
                        audioSource.clip = sound_A;
                        audioSource.Play();
                        toggleState = false;
                    }
                    else
                    {
                        animator.Play("OpenDrawer");
                        audioSource.clip = sound_B;
                        audioSource.Play();
                        toggleState = true;
                    }
                }
            }
            else
            {
                icon.SetActive(false);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                activate = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                activate = false;
            }
        }
    }
