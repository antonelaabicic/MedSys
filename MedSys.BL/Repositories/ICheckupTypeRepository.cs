﻿using MedSys.BL.DALModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedSys.BL.Repositories
{
    public interface ICheckupTypeRepository : IRepository<CheckupType>
    {
        CheckupType? FindCheckupTypeByCode(string code);
    }
}
