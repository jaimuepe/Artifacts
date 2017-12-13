
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InputController : MonoBehaviour
{
    Player player;

    public RopeTargetRenderer ropeTargetRenderer;

    public PauseCanvas pauseCanvas;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    void Update()
    {
        foreach (char c in Input.inputString)
        {
            CheatCodeManager.instance.CheckForCheatCodes(c);
        }

        if (Input.GetButtonDown("Pause"))
        {
            pauseCanvas.SwitchState();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            GameManager.instance.usingKeyboardMouse = !GameManager.instance.usingKeyboardMouse;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (player.CanDoStuff)
            {
                player.KillIgnoreCheats(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ropeTargetRenderer.gameObject.SetActive(!ropeTargetRenderer.gameObject.activeSelf);
        }

        if (player.CanDoStuff)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            bool movementLocked = false;

            if (!GameManager.instance.usingKeyboardMouse)
            {
                movementLocked = Input.GetAxis("Lock movement") > 0.5f;
            }

            if (movementLocked)
            {
                h = 0;
                v = 0;
            }

            player.movementDirection = new Vector2(h, v).normalized;

            if (Input.GetButtonDown("Fire1"))
            {
                player.ThrowRope();
            }

            if (Input.GetButtonDown("Jump"))
            {
                player.Jump();
            }

            if (Input.GetButtonDown("Cancel hook"))
            {
                player.CancelHook();
            }

        }
        else
        {
            player.movementDirection = new Vector2(0f, 0f);
        }
    }
}
