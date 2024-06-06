using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace CoulsonEngine.Interaction
{
    public class BoxTrigger : MonoBehaviour
    {
        void Start()
        {
            this.hitBox = base.GetComponent<BoxCollider>();
        }

        private void OnTriggerEnter(Collider other)
	    {
		    if (other.gameObject.CompareTag("Player"))
		    {
			    if (!isClickTrigger && allowTrigger)
                {
                    whenEnter.Invoke();
                }
                else
                {
                    canBeClicked = true;
                    clickTriggerIcon.SetActive(true);
                }
		    }
        }

        private void OnTriggerExit(Collider other)
	    {
		    if (!isClickTrigger && allowTrigger)
            {
                whenExit.Invoke();
            }
            else
            {
                canBeClicked = false;
                clickTriggerIcon.SetActive(false);
            }
        }

        void Update()
        {
            if (canBeClicked && UnityEngine.Input.GetKeyDown(KeyCode.E))
            {
                whenEnter.Invoke();
            }

            if (!allowTrigger) 
            {
                clickTriggerIcon.SetActive(false); 
                canBeClicked = false;
            }
        }

        public void ToggleTrigger(bool toggle)
        {
            allowTrigger = toggle;
        }

        BoxCollider hitBox;

        public UnityEvent whenEnter;

        public UnityEvent whenExit;
        public bool canBeClicked;
        private bool allowTrigger = true;

        [SerializeField] private bool isClickTrigger = false;

        [SerializeField] private GameObject clickTriggerIcon;
    }
}
