﻿
namespace HVAC
{
    public class EquipmentRun3_地源热泵
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_建筑总冷负荷">调用</param>
        /// <param name="_负荷率">1为刚进去额定负荷， 0.6，0.8，0.9为变负荷三选一</param>
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
        /// <param name="_冷冻水泵_流量_上次计算"></param>
        /// <param name="_冷冻水泵_运行功率"></param>
        /// <param name="_冷冻水泵_运行频率">可调节 给我传入是已启动水泵的平均频率</param>
        /// <param name="_空调机组电动调节阀_阀门开度">可调节 给我传入是已启动水泵的平均频率</param>
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
        /// <param name="ISCan_频率or阀门">调阀门还是调水泵频率  调频率为true</param>
        public static void equipment_run(float _建筑总冷负荷,
                                float _负荷率,

                                ref float _冷却水泵_进口压力,
                                float _冷却水泵_扬程,
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
                                float _冷冻水泵_扬程,
                                ref float _冷冻水泵_出口压力,
                                ref float _冷冻水泵_流量,
                                float _冷冻水泵_流量_上次计算,
                                ref float _冷冻水泵_运行功率,
                                float _冷冻水泵_运行频率,
                                float _空调机组电动调节阀_阀门开度,
                                float _初调节_阀门开度, //0-1
                                float _冷冻水泵_运行台数,

                                float _冷水机组_额定制冷量,
                                float _冷水机组_额定功率,
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
                                ref bool ISCan_满足负荷运行,
                                int ISCan_频率or阀门  //0额定负荷运行 1调阀门 2调频率
                                )
        {
            ISCan_机组均启动 = false;
            ISCan_满足负荷运行 = false;

            if (_冷却水泵_开启台数 >= 1 && _冷冻水泵_运行台数 >= 1 && _冷水机组_开启台数 >= 1)
            {
                ISCan_机组均启动 = true;
            }
            else
            {
                _冷水机组_机组功率 = 0;
                _冷水机组_COP = 0;
                _系统_COP = 0;
            }

            if (ISCan_机组均启动 && _建筑总冷负荷 > 0)
            {
                float _建筑总冷负荷_变工况 = _建筑总冷负荷 * _负荷率;
                float _冷却水流量_最大值 = (_建筑总冷负荷 / (4.17f * 1000 * 5) * 3600) * 1.3f;
                float _冷冻水流量_最大值 = (_建筑总冷负荷 / (4.17f * 1000 * 5) * 3600);

                #region  冷却水泵
                if (_冷却水泵_开启台数 >= 1)
                {
                    _冷却水泵_流量 = _建筑总冷负荷 * (1 + _冷水机组_额定功率 / _冷水机组_额定制冷量) / (4.17f * 1000 * 5) * 3600 / _冷却水泵_开启台数;
                }
                else
                {
                    _冷却水泵_流量 = 0;
                }

                // _冷却水泵_进口压力 = 200 + 10 - 37 * (_冷却水泵_流量 / _冷却水流量_最大值) * (_冷却水泵_流量 / _冷却水流量_最大值);
                _冷却水泵_进口压力 = 30;  //水柱= 30kPa
                // _冷却水泵_扬程 = 40 + 40 + 80;  读取方案对比时设计流量下的水泵扬程
                _冷却水泵_出口压力 = _冷却水泵_进口压力 + _冷却水泵_扬程;
                _冷却水泵_运行功率 = 1000 * 9.81f * _冷却水泵_流量 * _冷却水泵_扬程 / 3600 / 1000 / 10;
                #endregion


                switch (ISCan_频率or阀门)
                {
                    case 0:
                        {
                            if (_冷冻水泵_运行台数 >= 1)
                            {
                                //_冷冻水泵_流量 = 0.9f * _冷冻水泵_流量_上次计算 + 0.1f * (_建筑总冷负荷 / (4.17f * 1000 * 5) * 3600 / _冷冻水泵_运行台数 * _冷冻水泵_运行频率 / (50 * _建筑总冷负荷 / _冷水机组_开启台数 / _冷水机组_额定制冷量));
                                //_冷冻水泵_流量 = 0.0f * _冷冻水泵_流量_上次计算 + 1f * (_建筑总冷负荷 / (4.17f * 1000 * 5) * 3600 / _冷冻水泵_运行台数 * _冷冻水泵_运行频率 / (50 * _建筑总冷负荷 / _冷水机组_开启台数 / _冷水机组_额定制冷量));
                                _冷冻水泵_流量 = _初调节_阀门开度 * (_建筑总冷负荷 / (4.17f * 1000 * 5) * 3600 / _冷冻水泵_运行台数 * _冷冻水泵_运行频率 / 50);
                            }
                            else
                            {
                                _冷冻水泵_流量 = 0;
                            }

                            _冷水机组_制冷量 = (_建筑总冷负荷 / _冷水机组_开启台数) / 50 * _冷冻水泵_运行频率;
                        }
                        break;
                    case 1:
                        {
                            if (_冷冻水泵_运行台数 >= 1)
                            {
                                //_冷冻水泵_流量 = 0.9f * _冷冻水泵_流量_上次计算 + 0.1f * ((1 + 29 * _空调机组电动调节阀_阀门开度) / 30 * _冷冻水流量_最大值) / _冷冻水泵_运行台数;
                                _冷冻水泵_流量 = 0f * _冷冻水泵_流量_上次计算 + 1f * ((1 + 29 * _空调机组电动调节阀_阀门开度) / 30 * _冷冻水流量_最大值) / _冷冻水泵_运行台数;
                            }
                            else
                            {
                                _冷冻水泵_流量 = 0;
                            }

                            _冷水机组_制冷量 = (_建筑总冷负荷 / _冷水机组_开启台数) * _冷冻水泵_流量 * _冷冻水泵_运行台数 / _冷冻水流量_最大值;
                        }
                        break;

                    case 2:
                    default:
                        {
                            if (_冷冻水泵_运行台数 >= 1)
                            {
                                //_冷冻水泵_流量 = 0.9f * _冷冻水泵_流量_上次计算 + 0.1f * (_建筑总冷负荷 / (4.17f * 1000 * 5) * 3600 / _冷冻水泵_运行台数 * _冷冻水泵_运行频率 / (50 * _建筑总冷负荷 / _冷水机组_开启台数 / _冷水机组_额定制冷量));
                                //_冷冻水泵_流量 = 0.0f * _冷冻水泵_流量_上次计算 + 1f * (_建筑总冷负荷 / (4.17f * 1000 * 5) * 3600 / _冷冻水泵_运行台数 * _冷冻水泵_运行频率 / (50 * _建筑总冷负荷 / _冷水机组_开启台数 / _冷水机组_额定制冷量));
                                _冷冻水泵_流量 = (_建筑总冷负荷 / (4.17f * 1000 * 5) * 3600 / _冷冻水泵_运行台数 * _冷冻水泵_运行频率 / 50);
                            }
                            else
                            {
                                _冷冻水泵_流量 = 0;
                            }

                            _冷水机组_制冷量 = (_建筑总冷负荷 / _冷水机组_开启台数) / 50 * _冷冻水泵_运行频率;
                        }
                        break;
                }


                if (_冷水机组_制冷量 > _冷水机组_额定制冷量)
                {
                    _冷水机组_制冷量 = _冷水机组_额定制冷量;
                }
                _冷却塔_进水温度 = _冷水机组_冷却水进口水温 + 5;
                // _冷却塔_进水压力 = _冷却水泵_进口压力 + _冷却水泵_扬程 - _冷水机组_冷凝器压降json * (_冷水机组_制冷量 / _冷水机组_额定制冷量) * (_冷水机组_制冷量 / _冷水机组_额定制冷量);


                _冷冻水泵_进口压力 = 200 + 50;
                // _冷冻水泵_扬程 = _冷水机组_蒸发器压降json * (_冷水机组_制冷量 / _冷水机组_额定制冷量) * (_冷水机组_制冷量 / _冷水机组_额定制冷量) + 90 + 80;
                _冷冻水泵_出口压力 = _冷冻水泵_进口压力 + _冷冻水泵_扬程 * (_冷冻水泵_流量 * _冷冻水泵_运行台数 / _冷冻水流量_最大值) * (_冷冻水泵_流量 * _冷冻水泵_运行台数 / _冷冻水流量_最大值);

                _冷冻水泵_运行功率 = 1000 * 9.81f * _冷冻水泵_流量 * _冷冻水泵_扬程 / 3600 / 1000 / 10;
                // _冷冻水泵_运行频率 = 50 * _冷水机组_制冷量 /_冷水机组_额定制冷量;



                _冷水机组_冷冻水进口压力 = _冷冻水泵_出口压力 - 5 * (_冷冻水泵_流量 * _冷冻水泵_运行台数 / _冷冻水流量_最大值) * (_冷冻水泵_流量 * _冷冻水泵_运行台数 / _冷冻水流量_最大值);
                _冷水机组_冷冻水出口压力 = _冷水机组_冷冻水进口压力 - _冷水机组_蒸发器压降json * (_冷水机组_制冷量 / _冷水机组_额定制冷量) * (_冷水机组_制冷量 / _冷水机组_额定制冷量);
                _冷水机组_冷却水出口水温 = _冷水机组_冷却水进口水温 + 5;
                _冷水机组_冷却水进口压力 = _冷却水泵_出口压力 - 5 * (_冷却水泵_流量*_冷却水泵_开启台数 / _冷却水流量_最大值) * (_冷却水泵_流量 * _冷却水泵_开启台数 / _冷却水流量_最大值);
                _冷水机组_冷却水出口压力 = _冷水机组_冷却水进口压力 - _冷水机组_冷凝器压降json * (_冷水机组_制冷量 / _冷水机组_额定制冷量) * (_冷水机组_制冷量 / _冷水机组_额定制冷量);
                _冷水机组_冷冻水流量 = _冷冻水泵_流量 * _冷冻水泵_运行台数 / _冷水机组_开启台数;
                _冷水机组_冷却水流量 = _冷却水泵_流量 * _冷却水泵_开启台数 / _冷水机组_开启台数;

                _冷却塔_进水压力 = _冷水机组_冷却水出口压力 - 60 * (_冷却水泵_流量 * _冷却水泵_开启台数 / _冷却水流量_最大值) * (_冷却水泵_流量 * _冷却水泵_开启台数 / _冷却水流量_最大值);
                _冷却塔_出水压力 = _冷却塔_进水压力 - 60 * (_冷却水泵_流量 * _冷却水泵_开启台数 / _冷却水流量_最大值) * (_冷却水泵_流量 * _冷却水泵_开启台数 / _冷却水流量_最大值);


                float _冷水机组_COP_修正 = 0.0038f * _冷水机组_冷却水进口水温 * _冷水机组_冷却水进口水温 - 0.4658f * _冷水机组_冷却水进口水温 + 17.06f;
                _冷水机组_COP = _冷水机组_COP_修正 * (-1.0009f * _负荷率 * _负荷率 * _负荷率 + 0.3945f * _负荷率 * _负荷率 + 1.4007f * _负荷率 + 0.2105f);
                _冷水机组_机组功率 = _冷水机组_制冷量 / _冷水机组_COP;
                _系统_COP = _建筑总冷负荷_变工况 / (_冷水机组_机组功率 * _冷水机组_开启台数 + _冷却水泵_运行功率 * _冷却水泵_开启台数 + _冷却塔_功率 * _冷却塔_开启台数 + _冷冻水泵_运行功率 * _冷冻水泵_运行台数);



                if (_负荷率 >= 0.99f && _冷水机组_制冷量 * _冷水机组_开启台数 >= _建筑总冷负荷)
                {
                    ISCan_满足负荷运行 = true;
                }
                else if (_负荷率 <= 0.99f && _冷水机组_制冷量 * _冷水机组_开启台数 <= _建筑总冷负荷_变工况 + 5 && _冷水机组_制冷量 * _冷水机组_开启台数 >= _建筑总冷负荷_变工况 - 10)
                {
                    ISCan_满足负荷运行 = true;
                }
            }
        }
    }
}
