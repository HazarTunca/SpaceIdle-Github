using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShop
{
    void OpenPanel();
    void SelectButton(int cardIx);
    void Purchase(int cardIx);
    void AutoInitialize();
}
