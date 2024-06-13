using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class text_1 : MonoBehaviour
{
    
    public float moveDuration = 2f; // Длительность движения (в секундах)
    public float fadeDuration = 2f; // Длительность затухания (в секундах)
    public float shakeFrequency = 10f; // Частота тряски
    public float shakeMagnitude = 0.1f; // Амплитуда тряски

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Vector3 startPosition;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color; // Сохраняем изначальный цвет спрайта
        }
        else
        {
            Debug.LogWarning("SpriteRenderer component not found on GameObject.");
        }

        startPosition = transform.position;

        // Используем корутину для плавного движения, затухания и тряски
        StartCoroutine(MoveFadeShakeAndDestroyCoroutine());
    }

    IEnumerator MoveFadeShakeAndDestroyCoroutine()
    {
        float moveTimeElapsed = 0f;
        float fadeTimeElapsed = 0f;

        while (moveTimeElapsed < moveDuration || fadeTimeElapsed < fadeDuration)
        {
            // Прогресс движения (от 0 до 1)
            float moveProgress = moveTimeElapsed / moveDuration;

            // Интерполяция между начальной и целевой позициями для движения
            Vector3 targetPosition = startPosition + Vector3.up * 4f;
            transform.position = Vector3.Lerp(startPosition, targetPosition, moveProgress);

            // Прогресс затухания (от 0 до 1)
            float fadeProgress = fadeTimeElapsed / fadeDuration;

            // Интерполяция цвета для затухания
            if (spriteRenderer != null)
            {
                Color newColor = spriteRenderer.color;
                newColor.a = Mathf.Lerp(originalColor.a, 0f, fadeProgress); // Интерполяция альфа-канала к нулю
                spriteRenderer.color = newColor;
            }

            // Тряска объекта
            float shakeOffset = Mathf.Sin(Time.time * shakeFrequency) * shakeMagnitude;
            transform.position = new Vector3(startPosition.x + shakeOffset, transform.position.y, transform.position.z);

            // Увеличиваем время, прошедшее с начала движения и затухания
            moveTimeElapsed += Time.deltaTime;
            fadeTimeElapsed += Time.deltaTime;

            yield return null;
        }

        // Убеждаемся, что объект точно достигает целевой позиции
        transform.position = startPosition + Vector3.up * 4f;

        // Уничтожаем объект
        Destroy(gameObject);
    }
}
