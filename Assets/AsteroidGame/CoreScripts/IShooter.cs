using UnityEngine;

public interface IShooter
{
    Material GetBulletMaterial();
    void AddGamePoints(int gamePoints);
}
