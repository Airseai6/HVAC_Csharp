using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVAC
{
    class LoadCoolingCal
    {
        /// <summary>
        /// 冷负荷计算，用于与主函数传参
        /// </summary>
        /// <param name="temperature_env">室外逐时温度 读文件24个不同</param>
        /// <param name="temperature_set">室内设计温度</param>
        /// <param name="k_wall">外墙传热系数</param>
        /// <param name="k_roof">屋顶传热系数Kr</param>
        /// <param name="area_wall">区域外墙面积Fw  读文件</param>
        /// <param name="area_roof">区域屋顶面积Fr  读文件</param>
        /// <param name="load_partOne">外墙屋顶冷负荷（第一部分）</param>
        /// <param name="rate_wal_div_win">区域窗墙比 读文件</param>
        /// <param name="area_win">外窗窗口面积</param>
        /// <param name="k_window">Kch外窗传热系数</param>
        /// <param name="load_partTwo">外窗冷负荷（第二部分）</param>
        /// <param name="rate_solarRadiation">外窗的有效面积系数</param>
        /// <param name="area_winEff">窗上受太阳直接照射的面积</param>
        /// <param name="load_solarRadiation">透过标准窗玻璃的太阳总辐射照度 读文件24个不同</param>
        /// <param name="rate_solarDire">方向修正系数a</param>
        /// <param name="load_partTwoOne">太阳辐射（第二点一部分）</param>
        /// <param name="num_person">区域中的人数</param>
        /// <param name="m_newAir">新风量M(kg/h)</param>
        /// <param name="enthalpy_env">室外空气焓值hi kj/kg</param>
        /// <param name="enthalpy_set">室内空气焓值h2 kj/kg</param>
        /// <param name="load_partThree">新风负荷（第三部分）</param>
        /// <param name="rate_cluster">Cr群集系数</param>
        /// <param name="load_aPerson">每个人的热量(W)</param>
        /// <param name="load_partFour">人体负荷（第四部分）</param>
        /// <param name="load_powerDensity_lighting">功率密度（照明）</param>
        /// <param name="area_location">区域面积</param>
        /// <param name="load_partFive">照明冷负荷（第五部分）</param>
        /// <param name="load_powerDensity_equipment">功率密度（设备）</param>
        /// <param name="load_partSix">设备冷负荷（第六部分）</param>
        /// <param name="load_total">区域总冷负荷</param>
        public void get_total_cooling_load(float temperature_env, float temperature_set, float k_wall, float k_roof, float area_wall, float area_roof, ref float load_partOne,
            float rate_wal_div_win, ref float area_win, float k_window, ref float load_partTwo,
            float rate_solarRadiation, ref float area_winEff, float load_solarRadiation, float rate_solarDire, ref float load_partTwoOne,
            int num_person, float m_newAir, ref float enthalpy_env, ref float enthalpy_set, ref float load_partThree,
            float rate_cluster, float load_aPerson, ref float load_partFour,
            float load_powerDensity_lighting, float area_location, ref float load_partFive,
            float load_powerDensity_equipment, ref float load_partSix, ref float load_total)
        {
            LoadCooling part = new LoadCooling();
            area_win = area_wall * rate_wal_div_win;
            area_winEff = 0.8f * area_win;
            load_partOne = part.get_cooling_load_wall(k_wall, k_roof, area_wall, area_roof, temperature_env, temperature_set);
            load_partTwo = part.get_cooling_load_window(area_wall, rate_wal_div_win, k_window, temperature_env, temperature_set);
            load_partTwoOne = part.get_cooling_load_solarRadiation(area_wall, rate_wal_div_win, load_solarRadiation, rate_solarRadiation, rate_solarDire);
            load_partThree = part.get_cooling_load_newAir(num_person, temperature_env, temperature_set, m_newAir, ref enthalpy_env, ref enthalpy_set);
            load_partFour = part.get_cooling_load_person(num_person, load_aPerson, rate_cluster);
            load_partFive = part.get_cooling_load_lighting(load_powerDensity_lighting, area_location);
            load_partSix = part.get_cooling_load_equipment(load_powerDensity_equipment, area_location);
            load_total = 1.2f * (load_partOne + load_partTwo + load_partTwoOne + load_partThree + load_partFour + load_partFive + load_partSix);
        }
    }


    class LoadCooling
    {
        #region cooling load

        public float get_cooling_load_wall(float k_wall, float k_roof, float area_wall, float area_roof, float temperature_env, float temperature_set)
        {
            return (k_wall * area_wall + k_roof * area_roof) * (temperature_env - temperature_set);
        }

        public float get_cooling_load_window(float area_wall, float rate_wal_div_win, float k_window, float temperature_env, float temperature_set)
        {
            float area_window = area_wall * rate_wal_div_win;
            return k_window * area_window * (temperature_env - temperature_set);
        }

        public float get_cooling_load_solarRadiation(float area_wall, float rate_wal_div_win, float load_solarRadiation, float rate_solarRadiation, float rate_solarDire)
        {
            float area_window = area_wall * rate_wal_div_win;
            return load_solarRadiation * area_window * rate_solarRadiation * rate_solarDire;
        }

        public float get_cooling_load_newAir(int num_person, float temperature_env, float temperature_set, float m_newAir, ref float enthalpy_env, ref float enthalpy_set, float rate_hk = 1.1f, float rate_hb = 5.1f)
        {
            enthalpy_env = rate_hk * temperature_env + rate_hb;
            enthalpy_set = rate_hk * temperature_set + rate_hb;
            return  num_person * m_newAir * (enthalpy_env - enthalpy_set) / 3.6f;
        }

        public float get_cooling_load_person(int num_person, float load_aPerson, float rate_cluster)
        {
            return num_person * load_aPerson * rate_cluster;
        }

        public float get_cooling_load_lighting(float load_powerDensity_lighting, float area_location)
        {
            return load_powerDensity_lighting * area_location;
        }

        public float get_cooling_load_equipment(float load_powerDensity_equipment, float area_location)
        {
            return load_powerDensity_equipment * area_location;
        }

        #endregion
    }
}
