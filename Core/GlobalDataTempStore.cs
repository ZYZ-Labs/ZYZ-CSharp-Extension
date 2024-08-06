using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZYZ_CSharp_Extension.Core
{
    /// <summary>
    /// 全局参数临时存储池
    /// </summary>
    public class GlobalDataTempStore
    {
        private Dictionary<string, object?> DataStore;

        private static readonly Lazy<GlobalDataTempStore> lazyInstance = new Lazy<GlobalDataTempStore>(() => new GlobalDataTempStore());

        public static GlobalDataTempStore Instance
        {
            get
            {
                return lazyInstance.Value;
            }
        }
        private GlobalDataTempStore()
        {
            // 私有构造函数，防止外部实例化
            DataStore = new Dictionary<string, object?>();
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="key">字段</param>
        /// <param name="value">值</param>
        public void SaveData(string key, object value)
        {
            DataStore[key] = value;
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="key">字段</param>
        /// <param name="isDeleteAfter">是否获取之后删除</param>
        /// <returns></returns>
        public object? GetData(string key, bool isDeleteAfter = true)
        {
            object? data = DataStore[key];
            if (isDeleteAfter)
            {
                DataStore[key] = null;
            }
            return data;
        }
    }
}
