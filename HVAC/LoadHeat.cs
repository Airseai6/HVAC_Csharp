using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVAC
{
    class LoadHeatCal
    {
        public float get_total_heat_load(float temperature_env_winter, float temperature_set, float k_wall, float area_wall, float rate_temperature, float rate_direction,
            float rate_wind, float rate_twoWall, float rate_houseHight, float rate_interrupted, float cp_air, float denstiy_air, float volume_air)
        {
            LoadHeat part = new LoadHeat();
            return part.get_heat_load_baseAdd(temperature_env_winter, temperature_set, k_wall, area_wall, rate_temperature, rate_direction,
            rate_wind, rate_twoWall, rate_houseHight, rate_interrupted)
                + part.get_heat_load_coolAir(temperature_env_winter, temperature_set, cp_air, denstiy_air, volume_air);
        }
    }


    class LoadHeat
    {
        #region heat load
        public float get_heat_load_baseAdd(float temperature_env_winter, float temperature_set, float k_wall, float area_wall, float rate_temperature, float rate_direction,
            float rate_wind, float rate_twoWall, float rate_houseHight, float rate_interrupted)
        {
            float heat_load_base = rate_temperature * k_wall * area_wall * (temperature_set - temperature_env_winter);
            return heat_load_base * (1 + rate_direction + rate_wind + rate_twoWall) * (1 + rate_houseHight) * (1 + rate_interrupted);
        }

        public float get_heat_load_coolAir(float temperature_env_winter, float temperature_set, float cp_air, float denstiy_air, float volume_air)
        {
            return 0.28f * cp_air * denstiy_air * volume_air * (temperature_set - temperature_env_winter);
        }

        #endregion
    }
}
