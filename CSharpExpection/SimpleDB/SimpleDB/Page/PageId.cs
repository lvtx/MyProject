using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDB
{
    /// <summary>
    /// PageId是到特定表的特定页面的接口
    /// </summary>
    public interface PageId
    {
        /// <summary>
        /// 以整数集合的形式返回此页面id对象的表示形式（用于记录）
        /// 此类必须具有接受n个整数参数的构造函数，
        /// 其中n是从序列化返回的数组中整数的数目。
        /// </summary>
        /// <returns></returns>
        int[] Serialize();

        /// <summary>
        /// 具有此PageId的唯一tableid哈希码
        /// </summary>
        /// <returns></returns>
        int GetTableId();

        /// <summary>
        /// 返回此页面的哈希码，
        /// 由表号和页面号的串联表示
        /// （例如，如果将PageId用作BufferPool的哈希表中的键，则需要此哈希码。)
        /// </summary>
        /// <returns></returns>
        int HashCode();

        /// <summary>
        /// 将一个PageId与另一个进行比较。
        /// o要比较的对象（必须是PageId）
        /// 如果对象相等（例如，页码和表ID相同），则返回true
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        bool Equals(Object o);

        int PageNumber();
    }
}
