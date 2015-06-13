﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace WallstreetDataService.Model
{
    [DataContract]
    public class Registration
    {
        [DataMember]
        public String Id { get; set; }

        [DataMember]
        public int Shares { get; set; }
    }
}
