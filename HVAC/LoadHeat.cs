using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVAC
{
    class LoadHeatCal
    {
        /// <summary>
        /// 计算热负荷，用于传参
        /// </summary>
        /// <param name="rate_temperature">温差修正系数,W</param>
        /// <param name="k_wall">传热系数,W/(㎡·℃)</param>
        /// <param name="area_wall">计算传热面积,㎡</param>
        /// <param name="temperature_set">冬季室内设计温度,℃</param>
        /// <param name="temperature_env_winter">室外计算温度,℃</param>
        /// <param name="heat_load_base">基本耗热量,W</param>
        /// <param name="rate_direction">朝向修正</param>
        /// <param name="rate_wind">风力修正</param>
        /// <param name="rate_twoWall">两面外墙修正</param>
        /// <param name="rate_houseHight">房高附加</param>
        /// <param name="rate_interrupted">间歇附加率</param>
        /// <param name="heat_load_baseAdd">考虑各项附加后，某围护的耗热量,W</param>
        /// <param name="cp_air">干空气的定压质量比热容=1.0056kJ/(kg·℃)</param>
        /// <param name="denstiy_air">室外计算温度下的空气密度，Kg/m³</param>
        /// <param name="volume_air">渗透冷空气量，m3/h</param>
        /// <param name="load_coolAir">通过门窗冷风渗透耗热量，W</param>
        /// <param name="rate_increment">增量系数</param>
        /// <param name="total_heat_load">总的热负荷</param>
        public void get_total_heat_load(float rate_temperature, float k_wall, float area_wall, float temperature_set, float temperature_env_winter, ref float heat_load_base,
            float rate_direction, float rate_wind, float rate_twoWall, float rate_houseHight, float rate_interrupted, ref float heat_load_baseAdd,
            float cp_air, float denstiy_air, float volume_air, ref float load_coolAir, float rate_increment, ref float total_heat_load)
        {
            LoadHeat part = new LoadHeat();
            heat_load_baseAdd = part.get_heat_load_baseAdd(temperature_env_winter, temperature_set, k_wall, area_wall, rate_temperature, rate_direction,
            rate_wind, rate_twoWall, rate_houseHight, rate_interrupted, ref heat_load_base);
            load_coolAir = part.get_heat_load_coolAir(temperature_env_winter, temperature_set, cp_air, denstiy_air, volume_air);
            total_heat_load = rate_increment * (heat_load_baseAdd + load_coolAir);
        }
    }


    class LoadHeat
    {
        #region heat load
        public float get_heat_load_baseAdd(float temperature_env_winter, float temperature_set, float k_wall, float area_wall, float rate_temperature, float rate_direction,
            float rate_wind, float rate_twoWall, float rate_houseHight, float rate_interrupted, ref float heat_load_base)
        {
            heat_load_base = rate_temperature * k_wall * area_wall * (temperature_set - temperature_env_winter);
            return heat_load_base * (1 + rate_direction + rate_wind + rate_twoWall) * (1 + rate_houseHight) * (1 + rate_interrupted);
        }

        public float get_heat_load_coolAir(float temperature_env_winter, float temperature_set, float cp_air, float denstiy_air, float volume_air)
        {
            return 0.28f * cp_air * denstiy_air * volume_air * (temperature_set - temperature_env_winter);
        }

        #endregion
    }
}
