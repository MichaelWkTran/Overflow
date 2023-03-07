using UnityEngine;
using static SaveSystem;

public class SkinItem : ShopItem.ItemData
{
    public static SkinItem m_currentSkin = null;
    public RuntimeAnimatorController m_skin;
    public Sprite m_deathSprite;

    public void SetSkin()
    {
        //Find the player
        Player player = FindObjectOfType<Player>();

        //Stop the function if the player does not exist
        if (player == null) return;

        //Save Data
        m_data.m_currentSkinName = m_shopItem.m_name;

        //Set the skin of the player
        player.GetComponent<Animator>().runtimeAnimatorController = m_skin;
        player.deathParticle.textureSheetAnimation.SetSprite(0, m_deathSprite);
    }

    public override void OnClick()
    {
        SetSkin();
    }

    protected override void Load()
    {
        //Load data on whether the player had enabled this skin
        if (m_data.m_currentSkinName == m_shopItem.m_name)
        {
            m_currentSkin = this;
            SetSkin();
        }
    }
}
