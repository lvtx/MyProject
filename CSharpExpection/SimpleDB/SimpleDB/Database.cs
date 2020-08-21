using System;
using System.Reflection;

namespace SimpleDB
{
    public class Database
    {
        private static Database _instance = new Database();
        private readonly Catalog _catalog;
        private readonly BufferPool _bufferPool;
        public Database()
        {
            _catalog = new Catalog();
            _bufferPool = new BufferPool(BufferPool.DEFAULT_PAGES);
        }

        public static Catalog GetCatalog()
        {
            return _instance._catalog;
        }
        public static BufferPool GetBufferPool()
        {
            return _instance._bufferPool;
        }
        /**
 * Method used for testing -- create a new instance of the buffer pool and
 * return it
 */
        public static BufferPool ResetBufferPool(int pages)
        {
            //java.lang.reflect.Field bufferPoolF = null;
            try
            {              
                //bufferPoolF = Database.class.getDeclaredField("_bufferpool");
                //bufferPoolF.SetAccessible(true);
                //                bufferPoolF.SetValueDirect(_instance, new BufferPool(pages));
                //        } catch (NoSuchFieldException e) {
                //            e.printStackTrace();
                //        } catch (SecurityException e) {
                //            e.printStackTrace();
                //        } catch (IllegalArgumentException e) {
                //            e.printStackTrace();
                //        } catch (IllegalAccessException e) {
                //            e.printStackTrace();
                //        }
                ////        _instance._bufferpool = new BufferPool(pages);
                //        return _instance._bufferpool;
            }
            catch
            {

            }
            return new BufferPool(pages);
        }
        // 重置数据库，仅用于单元测试
        public static void Reset()
        {
            _instance = new Database();
        }
    }
}