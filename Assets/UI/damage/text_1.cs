using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class text_1 : MonoBehaviour
{
    
    public float moveDuration = 2f; // ������������ �������� (� ��������)
    public float fadeDuration = 2f; // ������������ ��������� (� ��������)
    public float shakeFrequency = 10f; // ������� ������
    public float shakeMagnitude = 0.1f; // ��������� ������

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Vector3 startPosition;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color; // ��������� ����������� ���� �������
        }
        else
        {
            Debug.LogWarning("SpriteRenderer component not found on GameObject.");
        }

        startPosition = transform.position;

        // ���������� �������� ��� �������� ��������, ��������� � ������
        StartCoroutine(MoveFadeShakeAndDestroyCoroutine());
    }

    IEnumerator MoveFadeShakeAndDestroyCoroutine()
    {
        float moveTimeElapsed = 0f;
        float fadeTimeElapsed = 0f;

        while (moveTimeElapsed < moveDuration || fadeTimeElapsed < fadeDuration)
        {
            // �������� �������� (�� 0 �� 1)
            float moveProgress = moveTimeElapsed / moveDuration;

            // ������������ ����� ��������� � ������� ��������� ��� ��������
            Vector3 targetPosition = startPosition + Vector3.up * 4f;
            transform.position = Vector3.Lerp(startPosition, targetPosition, moveProgress);

            // �������� ��������� (�� 0 �� 1)
            float fadeProgress = fadeTimeElapsed / fadeDuration;

            // ������������ ����� ��� ���������
            if (spriteRenderer != null)
            {
                Color newColor = spriteRenderer.color;
                newColor.a = Mathf.Lerp(originalColor.a, 0f, fadeProgress); // ������������ �����-������ � ����
                spriteRenderer.color = newColor;
            }

            // ������ �������
            float shakeOffset = Mathf.Sin(Time.time * shakeFrequency) * shakeMagnitude;
            transform.position = new Vector3(startPosition.x + shakeOffset, transform.position.y, transform.position.z);

            // ����������� �����, ��������� � ������ �������� � ���������
            moveTimeElapsed += Time.deltaTime;
            fadeTimeElapsed += Time.deltaTime;

            yield return null;
        }

        // ����������, ��� ������ ����� ��������� ������� �������
        transform.position = startPosition + Vector3.up * 4f;

        // ���������� ������
        Destroy(gameObject);
    }
}
