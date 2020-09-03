using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace SimpleDB
{
    /// <summary>
    /// SimpleDB中元组中字段值的接口
    /// </summary>
    public interface Field
    {
        /**
     * Write the bytes representing this field to the specified
     * DataOutputStream.
     * @see DataOutputStream
     * @param dos The DataOutputStream to write to.
     */
        void Serialize(BinaryWriter dos);

        /**
         * Compare the value of this field object to the passed in value.
         * @param op The operator
         * @param value The value to compare this Field to
         * @return Whether or not the comparison yields true.
         */
        bool Compare(Predicate.Op op, Field value);

        /**
         * Returns the type of this field (see {@link Type#INT_TYPE} or {@link Type#STRING_TYPE}
         * @return type of this field
         */
        Type CustomGetType();

        /**
         * Hash code.
         * Different Field objects representing the same value should probably
         * return the same hashCode.
         */
    }
}
