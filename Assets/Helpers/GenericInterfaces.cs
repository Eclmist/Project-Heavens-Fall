using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

interface IDamagable
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns>If the damage is successfully taken</returns>
    bool TakeDamage();
}