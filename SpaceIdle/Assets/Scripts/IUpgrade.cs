using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUpgrade
{
    void OpenPanel();
    void Upgrade(int abilityBtnIx);
    void AutoInitialize();
    void Wear(int itemCardIx);
    void UpdateDetailTxt();
}
