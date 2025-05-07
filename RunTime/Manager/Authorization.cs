using UnityEngine;
using TMPro;

namespace BoozGameAPIProvider
{
    public class Authorization : MonoBehaviour
    {
        public TMP_Text warningText;
        public virtual void Authorize()
        {
            Debug.Log("Vous avez acces au jeu");
        }

        public virtual void Unauthorize(string message = "Vous n\'avez pas acces a ce jeu")
        {
            Debug.Log(message);
            warningText.text = message;

        }
    }
}

