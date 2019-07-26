using System;
/*
 * 建筑冷热源系统选型实验，体现知识点即可  2019-07-20
 * 1、建筑冷热负荷计算，需要有逐时负荷体现尖峰负荷概念
 * 2、选型及运行系统部分的代码 TODO
 */
namespace HVAC
{
    class Program
    {
        static void Main(string[] args)
        {
            #region cooling and heat load
            OutdoorPara outdoorPara = new OutdoorPara();
            float[] total_cooling_load = new float[outdoorPara.temperature_env.Length];
            for (int i = 0; i < outdoorPara.temperature_env.Length; i++)
            {
                total_cooling_load[i] = 1.2f * get_total_cooling_load(outdoorPara.temperature_env[i], 23, 200, 150, 200, 0.3f, 0.2f, 0.8f,
                    0.5f, 150, 30, 0.98f, 10, 50, outdoorPara.load_solarRadiation[i], 0.2f, 3);
                Console.WriteLine("{0}时的冷负荷为：{1}", i, total_cooling_load[i]);
            }

            float total_heat_load = 1.2f * get_total_heat_load(-27, 18, 1.2f, 200, 1, 0, 0, 0.05f, 0, 0.2f, 1.0056f, 1.29f, 20);
            Console.WriteLine("冬季区域热负荷：{0}", total_heat_load);
            #endregion

            #region equipment run
            float a = 1;
            float b = 1;
            float c = 1;
            bool d = true;
            run_equipment(ref a, ref b, ref c, ref d);
            Console.WriteLine(a + " " + b + " " + c + " " + d);
            #endregion
        }
        public static float get_total_cooling_load(float temperature_env, float temperature_set, float area_location, float area_wall, float area_roof,
            float rate_wal_div_win, float k_wall, float k_roof, float k_window, float load_aPerson, int num_person, float rate_cluster,
            float load_powerDensity_lighting, float load_powerDensity_equipment, float load_solarRadiation, float rate_solarRadiation, float m_newAir)
        {
            Building part = new Building();
            float total_cooling_load = part.get_cooling_load_wall(k_wall, k_roof, area_wall, area_roof, temperature_env, temperature_set)
                + part.get_cooling_load_window(area_wall, rate_wal_div_win, k_window, temperature_env, temperature_set)
                + part.get_cooling_load_person(num_person, load_aPerson, rate_cluster)
                + part.get_cooling_load_lighting(load_powerDensity_lighting, area_location)
                + part.get_cooling_load_equipment(load_powerDensity_equipment, area_location)
                + part.get_cooling_load_solarRadiation(area_wall, rate_wal_div_win, load_solarRadiation, rate_solarRadiation)
                + part.get_cooling_load_newAir(num_person, temperature_env, temperature_set, m_newAir);

            return total_cooling_load;
        }
        public static float get_total_heat_load(float temperature_env_winter, float temperature_set, float k_wall, float area_wall, float rate_temperature, float rate_direction,
            float rate_wind, float rate_twoWall, float rate_houseHight, float rate_interrupted, float cp_air, float denstiy_air, float volume_air)
        {
            Building part = new Building();
            return part.get_heat_load_baseAdd(temperature_env_winter, temperature_set, k_wall, area_wall, rate_temperature, rate_direction,
            rate_wind, rate_twoWall, rate_houseHight, rate_interrupted)
                + part.get_heat_load_coolAir(temperature_env_winter, temperature_set, cp_air, denstiy_air, volume_air);
        }
        public static void run_equipment(ref float a, ref float b, ref float c, ref bool d)
        {
            HVAC_Equipment equipment_coolWater = new HVAC_Equipment();
            float[] coe = { 0.0018f, -0.289f, 13.211f };
            equipment_coolWater.run_equipment(200, 200, ref a, 1, 33, ref b, 10000, ref c, coe, ref d);
        }
    }

    class OutdoorPara
    {
        // 考虑这两个变量是逐时变化的，数组是由文件读取
        //public float[] temperature_env = new float[24];   //input
        //public float[] load_solarRadiation = new float[24];   //input

        public float[] temperature_env = { 28.8f, 28.6f, 28.3f, 28.1f, 27.9f, 28.4f, 29.3f, 30.4f, 31.4f, 32.3f, 33.2f, 34.0f, 34.5f,
               34.8f, 34.7f, 34.2f, 33.9f, 33.1f, 32.2f, 31.2f, 30.5f, 30.0f, 29.6f, 29.4f };
        public float[] load_solarRadiation = { 0, 0, 0, 0, 0, 0, 0, 10, 20, 30, 40, 60, 100,
               120, 100, 60, 40, 30, 10, 5, 0, 0, 0, 0 };
    }
    class Building
    {
        #region cooling load
        // 按照划分的区域计算
        //public float temperature_env;   //call
        //public float temperature_set;   //input
        //public float area_location;   //input 本次计算区域的地面面积

