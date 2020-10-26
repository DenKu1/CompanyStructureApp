using CompanyStructureApp.Domain.Core.Abstract;
using CompanyStructureApp.Domain.Interfaces.Repositories;
using System.Collections;
using System.Collections.Generic;

namespace CompanyStructureApp.Infrastructure.DataAccess
{
    public class CompanyStructureContainer : ICompanyStructureContainer
    {
        // Employee component is the root component of company structure 'tree'
        public EmployeeComponent RootElement { get; set; }

        public IEnumerator<EmployeeComponent> GetEnumerator()
        {
            return RootElement.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
