﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beta.Model.Component
{
    public interface IComponent
    {
        public List<Result> Query(Query query);
        public void Init(ComponentContext context);
    }
}
