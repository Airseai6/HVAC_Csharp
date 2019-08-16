using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVAC
{
    class EquipmentRun2
    {
        public void equipment_run(float _建筑总冷负荷,
                                float _冷水机组_额定制冷量,
                                float _冷水机组_额定功率,
                                float _冷水机组_制冷量,
                                float _冷水机组_冷冻水进口水温,
                                float _冷水机组_冷冻水出口水温,
                                float _冷水机组_冷冻水进口压力,
                                float _冷水机组_冷冻水出口压力,
                                float _冷水机组_冷却水进口水温,
                                float _冷水机组_冷却水出口水温,
                                float _冷水机组_冷却水进口压力,
                                float _冷水机组_冷却水出口压力,
                                float _冷水机组_冷冻水流量,
                                float _冷水机组_冷却水流量,
                                float _冷水机组_机组功率,
                                float _冷水机组_COP,
                                float _冷却水泵_进口压力,
                                float _冷却水泵_扬程,
                                float _冷却水泵_出口压力,
                                float _冷却水泵_流量,
                                float _冷却水泵_运行功,
                                float _冷却水泵_运行频率,
                                float _冷却塔_进水温度,
                                float _冷却塔_出水温度,
                                float _冷却塔_进水压力,
                                float _冷却塔_出水压力,
                                float _冷却塔_功率,
                                float _冷冻水泵_进口压力,
                                float _冷冻水泵_扬程,
                                float _冷冻水泵_出口压力,
                                float _冷冻水泵_流量,
                                float _冷冻水泵_运行功,
                                float _冷冻水泵_运行频率,
                                float _系统_COP
                                    )
        {

        }
    }

    class Equipment
    {
        float load_total;
        float load_unit_rated;  //机组额定制冷量
        float num_nuit;
        float cop_rated;

        float delt_pressure_condensation;  //json  冷凝
        float delt_pressure_evaporation;  //json   蒸发
        float power_unit;
        float power_unit_rated;



        public void pump_cooling(float pressure_input, float lift, float pressure_output, float flow, float power, float freq, float num)
        {
            pressure_input = 200 - 80 / 2;
            lift = delt_pressure_condensation * (power_unit / power_unit_rated) * (power_unit / power_unit_rated) + 80 + 40;
            pressure_output = pressure_input + lift;
            flow = load_total * (1 + 1 / cop_rated) / (4.17f * 1000 * 5) * 3600 / num;
            power = 1000 * 9.81f * flow * lift;
            freq = 50;
         }

        public void pump_freezing(float pressure_input, float lift, float pressure_output, float flow, float power, float freq, float num)
        {
            pressure_input = 200 + 50;
            lift = delt_pressure_evaporation * (power_unit / power_unit_rated) * (power_unit / power_unit_rated) + 90 + 80;
            pressure_output = pressure_input + lift;
            flow = load_total / (4.17f * 1000 * 5) * 3600 / num;
            power = 1000 * 9.81f * flow * lift;
            freq = 50 * load_total / (load_unit_rated * num_nuit);

        }

        public void cooling_tower(float temperature_input, float temperature_output, float pressure_input, float pressure_output)
        {
            temperature_input = temperature_output - 5;

        }

        public void water_unit()
        {
        }

    }
}
