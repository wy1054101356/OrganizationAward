using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace org.Model
{
   public static class Enums
    {
        /// <summary>
        /// 状态
        /// </summary>
        public enum StatusEnum : int
        {

            /// <summary>
            /// 下沉
            /// </summary>
            [Description("下沉")]
            Normal = 1,

            /// <summary>
            /// 被删除
            /// </summary>
            [Description("被删除|delete")]
            Deleted = 2,
        }

        /// <summary>
        /// 用户类型
        /// </summary>
        public enum UserTypeEnum : int
        {


            [Description("超级管理员")]
            SuperAdmin = 1,

            [Description("管理员")]
            Admin = 2,

            [Description("普通用户")]
            Normal = 3
        }

        /// <summary>
        /// 用户类型
        /// </summary>
        public enum UserTypeEnumDrop : int
        {

            [Description("管理员")]
            Admin = 2,

            [Description("普通用户")]
            Normal = 3
        }

        public enum BagType : int
        {
            [Description("未分配")]
            Normal = 0,
            [Description("传说")]
            CS = 1,
            [Description("英雄")]
            YX = 2,
            [Description("精英")]
            JY = 3
        }

        public enum RewardType : int {
            [Description("奖励")]
            reward = 1,

            [Description("惩罚")]
            punishment = 2
        }
    }
}
