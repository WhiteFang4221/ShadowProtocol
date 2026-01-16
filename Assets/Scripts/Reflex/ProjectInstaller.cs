using Reflex.Core;
using UnityEngine;

public class ProjectInstaller : MonoBehaviour, IInstaller
{
    public void InstallBindings(ContainerBuilder builder)
    {
        builder.AddSingleton(new GameInput());
        InstallPlayerConfigs(builder);
    }

    private void InstallPlayerConfigs(ContainerBuilder builder)
    {
        PlayerConfig config = Resources.Load<PlayerConfig>("Configs/PlayerConfig");
        
        if (config != null)
        {
            builder.AddSingleton(config);
            Debug.Log("PlayerConfig loaded and registered via DI.");
        }

        HealthConfig healthConfig = Resources.Load<HealthConfig>("Configs/PlayerHealthConfig");

        if (healthConfig != null)
        {
            builder.AddSingleton(healthConfig);
            Debug.Log("HealthConfig loaded and registered via DI.");
        }
    }
}