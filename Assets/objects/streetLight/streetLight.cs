using UnityEngine;

public class streetLight : MonoBehaviour
{
    // Поля для спрайтов
    [SerializeField] private Sprite lighter_0;
    [SerializeField] private Sprite lighter_1;
    [SerializeField] private Sprite lighter_2;
    [SerializeField] private Sprite lighter_3;

    // Поле для выбора значения с ограничением от 0 до 45
    [Range(0, 60)]
    [Tooltip("Выберите одно из значений: 0, 15, 30, 45")]
    public int selectedValue;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateSprite();
    }
    void UpdateSprite()
    {
        if (selectedValue >= 0 && selectedValue < 15)
        {
            spriteRenderer.sprite = lighter_0;
        }
        else if (selectedValue >= 15 && selectedValue < 30)
        {
            spriteRenderer.sprite = lighter_1;
        }
        else if (selectedValue >= 30 && selectedValue < 45)
        {
            spriteRenderer.sprite = lighter_2;
        }
        else if (selectedValue >= 45 && selectedValue <= 60)
        {
            spriteRenderer.sprite = lighter_3;
        }
    }
    void OnValidate()
    {
        UpdateSprite();
    }
}
