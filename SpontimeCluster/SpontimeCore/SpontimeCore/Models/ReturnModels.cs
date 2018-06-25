using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpontimeCore.Models
{
    public class ReturnModels
    {

        public class ShortEventModelList
        {
            List<ShortEventModel> list;

            public ShortEventModelList(List<ShortEventModel> _list)
            {
                list = _list;
            }

        }

        public class ShortUserModelList
        {
            List<ShortUserModel> list;

            public ShortUserModelList(List<ShortUserModel> _list)
            {
                list = _list;
            }

        }



    }
}