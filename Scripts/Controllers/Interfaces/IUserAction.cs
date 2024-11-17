using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUserAction
{
    void SetFlyMode(bool isPhy);
    void Hit(Vector3 position);
    void Restart();
}
