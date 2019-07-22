using System;
/*
 * 建筑冷热源系统选型实验，体现知识点即可  2019-07-20
 * 1、建筑冷热负荷计算，需要有逐时负荷体现尖峰负荷概念
 * 2、选型及运行系统部分的代码 TODO
 */
namespace HAVC
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Program program = new Program();
            OutdoorPara outdoorPara = new OutdoorPara();

            float[] total_cooling_load = new float[outdoorPara.temperature_env.Length];
            for (int i = 0; i < outdoorPara.temperature_env.Length; i++)
            {
                total_cooling_load[i] = program.get_total_cooling_load(outdoorPara.temperature_env[i], 23, 200, 150, 200, 0.3f, 0.2f, 
                    0.5f, 150, 30, 0.98f, 10, 50, outdoorPara.load_solarRadiation[i], 0.2f);
                Console.WriteLine("{0}时的冷负荷为：{1}", i, total_cooling_load[i]);
            }
            //Console.WriteLine("区域逐时冷负荷为：{0}", total_cooling_load);
        }
        public float get_total_cooling_load(float temperature_env, float temperature_set, float area_location, float area_wall, float area_roof, 
            float rate_wal_div_win, float k_wall, float k_window, float load_aPerson, int num_person, float rate_cluster, 
            float load_powerDensity_lighting, float load_powerDensity_equipment, float load_solarRadiation, float rate_solarRadiation)
        {
            Building part = new Building();
            float total_cooling_load = part.get_cooling_load_wall(k_wall, area_wall, area_roof, temperature_env, temperature_set)
                + part.get_cooling_load_window(area_wall, rate_wal_div_win, k_window, temperature_env, temperature_set)
                + part.get_cooling_load_person(num_person, load_aPerson, rate_cluster)
                + part.get_cooling_load_lighting(load_powerDensity_lighting, area_location)
                + part.get_cooling_load_equipment(load_powerDensity_equipment, area_location)
                + part.get_cooling_load_solarRadiation(area_wall, rate_wal_div_win, load_solarRadiation, rate_solarRadiation);

            return total_cooling_load;
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
        // 按照划分的区域计算
        //public float temperature_env;   //call
        //public float temperature_set;   //input
        //public float area_location;   //input 本次计算区域的地面面积

        //public float area_wall; //input
        //public float area_roof; //input
        //public float area_window;   //output
        //public float rate_wal_div_win; //input
        //public float k_wall;    //input
        //public float k_window;  //input

        //public float load_aPerson;  //input
        //public int num_person;  //input
        //public float rate_cluster;  //input 群集系数

        //public float load_powerDensity_lighting;  //input
        //public float load_powerDensity_equipment;  //input

        //public float load_solarRadiation;   //call
        //public float rate_solarRadiation;  //input 光照的某系数

        public float get_cooling_load_wall(float k_wall, float area_wall, float area_roof, float temperature_env, float temperature_set)
        {
            return k_wall * (area_wall+area_roof) * (temperature_env - temperature_set);
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
    }
}
