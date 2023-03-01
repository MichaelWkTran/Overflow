using UnityEngine;
using static SaveSystem;

public class HatItem : ShopItem.ItemData
{
    public static HatItem m_currentHat = null;
    public AnimationClip m_hatAnimation;
    public Sprite m_hatSprite;

    public override void OnClick()
    {
        //Save Data
        m_data.m_currentSkinName = m_shopItem.m_name;
    }

    protected override void Load()
    {
        //Load data on whether the player had enabled this hat
        if (m_data.m_currentHatName == m_shopItem.name)
        {
            m_currentHat = this;
        }
    }
}
