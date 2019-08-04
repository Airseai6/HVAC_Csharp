﻿using System;

/*
 * 建筑冷热源系统选型实验，体现知识点即可  2019-07-20
 * 1、建筑冷热负荷计算，需要有逐时负荷体现尖峰负荷概念
 * 2、选型及运行系统部分的代码 2019-07-27  分成两个文件2019-7-29
 * 2019-8-4代码重构
 */
namespace HVAC
{
    class Program
    {
        static void Main(string[] args)
        {
            #region cooling load
            float[] temperature_env = { 28.8f, 28.6f, 28.3f, 28.1f, 27.9f, 28.4f, 29.3f, 30.4f, 31.4f, 32.3f, 33.2f, 34.0f, 34.5f,
                   34.8f, 34.7f, 34.2f, 33.9f, 33.1f, 32.2f, 31.2f, 30.5f, 30.0f, 29.6f, 29.4f };
            float[] load_solarRadiation = { 0, 0, 0, 0, 0, 0, 0, 10, 20, 30, 40, 60, 100,
                   120, 100, 60, 40, 30, 10, 5, 0, 0, 0, 0 };

            LoadCoolingCal ccal = new LoadCoolingCal();
            float[] total_cooling_load = new float[temperature_env.Length];
            float load_partOne = 0;
            float area_win = 0;
            float load_partTwo = 0;
            float area_winEff = 0;
            float load_partTwoOne = 0;
            float enthalpy_env = 0;
            float enthalpy_set = 0;
            float load_partThree = 0;
            float load_partFour = 0;
            float load_partFive = 0;
            float load_partSix = 0;
            float load_total = 0;
            for (int i = 0; i < temperature_env.Length; i++)
            {
                ccal.get_total_cooling_load(temperature_env[i], 23, 0.4f, 0.4f, 200, 150, ref load_partOne,
                    0.5f, ref area_win, 0.3f, ref load_partTwo,
                    0.8f, ref area_winEff, load_solarRadiation[i], 0.8f, ref load_partTwoOne,
                    30, 30, ref enthalpy_env, ref enthalpy_set, ref load_partThree,
                    0.98f, 70, ref load_partFour,
                    11, 400, ref load_partFive,
                    120, ref load_partSix, ref load_total);
                total_cooling_load[i] = load_total;
                Console.WriteLine("{0}时的冷负荷为：{1}", i, total_cooling_load[i]);
            }
            #endregion


            #region heat load
            LoadHeatCal hcal = new LoadHeatCal();
            float total_heat_load = 1.2f * hcal.get_total_heat_load(-27, 18, 1.2f, 200, 1, 0, 0, 0.05f, 0, 0.2f, 1.0056f, 1.29f, 20);
            Console.WriteLine("冬季区域热负荷：{0}", total_heat_load);
            #endregion


            #region equipment run
            EquipmentRun eRun = new EquipmentRun();
            float a = 1;
            float b = 1;
            float c = 1;
            bool d = true;
            eRun.run_equipment(ref a, ref b, ref c, ref d);
            Console.WriteLine(a + " " + b + " " + c + " " + d);


            bool flag_one = false;
            float flow_water = 0;
            float flow_gas = 0;
            float cost_inital = 0;
            float cost_run = 0;
            float cost_pip = 0;
            float cost_pump = 0;
            float cost_heatExchanger = 0;
            eRun.run_boiler(ref cost_inital, ref cost_run, ref flag_one, ref flow_water, ref flow_gas, ref cost_pip, ref cost_pump, ref cost_heatExchanger);
            Console.WriteLine(flag_one + " " + flow_water + " " + flow_gas);
            #endregion
        }
    }
}