        //public float area_wall; //input
        //public float area_roof; //input
        //public float area_window;   //output
        //public float rate_wal_div_win; //input
        //public float k_wall;    //input
        //public float k_roof;    //input
        //public float k_window;  //input

        //public float load_aPerson;  //input
        //public int num_person;  //input
        //public float rate_cluster;  //input 群集系数

        //public float load_powerDensity_lighting;  //input
        //public float load_powerDensity_equipment;  //input

        //public float load_solarRadiation;   //call
        //public float rate_solarRadiation;  //input 光照的某系数

        public float get_cooling_load_wall(float k_wall, float k_roof, float area_wall, float area_roof, float temperature_env, float temperature_set)
        {
            return (k_wall * area_wall + k_roof * area_roof) * (temperature_env - temperature_set);
        }
        public float get_cooling_load_window(float area_wall, float rate_wal_div_win, float k_window, float temperature_env, float temperature_set)
        {
            float area_window = area_wall * rate_wal_div_win;
            return k_window * area_window * (temperature_env - temperature_set);
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
        public float get_cooling_load_solarRadiation(float area_wall, float rate_wal_div_win, float load_solarRadiation, float rate_solarRadiation)
        {
            float area_window = area_wall * rate_wal_div_win;
            return load_solarRadiation * area_window * rate_solarRadiation;
        }
        public float get_cooling_load_newAir(int num_person, float temperature_env, float temperature_set, float m_newAir, float rate_hk = 1.1f, float rate_hb = 5.1f)
        {
            float enthalpy_env = rate_hk * temperature_env + rate_hb;
            float enthalpy_set = rate_hk * temperature_set + rate_hb;
            return 3.6f * num_person * m_newAir * (enthalpy_env - enthalpy_set);
        }
        #endregion

        #region heat load
        //public float temperature_env_winter;  //冬季室外计算温度（读json）
        //public float temperature_set;   //input
        //public float k_wall;    //input
        //public float area_wall; //input
        //public float rate_temperature;    //温度修正

        //public float rate_direction;    //朝向修正
        //public float rate_wind; //风力修正
        //public float rate_twoWall;  //两面外墙修正
        //public float rate_houseHight;   //房高附加
        //public float rate_interrupted;  //间歇附加率

        //public float cp_air;    //干空气比热容
        //public float denstiy_air;   //空气密度
        //public float volume_air;    //体积

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

    class HVAC_Equipment
    {
        //public float flow_pump_primarySide; //一次侧循环泵流量
        //public float head_pump_primarySide; //一次侧循环泵扬程
        //public float power_pump_primarySide;    //一次侧循环泵功率
        //public float flow_pump_secondarySide;   //二次侧循环泵流量
        //public float head_pump_secondarySide;   //二次侧循环泵扬程
        //public float power_pump_secondarySide;  //二次侧循环泵功率

        //public float temperature_userSide_out = 12; //用户侧回水12度
        //public float temperature_userSide_in = 7;   //用户侧进水7度

        //public float temperature_equipSide_in;   //机组侧进水温度（冷却塔回水）
        //public float load_cooling_env;  //环境冷负荷
        //public float load_cooling;  //机组制冷量
        //public float load_heat;  //机组制热量
        //public float power_cooling;  //机组制冷功率
        //public float power_heat;  //机组制热功率

        //public float COP;  //机组COP
        //public int num_equipment;  //机组COP
        //public float cost_inital;  //机组初投资
        //public float cost_run;  //机组运行费用
        //public float[] coe_temAndCop; //公式系数

        public void run_equipment(float load_cooling_env, float load_cooling, ref float power_cooling, int num_equipment,
            float temperature_equipSide_in, ref float COP, float cost_inital, ref float cost_run, float[] coe_temAndCop, ref bool flag_one)
        {
            flag_one = num_equipment * load_cooling > load_cooling_env ? true : false; //是否满足环境负荷

            COP = coe_temAndCop[0] * temperature_equipSide_in * temperature_equipSide_in + coe_temAndCop[1] * temperature_equipSide_in + coe_temAndCop[2];
            power_cooling = COP > 0 ? load_cooling / COP : 0;
            cost_run = power_cooling * 0.89f * 16 * 180;   //计算年运行费用 一度电0.89，一天16h, 一年供冷180天。
            float cost_total = cost_inital + cost_run;
        }
    }
}
