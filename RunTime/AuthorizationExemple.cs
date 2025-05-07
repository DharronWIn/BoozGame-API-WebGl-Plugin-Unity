using BoozGameAPIProvider;
using UnityEngine;
using TMPro;

public class AuthorizationExemple : Authorization
{
    public GameObject canvasWarning;
    public GameObject transition;

    public override void Authorize()
    {
        base.Authorize();
        canvasWarning.SetActive(false);
        transition.SetActive(true);
    }

    public override void Unauthorize(string message)
    {
        base.Unauthorize(message);
        transition.SetActive(false);
        canvasWarning.SetActive(true);


    }
}
