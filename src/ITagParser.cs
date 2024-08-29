using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedMaple.Internationalization
{
    public interface ITagParser
    {
        /// <summary>
        /// Should return true if success
        /// </summary>
        /// <param name="tag">Tag to parse</param>
        /// <param name="destination"></param>
        /// <returns></returns>
        bool Parse(string tag, LanguageTag destination);
    }
}
