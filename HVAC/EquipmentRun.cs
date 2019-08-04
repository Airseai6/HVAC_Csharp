using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVAC
{
    class EquipmentRun
    {
        public void run_equipment(ref float a, ref float b, ref float c, ref bool d)
        {
            HVAC_Equipment equipment_coolWater = new HVAC_Equipment();
            float[] coe_COP_cool_coolWater = { 0.0018f, -0.289f, 13.211f };
            float[] coe_COP_cool_groundPump = { 0.0038f, -0.4658f, 17.06f };
            float[] coe_COP_cool_airPump = { -0.0016f, 0.0083f, 4.0685f };
            float[] coe_COP_heat_groundPump = { 0.0022f, 0.0855f, 2.9625f };
            float[] coe_COP_heat_airPump = { -0.0003f, 0.0506f, 2.968f };
            equipment_coolWater.run_equipment(200, 200, ref a, 1, 33, ref b, 10000, ref c, coe_COP_cool_coolWater, ref d);
        }

        public void run_boiler(ref float cost_inital, ref float cost_run, ref bool flag_one, ref float flow_water, ref float flow_gas, ref float cost_pip,
            ref float cost_pump, ref float cost_heatExchanger)
        {
            HVAC_Equipment equipment_boiler = new HVAC_Equipment();
            equipment_boiler.run_boiler(800, 400, 2, ref cost_inital, 14, ref cost_run, ref flag_one, ref flow_water, ref flow_gas, 488, 15, ref cost_pip, ref cost_pump, ref cost_heatExchanger);
        }
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
            flag_one = num_equipment * load_cooling >= load_cooling_env ? true : false; //是否满足环境负荷
            //flag_one = num_equipment * load_heat > load_heat_env ? true : false;

            COP = coe_temAndCop[0] * temperature_equipSide_in * temperature_equipSide_in + coe_temAndCop[1] * temperature_equipSide_in + coe_temAndCop[2];
            power_cooling = COP > 0 ? load_cooling / COP : 0;
            cost_run = power_cooling * 0.89f * 16 * 180;   //计算年运行费用 一度电0.89，一天16h, 一年供冷180天。
            float cost_total = cost_inital + cost_run;
        }

        public void run_boiler(float load_heat_env, float load_heat, int num_equipment, ref float cost_inital, float cost_gasPrice,
            ref float cost_run, ref bool flag_one, ref float flow_water, ref float flow_gas, float time_numHours, float num_years, ref float cost_pip, ref float cost_pump, ref float cost_heatExchanger)
        {
            flag_one = num_equipment * load_heat >= load_heat_env ? true : false;

            #region creat a dict
            float[] array = new float[] { };
            Dictionary<float, Array> dict_boiler = new Dictionary<float, Array>();
            array = new float[] { 51.43f, 36.7f };
            dict_boiler.Add(300, array);
            array = new float[] { 68.57f, 49 };
            dict_boiler.Add(400, array);
            array = new float[] { 85.71f, 61.5f };
            dict_boiler.Add(500, array);
            array = new float[] { 102.86f, 73.5f };
            dict_boiler.Add(600, array);

            flow_water = (float)dict_boiler[load_heat].GetValue(0);
            flow_gas = (float)dict_boiler[load_heat].GetValue(1);
            #endregion

            cost_inital = num_equipment * 220 * load_heat;
            cost_pip = 0.1f * cost_inital;
            cost_pump = 6 * 4000;
            cost_heatExchanger = 16 * load_heat;
            cost_run = cost_inital + cost_gasPrice * flow_gas * time_numHours * num_years;
        }

        public void central_heat(float load_heat_env)
        {
            float cost_inital = 16 * load_heat_env;
            float flow_equSide = load_heat_env / 4.2f / 35 * 3.6f;
            float flow_userSide = load_heat_env / 4.2f / 5 * 3.6f;
        }
    }
}

