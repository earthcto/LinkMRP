using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace SESD.MRP.REF
{
    public class SubStockRecMaterial
    {
        private long _MRNO;

        public long MRNO
        {
            get { return _MRNO; }
            set { _MRNO = value; }
        }

        private String _MaterialCode;

        public String MaterialCode
        {
            get { return _MaterialCode; }
            set { _MaterialCode = value; }
        }
	

        private long _SubStockID;

        public long SubStockID
        {
            get { return _SubStockID; }
            set { _SubStockID = value; }
        }

        private Decimal _Quantity;

        public Decimal Quantity
        {
            get { return _Quantity; }
            set { _Quantity = value; }
        }
	
    }
    public class SubStockRecMaterialCollec : CollectionBase
    {
        public void Add(SubStockRecMaterial obj)
        {
            this.InnerList.Add(obj);
        }
    }
}