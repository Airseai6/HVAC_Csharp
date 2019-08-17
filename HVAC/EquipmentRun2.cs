﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVAC
{
    class EquipmentRun2
    {
        public float random()
        {
            var seed = Guid.NewGuid().GetHashCode();
            Random r = new Random(seed);
            int i = r.Next(-100, 100);
            return (float)i;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_建筑总冷负荷">调用</param>
        /// <param name="_负荷率">1， 0.6，0.8，0.9</param>
        /// <param name="_冷却水泵_进口压力"></param>
        /// <param name="_冷却水泵_扬程"></param>
        /// <param name="_冷却水泵_出口压力"></param>
        /// <param name="_冷却水泵_流量"></param>
        /// <param name="_冷却水泵_运行功率"></param>
        /// <param name="_冷却水泵_开启台数"></param>
        /// <param name="_冷却塔_进水温度"></param>
        /// <param name="_冷却塔_进水压力"></param>
        /// <param name="_冷却塔_出水压力"></param>
        /// <param name="_冷却塔_功率">读json或者前面的数</param>
        /// <param name="_冷却塔_开启台数"></param>
        /// <param name="_冷冻水泵_进口压力"></param>
        /// <param name="_冷冻水泵_扬程"></param>
        /// <param name="_冷冻水泵_出口压力"></param>
        /// <param name="_冷冻水泵_流量"></param>
        /// <param name="_冷冻水泵_运行功率"></param>
        /// <param name="_冷冻水泵_运行频率">可调节 给我传入是已启动水泵的平均频率</param>
        /// <param name="_冷冻水泵_运行台数"></param>
        /// <param name="_冷水机组_额定制冷量">读json或者前面的数</param>
        /// <param name="_冷水机组_额定功率">读json或者前面的数</param>
        /// <param name="_冷水机组_冷冻水出口压力"></param>
        /// <param name="_冷水机组_制冷量"></param>
        /// <param name="_冷水机组_冷冻水进口压力"></param>
        /// <param name="_冷水机组_冷却水进口水温">可调节 = 冷却塔出水温度</param>
        /// <param name="_冷水机组_冷却水出口水温"></param>
        /// <param name="_冷水机组_冷却水进口压力"></param>
        /// <param name="_冷水机组_冷却水出口压力"></param>
        /// <param name="_冷水机组_冷冻水流量"></param>
        /// <param name="_冷水机组_冷却水流量"></param>
        /// <param name="_冷水机组_机组功率"></param>
        /// <param name="_冷水机组_COP"></param>
        /// <param name="_冷水机组_冷凝器压降json">读json或者前面的数</param>
        /// <param name="_冷水机组_蒸发器压降json">读json或者前面的数</param>
        /// <param name="_冷水机组_开启台数"></param>
        /// <param name="_系统_COP"></param>
        /// <param name="ISCan_机组均启动">当他点击冷水机组启动后，如果这个值false，提示有设备未启动</param>
        /// <param name="ISCan_满足负荷运行">启动机组后，当这个值为true，提示 恭喜你，调试成功</param>
        public void equipment_run(float _建筑总冷负荷,
                                float _负荷率,

                                ref float _冷却水泵_进口压力,
                                ref float _冷却水泵_扬程,
                                ref float _冷却水泵_出口压力,
                                ref float _冷却水泵_流量,
                                ref float _冷却水泵_运行功率,
                                float _冷却水泵_开启台数,

                                ref float _冷却塔_进水温度,
                                ref float _冷却塔_进水压力,
                                ref float _冷却塔_出水压力,
                                float _冷却塔_功率,
                                float _冷却塔_开启台数,

                                ref float _冷冻水泵_进口压力,
                                ref float _冷冻水泵_扬程,
                                ref float _冷冻水泵_出口压力,
                                ref float _冷冻水泵_流量,
                                ref float _冷冻水泵_运行功率,
                                float _冷冻水泵_运行频率, //修改
                                float _冷冻水泵_运行台数,

                                float _冷水机组_额定制冷量,  //json
                                float _冷水机组_额定功率,  //json
                                ref float _冷水机组_冷冻水出口压力,
                                ref float _冷水机组_制冷量,
                                ref float _冷水机组_冷冻水进口压力,
                                float _冷水机组_冷却水进口水温,
                                ref float _冷水机组_冷却水出口水温,
                                ref float _冷水机组_冷却水进口压力,
                                ref float _冷水机组_冷却水出口压力,
                                ref float _冷水机组_冷冻水流量,
                                ref float _冷水机组_冷却水流量,
                                ref float _冷水机组_机组功率,
                                ref float _冷水机组_COP,
                                float _冷水机组_冷凝器压降json,
                                float _冷水机组_蒸发器压降json,
                                float _冷水机组_开启台数,

                                ref float _系统_COP,
                                ref bool ISCan_机组均启动,
                                ref bool ISCan_满足负荷运行
                                )
        {
            ISCan_机组均启动 = false;
            ISCan_满足负荷运行 = false;

            if (_冷却水泵_开启台数 >= 1 && _冷却塔_开启台数 >= 1 && _冷冻水泵_运行台数 >= 1 && _冷水机组_开启台数 >= 1)
            {
                ISCan_机组均启动 = true;
            }

            if (ISCan_机组均启动)
            {
                _冷却水泵_进口压力 = 200 - 80 / 2 + random() / 50;
                _冷却水泵_扬程 = 40 + 40 + 80;
                _冷却水泵_出口压力 = _冷却水泵_进口压力 + _冷却水泵_扬程;
                _冷却水泵_流量 = _建筑总冷负荷 * (1 + _冷水机组_额定功率 / _冷水机组_额定制冷量) / (4.17f * 1000 * 5) * 3600 / _冷却水泵_开启台数 + random() / 50;
                _冷却水泵_运行功率 = 1000 * 9.81f * _冷却水泵_流量 * _冷却水泵_扬程 / 3600 / 1000 / 10;

                _冷水机组_制冷量 = _冷水机组_额定制冷量 / 50 * _冷冻水泵_运行频率;

                _冷却塔_进水温度 = _冷水机组_冷却水进口水温 + 5;
                _冷却塔_进水压力 = _冷却水泵_进口压力 + _冷却水泵_扬程 - _冷水机组_冷凝器压降json * (_冷水机组_制冷量 / _冷水机组_额定制冷量) * (_冷水机组_制冷量 / _冷水机组_额定制冷量);
                _冷却塔_出水压力 = _冷却水泵_进口压力;

                _冷冻水泵_进口压力 = 200 + 50 + random() / 50;
                _冷冻水泵_扬程 = _冷水机组_蒸发器压降json * (_冷水机组_制冷量 / _冷水机组_额定制冷量) * (_冷水机组_制冷量 / _冷水机组_额定制冷量) + 90 + 80;
                _冷冻水泵_出口压力 = _冷冻水泵_进口压力 + _冷冻水泵_扬程;
                _冷冻水泵_流量 = _建筑总冷负荷 / (4.17f * 1000 * 5) * 3600 / _冷冻水泵_运行台数 * _冷冻水泵_运行频率 / (50 * _建筑总冷负荷 / _冷水机组_开启台数 / _冷水机组_额定制冷量);
                _冷冻水泵_运行功率 = 1000 * 9.81f * _冷冻水泵_流量 * _冷冻水泵_扬程 / 3600 / 1000 / 10;
                // _冷冻水泵_运行频率 = 50 * _冷水机组_制冷量 /_冷水机组_额定制冷量;

                _冷水机组_冷冻水进口压力 = _冷冻水泵_出口压力;
                _冷水机组_冷冻水出口压力 = _冷水机组_冷冻水进口压力 - _冷水机组_蒸发器压降json * (_冷水机组_制冷量 / _冷水机组_额定制冷量) * (_冷水机组_制冷量 / _冷水机组_额定制冷量);
                _冷水机组_冷却水出口水温 = _冷水机组_冷却水进口水温 + 5;
                _冷水机组_冷却水进口压力 = _冷却水泵_出口压力;
                _冷水机组_冷却水出口压力 = _冷却水泵_出口压力 - _冷水机组_冷凝器压降json * (_冷水机组_制冷量 / _冷水机组_额定制冷量) * (_冷水机组_制冷量 / _冷水机组_额定制冷量);
                _冷水机组_冷冻水流量 = _冷冻水泵_流量 * _冷冻水泵_运行台数 / _冷水机组_开启台数;
                _冷水机组_冷却水流量 = _冷却水泵_流量 * _冷却水泵_开启台数 / _冷水机组_开启台数;

                float _冷水机组_COP_修正 = 0.0034f * _冷水机组_冷却水进口水温 * _冷水机组_冷却水进口水温 - 0.4235f * _冷水机组_冷却水进口水温 + 15.5095f;
                _冷水机组_COP = _冷水机组_COP_修正 * (-1.0009f * _负荷率 * _负荷率 * _负荷率 + 0.3945f * _负荷率 * _负荷率 + 1.4007f * _负荷率 + 0.2105f);
                _冷水机组_机组功率 = _冷水机组_制冷量 / _冷水机组_COP;
                _系统_COP = _建筑总冷负荷 / (_冷水机组_机组功率 * _冷水机组_开启台数 + _冷却水泵_运行功率 * _冷却水泵_开启台数 + _冷却塔_功率 * _冷却塔_开启台数 + _冷冻水泵_运行功率 * _冷冻水泵_运行台数);

                if (_负荷率 >= 0.99f && _冷水机组_制冷量 * _冷水机组_开启台数 >= _建筑总冷负荷)
                {
                    ISCan_满足负荷运行 = true;
                }
                if (_负荷率 <= 0.99f && _冷水机组_制冷量 * _冷水机组_开启台数 <= _建筑总冷负荷)
                {
                    ISCan_满足负荷运行 = true;
                }
            }
        }
    }
}
