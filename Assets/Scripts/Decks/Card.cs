using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class DeckCard {
    public int id;
    public Card card;
    public int quantity;
}

[Serializable]
public class Card {
    public int id;
    public string name;
    public int damage;
    public int health;
    public string image;

    public IEnumerator GetSprite(Action<Sprite> onComplete){
        string fileName = GetSafeFileName(image);
        string filePath = Path.Combine(Application.persistentDataPath, fileName);

        if (File.Exists(filePath))
        {
            byte[] bytes = File.ReadAllBytes(filePath);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(bytes);
            Sprite sprite = Sprite.Create(
                texture,
                new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f)
            );
            onComplete?.Invoke(sprite);
            yield break;
        }

        UnityWebRequest request = UnityWebRequestTexture.GetTexture(image);

        yield return request.SendWebRequest();

        if(request.result != UnityWebRequest.Result.Success) {
            Debug.LogError(request.error);
        }
        else {
            Texture2D texture = DownloadHandlerTexture.GetContent(request);
            byte[] textureBytes = texture.EncodeToPNG();
            File.WriteAllBytes(filePath, textureBytes);

            Sprite sprite = Sprite.Create(
                texture,
                new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f)
            );
            onComplete?.Invoke(sprite);
        }
    }

    private string GetSafeFileName(string url)
    {
        return Path.GetFileNameWithoutExtension(url).Replace('/', '_').Replace(':', '_') + ".png";
    }
    
}