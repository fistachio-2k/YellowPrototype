using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;


[RequireComponent(typeof(PlayerAI))]
public class Controller : MonoBehaviour
{
    private PlayerControls _playerControls;
    private Camera _cam;
    private PlayerAI _ai;
    private LayerMask _leftMouseMask;
    private LayerMask _rightMouseMask;
    private Button[] _buttons;

    public Canvas rightClickCanvas;


    private void Awake()
    {
        _playerControls = new PlayerControls();
        _ai = GetComponent<PlayerAI>();
        _cam = Camera.main;
        _leftMouseMask = LayerMask.GetMask("Ground", "Obstruction");
        _rightMouseMask = LayerMask.GetMask("Interactable");
        _buttons = rightClickCanvas.GetComponentsInChildren<Button>(true);
    }

    private void OnEnable()
    {
        _playerControls.Enable();
    }

    private void OnDisable()
    {
        _playerControls.Disable();
    }
    
    // visualize the radius of interaction
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 5f);
    }

    
    void Update()
    {
        // Read Left Muse value
        bool goInput = _playerControls.Player.Go.triggered;

        // Read Right Muse value
        bool interactInput = _playerControls.Player.Interact.triggered;


        if (goInput)
        {
            Vector2 mousePos = _playerControls.Player.MosuePosition.ReadValue<Vector2>();
            RaycastHit hit;
            Ray ray = _cam.ScreenPointToRay(mousePos);
            if (Physics.Raycast(ray, out hit, 100f, _leftMouseMask) && !IsMouseOverUI())
            {
                rightClickCanvas.enabled = false;
                switch (LayerMask.LayerToName(hit.transform.gameObject.layer))
                {
                    case "Obstruction":
                        return;
                    case "Ground":
                        // Move player to hit point
                        _ai.MoveToPoint(hit.point);
                        break;
                }

            }
        }

        else if (interactInput)
        {
            Vector2 mousePos = _playerControls.Player.MosuePosition.ReadValue<Vector2>();
            RaycastHit hit;
            Ray ray = _cam.ScreenPointToRay(mousePos);
            if (Physics.Raycast(ray, out hit, 100f, _rightMouseMask))
            {
                // TODO Need to add distance check

                Interactable item = hit.transform.gameObject.GetComponent<Interactable>();
                
               if (item != null)
                {
                    Action[] events = item.CalcInteractions();
                    
                    // empty the buttons
                    Button btn1 = _buttons[0];
                    Button btn2 = _buttons[1];
                    btn1.onClick.RemoveAllListeners();
                    btn2.onClick.RemoveAllListeners();
                    Text txt1 = btn1.GetComponentInChildren<Text>();
                    Text txt2 = btn2.GetComponentInChildren<Text>();
                    txt1.text = "";
                    txt2.text = "";

                    if (events.Length >= 1) // first method
                    {
                        txt1.text = events[0].Method.Name;
                        btn1.onClick.RemoveAllListeners();
                        btn1.onClick.AddListener(delegate { events[0](); });
                    }
                    if (events.Length == 2) // second method
                    {
                        txt2.text = events[1].Method.Name;
                        btn2.onClick.RemoveAllListeners();
                        btn2.onClick.AddListener(delegate { events[1](); });
                    }
                    
                    rightClickCanvas.transform.position = hit.point;
                    rightClickCanvas.enabled = true;
                }
            }
        }
    }

    private void foo(int i)
    {
        Debug.Log("plzzz" + i);
    }

    private bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();    
    }
}
