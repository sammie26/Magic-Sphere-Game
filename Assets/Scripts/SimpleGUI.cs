using System;
using UnityEngine;
using UnityEngine.UIElements;

public class GuiScript : MonoBehaviour
{
    public enum CurrentState
    {
        Lost,
        Won,
        Resting,
        Climbing,
    }
    private ShieldLogic shield;
    private PlayerMovement playerMovement;
    private PlayerGameLogic playerGameLogic;
    private Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shield = GetComponent<ShieldLogic>();
        rb = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();
        playerGameLogic = GetComponent<PlayerGameLogic>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    void OnGUI()
    {
        // https://docs.unity3d.com/6000.0/Documentation/ScriptReference/MonoBehaviour.OnGUI.html
        // https://docs.unity3d.com/6000.0/Documentation/ScriptReference/GUI.Label.html
        GUI.Label(new Rect(10, 10, 200, 20), $"Remaining Armour: {shield.ShieldPercentage:F2}%");
        GUI.Label(new Rect(10, 30, 200, 20), $"Movement Speed: {GetCurrentMovementSpeed()}");
        GUI.Label(new Rect(10, 70, 200, 20), $"Current Status: {GetCurrentState()}");
        GUI.Label(new Rect(10, 50, 200, 20), $"Distance to Portal: {playerGameLogic.DistanceToPortal:F2}");
        GUI.Label(new Rect(10, 90, 400, 20), $"Angular Velocity: {rb.angularVelocity.magnitude:F2}");
        GUI.Label(new Rect(10, 110, 400, 20), $"Linear Velocity: {rb.linearVelocity.magnitude:F2}");

        // Remaining Armor (as a percentage of shields left). 
        // 2. Moving Speed (either Slow or Fast depending on whether the player is sprinting). 
        // 3. Absolute Distance between the player and the portal. 
        // 4. Current Player Status (one of the following four): 
        // ▪ Resting → If no movement input is detected. 
        // ▪ Climbing → If the player is moving. 
        // ▪ Lost → If the player has lost all shields. 
        // ▪ Won → If the player reaches the portal.
    }

    private CurrentState GetCurrentState()
    {
        // Debug.Log($"playerGameLogic.currentState: {playerGameLogic.CurrentState}");
        // Debug.Log($"playerMovement.movementState: {playerMovement.movementState}");
        // return CurrentState.Won;

        if (playerGameLogic.CurrentState == PlayerGameLogic.State.Won)
        {
            return CurrentState.Won;
        }
        if (playerGameLogic.CurrentState == PlayerGameLogic.State.Lost)
        {
            return CurrentState.Lost;
        }
        if (playerMovement.movementState == PlayerMovement.MovementState.Resting)
        {
            return CurrentState.Resting;
        }
        return CurrentState.Climbing;
    }

    private PlayerMovement.MovementSpeed GetCurrentMovementSpeed()
    {
        return playerMovement.movementSpeed;
    }

}
