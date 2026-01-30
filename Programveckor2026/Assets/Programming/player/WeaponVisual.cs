using UnityEngine;

public class WeaponVisual : MonoBehaviour
{
    [SerializeField] private SpriteRenderer weaponRenderer;

    private void Awake()
    {
        if (weaponRenderer != null) weaponRenderer.enabled = false;
    }

    public void SetWeaponSprite(Sprite weaponSprite)
    {
        if (weaponRenderer == null) return;

        weaponRenderer.sprite = weaponSprite;
        weaponRenderer.enabled = true;
    }

    public void ClearWeapon()
    {
        weaponRenderer.sprite = null;
        weaponRenderer.enabled = false;
    }
}
