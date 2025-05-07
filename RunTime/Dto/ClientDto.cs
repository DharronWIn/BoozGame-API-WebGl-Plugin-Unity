using System;
using UnityEngine;

namespace BoozGameAPIProvider
{
    [Serializable]
    public class ClientDto 
{
    public string fullName;
    public object email;
    public string phoneNumber;
    public string profil;
    public DateTime createdAt;
    public DateTime updatedAt;
    }
}
