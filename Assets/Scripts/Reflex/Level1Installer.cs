using Reflex.Core;
using UnityEngine;

public class Level1Installer : MonoBehaviour, IInstaller
{
    [SerializeField] private Player _player;
    public void InstallBindings(ContainerBuilder builder)
    {
        builder.AddSingleton(_player, typeof(Player), typeof(IPlayerPosition));
    }
}
