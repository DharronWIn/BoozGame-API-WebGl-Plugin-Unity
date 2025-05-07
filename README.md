# BoozGame WebGL API Plugin

Un plugin Unity simple et efficace pour intégrer l'API BoozGame à vos jeux **WebGL**.  
Il permet de **vérifier l'accès d'un joueur à un jeu** en fonction de son token, et de **sauvegarder ses données** dans notre base de données sécurisée.

---

## 🔗 Base URL de l'API

```
https://api.boozgame.com/
```

---

## ✅ Fonctionnalités

- Récupère automatiquement le **token utilisateur** et l'**ID du jeu** depuis l'URL.
- Vérifie l'accès du joueur via une requête à l'API.
- Permet d'afficher un message ou une action personnalisée selon que l'accès est autorisé ou non.
- Gère la **sauvegarde des données** du joueur via l'API.
- Facile à intégrer via un prefab Unity.

---

## ⚙️ Prérequis

Avant d'utiliser ce plugin, vous devez importer les dépendances suivantes dans votre projet Unity :

- [BestHTTP](https://assetstore.unity.com/packages/tools/network/best-http-2-155981)
- [Newtonsoft.Json for Unity (via UPM)](https://github.com/jilleJr/Newtonsoft.Json-for-Unity.git#upm)
- [TextMeshPro](https://docs.unity3d.com/Packages/com.unity.textmeshpro@latest)

---

## 🧱 Installation & Intégration

### 1. Ajouter le prefab `ClientManager`

- Placez le prefab **`ClientManager`** dans **la première scène** de votre jeu WebGL.
- Ce script est responsable de :
  - La récupération des données (`token`, `gameId`) depuis l'URL.
  - La vérification d’accès à l’API.
  - Le stockage des données utilisateur dans la variable `clientData`.
  - La gestion des requêtes de **sauvegarde des données**.

### 2. Personnaliser l’interface avec `Authorization`

- Le script **`Authorization`** permet de gérer les actions à effectuer **si le joueur est autorisé ou non** à accéder au jeu.
- Ce script peut être **surchargé** (`override`) pour ajouter des comportements personnalisés (ex. : afficher une popup, charger une UI, etc.).
- Dans **`ClientManager`**, vous pouvez renseigner votre propre version de `Authorization` depuis l’inspector Unity.

---

## 🧠 Structure des scripts

### `ClientManager`

- **Responsable principal de l’authentification et de la gestion des données.**
- Variables clés :
  - `clientData` : contient les données du joueur et du jeu.
- Fonctions :
  - Vérification d'accès à l’API `CheckUserAuthorized`.
  - Sauvegarde des données côté serveur `SaveGameData`.

### `AuthorizeManager`

- Classe de base contenant deux fonctions **virtual** à surcharger :
  ```csharp
  public virtual void Authorize() 
  public virtual void Unauthorize(string message)
  ```
- Utilisée pour réagir selon le résultat de la vérification d’accès.

---

## 🌐 Exemple d'URL WebGL attendue

Votre jeu doit être lancé avec une URL contenant ces paramètres :

```
https://yourgameurl.com?q=USER_TOKEN&gameId=GAME_ID
```

---

## 📤 Exemple de sauvegarde de données

```csharp

[Serializable]
public class GameDataDto
{
  public string gameId;
  public int score;
  public GameProgress gameProgress;
}

[Serializable]
public class GameProgress
{
  public int level;
  public int progress;
}
ClientManager.Instance.SaveGameData(GameDataDto gameDataDto);
```

---

## 🚀 Bonnes pratiques

- Toujours placer `ClientManager` en haut de la hiérarchie de la scène initiale.
- Surcharger `Authorization` pour personnaliser les messages ou actions (UI, redirection, blocage…).
- Tester localement avec des paramètres d'URL fictifs avant le déploiement.

---

## 📬 Support

Pour toute question ou intégration avancée, contactez-nous à :  
**support@boozgame.com**

---

## 📄 Licence

© By Lionel Abrogoua 2025 BoozGame. Tous droits réservés.