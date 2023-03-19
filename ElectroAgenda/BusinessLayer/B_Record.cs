using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EntityLayer;
using DataLayer;

namespace BusinessLayer
{
    public class B_Record
    {
        public bool insertRecord(E_Record record)
        {
            return d_record.insert(record);
        }

        public E_Record findRecord(E_Record record)
        {
            return d_record.find(record);
        }

        public bool deleteRecord(E_Record record)
        {
            return d_record.delete(record);
        }

        public bool updateRecord(E_Record record)
        {
            return d_record.update(record);
        }

        private E_Record e_record = new E_Record();
        private D_Record d_record = new D_Record();
    }
}
